namespace Morin.Shared.Models;

public class ClassModel : Model
{
    public int Pid { get; set; }
    public int SourceID { get; set; }
    public string? ClassName { get; set; }

    public List<ClassModel> ChildNodes { get; set; }
    public ClassModel()
    {
        ChildNodes = [];
        Pid = 0;
    }
}
