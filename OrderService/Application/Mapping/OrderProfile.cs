using AutoMapper;
using OrderService.Application.Commands;
using OrderService.Application.Dtos;
using OrderService.Application.DTOs;
using OrderService.Domain.Entities;
using Microshop.Contracts.Events;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrderService.Application.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<OrderItemDto, OrderItem>();
        CreateMap<OrderDto, Order>();
        CreateMap<Order, OrderCreatedEvent>()
    .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id));

        CreateMap<OrderItem, OrderItemEvent>();
        CreateMap<CreateOrderCommand, Order>();

    }
}
