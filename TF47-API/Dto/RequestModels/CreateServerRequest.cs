namespace TF47_API.Dto.RequestModels
{
    public record CreateServerRequest(string Name, string Description, string IP, string Port, string Branch);
}