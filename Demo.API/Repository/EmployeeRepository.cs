using Demo.API.Data;
using Demo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Repository
{
    public class EmployeeRepository : GenericRepository<EmployeeModel>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {  
        }
        public async Task<IEnumerable<EmployeeModel>> GetEmployeesByIdsAsync(int[] employeeIds)
        {
            var records = await _dbSet.Where(e => employeeIds.Contains(e.Id)).ToListAsync();
            return records;
        }
        public async Task<EmployeeModel> UpdateEmployeeAsync(int employeeId, EmployeeModel employee)
        {
            var record = await _dbSet.Where(e => e.Id == employeeId).FirstOrDefaultAsync();
            if (record != null)
            {
                record.Name = employee.Name;
                record.Age = employee.Age;

                await _context.SaveChangesAsync();
            }
            return employee;
        }
    }
}
