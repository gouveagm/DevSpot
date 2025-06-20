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
        await this.CreateUserIfNotExistAsync(AdminEmail, AdminDefaultPassword, Roles.Admin);
        await this.CreateUserIfNotExistAsync(JobSeekerEmail, JobSeekerDefaultPassword, Roles.JobSeeker);
        await this.CreateUserIfNotExistAsync(EmployerEmail, EmployerDefaultPassword, Roles.Employer);
    }

    private async Task CreateUserIfNotExistAsync(string email, string password, string role)
    {
        var user = await this._userManager.FindByEmailAsync(email);
        if (user == null)
        {
            await this.CreateUserAsync(email, password, role);
        }
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
