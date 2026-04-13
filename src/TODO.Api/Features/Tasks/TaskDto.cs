namespace TODO.Api.Features.Tasks{
    public record TaskResponseDto 
    {
        public Guid Id { get; init; }
        public string Label { get; init; } = string.Empty;
        public DateOnly Deadline { get; init; }
    }

    public record TaskRegisterDto(string Label, DateOnly Deadline);
}