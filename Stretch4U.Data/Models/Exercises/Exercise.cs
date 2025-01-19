namespace Stretch4U.Data.Models.Exercises;

public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public int Sets { get; set; }
    public int MobilityLevel { get; set; }
}
