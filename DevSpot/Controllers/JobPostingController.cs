using DevSpot.Models;
using DevSpot.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSpot.Controllers;

public class JobPostingController : Controller
{
    private readonly IRepository<JobPosting> _jobPostingRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public JobPostingController(IRepository<JobPosting> jobPostingRepository, UserManager<IdentityUser> userManager)
    {
        _jobPostingRepository = jobPostingRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var jobPostings = await _jobPostingRepository.GetAllAsync();
        return View(jobPostings);
    }

    public IActionResult Create()
    {
        return View();
    }
}
