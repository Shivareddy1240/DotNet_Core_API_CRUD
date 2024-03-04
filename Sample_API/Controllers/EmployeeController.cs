using Microsoft.AspNetCore.Mvc;
using Sample_API.Models;
using Sample_API.Repositories;

namespace Sample_API.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }
        [HttpPost(nameof(CreateEmployee), Name = "CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody]CreateEmployeeDTO employee)
        {
            try
            {
                var createdEmployee = await _employeeRepo.CreateEmployee(employee);
                return CreatedAtRoute("CreateEmployee", new { id = createdEmployee.EmployeeID }, createdEmployee);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet(nameof(GetEmployees), Name = "GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var companies = await _employeeRepo.GetEmployees();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetEmployee/{id}", Name = "EmployeeById")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepo.GetEmployee(id);
                if (employee == null)
                    return NotFound();
                return Ok(employee);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("UpdateEmployee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id,[FromBody] UpdateEmployeeDTO employee)
        {
            try
            {
                var employeeDetails = await _employeeRepo.GetEmployee(id);
                if (employeeDetails == null)
                    return NotFound();
                await _employeeRepo.UpdateEmployee(id, employee);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepo.GetEmployee(id);
                if (employee == null)
                    return NotFound();
                await _employeeRepo.DeleteEmployee(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
