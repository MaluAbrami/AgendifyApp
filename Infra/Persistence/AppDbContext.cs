using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Persistence;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<Schedule> Schedules { get; set; } = null!;
    public DbSet<ScheduleRule> ScheduleRules { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);  
        
        // Usuário -> Empresas (1:N) com DeleteBehavior.Cascade
        builder.Entity<Company>()
            .HasOne(c => c.Owner)
            .WithMany(u => u.Companies)
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Empresa -> Agendas (1:N) com DeleteBehavior.Cascade
        builder.Entity<Schedule>()
            .HasOne(s => s.Company)
            .WithMany(c => c.Schedules)
            .HasForeignKey(s => s.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Empresa -> Serviços (1:N) exemplo com Cascade
        builder.Entity<Service>()
            .HasOne(s => s.Company)
            .WithMany(c => c.Services)
            .HasForeignKey(s => s.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Agenda -> Regras (1:N) com DeleteBehavior.Cascade
        builder.Entity<ScheduleRule>()
            .HasOne(r => r.Schedule)
            .WithMany(s => s.Rules)
            .HasForeignKey(r => r.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Usuário -> Agendamentos (1:N) com DeleteBehavior.SetNull
        builder.Entity<Appointment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Service -> Agendamentos (1:N) com DeleteBehavior.SetNull
        builder.Entity<Appointment>()
            .HasOne(a => a.Service)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.NoAction);
        
        // Agenda -> Agendamentos (1:N) com DeleteBehavior.Cascade
        builder.Entity<Appointment>()
            .HasOne(a => a.Schedule)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
