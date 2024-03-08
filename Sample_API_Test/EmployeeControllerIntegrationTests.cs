using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using Sample_API;
using Sample_API.Models;

namespace Sample_API.Tests
{
	public class EmployeeController_IntegrationTests
	{
		private WebApplicationFactory<Program> _factory;
		private HttpClient _client;

		[SetUp]
		public void Setup()
		{
			_factory = new WebApplicationFactory<Program>();
			_client = _factory.CreateClient();
		}

		[TearDown]
		public void TearDown()
		{
			_client.Dispose();
			_factory.Dispose();
		}

		[Test]
		public async Task CreateEmployee_ReturnsCreatedEmployee()
		{
			// Arrange
			var employee = new CreateEmployeeDTO
			{
				EmployeeID = 1,
				FirstName = "John",
				LastName = "Doe",
				Department = "IT",
				Position = "Developer",
				Salary = 50000,
				HireDate =DateTime.Now.Date,
				DepartmentID = 1,
				MobileNumber = 1234567890
			};

			// Act
			var response = await _client.PostAsJsonAsync("https://localhost:7215/CreateEmployee", employee);
			response.EnsureSuccessStatusCode();

			var createdEmployee = await response.Content.ReadFromJsonAsync<CreateEmployeeDTO>();

			// Assert
			Assert.NotNull(createdEmployee);
			Assert.AreEqual(employee.FirstName, createdEmployee.FirstName);
		}

		[Test]
		public async Task GetEmployees_ReturnsListOfEmployees()
		{
			// Act
			var response = await _client.GetAsync("https://localhost:7215/GetEmployees");
			response.EnsureSuccessStatusCode();

			var employees = await response.Content.ReadFromJsonAsync<IEnumerable<Employee>>();

			// Assert
			Assert.NotNull(employees);
		}

		[Test]
		public async Task GetEmployee_ReturnsEmployeeById()
		{
			// Act
			var response = await _client.GetAsync("https://localhost:7215/GetEmployee/2");
			response.EnsureSuccessStatusCode();

			var employee = await response.Content.ReadFromJsonAsync<Employee>();

			// Assert
			Assert.NotNull(employee);
			Assert.AreEqual(2, employee.EmployeeID);
		}

		[Test]
		public async Task UpdateEmployee_ReturnsNoContent()
		{
			// Arrange
			var updatedEmployee = new UpdateEmployeeDTO
			{
				FirstName = "UpdatedFirstName",
				LastName = "UpdatedLastName",
				Department = "UpdatedDepartment",
				Position = "UpdatedPosition",
				Salary = 60000,
				DepartmentID = 2,
				MobileNumber = 9876543210
			};

			// Act
			var response = await _client.PutAsJsonAsync("https://localhost:7215/UpdateEmployee/2", updatedEmployee);
			response.EnsureSuccessStatusCode();

			// Assert
			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Test]
		public async Task DeleteEmployee_ReturnsNoContent()
		{
			// Act
			var response = await _client.DeleteAsync("https://localhost:7215/DeleteEmployee/3");
			response.EnsureSuccessStatusCode();

			// Assert
			Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
		}
	}
}
