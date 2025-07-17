namespace Golestan.Infrastructure.Data.Repositories;

using Application.DTOs.Term;
using Application.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;


public class TermRepository(AppDbContext context) : ITermRepository {

    public async Task<TermDto?> GetCurrentTerm()
    {
        var currentDate = DateTime.UtcNow;

        return await context.Terms
            .AsNoTracking()
            .Where(t => !t.IsClosed)
            .Select(t => new TermDto()
            {
                ExamsStartTime = t.ExamsStartTime,
                ExamsEndTime = t.ExamsEndTime,
                SelectionStartTime = t.SectionSelectionStartTime,
                SelectionEndTime = t.SectionSelectionEndTime,
                TermIdentifier = t.TermIdentifier,
                StartTime = t.StartTime,
                TermName = t.TermName,
                Year = t.Year,
                Id = t.Id
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Term?> GetLastTerm()
    {
        return await context.Terms
            .AsNoTracking()
            .OrderByDescending(t => t.StartTime)
            .FirstOrDefaultAsync();
    }

    public async Task<Term?> GetCurrentTermEntity()
    {
        var currentDate = DateTime.UtcNow;

        return await context.Terms
            .AsNoTracking()
            .FirstOrDefaultAsync(t => !t.IsClosed);
    }

    public async Task<List<TermDto>> GetAllTerms()
    {
        return await context.Terms
            .AsNoTracking()
            .Select(t => new TermDto()
            {
                ExamsStartTime = t.ExamsStartTime,
                ExamsEndTime = t.ExamsEndTime,
                SelectionStartTime = t.SectionSelectionStartTime,
                SelectionEndTime = t.SectionSelectionEndTime,
                Year = t.Year,
                TermIdentifier = t.TermIdentifier
            })
            .ToListAsync();
    }

    public async Task<Result> AddTerm(Term term)
    {
        await context.Terms.AddAsync(term);
        var students = await context.Students.Include(s => s.Terms).ToListAsync();

        students.ForEach(student => {
            student.Terms.Add(term);
            context.Students.Update(student);
        });

        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Term added",
        };
    }

    public async Task<Result> EditTerm(Term term)
    {
        context.Terms.Update(term);
        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Term updated",
        };
    }

    public async Task<Result> CloseTerm(Term term)
    {
        context.Terms.Update(term);

        var sections = await context.Sections
            .Where(s => s.TermId == term.Id)
            .ToListAsync();

        context.Sections.RemoveRange(sections);
        context.Update(sections);

        var courses = await context.Courses
            .ToListAsync();

        foreach (var course in courses){
            course.ExamIsSet = false;
            context.Courses.Update(course);
        }

        await context.SaveChangesAsync();

        return new Result()
        {
            Succeeded = true,
            Message = "Term closed",
        };
    }

    public Task<bool> IsInsideAnyTermCurrently()
    {
        var currentDate = DateTime.UtcNow;

        return context.Terms
            .AsNoTracking()
            .AnyAsync(t => !t.IsClosed);
    }

}
