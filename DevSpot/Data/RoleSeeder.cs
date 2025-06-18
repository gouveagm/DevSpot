using DevSpot.Constants;
using Microsoft.AspNetCore.Identity;

namespace DevSpot.Data;

public class RoleSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleSeeder(RoleManager<IdentityRole> roleManager)
    {
        this._roleManager = roleManager;
    }

    public async Task SeedRolesAsync()
    {
        if (!await this._roleManager.RoleExistsAsync(Roles.Admin))
        {
            await this._roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }
        if (!await this._roleManager.RoleExistsAsync(Roles.JobSeeker))
        {
            await this._roleManager.CreateAsync(new IdentityRole(Roles.JobSeeker));
        }
        if (!await this._roleManager.RoleExistsAsync(Roles.Employer))
        {
            await this._roleManager.CreateAsync(new IdentityRole(Roles.Employer));
        }
    }
}
