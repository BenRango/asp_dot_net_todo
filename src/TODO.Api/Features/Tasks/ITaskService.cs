namespace TODO.Api.Features.Tasks
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetAllTasks();
        Task<TaskResponseDto?> GetOneById(Guid Id);
        Task<TaskResponseDto> CreateOne(TaskRegisterDto TaskDto);
    }
}