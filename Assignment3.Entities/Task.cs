namespace Assignment3.Entities;

public class Task
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(100)]
    [Required]
    public string Title { get; set; }

    public User? AssignedTo { get; set; }

    public string? Description { get; set; }
    public Assignment3.Core.State State { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    [Required]
    public virtual ICollection<Tag> Tags { get; set; }

    public Task (string title) 
    {
        Title = title;
        State = Assignment3.Core.State.New; //This should probably be set but I suppose it can just be updated later
        Updated = DateTime.Now;
        Tags = new List<Tag>();
    }

}
