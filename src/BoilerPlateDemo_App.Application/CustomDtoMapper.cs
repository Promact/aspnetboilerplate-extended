using AutoMapper;
using CETAutomation.Applications.Dto;
using CETAutomation.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlateDemo_App
{
    internal class CustomDtoMapper
    {

        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditApplicationDto, Application>().ForMember(dto => dto.Id, options => options.Ignore()).ReverseMap();

        }
    }
}
