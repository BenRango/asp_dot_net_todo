namespace TODO.Api.Features.Tasks
{
    public class TodoTask
    {
        public Guid Id {get; set;} = Guid.NewGuid();
        public string Label {get; set;} = string.Empty;
        public DateOnly Deadline { get; set;}
        protected TodoTask() { }

    }
}