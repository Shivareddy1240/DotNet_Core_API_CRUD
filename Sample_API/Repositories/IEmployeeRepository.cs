using Sample_API.Models;

namespace Sample_API.Repositories
{
    public interface IEmployeeRepository
    {
        public Task<IEnumerable<Employee>> GetEmployees();
        public Task<Employee> GetEmployee(int employeeId);
        public Task<CreateEmployeeDTO> CreateEmployee(CreateEmployeeDTO company);
        public Task UpdateEmployee(int id, UpdateEmployeeDTO company);
        public Task DeleteEmployee(int id);
    }
}
