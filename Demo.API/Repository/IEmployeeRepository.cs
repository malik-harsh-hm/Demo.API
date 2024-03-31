using Demo.API.Models;

namespace Demo.API.Repository
{
    public interface IEmployeeRepository: IGenericRepository<EmployeeModel>
    {
        Task<IEnumerable<EmployeeModel>> GetEmployeesByIdsAsync(int[] employeeIds);
        Task<EmployeeModel> UpdateEmployeeAsync(int employeeId, EmployeeModel employee);
    }
}
