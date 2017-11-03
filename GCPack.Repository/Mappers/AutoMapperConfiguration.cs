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
            //DocumentTypeID
            CreateMap<DocumentType, DocumentTypeModel>().ReverseMap();
            CreateMap<File, FileItem>().ReverseMap();
            CreateMap<GetDocuments18_Result, DocumentModel>()
                .ForMember(dr => dr.DocumentStateCode, m => m.MapFrom(dt => dt.DocumentStateCode))
                .ForMember(dr => dr.DocumentStateName, m => m.MapFrom(dt => dt.DocumentStateName))
                // LF 30.10.2017 - zruseno naplnovani AuthorID podle ownera, owner jako tako v aplikaci RD nebude figurovat
                //.ForMember(dr => dr.AuthorID, m => m.MapFrom(dt => dt.DocumentOwnerID))
                .ReverseMap();
            CreateMap<DocumentType, Item>()
                .ForMember(i => i.Value, m => m.MapFrom(dt => dt.Name));
            CreateMap<UserModel, Item>()
                .ForMember(i => i.Value, m => m.MapFrom(um => um.LastName + " " + um.FirstName));
            CreateMap<RoleModel, Item>()
                .ForMember(i => i.Value, m => m.MapFrom(r => r.RoleName))
                .ForMember(i => i.ID, m => m.MapFrom(r => r.RoleId));
        }


    }

}