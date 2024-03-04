using System.ComponentModel.DataAnnotations;

namespace Sample_API.Models
{

    public class Employee
    {
        public int EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Department { get; set; }

        public string Position { get; set; }

        public decimal? Salary { get; set; }

        public DateTime? HireDate { get; set; }

        public int? DepartmentID { get; set; }
        public long MobileNumber { get; set; }
    }
}
