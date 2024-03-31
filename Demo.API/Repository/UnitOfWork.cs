
using Demo.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IEmployeeRepository EmployeeRepository {  get; private set; }
        public UnitOfWork(AppDbContext context, IEmployeeRepository employeeRepository)
        {
            _context = context;
            EmployeeRepository = employeeRepository;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
