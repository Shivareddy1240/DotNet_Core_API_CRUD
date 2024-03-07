using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sample_API.Controllers;
using Sample_API.Models;
using Sample_API.Repositories;
using Sample_API.DBContext;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Sample_API.Tests
{
	public class EmployeeControllerIntegrationTests
	{
		private readonly HttpClient _client;
		private readonly IEmployeeRepository _employeeRepository;
		private int _employeeId;

		public EmployeeControllerIntegrationTests()
		{
			_client = new HttpClient { BaseAddress = new Uri("http://localhost:1111/") };

			var configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(new Dictionary<string, string>
				{
			{ "ConnectionStrings:SqlConnection", "server=(localdb)\\MSSQLLocalDB;database=testdb;Integrated Security=true;encrypt=false" }
				})
				.Build();

			var dbContext = new ApplicationDbContext(configuration);

			_employeeRepository = new EmployeeRepository(dbContext);
		}

		[SetUp]
		public async Task Setup()
		{
			var employees = await _employeeRepository.GetEmployees();
			if (!employees.Any())
			{
				var createEmployeeDTO = new CreateEmployeeDTO
				{
					EmployeeID = 1000,
					FirstName = "John",
					LastName = "Doe",
					Department = "IT",
					Position = "Developer",
					Salary = 50000,
					HireDate = DateTime.Now,
					DepartmentID = 1,
					MobileNumber = 1234567890
				};

				var createResponse = await _employeeRepository.CreateEmployee(createEmployeeDTO);
				_employeeId = createResponse.EmployeeID;
			}
			else
			{
				_employeeId = employees.Max(e => e.EmployeeID);
			}
		}



		[Test]
		public async Task TestGetEmployees()
		{
			// Arrange
			var controller = new EmployeeController(_employeeRepository);

			// Act
			var getResponse = await controller.GetEmployees();
			var okResult = getResponse as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);

			var employees = okResult.Value as IEnumerable<Employee>;
			Assert.NotNull(employees);

			// Add more specific assertions based on your data
			Assert.IsTrue(employees.Any()); // Check if there are any employees returned
			var employee = employees.FirstOrDefault();
			Assert.NotNull(employee);
			Assert.AreEqual("John", employee.FirstName);
			// Add more assertions for other properties if needed
		}


		[Test]
		public async Task TestInsertEmployee()
		{
			// Arrange
			var controller = new EmployeeController(_employeeRepository);

			// Get the maximum existing EmployeeID from the database
			var employees = await _employeeRepository.GetEmployees();
			var maxEmployeeId = employees.Select(e => e.EmployeeID).DefaultIfEmpty(0).Max();

			// Create a new employee with a unique EmployeeID
			var createEmployeeDTO = new CreateEmployeeDTO
			{
				EmployeeID = maxEmployeeId + 1, // Increment the maxEmployeeId to ensure uniqueness
				FirstName = "Jane",
				LastName = "Smith",
				Department = "HR",
				Position = "Manager",
				Salary = 60000,
				HireDate = DateTime.Now,
				DepartmentID = 2,
				MobileNumber = 9876543210
			};

			// Act
			var result = await controller.CreateEmployee(createEmployeeDTO);

			var okResult = result as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.AreEqual(200, okResult.StatusCode);

			var createdEmployee = okResult.Value as CreateEmployeeDTO;
			Assert.NotNull(createdEmployee);

			Assert.AreEqual(createdEmployee, createEmployeeDTO);
		}






		[Test]
		public async Task TestUpdateEmployee()
		{
			// Arrange
			var controller = new EmployeeController(_employeeRepository);
			var updatedEmployeeDTO = new UpdateEmployeeDTO
			{
				FirstName = "JohnUpdated",
				LastName = "DoeUpdated",
				Department = "ITUpdated",
				Position = "DeveloperUpdated",
				Salary = 70000,
				DepartmentID = 3,
				MobileNumber = 1234567891
			};

			// Act
			var updateResponse = await controller.UpdateEmployee(_employeeId, updatedEmployeeDTO);

			// Assert
			Assert.AreEqual(204, (updateResponse as Microsoft.AspNetCore.Mvc.StatusCodeResult)?.StatusCode);
		}

		[Test]
		public async Task TestDeleteEmployee()
		{
			// Arrange
			var controller = new EmployeeController(_employeeRepository);

			// Act
			var deleteResponse = await controller.DeleteEmployee(_employeeId);

			// Assert
			Assert.AreEqual(204, (deleteResponse as Microsoft.AspNetCore.Mvc.StatusCodeResult)?.StatusCode);
		}
	}
}
