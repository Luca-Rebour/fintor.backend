using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum ErrorCode
    {
        Unknown = 0,
        InsufficientBalance,
        GoalCompleted,
        AccountNotFound,
        Forbidden,
        ValidationError,
        EmailAlreadyInUse,
        NotFound
    }
}
