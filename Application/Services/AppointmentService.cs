using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task RegisterAppointment(Appointment appointment)
    {
        try
        {
            await _unitOfWork.AppointmentRepository.CreateAsycn(appointment);
            await _unitOfWork.CommitAssync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Appointment?> GetAppointmentById(Guid id)
    {
        try
        {
            return await _unitOfWork.AppointmentRepository.GetFullAppointment(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Appointment>?> GetAllFullAppointmentsBySchedule(Guid scheduleId)
    {
        try
        {
            return await _unitOfWork.AppointmentRepository.GetAllFullAppointments(scheduleId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<List<Appointment>?> GetAllAppointmentsPendingBySchedule(Guid scheduleId, DateOnly date)
    {
        try
        {
            return await _unitOfWork.AppointmentRepository.GetAllPendingAppointmentsBySchedule(scheduleId, date);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteAppointment(Appointment appointment)
    {
        try
        {
            await _unitOfWork.AppointmentRepository.DeleteAsync(appointment);
            await _unitOfWork.CommitAssync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Appointment> UpdateAppointment(Appointment appointment)
    {
        try
        {
            var appointmentUpdated = await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
            await _unitOfWork.CommitAssync();
            return appointmentUpdated;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}