namespace Blogy.Business.Dtos.AiDtos;

public record ToxicityAnalysisResult(
    string TranslatedText,
    double ToxicityScore,
    bool IsToxic
);
