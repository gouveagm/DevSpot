using DevSpot.Constants;
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

    [HttpDelete]
    [Authorize(Roles = "Admin,Employer")]
    public async Task<IActionResult> Delete(int id)
    {
        var jobPosting = await _jobPostingRepository.GetByIdAsync(id);
        if (jobPosting == null)
        {
            return NotFound($"Job posting with ID {id} not found.");
        }

        if (User.IsInRole(Roles.Employer) && jobPosting.PostedById != _userManager.GetUserId(User))
        {
            return Forbid("You do not have permission to delete this job posting.");
        }

        try
        {
            await this._jobPostingRepository.DeleteAsync(id);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
