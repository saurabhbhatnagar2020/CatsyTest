using System.ComponentModel.DataAnnotations.Schema;

namespace CatsyTest.Model
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public List<OrderDetailsViewModel>? OrderData { get; set; }
        public DateTime OrderDate { get; set; }
       
    }

    public class OrderDetailsViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate{ get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderDetails>? OrderDetails { get; set; }
    }
    public class OrderDetails
    {
        public int OrderDetailsId { get; set; }


        [ForeignKey("Order")]
        public int OrderId { get; set; }


        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
