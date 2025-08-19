using AutoMapper;
using DocumentManagement.Models.Documents;
using DocumentManagement.PublicModels.Documents;

namespace DocumentManagement.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DocumentDto, Document>();
        CreateMap<Document, DocumentDto>();
    }
}