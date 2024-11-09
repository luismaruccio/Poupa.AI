namespace Poupa.AI.Application.DTOs.Common
{
    public class FailureResponse(string error)
    {
        public string Error { get; } = error;
    }
}
