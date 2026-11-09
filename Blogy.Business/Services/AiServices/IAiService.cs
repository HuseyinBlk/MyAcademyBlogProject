using Blogy.Business.Dtos.AiDtos;

namespace Blogy.Business.Services.AiServices;

public interface IAiService
{
    Task<string> GenerateAboutSummaryAsync(string? customPrompt = null);

    Task<AiContactReplyResult> GenerateReplyForContactAsync(string userMessage);

    Task<string> GenerateArticleAsync(string keywords, string summaryPrompt);

    Task<AiBlogGenerationResult> GenerateFullBlogAsync(string? keywords, string? contentOrDraft, string? customInstructions, List<string>? categories = null);

    Task<ToxicityAnalysisResult> AnalyzeToxicityWithGeminiAsync(string turkishText);
}
