using Domain.Entities;
using Domain.Interfaces;
using Infra.Persistence;
using Infra.UnitOfWork.Repositories;

namespace Infra.Repositories;

public class AppointmentRepository(AppDbContext context) : BaseRespository<Appointment>(context), IAppointmentRepository
{
    
}