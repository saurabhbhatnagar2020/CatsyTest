using CatsyTest.Configuration;
using CatsyTest.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatsyTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        //  private readonly CatsyTestContext _context;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _unitOfWork.Orders.All();
            foreach(var order in orders)
            {
                var orderdetails = await _unitOfWork.OrderDetails.GetOrderDetails(order.OrderId);
                if (orderdetails != null)
                {
                    foreach(var item in orderdetails)
                    {
                        order.OrderDetails.Add(item);
                    }
                }
            }
            return Ok(orders);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var item = await _unitOfWork.Orders.GetById(id);

            if (item == null)
                return NotFound();


             var orderdetails = await _unitOfWork.OrderDetails.GetOrderDetails(id);
            if (orderdetails != null)
            {
                foreach (var ord in orderdetails)
                {
                    item.OrderDetails.Add(ord);
                }
            }

            return Ok(item);
        }
        [HttpPost("CreateNewOrder")]
        public async Task<IActionResult> CreateNewOrder(OrderViewModel order)
        {
            Order orderData = new Order();
            orderData.OrderDate = order.OrderDate;
            orderData.CustomerId = order.CustomerId;
            orderData.OrderId = order.OrderId;
            if (ModelState.IsValid)
            {
                order.OrderId = 0;
                await _unitOfWork.Orders.Add(orderData);
                await _unitOfWork.CompleteAsync();

                // add new order details here 
                if (order.OrderData != null && order.OrderData.Count > 0)
                {
                    foreach (var ordDet in order.OrderData)
                    {
                        OrderDetails orderdet = new OrderDetails();
                        orderdet.ProductId = ordDet.ProductId;
                        orderdet.Quantity = ordDet.Quantity;
                        orderdet.OrderId = orderData.OrderId;
                        orderdet.Price = ordDet.Price;
                        await _unitOfWork.OrderDetails.Add(orderdet);
                        await _unitOfWork.CompleteAsync();
                    }
                }



                return Ok(order);

            }
            return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
        }


        //[HttpPost("AddItemsToOrder")]
        //public async Task<IActionResult> AddItemsToOrder(OrderDetailsViewModel orderdetails)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        orderData.OrderDetailsId = 0;

        //        await _unitOfWork.OrderDetails.Add(orderData);
        //        await _unitOfWork.CompleteAsync();
        //        return CreatedAtAction("GetOrder", new { orderData.OrderId }, orderdetails);
        //    }
        //    return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
        //}

        [HttpGet("GetOrderDetailsByCustomerId/{CustomerId}")]
        public async Task<IActionResult> GetOrderDetailsByCustomerId(int CustomerId)
        {
            var item = await _unitOfWork.Orders.GetOrderDetailsByCustomerId(CustomerId);

            if (item == null)
                return NotFound();

            foreach (var order in item)
            {
                var orderdetails = await _unitOfWork.OrderDetails.GetOrderDetails(order.OrderId);
                if (orderdetails != null)
                {
                    foreach (var ord in orderdetails)
                    {
                        order.OrderDetails.Add(ord);
                    }
                }
            }

        

            return Ok(item);
        }
    }
}
