namespace Assignment3.Entities.Tests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class TaskRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.Tasks.AddRange(new Task("taskTitle1"), new Task("taskTitle2"));
        context.Tags.AddRange(new Tag("tag1"), new Tag("tag2"));
        context.SaveChanges();

        _context = context;
        _repository = new TaskRepository(_context);
    }

    [Fact]
    public void Create_task_returns_response_and_id() 
    {
        //Arrange
        var listOfTagIds = new[]{"1", "2"};

        //Act
        var (response, created) = _repository.Create(new Core.TaskCreateDTO("taskTitle3", null, null, listOfTagIds));

        //Assert
        response.Should().Be(Response.Created);
        created.Should().Be(new TaskDTO(3, "taskTitle3", null, listOfTagIds, State.New).Id);
    }


    [Fact]
    public void Delete_task_returns_response() 
    {
        //Arrange

        //Act
        var response = _repository.Delete(2);

        //Assert
        response.Should().Be(Response.Deleted);
        
    }

    [Fact]
    public void Read_given_TaskId_returns_TaskDTO() 
    {
        //Arrange
        //Happens in database creation

        //Act
        var response = _repository.Read(2);
        var tagList = new List<string>();

        //Assert
        response.Should().BeEquivalentTo(new TaskDetailsDTO(2, "taskTitle2",null, new DateTime(), null, tagList, State.New, new DateTime()));

    }

    [Fact]
    public void Update_given_TagUpdateDTO_returns_Response() 
    {
        //Arrange
        //Happens in database creation
        var tagList = new List<string>();

        //Act
        var response = _repository.Update(new TaskUpdateDTO(2, "nytNavn", null, null, tagList, State.Active));

        //Assert
        response.Should().Be(Core.Response.Updated);

    }

    [Fact]
    public void ReadAll_returns_all_TaskDTOs() 
    {
        //Arrange
        //Happens in database creation
        var tagList = new List<string>();

        //Act
        var response = _repository.ReadAll();
        var output = new[] {new TaskDTO(1, "taskTitle1", null, tagList, State.New), new TaskDTO(2, "taskTitle2", null, tagList, State.New)};

        //Assert
        response.Should().BeEquivalentTo(output);
    }

}
