using Domain.Enums;
using Domain.Exceptions;
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
        public string CurrencyCode { get; set; }
        public string Name { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Icon { get; set; }

        public void Validate()
        {
            if (Name.Length > 30)
            {
                throw new BusinessRuleException("Account name is too long. Maximum allowed length is 30 characters.", ErrorCode.ValidationError);
            }

            if (InitialBalance < 0)
            {
                throw new BusinessRuleException("Initial balance cannot be negative.", ErrorCode.ValidationError);
            }
            }

    }
}
