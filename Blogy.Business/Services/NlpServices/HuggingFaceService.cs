using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Blogy.Business.Configurations;
using Blogy.Business.Dtos.AiDtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blogy.Business.Services.NlpServices;

public class HuggingFaceService(
    IOptions<HuggingFaceOptions> options,
    ILogger<HuggingFaceService> logger) : IHuggingFaceService
{
    private readonly HuggingFaceOptions _options = options.Value;
    private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(20) };

    private const string HelsinkiUrl = "https://router.huggingface.co/hf-inference/models/Helsinki-NLP/opus-mt-tr-en";
    private const string ToxicBertUrl = "https://router.huggingface.co/hf-inference/models/unitary/toxic-bert";

    public async Task<ToxicityAnalysisResult> AnalyzeToxicityAsync(string turkishText)
    {
        if (string.IsNullOrWhiteSpace(turkishText))
        {
            return new ToxicityAnalysisResult(string.Empty, 0.0, false);
        }

        var englishText = await TranslateToEnglishAsync(turkishText);

        if (string.Equals(englishText, turkishText, StringComparison.OrdinalIgnoreCase))
        {
            logger.LogWarning("Helsinki çevirisi başarısız olduğu için analiz tamamlanamadı.");
            return new ToxicityAnalysisResult(string.Empty, -1.0, false);
        }

        var toxicityScore = await CalculateToxicityScoreAsync(englishText);

        var isToxic = toxicityScore > 0.50;

        return new ToxicityAnalysisResult(englishText, toxicityScore, isToxic);
    }

    private async Task<string> TranslateToEnglishAsync(string turkishText)
    {
        try
        {
            var payload = JsonSerializer.Serialize(new { inputs = turkishText });
            using var request = new HttpRequestMessage(HttpMethod.Post, HelsinkiUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Helsinki Çeviri API hatası: {StatusCode}", response.StatusCode);
                return turkishText;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonString);

            if (doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0)
            {
                var first = doc.RootElement[0];
                if (first.TryGetProperty("translation_text", out var textProp))
                {
                    return textProp.GetString() ?? turkishText;
                }
            }

            return turkishText;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Helsinki çevirisinde hata oluştu.");
            return turkishText;
        }
    }

    private async Task<double> CalculateToxicityScoreAsync(string englishText)
    {
        try
        {
            var payload = JsonSerializer.Serialize(new { inputs = englishText });
            using var request = new HttpRequestMessage(HttpMethod.Post, ToxicBertUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Toxic-BERT API hatası: {StatusCode}", response.StatusCode);
                return 0.0;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonString);

            if (doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0)
            {
                var innerArray = doc.RootElement[0];
                if (innerArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in innerArray.EnumerateArray())
                    {
                        var label = item.GetProperty("label").GetString();
                        if (string.Equals(label, "toxic", StringComparison.OrdinalIgnoreCase))
                        {
                            return item.GetProperty("score").GetDouble();
                        }
                    }
                }
            }

            return 0.0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Toxic-BERT analizinde hata oluştu.");
            return 0.0;
        }
    }
}
