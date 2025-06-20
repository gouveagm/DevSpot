using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevSpot.Models;

public class JobPosting
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public string CompanyName { get; set; }
    public DateTime PostedDate { get; set; } = DateTime.UtcNow;
    public bool IsApproved { get; set; }
    [Required]
    public string PostedById { get; set; }

    [ForeignKey(nameof(PostedById))]
    public IdentityUser User { get; set; }
}
