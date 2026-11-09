namespace Blogy.Business.Dtos.AiDtos;

public record AiBlogGenerationResult(
    string Title,
    string Content,
    string Tags,
    string? SuggestedCategory
);
