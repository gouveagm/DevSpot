using DevSpot.Constants;
using Microsoft.AspNetCore.Identity;

namespace DevSpot.Data;

public class UserSeeder
{
    private const string AdminEmail = "admin@devspot.com";
    private const string AdminDefaultPassword = "Admin123!";
    private const string JobSeekerEmail = "jobseeker@devspot.com";
    private const string JobSeekerDefaultPassword = "JobSeeker123!";
    private const string EmployerEmail = "employer@devspot.com";
    private const string EmployerDefaultPassword = "Employer123!";

    private readonly UserManager<IdentityUser> _userManager;

    public UserSeeder(UserManager<IdentityUser> userManager)
    {
        this._userManager = userManager;
    }

    public async Task SeedUserAsync()
    {
        await this.CreateUserAsync(AdminEmail, AdminDefaultPassword, Roles.Admin);
        await this.CreateUserAsync(JobSeekerEmail, JobSeekerDefaultPassword, Roles.JobSeeker);
        await this.CreateUserAsync(EmployerEmail, EmployerDefaultPassword, Roles.Employer);
    }

    private async Task CreateUserAsync(string email, string password, string role)
    {
        var user = new IdentityUser { UserName = email, Email = email , EmailConfirmed = true};
        var result = await this._userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await this._userManager.AddToRoleAsync(user, role);
        }
        else
        {
            throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}
