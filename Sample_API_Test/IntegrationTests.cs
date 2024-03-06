using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Sample_API.Controllers;
using Sample_API.Models;
using Sample_API.Repositories;

namespace Sample_API.Tests
{
	[TestFixture]
	public class EmployeeControllerTests
	{
		private Mock<IEmployeeRepository> _mockRepo;
		private EmployeeController _controller;

		[SetUp]
		public void Setup()
		{
			_mockRepo = new Mock<IEmployeeRepository>();
			_controller = new EmployeeController(_mockRepo.Object);
		}

		// Other test cases for CreateEmployee and GetEmployees are omitted for brevity

		[Test]
		public async Task GetEmployee_ExistingId_ReturnsOkResult()
		{
			// Arrange
			int id = 1;
			var employee = new Employee { EmployeeID = id, /* initialize other properties */ };
			_mockRepo.Setup(repo => repo.GetEmployee(id)).ReturnsAsync(employee);

			// Act
			var result = await _controller.GetEmployee(id);

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(result);
			var okResult = (OkObjectResult)result;
			Assert.AreEqual(employee, okResult.Value);
		}

		[Test]
		public async Task GetEmployee_NonExistingId_ReturnsNotFoundResult()
		{
			// Arrange
			int id = 1;
			_mockRepo.Setup(repo => repo.GetEmployee(id)).ReturnsAsync((Employee)null);

			// Act
			var result = await _controller.GetEmployee(id);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[Test]
		public async Task UpdateEmployee_ExistingId_ReturnsNoContentResult()
		{
			// Arrange
			int id = 1;
			var employee = new UpdateEmployeeDTO { /* initialize employee properties */ };
			_mockRepo.Setup(repo => repo.GetEmployee(id)).ReturnsAsync(new Employee());
			_mockRepo.Setup(repo => repo.UpdateEmployee(id, employee)).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.UpdateEmployee(id, employee);

			// Assert
			Assert.IsInstanceOf<NoContentResult>(result);
		}

		[Test]
		public async Task UpdateEmployee_NonExistingId_ReturnsNotFoundResult()
		{
			// Arrange
			int id = 1;
			var employee = new UpdateEmployeeDTO { /* initialize employee properties */ };
			_mockRepo.Setup(repo => repo.GetEmployee(id)).ReturnsAsync((Employee)null);

			// Act
			var result = await _controller.UpdateEmployee(id, employee);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		[Test]
		public async Task DeleteEmployee_ExistingId_ReturnsNoContentResult()
		{
			// Arrange
			int id = 1;
			_mockRepo.Setup(repo => repo.GetEmployee(id)).ReturnsAsync(new Employee());
			_mockRepo.Setup(repo => repo.DeleteEmployee(id)).Returns(Task.CompletedTask);

			// Act
			var result = await _controller.DeleteEmployee(id);

			// Assert
			Assert.IsInstanceOf<NoContentResult>(result);
		}

		[Test]
		public async Task DeleteEmployee_NonExistingId_ReturnsNotFoundResult()
		{
			// Arrange
			int id = 1;
			_mockRepo.Setup(repo => repo.GetEmployee(id)).ReturnsAsync((Employee)null);

			// Act
			var result = await _controller.DeleteEmployee(id);

			// Assert
			Assert.IsInstanceOf<NotFoundResult>(result);
		}
	}
}
