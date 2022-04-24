using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{

  // EF Core Features
  // - Owned Types
  // - Shadow Property
  // - Backing Field

  public class Order : Entity, IAggregateRoot
  {
    public DateTime CreateDate { get; private set; }
    public Address Address { get; private set; }
    public string BuyerId { get; private set; }

    private readonly List<OrderItem> _orderItems;

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public Order(Address address, string buyerId)
    {
      Address = address;
      BuyerId = buyerId;
      _orderItems = new List<OrderItem>();
      CreateDate = DateTime.UtcNow;
    }

    public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl)
    {
      var existProduct = _orderItems.Any(x => x.ProductId == productId);
      if (!existProduct)
      {
        var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
        _orderItems.Add(newOrderItem);
      }
    }

    public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);
  }
}
