namespace SmartRx_DrugChecker.Models;

public class ProfessionalProfile
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public string Title { get; set; } = "";
    public string Specialty { get; set; } = "";
    public string Qualification { get; set; } = "";
    public string Organization { get; set; } = "";
    public string Location { get; set; } = "";
    public string MapUrl { get; set; } = "";
    public string About { get; set; } = "";
    public string? ProfileImageUrl { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}
