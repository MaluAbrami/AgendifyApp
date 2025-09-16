using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class OpeningHoursService : IOpeningHoursService
{
    public List<TimeOnly> GetAvailableSlots(ScheduleRule rule, int durationTimeService, List<Appointment> appointments)
    {
        var availableSlots = new List<TimeOnly>();
        if (durationTimeService <= 0)
            throw new ArgumentException("A duração do serviço não pode ser zero ou negativa.");
        
        // Cria blocos de trabalho dividindo o almoço (se existir)
        var workBlocks = new List<(TimeOnly Start, TimeOnly End)>();

        if (rule.StartLunchTime.HasValue && rule.EndLunchTime.HasValue)
        {
            workBlocks.Add((rule.StartTime, rule.StartLunchTime.Value));
            workBlocks.Add((rule.EndLunchTime.Value, rule.EndTime)); 
        }
        else
        {
            workBlocks.Add((rule.StartTime, rule.EndTime));
        }

        foreach (var block in workBlocks)
        {
            var current = block.Start;

            var blockAppointments = appointments
                .Where(a => a.StartTime < block.End && a.EndTime > block.Start)
                .OrderBy(a => a.StartTime)
                .ToList();

            foreach (var appt in blockAppointments)
            {
                if (appt.StartTime > current)
                {
                    AddSlotsBetween(current, appt.StartTime, durationTimeService, availableSlots);
                }

                if (appt.EndTime > current)
                    current = appt.EndTime;
            }

            if (current < block.End)
            {
                AddSlotsBetween(current, block.End, durationTimeService, availableSlots);
            }
        }
        
        return availableSlots;
    }

    private void AddSlotsBetween(TimeOnly start, TimeOnly end, int duration, List<TimeOnly> slots)
    {
        if (duration <= 0)
            throw new ArgumentException("A duração do serviço deve ser maior que 0 minutos");

        var current = start;

        while (current.AddMinutes(duration) <= end)
        {
            slots.Add(current);
            current = current.AddMinutes(duration); 
        }
    }
}