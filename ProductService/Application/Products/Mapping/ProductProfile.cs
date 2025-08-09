using Application.Products.Dtos;
using AutoMapper;
using Entities;
using ProductService.Application.Products.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProductService.Application.Products.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, CreateProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
        CreateMap<Product, UpdateProductDto>();
        CreateMap<UpdateProductDto, Product>();
    }
}
