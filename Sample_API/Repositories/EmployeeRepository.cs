using Dapper;
using System.Data;
using Sample_API.DBContext;
using Sample_API.Models;

namespace Sample_API.Repositories;

public class EmployeeRepository:IEmployeeRepository
{
    private readonly ApplicationDbContext _context;
    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateEmployeeDTO> CreateEmployee(CreateEmployeeDTO employee)
    {
        var query = "INSERT INTO Employee (EmployeeID, FirstName, LastName,Department,Position,Salary,HireDate,DepartmentID,MobileNumber) VALUES (@EmployeeID, @FirstName, @LastName,@Department,@Position,@Salary,@HireDate,@DepartmentID,@MobileNumber)";
        var parameters = new DynamicParameters();
        parameters.Add("EmployeeID", employee.EmployeeID, DbType.Int32);
        parameters.Add("FirstName", employee.FirstName, DbType.String);
        parameters.Add("LastName", employee.LastName, DbType.String);
        parameters.Add("Department", employee.Department, DbType.String);
        parameters.Add("Position", employee.Position, DbType.String);
        parameters.Add("Salary", employee.Salary, DbType.Decimal);
        parameters.Add("HireDate", employee.HireDate, DbType.DateTime);
        parameters.Add("DepartmentID", employee.DepartmentID, DbType.Int32);
        parameters.Add("MobileNumber", employee.MobileNumber, DbType.Int64);
        using var connection = _context.CreateConnection();
        var id = await connection.QuerySingleAsync<int>(query, parameters);
        var createdEmployee = new CreateEmployeeDTO()
        {
            EmployeeID =  employee.EmployeeID,
            FirstName= employee.FirstName,
            LastName= employee.LastName,
            Department=employee.Department, 
            Position=employee.Position,
            Salary=employee.Salary,
            HireDate=employee.HireDate, 
            DepartmentID=employee.DepartmentID,
            MobileNumber=employee.MobileNumber, 
        };
        return createdEmployee;
    }

    public async Task<IEnumerable<Employee>> GetEmployees()
    {
        var query = "SELECT * FROM Employee";
        using var connection = _context.CreateConnection();
        var companies = await connection.QueryAsync<Employee>(query);
        return companies.ToList();
    }
    public async Task<Employee> GetEmployee(int employeeId)
    {
        var query = "SELECT * FROM Employee WHERE EmployeeID = @employeeId";
        using var connection = _context.CreateConnection();
        var employee = await connection.QuerySingleOrDefaultAsync<Employee>(query, new { employeeId });
        return employee;
    }
    public async Task UpdateEmployee(int id, UpdateEmployeeDTO employee)
    {
        var query = "UPDATE Employee SET FirstName = @FirstName, LastName = @LastName, Department = @Department,Position = @Position,Salary = @Salary,DepartmentID = @DepartmentID,MobileNumber=@MobileNumber  WHERE EmployeeID = @Id";
        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);
        parameters.Add("FirstName", employee.FirstName, DbType.String);
        parameters.Add("LastName", employee.LastName, DbType.String);
        parameters.Add("Department", employee.Department, DbType.String);
        parameters.Add("Position", employee.Position, DbType.String);
        parameters.Add("Salary", employee.Salary, DbType.Decimal);
        parameters.Add("DepartmentID", employee.DepartmentID, DbType.Int32);
        parameters.Add("MobileNumber", employee.MobileNumber, DbType.Int64);
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, parameters);
    }
    public async Task DeleteEmployee(int id)
    {
        var query = "DELETE FROM Employee WHERE EmployeeID = @Id";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new { id });
    }
}