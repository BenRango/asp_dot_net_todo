using Microsoft.AspNetCore.Mvc;

namespace TODO.Api.Features.Tasks
{
    public static class TaskEndpoints
    {
        public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/tasks").WithTags("Tasks");
            group.RequireAuthorization();
            group.MapGet("/", async (ITaskService taskServices) =>
            {
                var tasks = await taskServices.GetAllTasks();
                return Results.Ok(tasks);
            });
            group.MapGet("/tasks/{id}", async (Guid id, [FromServices] ITaskService service) =>{
                TaskResponseDto? task = await service.GetOneById(id);
                return task is null ? Results.NotFound("Aucune tâche avec cet Id") : Results.Ok(task);
            });
            group.MapPost("/tasks", async ([FromBody] TaskRegisterDto taskDto, [FromServices] ITaskService service) =>{
            return Results.Ok(await service.CreateOne(taskDto));  
            });
        }
    }
}