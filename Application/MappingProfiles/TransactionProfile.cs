using Application.DTOs.Accounts;
using Application.DTOs.Transactions;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDTO>()
                .ForMember(dest => dest.TransactionType,
                    opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.IsRecurringTransaction,
                    opt => opt.MapFrom(src => src.RecurringTransaction != null))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.AccountName,
                    opt => opt.MapFrom(src => src.Account.Name))
                .ForMember(dest => dest.CurrencyCode,
                    opt => opt.MapFrom(src => src.Account.Currency.Code))
                .ForMember(dest => dest.Icon,
                    opt => opt.MapFrom(src => src.Category.Icon));
        }
    }
}
