using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IMemberSubscriptionRepository
    {
        public Task<MemberSubscription> AddAsync(MemberSubscription subscription);

        Task<MemberSubscription> GetById(int membersubscriptionId);

/*        public Task RemoveUnPaidLeases();*/

    }
}
