using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microshop.Contracts.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public List<OrderItemEvent> Items { get; set; } = new();

    public OrderCreatedEvent() { }

    public OrderCreatedEvent(Guid orderId, Guid userId, string customerEmail, string customerPhone, List<OrderItemEvent> items)
    {
        OrderId = orderId;
        UserId = userId;
        CustomerEmail = customerEmail;
        CustomerPhone = customerPhone;
        Items = items;
    }
}

public class OrderItemEvent
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public OrderItemEvent() { }

    public OrderItemEvent(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}