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
            CreateMap<JobPositionModel, JobPosition>().ReverseMap();
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<UserRoleModel, UserRole>().ReverseMap();
            CreateMap<RoleModel, Role>();
        }


        }

}