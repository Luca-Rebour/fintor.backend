using Application.DTOs.Accounts;
using Application.DTOs.RecurringTransactions;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class RecurringTransactionProfile : Profile
    {
        public RecurringTransactionProfile() {
            CreateMap<RecurringTransaction, RecurringTransactionDTO>();
        }
    }
}
