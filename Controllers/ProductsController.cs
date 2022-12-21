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
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

      //  private readonly CatsyTestContext _context;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _unitOfWork.Product.All();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var item = await _unitOfWork.Product.GetById(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                //product.ProductId =0;

                await _unitOfWork.Product.Upsert(product);
                await _unitOfWork.CompleteAsync();
                return Ok(product);
            }

            return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var item = await _unitOfWork.Product.GetById(id);

            if (item == null)
                return BadRequest();

            await _unitOfWork.Product.Delete(id);
            await _unitOfWork.CompleteAsync();

            return Ok(item);
        }


     
    }
}
