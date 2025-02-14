using WebAPI.Models;

public class Environment2D
{
    public Guid EnvironmentId { get; set; } 
    public string Name { get; set; }
    public int MaxLength { get; set; }
    public int MaxHeight { get; set; }
    public string Username { get; set; }
    public List<Object2D> Objects { get; set; } = new List<Object2D>();
}
