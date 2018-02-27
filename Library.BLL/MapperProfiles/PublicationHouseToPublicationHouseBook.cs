﻿using AutoMapper;
using Library.Entities.Entities;
using Library.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.BLL.MapperProfiles
{
    public class PublicationHouseToPublicationHouseBook : Profile
    {
        public PublicationHouseToPublicationHouseBook()
        {
            CreateMap<PublicationHouseViewModel, PublicationHouseBook>()
                .ForMember(memb => memb.PublicationHouse, dest => dest.MapFrom(x => x)).ReverseMap();
        }
    }
}