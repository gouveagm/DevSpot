using DevSpot.Data;
using DevSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Repository;

public class JobPostingRepository : IRepository<JobPosting>
{
    private readonly ApplicationDbContext _context;

    public JobPostingRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task AddAsync(JobPosting entity)
    {
        await this._context.JobPostings.AddAsync(entity);
        await this._context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var element = await this._context.JobPostings.FindAsync(id);
        if (element is null)
        {
            throw new KeyNotFoundException($"JobPosting with ID {id} not found."); 
        }

        this._context.JobPostings.Remove(element);
        await this._context.SaveChangesAsync();
    }

    public async Task<IEnumerable<JobPosting>> GetAllAsync()
    {
        return await this._context.JobPostings.ToListAsync();
    }

    public async Task<JobPosting> GetByIdAsync(int id)
    {
        var element = await this._context.JobPostings.FindAsync(id);
        if (element is null)
        {
            throw new KeyNotFoundException($"JobPosting with ID {id} not found.");
        }
        
        return element;
    }
    
    public async Task UpdateAsync(JobPosting entity)
    {
        this._context.JobPostings.Update(entity);
        await this._context.SaveChangesAsync();
    }
}
