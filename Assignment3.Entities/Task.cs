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
    public State State { get; set; }

    [Required]
    public virtual ICollection<Tag> Tags { get; set; }

}


public enum State {
    New, 
    Active, 
    Resolved, 
    Closed, 
    Removed 
}