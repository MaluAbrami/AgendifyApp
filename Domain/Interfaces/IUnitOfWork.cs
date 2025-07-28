namespace Domain.Interfaces;

public interface IUnitOfWork
{
    public void Commit();
    public ICompanyRepository CompanyRepository { get; }
}