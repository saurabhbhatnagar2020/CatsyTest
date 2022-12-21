using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatsyTest.Data;
using CatsyTest.Model;
using CatsyTest.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace CatsyTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

      //  private readonly CatsyTestContext _context;

        public CustomersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _unitOfWork.Customer.All();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var item = await _unitOfWork.Customer.GetById(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CustomerId = 0;

                await _unitOfWork.Customer.Upsert(customer);
                await _unitOfWork.CompleteAsync();
                return Ok(customer);
            }

            return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var item = await _unitOfWork.Customer.GetById(id);

            if (item == null)
                return BadRequest();

            await _unitOfWork.Customer.Delete(id);
            await _unitOfWork.CompleteAsync();

            return Ok(item);
        }


     
    }
}
