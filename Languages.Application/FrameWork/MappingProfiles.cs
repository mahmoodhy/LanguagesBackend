using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.FrameWork
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<dictionaryRoot, dictionaryRootDto>().ReverseMap();
            CreateMap<Aimeaning, Content>().ReverseMap();
            //CreateMap<Phonetic, Phonetic>().ReverseMap();
            //CreateMap<Meaning, Meaning>().ReverseMap();
            //CreateMap<Definition, Definition>().ReverseMap();
            //CreateMap<License, License>().ReverseMap();
        }
    }

}
