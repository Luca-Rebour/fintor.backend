using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum PendingTransactionStatus
    {
        Pending = 0,
        Approved = 1,
        Cancelled = 2,
        Rescheduled = 3

    }
}
