namespace Blogy.Business.Configurations;

public class HuggingFaceOptions
{
    public const string Section = "HuggingFace";
    public string ApiKey { get; set; } = string.Empty;
}
