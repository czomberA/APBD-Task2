namespace Task2;

public class University
{
    public string CreatedAt { get; set; }
    public string Author{ get; set; }
    public HashSet<Student> Students { get; set; }
    public HashSet<ActiveStudies> ActiveStudies { get; set; }
}