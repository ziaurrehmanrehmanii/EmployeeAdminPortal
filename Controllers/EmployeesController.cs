using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/Employees
        // Get all employees
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var allEmployees = dbContext.Employees.ToList();
            return Ok(allEmployees);
        }

        // GET: api/Employees/5
        // Get employee by id
        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        // POST: api/Employees
        // Add new employee
        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            // Check if the email is unique
            var existingEmail = dbContext.Employees.FirstOrDefault(e => e.Email == employee.Email);
            if (existingEmail != null)
            {
                return BadRequest("Email already exists");
            }
            // Generate a new Guid for the new employee
            employee.Id = Guid.NewGuid();
            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT: api/Employees/5
        // Update employee
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(Guid id, EmployeeUpdate employeeUpdate)
        {
            

            var existingEmployee = dbContext.Employees.Find(id);
            var existingEmail = dbContext.Employees.FirstOrDefault(e => e.Email == employeeUpdate.Email);
            // Check if Employe Exist
            if (existingEmployee == null)
            {
                return NotFound();
            }
            // Check if the email is unique
            if (existingEmail != null)
            {
                return BadRequest("Email already exists");
            }
            
            
            // Name Check
            if (employeeUpdate.Name != null)
            {
                existingEmployee.Name = employeeUpdate.Name;
            }
           
            // Email Check
            if (employeeUpdate.Email != null)
            {
                existingEmployee.Email = employeeUpdate.Email;
            }

            // Salary Check
            if(employeeUpdate.Salary != null)
            {
                existingEmployee.Salary = employeeUpdate.Salary;
            }

            // Phone Check
            if (employeeUpdate.Phone != null)
            {
                existingEmployee.Phone = employeeUpdate.Phone;
            }

            var employee = new Employee
            {
                Id = existingEmployee.Id,
                Name = existingEmployee.Name,
                Email = existingEmployee.Email,
                Salary = existingEmployee.Salary,
                Phone = existingEmployee.Phone
            };

            dbContext.Employees.Update(existingEmployee);
            dbContext.SaveChanges();
            return Ok(employee);
        }

        // DELETE: api/Employees/5
        // Delete employee
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();
            return Ok();
        }

        // DELETE: api/Employees/all
        // Delete all employees
        [HttpDelete("all")]
        public IActionResult DeleteAllEmployees()
        {
            dbContext.Employees.RemoveRange(dbContext.Employees);
            dbContext.SaveChanges();
            return Ok();
        }

        public class EmployeeUpdate
        {
            public string? Name { get; set; }
            public string? Email { get; set; }
            public decimal? Salary { get; set; }
            public string? Phone { get; set; }
        }
    }
}
