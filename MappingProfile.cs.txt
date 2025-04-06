using System;
using AutoMapper;
using WebRest.EF.Models;

namespace WebRestAPI.Code;

 public class MappingProfile : Profile {
     public MappingProfile() {
         // Add as many of these lines as you need to map your objects
         CreateMap<Customer, Customer>();
         CreateMap<Address, Address>();
     }
 }
