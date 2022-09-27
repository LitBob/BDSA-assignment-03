namespace Assignment3.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    public virtual ICollection<Task> Tasks { get; set; }

    public User (string name, string email)
    {
        Name = name;
        Email = email;
        Tasks = new List<Task>();
    }
}
