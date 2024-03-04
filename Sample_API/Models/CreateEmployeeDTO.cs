using System.ComponentModel.DataAnnotations;

namespace Sample_API.Models
{
    public class CreateEmployeeDTO
    {
        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Department { get; set; }

        public string Position { get; set; }

        [Range(0, 99999999.99)]
        public decimal? Salary { get; set; }

        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        public int? DepartmentID { get; set; }
        public long MobileNumber { get; set; }
    }
}
