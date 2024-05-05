namespace Morin.Shared.Models;

public class MenuModel : Model
{
    public int Pid { get; set; }
    public bool Selected { get; set; }
    public int Sort { get; set; }
    public string? Icon { get; set; }
    public string? Title { get; set; }
    public string? ViewModel { get; set; }
    public bool Visvisibility { get; set; } = true;
}
