using Blogy.Business.Dtos.AiDtos;

namespace Blogy.Business.Services.NlpServices;

public interface IHuggingFaceService
{
    Task<ToxicityAnalysisResult> AnalyzeToxicityAsync(string turkishText);
}
