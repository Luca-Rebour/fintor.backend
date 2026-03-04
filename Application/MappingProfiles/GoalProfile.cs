using Application.DTOs.Categories;
using Application.DTOs.Goals;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class GoalProfile : Profile
    {
        public GoalProfile()
        {
            CreateMap<Goal, GoalDTO>();
            CreateMap<CreateGoalDTO, Goal>();

        }
    }
}
