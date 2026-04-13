using AutoMapper;
using TODO.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace TODO.Api.Features.Tasks{
    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;
        public TaskService(IMapper mapper, AppDbContext db)
        {
            _db = db;
            _mapper = mapper;
        }
        private List<TaskResponseDto> Tasks = new List<TaskResponseDto>();
        public async Task<IEnumerable<TaskResponseDto>> GetAllTasks()
        {
            var raw = await _db.TodoTask.ToListAsync();
            return _mapper.Map<IEnumerable<TaskResponseDto>>(raw);
        }

        public async Task<TaskResponseDto?> GetOneById(Guid Id)
        {
            var task = await _db.TodoTask.FirstOrDefaultAsync( t =>t.Id == Id);
            TaskResponseDto response = _mapper.Map<TaskResponseDto>(task); 
            return response;
        }

        public async Task<TaskResponseDto> CreateOne(TaskRegisterDto taskDto)
        {
            var task = _mapper.Map<TodoTask>(taskDto);

            bool isExists = Tasks.Find((t)=>t.Label == task.Label) != null;
            if (isExists)
            {
                throw new BadHttpRequestException("Dupplicated Label");
            }
            _db.TodoTask.Add(task);
            await _db.SaveChangesAsync();
            TaskResponseDto responseTask = _mapper.Map<TaskResponseDto>(task);
            Tasks.Add(responseTask);
            return responseTask ;
        
        }
    }
}