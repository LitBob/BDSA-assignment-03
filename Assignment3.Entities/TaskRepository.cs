using Assignment3.Core;

namespace Assignment3.Entities;

public class TaskRepository : Assignment3.Core.ITaskRepository
{
    private readonly KanbanContext _context;

    public TaskRepository(KanbanContext context)
    {
        _context = context;
    }
    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var entity = _context.Tasks.FirstOrDefault(t => t.Title == task.Title);
        Response response;
        
        if (entity is null)
        {
            entity = new Task(task.Title);

            _context.Tasks.Add(entity);
            _context.SaveChanges();

            response = Response.Created;
        }
        else 
        {
            response =  Response.Conflict;
        }

        return (response, entity.Id);
    }

    public Response Delete(int taskId)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
        Response response;

        if (task is null) 
        {
            response = Response.NotFound;
        }
        else if (task.State == State.Active)
        {
            task.State = State.Removed;
            _context.SaveChanges();
            response = Response.Deleted; //Updated?
        }
        else if (task.State == State.New)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();

            response = Response.Deleted;
            
        }
        else //Only land here if State is one of the conflicting types
        {
            response = Response.Conflict;
        }

        return response;
    }

    public TaskDetailsDTO Read(int taskId)
    {
        var tasks = from t in _context.Tasks
                    where t.Id == taskId
                    select new TaskDetailsDTO(t.Id, t.Title, t.Description, t.Created, t.AssignedTo!.Name, t.Tags.Select(t => t.Name).ToList(), t.State, t.Updated);
       
        if (tasks is null) return null;
        else return tasks.FirstOrDefault(); //This will never be null here
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var tasks = from t in _context.Tasks
                    select new TaskDTO(t.Id, t.Title, t.AssignedTo!.Name, t.Tags.Select(t => t.Name).ToList(), t.State);

        return tasks.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(Core.State state)
    {
        var tasks = from t in _context.Tasks
                    where t.State == state
                    select new TaskDTO(t.Id, t.Title, t.AssignedTo!.Name, t.Tags.Select(t => t.Name).ToList(), t.State);

        return tasks.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        var specificTag = _context.Tags.Find(tag);
        var tasks = from t in _context.Tasks
                    where t.Tags == specificTag
                    select new TaskDTO(t.Id, t.Title, t.AssignedTo!.Name, t.Tags.Select(t => t.Name).ToList(), t.State);

        return tasks.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        var tasks = from t in _context.Tasks
                    where t.AssignedTo!.Id == userId
                    select new TaskDTO(t.Id, t.Title, t.AssignedTo!.Name, t.Tags.Select(t => t.Name).ToList(), t.State);

        return tasks.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        var tasks = from t in _context.Tasks
                    where t.State == State.Removed
                    select new TaskDTO(t.Id, t.Title, t.AssignedTo!.Name, t.Tags.Select(t => t.Name).ToList(), t.State);

        return tasks.ToArray();
    }

    public Response Update(TaskUpdateDTO task)
    {
        var entity = _context.Tasks.Find(task.Id);

        // Get all tags from task
        var allTags = new List<Tag> {};
        foreach(string s in task.Tags) {
            int sInt = int.Parse(s);
            var tag = _context.Tags.Find(sInt);
            if (tag != null) allTags.Add(tag);
        }

        Response response;

        if (entity is null) 
        {
            response = Response.NotFound;
        }
        else if (_context.Tasks.FirstOrDefault(t => t.Id != task.Id && t.Title == task.Title) != null)
        {
            response = Response.Conflict;
        }
        else 
        {
            entity.Title = task.Title;
            entity.Tags = allTags;
            if (task.State != entity.State) {   //Only update "updated" if state is changed
                entity.Updated = DateTime.UtcNow;
            }
            entity.State = task.State;
            _context.SaveChanges();
            response = Response.Updated;
        }
        return response;
    }
}
