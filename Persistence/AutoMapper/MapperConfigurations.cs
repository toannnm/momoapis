using Application.Extensions;
using Application.Models.DocumentModels;
using Application.Models.MomoModels;
using Application.Models.OrderModels;
using Application.Models.UserModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Newtonsoft.Json;

namespace Persistence.AutoMapper
{
    public class MapperConfigurations : Profile
    {
        public MapperConfigurations()
        {
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));

            #region User Mapper
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<User, RegisterModel>().ReverseMap();
            #endregion


            #region Document Mapper
            CreateMap<Document, CreateDocumentModel>()
                .ReverseMap();
            CreateMap<Document, UpdateDocumentModel>()
                .ReverseMap();

            CreateMap<Document, DocumentModel>()
                .ForMember(d => d.Images, s => s.MapFrom(x => JsonConvert.SerializeObject(x.Images)))
                .ReverseMap()
                .ForMember(d => d.Images, x => x.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.Images)));
            #endregion


            #region Order Mapper
            CreateMap<OrderModel, Order>().ReverseMap();
            CreateMap<OrderModels, Order>().ReverseMap();

            CreateMap<CreateOrderModel, Order>()
                .ForMember(d => d.OrderStatus, s => s.MapFrom(x => OrderStatusEnum.Processing)).ReverseMap()
                .ReverseMap();

            CreateMap<OrderDetails, OrderDetail>().ReverseMap();


            CreateMap<CreateOrderDetail, OrderDetail>().ReverseMap();
            CreateMap<CreateOrderModel, Order>().ReverseMap();
            #endregion Momo Mapper

            #region Payment Mapper

            CreateMap<QueryTransactionResponse, QueryTransactionModel>()
                .ReverseMap();
            CreateMap<RefundResponse, RefundModel>()
                .ForMember(d => d.transId, s => s.MapFrom(x => x.transId))
                .ForMember(d => d.description, s => s.MapFrom(x => x.RefundReason))
                .ForMember(d => d.message, s => s.MapFrom(x => x.message))
                .ReverseMap();
            #endregion

            CreateMap<ExcelDocumentModel, Document>().ReverseMap();

        }
    }
}
