using OrderService.Application.Dtos;
using OrderService.Domain.Entities;

namespace OrderService.Application.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }

    //public OrderDto(Order order)
    //{
    //    Id = order.Id;
    //    UserId = order.UserId;
    //    CreatedAt = order.CreatedAt;
    //    Status = order.Status.ToString();
    //    Items = order.Items.Select(i => new OrderItemDto(i)).ToList();
    //    TotalPrice = order.TotalPrice;
    //}
}