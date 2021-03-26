namespace TF47_Backend.Dto.RequestModels
{
    public record CreateChangelogRequest(string Title, string[] Tags, string Text);

    public record UpdateChangelogRequest(string Title, bool IgnoreTags, string[] Tags, string Text);
}