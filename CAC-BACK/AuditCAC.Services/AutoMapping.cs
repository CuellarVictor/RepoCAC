using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.BolsasMedicion;
using AuditCAC.Domain.Dto.Catalogo;
using AuditCAC.Domain.Dto.PanelEnfermedad;
using AuditCAC.Domain.Entities;
using AutoMapper;

namespace AuditCAC.Services
{
    public class AutoMapping : Profile
    {
        #region Constructor
        public AutoMapping()
        {
            CreateMap<CatalogoCoberturaModel, CatalogoCoberturaDto>();
            CreateMap<CatalogoCoberturaDto, CatalogoCoberturaModel>();
            CreateMap<CatalogoDto, CatalogModel>();
            CreateMap<CatalogModel, CatalogoDto>();
            CreateMap<ItemDto, ItemModel>();
            CreateMap<ItemModel, ItemDto>();
            CreateMap<BolsaMedicionNuevaDto, MedicionModel>();
            CreateMap<MedicionModel, BolsaMedicionNuevaDto>();
            CreateMap<MedicionModel, MedicionesDto>();
            CreateMap<MedicionesDto, MedicionModel>();
            CreateMap<CatalogoCoberturaModel, InputsCatalogoCoberturaDto>();
            CreateMap<InputsCatalogoCoberturaDto, CatalogoCoberturaModel>();
        }
        #endregion
    }
}
