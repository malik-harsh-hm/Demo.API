using Asp.Versioning;
using Demo.API.ModelBinders;
using Demo.API.Models;
using Demo.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[ApiVersion("1.0")]
    [Authorize(Roles = "MANAGER")]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        // GET api/employees
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var result = await _unitOfWork.EmployeeRepository.GetAllAsync();
            return Ok(result);
        }

        // GET api/employees/{employeeId}
        [HttpGet]
        [Route("{employeeId:int}")]
        [ProducesResponseType<EmployeeModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int employeeId)
        {
            if (employeeId <= 0)
            {
                return BadRequest();
            }
            var result = await _unitOfWork.EmployeeRepository.GetAsync(employeeId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET api/employees/search?employeeIds=1,2,3
        [HttpGet]
        [Route("search")]
        [ProducesResponseType<IEnumerable<EmployeeModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchEmployees([ModelBinder(typeof(CustomModelBinder))] int[] employeeIds)
        {
            if (employeeIds.Length == 0)
            {
                return BadRequest();
            }
            var result = await _unitOfWork.EmployeeRepository.GetEmployeesByIdsAsync(employeeIds);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST api/employees
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeModel employee)
        {
            // TODO: Model Validation

            await _unitOfWork.EmployeeRepository.AddAsync(employee);
            await _unitOfWork.SaveAsync();

            var resourceUri = $"/api/employees/{employee.Id}";
            return Created(resourceUri, employee);

        }

        // PUT api/employees/{employeeId}
        [HttpPut]
        [Route("{employeeId:int}")]
        [ProducesResponseType<EmployeeModel>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int employeeId, [FromBody] EmployeeModel employee)
        {
            // TODO: Model Validation

            await _unitOfWork.EmployeeRepository.UpdateEmployeeAsync(employeeId, employee);
            await _unitOfWork.SaveAsync();
            return Ok(employee);
        }


        // DELETE api/employees/{employeeId}
        [HttpDelete]
        [Route("{employeeId:int}")]
        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            if (employeeId <= 0)
            {
                return BadRequest();
            }
            var result = await _unitOfWork.EmployeeRepository.RemoveAsync(employeeId);
            await _unitOfWork.SaveAsync();

            if (result)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
