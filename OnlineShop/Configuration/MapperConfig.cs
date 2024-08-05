using AutoMapper;
using OnlineShop.DataModels;
using OnlineShop.DTOs;
using Microsoft.EntityFrameworkCore.Query;
using OnlineShop.DTOs.Roles;
using OnlineShop.DTOs.Category;
using OnlineShop.DTOs.Product;

namespace OnlineShop.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {



            //For roles
            CreateMap<Role, CreateRoleDto>().ReverseMap();
            CreateMap<Role, GetRoleDto>().ReverseMap();
            CreateMap<Role, UpdateRoleDto>().ReverseMap();
            CreateMap<Role, DeleteRoleDto>().ReverseMap();
            
            //For Categories
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, DeleteCategoryDto>().ReverseMap();
            
            //For products
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, GetProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, DeleteProductDto>().ReverseMap();




            // Mappings to handle cyclical references
            CreateMap<Category, GetCategoryDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
            CreateMap<Product, GetCategoryDto>();

            //CreateMap<Category, GetCategoryProductsDto>()
            //    .AfterMap((src, dest) =>
            //    {
            //        dest.Products.ForEach(x => x.Price -= src.CategoryDiscount);
            //    });

            //CreateMap<Product, ProductDto>()
            //    .ForMember(
            //        dest => dest.Price,
            //        opt => opt.MapFrom((src, dest, member, context) => src.Price - src.Category.CategoryDiscount));


            //CreateMap<Category, GetCategoryProductsDto>()
            //    .ConvertUsing<CategoryProductsMapper>();
            //CreateMap<Category, List<ProductDto>>().ReverseMap();        

            //CreateMap<Product, ProductDto>()
            //    .ForMember(dest => dest.Price, source => source.MapFrom<GetProductDiscount>())
            //    .ForMember(dest => dest.CategoryName, source => source.MapFrom(s => s.Category.CategoryName));


        }
    }
}
