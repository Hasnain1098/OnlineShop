using AutoMapper;
using OnlineShop.DataModels;
using OnlineShop.DTOs;
using Microsoft.EntityFrameworkCore.Query;
using OnlineShop.DTOs.Roles;

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
