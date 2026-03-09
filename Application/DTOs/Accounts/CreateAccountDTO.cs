using Domain.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.DTOs.Accounts
{
    public class CreateAccountDTO
    {
        public string CurrencyCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal InitialBalance { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Icon { get; set; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new BusinessRuleException("Account name is required.", ErrorCode.ValidationError);
            }

            if (Name.Length > 30)
            {
                throw new BusinessRuleException("Account name is too long. Maximum allowed length is 30 characters.", ErrorCode.ValidationError);
            }

            if (string.IsNullOrWhiteSpace(CurrencyCode))
            {
                throw new BusinessRuleException("Currency code is required.", ErrorCode.ValidationError);
            }

            if (CurrencyCode.Length > 5)
            {
                throw new BusinessRuleException("Currency code is too long. Maximum allowed length is 5 characters.", ErrorCode.ValidationError);
            }

            if (InitialBalance < 0)
            {
                throw new BusinessRuleException("Initial balance cannot be negative.", ErrorCode.ValidationError);
            }
            }

    }
}
