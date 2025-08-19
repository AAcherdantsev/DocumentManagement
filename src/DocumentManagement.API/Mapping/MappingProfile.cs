using AutoMapper;
using DocumentManagement.Models.Documents;
using DocumentManagement.PublicModels.Documents;

namespace DocumentManagement.API.Mapping;

/// <summary>
/// MappingProfile is a configuration class for AutoMapper that defines the object mappings
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<DocumentDto, Document>();
        CreateMap<Document, DocumentDto>();
    }
}