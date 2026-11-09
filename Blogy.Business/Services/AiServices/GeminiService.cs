using System.Net.Http.Json;
using System.Text.Json;
using Blogy.Business.Configurations;
using Blogy.Business.Consts;
using Blogy.Business.Dtos.AiDtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blogy.Business.Services.AiServices;

public class GeminiService(
    IOptions<GeminiOptions> options,
    ILogger<GeminiService> logger) : IAiService
{
    private readonly GeminiOptions _options = options.Value;

    private static readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    public async Task<string> GenerateAboutSummaryAsync(string? customPrompt = null)
    {
        var prompt = AiPrompts.GetAboutSummaryPrompt(customPrompt);
        return await ExecutePromptAsync(prompt);
    }

    public async Task<AiContactReplyResult> GenerateReplyForContactAsync(string userMessage)
    {
        string rawResponse = string.Empty;

        try
        {

            rawResponse = await ExecutePromptAsync(
                userPrompt: $"Kullanıcı İletişim Mesajı:\n\"{userMessage}\"",
                systemInstruction: AiPrompts.ContactAssistantSystemInstruction,
                forceJson: true
            );

            using var doc = JsonDocument.Parse(rawResponse);
            var root = doc.RootElement;

            var detectedLang = root.TryGetProperty("detectedLanguage", out var langProp)
                ? langProp.GetString() ?? "Türkçe"
                : "Türkçe";

            var reply = root.TryGetProperty("reply", out var replyProp)
                ? replyProp.GetString() ?? rawResponse
                : rawResponse;

            return new AiContactReplyResult(reply, detectedLang);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Gemini yanıtı işlenirken sorun oluştu. Ham yanıt kullanılıyor.");
            return new AiContactReplyResult(
                Reply: string.IsNullOrWhiteSpace(rawResponse)
                    ? "Mesajınız başarıyla alındı. En kısa sürede geri dönüş sağlayacağız."
                    : rawResponse,
                DetectedLanguage: "Türkçe"
            );
        }
    }

    public async Task<string> GenerateArticleAsync(string keywords, string summaryPrompt)
    {
        var prompt = AiPrompts.GetArticlePrompt(keywords, summaryPrompt);
        return await ExecutePromptAsync(prompt);
    }

    public async Task<AiBlogGenerationResult> GenerateFullBlogAsync(
        string? keywords,
        string? contentOrDraft,
        string? customInstructions,
        List<string>? categories = null)
    {

        var combinedInput = $"{keywords} {contentOrDraft} {customInstructions}".ToLowerInvariant();
        string[] blacklist = ["ignore previous", "forget all rules", "jailbreak", "dan mode", "system prompt", "<script>", "delete from", "drop table"];

        if (blacklist.Any(badWord => combinedInput.Contains(badWord)))
        {
            return new AiBlogGenerationResult(
                Title: "Güvenlik Engeli",
                Content: "Girdiğiniz talimatlarda güvenlik protokollerini ihlal eden şüpheli ifadeler tespit edildiği için istek engellendi.",
                Tags: "guvenlik",
                SuggestedCategory: categories?.FirstOrDefault()
            );
        }

        var categoryListStr = categories != null && categories.Any()
            ? string.Join(", ", categories)
            : "Genel, Teknoloji, Yazılım, Kültür";

        var userPrompt = $"""
            Sitedeki Mevcut Kategoriler: {categoryListStr}

            <keywords>
            {keywords ?? "Belirtilmedi"}
            </keywords>

            <raw_draft>
            {contentOrDraft ?? "Sıfırdan üretilecek"}
            </raw_draft>

            <custom_instructions>
            {customInstructions ?? "Standart kurumsal ve akıcı bir blog dili kullan"}
            </custom_instructions>
            """;

        var rawResponse = await ExecutePromptAsync(
            userPrompt: userPrompt,
            systemInstruction: AiPrompts.FullBlogSystemInstruction,
            forceJson: true
        );

        try
        {
            using var doc = JsonDocument.Parse(rawResponse);
            var root = doc.RootElement;

            var title = root.TryGetProperty("title", out var titleProp) ? titleProp.GetString() ?? "" : "";
            var content = root.TryGetProperty("content", out var contentProp) ? contentProp.GetString() ?? "" : "";
            var tags = root.TryGetProperty("tags", out var tagsProp) ? tagsProp.GetString() ?? "" : "";
            var category = root.TryGetProperty("suggestedCategory", out var catProp) ? catProp.GetString() : null;

            return new AiBlogGenerationResult(title, content, tags, category);
        }
        catch
        {
            return new AiBlogGenerationResult(
                Title: keywords ?? "Yeni Blog",
                Content: rawResponse,
                Tags: "blog, yazi",
                SuggestedCategory: categories?.FirstOrDefault()
            );
        }
    }

    public async Task<ToxicityAnalysisResult> AnalyzeToxicityWithGeminiAsync(string turkishText)
    {
        if (string.IsNullOrWhiteSpace(turkishText))
        {
            return new ToxicityAnalysisResult(string.Empty, 0.0, false);
        }

        try
        {
            var rawResponse = await ExecutePromptAsync(
                userPrompt: $"Analiz edilecek yorum:\n\"{turkishText}\"",
                systemInstruction: AiPrompts.CommentToxicitySystemInstruction,
                forceJson: true
            );

            using var doc = JsonDocument.Parse(rawResponse);
            var root = doc.RootElement;

            var translatedText = root.TryGetProperty("translatedText", out var trProp) ? trProp.GetString() ?? turkishText : turkishText;
            var score = root.TryGetProperty("toxicityScore", out var scProp) ? scProp.GetDouble() : 0.0;
            var isToxic = root.TryGetProperty("isToxic", out var toxProp) ? toxProp.GetBoolean() : (score > 0.50);

            return new ToxicityAnalysisResult(translatedText, score, isToxic);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Gemini toksisite analizinde hata oluştu.");
            return new ToxicityAnalysisResult(string.Empty, -1.0, false);
        }
    }

    private async Task<string> ExecutePromptAsync(string userPrompt, string? systemInstruction = null, bool forceJson = false)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            logger.LogWarning("Gemini API Key tanımlı değil!");
            return "AI servisi şu anda yapılandırılmamış (API Anahtarı eksik).";
        }

        var endpoint = $"{_options.BaseUrl.TrimEnd('/')}/{_options.Model}:generateContent";

        var requestBody = new Dictionary<string, object>
        {
            ["contents"] = new[]
            {
                new
                {
                    role = "user",
                    parts = new[] { new { text = userPrompt } }
                }
            }
        };

        if (!string.IsNullOrWhiteSpace(systemInstruction))
        {
            requestBody["system_instruction"] = new
            {
                parts = new[] { new { text = systemInstruction } }
            };
        }

        if (forceJson)
        {
            requestBody["generationConfig"] = new
            {
                response_mime_type = "application/json"
            };
        }

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Add("X-goog-api-key", _options.ApiKey);
            request.Content = JsonContent.Create(requestBody);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                logger.LogError("Gemini API Hatası: {StatusCode} - {Error}", response.StatusCode, errorBody);
                return "Yapay zeka yanıt üretirken bir hata oluştu.";
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(responseStream);

            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text?.Trim() ?? "Yanıt üretilemedi.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Gemini API isteği sırasında hata oluştu.");
            return "AI servisine bağlanırken bir sorun oluştu.";
        }
    }
}
