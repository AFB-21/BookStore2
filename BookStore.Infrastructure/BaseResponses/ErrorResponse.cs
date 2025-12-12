namespace BookStore.Infrastructure.BaseResponses
{
    public class ErrorResponse
    {
        public string Message { get; set; } = default!;
        public Dictionary<string, string[]>? Errors { get; set; }
    }

}
