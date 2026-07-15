using Membership.Entities.QuocDT.Models;
using Membership.Services.QuocDT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Membership.APIWebApp.QuocDT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CustomerMembershipsQuocDtController : ControllerBase
    {
        private readonly ICustomerMembershipsService _service;

        public CustomerMembershipsQuocDtController(ICustomerMembershipsService service)
        {
            _service = service;
        }

        // GET: api/<CustomerMembershipsQuocDtController>
        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "1,2")]
        public IQueryable<CustomerMembershipsQuocDt> Get()
        {
            return _service.GetQueryable();
        }

        // GET api/<CustomerMembershipsQuocDtController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<CustomerMembershipsQuocDt>> Get(Guid id)
        {
            try
            {
                var membership = await _service.GetByIdAsync(id);
                if (membership == null)
                {
                    return NotFound();
                }
                return membership;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                // Return an appropriate error response
                return StatusCode(500, "Internal server error");
            }

            //return "value";
        }

        // POST api/<CustomerMembershipsQuocDtController>
        [HttpPost]
        [Authorize(Roles = "1")]
        //public void Post([FromBody] string value)
        //{
        //}
        public async Task<ActionResult> Post([FromBody] CustomerMembershipsQuocDt customer)
        {
            try
            {
                if (customer == null)
                {
                    return BadRequest("Invalid payload.");
                }

                if (customer.TierId <= 0)
                {
                    return BadRequest("TierId is required.");
                }

                if (customer.MembershipIdquocDt == Guid.Empty)
                {
                    customer.MembershipIdquocDt = Guid.NewGuid();
                }

                customer.Tier = null;

                var result = await _service.CreateAsync(customer);
                if (result > 0)
                {
                    return CreatedAtAction(nameof(Get), new { id = customer.MembershipIdquocDt }, customer);
                }
                else
                {
                    return BadRequest("Failed to create the customer membership.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                // Return an appropriate error response
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<CustomerMembershipsQuocDtController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}
        public async Task<ActionResult> Put(Guid id, [FromBody] CustomerMembershipsQuocDt customer)
        {
            if (id != customer.MembershipIdquocDt)
            {
                return BadRequest("ID mismatch.");
            }
            try
            {
                var result = await _service.UpdateAsync(customer);
                if (result > 0)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound("Customer membership not found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                // Return an appropriate error response
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/<CustomerMembershipsQuocDtController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        //public void Delete(int id)
        //{
        //}
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (result > 0)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound("Customer membership not found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                // Return an appropriate error response
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CustomerMembershipsQuocDt>>> Search(
            [FromQuery] string? customerName, 
            [FromQuery] int? currentPointsBalance, 
            [FromQuery] string? tierName)
        {
            try
            {
                var memberships = await _service.SearchAsync(customerName, currentPointsBalance, tierName);
                var paginatedList = memberships
                    .ToList();
                return Ok(paginatedList);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
