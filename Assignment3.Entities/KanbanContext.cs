namespace Assignment3.Entities;

public partial class KanbanContext : DbContext
{
    public KanbanContext(DbContextOptions<KanbanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Tag> Tags => Set<Tag>();
    public virtual DbSet<Task> Tasks => Set<Task>();
    public virtual DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Task>()
            .Property(e => e.State)
            .HasConversion(
                v => v.ToString(),
                v => (State)Enum.Parse(typeof(State), v));
    }


}
