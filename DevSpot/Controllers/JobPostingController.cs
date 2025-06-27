using DevSpot.Models;
using DevSpot.Repository;
using DevSpot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSpot.Controllers;

[Authorize]
public class JobPostingController : Controller
{
    private readonly IRepository<JobPosting> _jobPostingRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public JobPostingController(IRepository<JobPosting> jobPostingRepository, UserManager<IdentityUser> userManager)
    {
        _jobPostingRepository = jobPostingRepository;
        _userManager = userManager;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var jobPostings = await _jobPostingRepository.GetAllAsync();
        return View(jobPostings);
    }

    [Authorize(Roles = "Admin,Employer")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(JobPostingViewModel jobPosting)
    {
        if (ModelState.IsValid)
        {
            var jobPostingEntity = new JobPosting
            {
                Title = jobPosting.Title,
                Description = jobPosting.Description,
                Location = jobPosting.Location,
                CompanyName = jobPosting.CompanyName,
                PostedById = _userManager.GetUserId(User),
            };

            await _jobPostingRepository.AddAsync(jobPostingEntity);
            return RedirectToAction(nameof(Index));
        }

        return View(jobPosting);
    }

    
    [Authorize(Roles = "Admin,Employer")]
    public async Task<IActionResult> Delete(int id)
    {
        var jobPosting = await _jobPostingRepository.GetByIdAsync(id);
        if (jobPosting == null)
        {
            return NotFound();
        }
        if (jobPosting.PostedById != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        await _jobPostingRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
