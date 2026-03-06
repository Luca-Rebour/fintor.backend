using Application.DTOs.PendingApprovalTransactions;
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
    public class PendingApprovalTransactionProfile : Profile
    {
        public PendingApprovalTransactionProfile()
        {
            CreateMap<PendingApprovalTransaction, PendingApprovalTransactionDTO>()
                    .ForMember(dest => dest.Icon,
                        opt => opt.MapFrom(src => src.Category.Icon))
                    .ForMember(dest => dest.AccountName,
                        opt => opt.MapFrom(src => src.Account.Name))
                    .ForMember(dest => dest.CurrencyCode,
                        opt => opt.MapFrom(src => src.Account.Currency));
        }
    }
}
