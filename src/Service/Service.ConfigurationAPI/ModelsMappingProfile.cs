using AutoMapper;
using Service.ConfigurationAPI.Models;
using Service.Core.Abstractions.Execution;
using Service.Core.Configuration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.ConfigurationAPI
{
    /// <summary>
    /// Main profile used to map service internal objects to exposed models.
    /// </summary>
    public class ModelsMappingProfile : Profile
    {
        public ModelsMappingProfile()
        {
            CreateMap<SessionDAO, SessionDTO>();
            CreateMap<LogDAO, LogDTO>();
        }
    }
}
