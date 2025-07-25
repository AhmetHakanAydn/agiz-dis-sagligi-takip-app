using DentalHealthApp.Core.DTOs;
using DentalHealthApp.Core.Entities;
using DentalHealthApp.Core.Interfaces;
using DentalHealthApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthApp.Business.Services;

public class NoteService : INoteService
{
    private readonly DentalHealthDbContext _context;

    public NoteService(DentalHealthDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NoteDto>> GetUserNotesAsync(int userId)
    {
        var notes = await _context.Notes
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.NoteDate)
            .ThenByDescending(n => n.CreatedAt)
            .ToListAsync();

        return notes.Select(n => new NoteDto
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            NoteDate = n.NoteDate,
            CreatedAt = n.CreatedAt
        });
    }

    public async Task<IEnumerable<NoteDto>> GetUserNotesForPeriodAsync(int userId, DateTime startDate, DateTime endDate)
    {
        var notes = await _context.Notes
            .Where(n => n.UserId == userId && n.NoteDate >= startDate && n.NoteDate <= endDate)
            .OrderByDescending(n => n.NoteDate)
            .ThenByDescending(n => n.CreatedAt)
            .ToListAsync();

        return notes.Select(n => new NoteDto
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            NoteDate = n.NoteDate,
            CreatedAt = n.CreatedAt
        });
    }

    public async Task<NoteDto?> CreateNoteAsync(int userId, NoteCreateDto noteDto)
    {
        var note = new Note
        {
            UserId = userId,
            Title = noteDto.Title,
            Content = noteDto.Content,
            NoteDate = noteDto.NoteDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            NoteDate = note.NoteDate,
            CreatedAt = note.CreatedAt
        };
    }

    public async Task<bool> DeleteNoteAsync(int noteId, int userId)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == noteId && n.UserId == userId);

        if (note == null)
            return false;

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        return true;
    }
}