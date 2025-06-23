using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repository;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Tests;

public class JobPostingRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public JobPostingRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("JobPostingDb")
            .Options;
    }

    private ApplicationDbContext CreateDbContext()
    {
        return new ApplicationDbContext(_options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddJobPosting()
    {
        using var context = CreateDbContext();

        var repository = new JobPostingRepository(context);

        var jobPosting = new JobPosting
        {
            Title = "Test Title",
            Description = "Test Description",
            PostedDate = DateTime.UtcNow,
            CompanyName = "Test Company",
            Location = "Test Location",
            PostedById = "TestUserId",
        };

        await repository.AddAsync(jobPosting);
        var result = await context.JobPostings.FirstOrDefaultAsync(jp => jp.Title == "Test Title");
        Assert.NotNull(result);
        Assert.Equal("Test Title", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnJobPosting()
    {
        using var context = CreateDbContext();
        var repository = new JobPostingRepository(context);
        var jobPosting = new JobPosting
        {
            Title = "Test Title",
            Description = "Test Description",
            PostedDate = DateTime.UtcNow,
            CompanyName = "Test Company",
            Location = "Test Location",
            PostedById = "TestUserId",
        };

        await context.JobPostings.AddAsync(jobPosting);
        await context.SaveChangesAsync();
        var result = await repository.GetByIdAsync(jobPosting.Id);
        Assert.NotNull(result);
        Assert.Equal("Test Title", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowKeyNotFoundException()
    {
        using var context = CreateDbContext();
        var repository = new JobPostingRepository(context);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.GetByIdAsync(9999));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllJobPostings()
    {
        using var context = CreateDbContext();
        var repository = new JobPostingRepository(context);
        var previousCount = (await repository.GetAllAsync()).Count();

        await context.JobPostings.AddRangeAsync(new List<JobPosting>
        {
            new JobPosting { Title = "Job 1", Description = "Description 1", PostedDate = DateTime.UtcNow, CompanyName = "Company 1", Location = "Location 1", PostedById = "UserId1" },
            new JobPosting { Title = "Job 2", Description = "Description 2", PostedDate = DateTime.UtcNow, CompanyName = "Company 2", Location = "Location 2", PostedById = "UserId2" }
        });
        await context.SaveChangesAsync();

        var result = await repository.GetAllAsync();

        Assert.Equal(previousCount + 2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateJobPosting()
    {
        using var context = CreateDbContext();
        var repository = new JobPostingRepository(context);
        var jobPosting = new JobPosting
        {
            Title = "Old Title",
            Description = "Old Description",
            PostedDate = DateTime.UtcNow,
            CompanyName = "Old Company",
            Location = "Old Location",
            PostedById = "TestUserId",
        };
        await context.JobPostings.AddAsync(jobPosting);
        await context.SaveChangesAsync();

        jobPosting.Title = "Updated Title";
        await repository.UpdateAsync(jobPosting);

        var result = await context.JobPostings.FirstOrDefaultAsync(jp => jp.Id == jobPosting.Id);
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveJobPosting()
    {
        using var context = CreateDbContext();
        var repository = new JobPostingRepository(context);
        var jobPosting = new JobPosting
        {
            Title = "Test Title",
            Description = "Test Description",
            PostedDate = DateTime.UtcNow,
            CompanyName = "Test Company",
            Location = "Test Location",
            PostedById = "TestUserId",
        };
        await context.JobPostings.AddAsync(jobPosting);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(jobPosting.Id);

        var result = await context.JobPostings.FindAsync(jobPosting.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowKeyNotFoundException()
    {
        using var context = CreateDbContext();
        var repository = new JobPostingRepository(context);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.DeleteAsync(9999));
    }
}