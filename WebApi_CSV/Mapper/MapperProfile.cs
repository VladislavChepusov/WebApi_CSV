using AutoMapper;
using WebApi_CSV.Models;

namespace WebApi_CSV.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // ЧТО ВО ЧТО МАПИТСЯ
            CreateMap<ResultModel, DAL.Entities.Results>()
                .ForMember(d => d.AllTime, m => m.MapFrom(s => s.AllTime.Ticks));


            CreateMap<ValueModel, DAL.Entities.Values>()
                  .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()));


            CreateMap<DAL.Entities.Values, ResponseValuesModel>();
                
        }
    }
}
