using System.ComponentModel.DataAnnotations;

namespace DevSpot.ViewModels;

public class JobPostingViewModel
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public string CompanyName { get; set; }
}
