using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GCPack.Model;
using GCPack.Repository;

namespace GCPack.Repository.Mappers
{

        public class ViewModelToModelMappingProfile : Profile
        {
            public override string ProfileName
            {
                get
                {
                    return "ViewModelToModelMappingProfile";
                }
            }

        public ViewModelToModelMappingProfile()
        {
            CreateMap<DocumentState, DocumentStateModel>().ReverseMap();
            CreateMap<AppSystem, AppSystemModel>().ReverseMap();
            CreateMap<Division, DivisionModel>().ReverseMap();
            CreateMap<Project, ProjectModel>().ReverseMap();
            CreateMap<Workplace, WorkplaceModel>().ReverseMap();
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<UserRoleModel, UserRole>().ReverseMap();
            CreateMap<RoleModel, Role>().ReverseMap();
            CreateMap<JobPosition, JobPositionModel>().ReverseMap();
            CreateMap<Document, DocumentModel>().ReverseMap();
            CreateMap<DocumentType, DocumentTypeModel>().ReverseMap();
            CreateMap<File, FileItem>().ReverseMap();
            CreateMap<GetDocuments16_Result, DocumentModel>()
                .ForMember(dr => dr.DocumentStateCode, m => m.MapFrom(dt => dt.DocumentStateCode))
                .ForMember(dr => dr.DocumentStateName, m => m.MapFrom(dt => dt.DocumentStateName))
                .ForMember(dr => dr.OwnerID, m => m.MapFrom(dt => dt.DocumentOwnerID))
                .ReverseMap();
            CreateMap<DocumentType, Item>()
                .ForMember(i => i.Value, m => m.MapFrom(dt => dt.Name));
            CreateMap<UserModel, Item>()
                .ForMember(i => i.Value, m => m.MapFrom(um => um.LastName + " " + um.FirstName));
            CreateMap<RoleModel, Item>()
                .ForMember(i => i.Value, m => m.MapFrom(r => r.RoleDescription))
                .ForMember(i => i.ID, m => m.MapFrom(r => r.RoleId));
        }


    }

}