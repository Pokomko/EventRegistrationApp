using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly AppDbContext _context;

    public ParticipantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateParticipantAsync(Participant participantDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Participant?> GetParticipantByIdAsync(Guid id)
    {
        var participant =  await _context.Participants
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        return participant;
    }

    public async Task<Participant?> GetParticipantByUserIdAsync(Guid userId)
    {
        var participant = await _context.Participants
            .Where(p => p.UserId == userId)
            .FirstOrDefaultAsync();

        return participant;
    }

    public async Task UpdateParticipantAsync(Participant participant)
    {
        try
        {
            _context.Participants.Update(participant);
            await _context.SaveChangesAsync();
        }
        catch {
            throw;
        }
    }
}
