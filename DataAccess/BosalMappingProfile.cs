using AutoMapper;
using DataAccess.EntityModels;
using Domain;

namespace DataAccess
{
    public class BosalMappingProfile : Profile
    {
        public BosalMappingProfile()
        {
            CreateMap<MontazaEntity, Montaza>();
        }
    }
}
