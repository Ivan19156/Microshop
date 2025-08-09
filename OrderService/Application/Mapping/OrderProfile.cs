using AutoMapper;
using OrderService.Application.Dtos;
using OrderService.Application.DTOs;
using OrderService.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrderService.Application.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<OrderDto, Order>();
        }
    }
}
