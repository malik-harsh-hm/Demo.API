namespace Demo.API.Repository
{
    public interface IUnitOfWork: IDisposable
    {
        IEmployeeRepository EmployeeRepository { get; } // we have only get because we don't want to set the repository. setting the repository will be done in the UnitOfWork class
        Task<int> SaveAsync(); // this method will save all the changes made to the database
    }
}
