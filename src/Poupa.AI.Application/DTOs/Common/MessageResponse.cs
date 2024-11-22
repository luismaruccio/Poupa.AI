namespace Poupa.AI.Application.DTOs.Common
{
    public class MessageResponse(string message)
    {
        public string Message { get; } = message;
    }
}
