using BataCMS.Data;
using BataCMS.Data.Models;
using COHApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BataCMS.Data.Interfaces;

namespace COHApp.Data.Repositories
{
    public class MemberSubscriptionRepository : IMemberSubscriptionRepository
    {

        private readonly AppDbContext _appDbContext;

        public IEnumerable<MemberSubscription> MemberSubscriptions => _appDbContext.MemberSubscriptions;

        public MemberSubscriptionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<MemberSubscription> AddAsync(MemberSubscription subscription)
        {
            await _appDbContext.MemberSubscriptions.AddAsync(subscription);
            await _appDbContext.SaveChangesAsync();
            return subscription;
        }

        public async Task<MemberSubscription> GetById(int membersubscriptionId)
        {
            return await _appDbContext.MemberSubscriptions.FirstOrDefaultAsync(p => p.MemberSubscriptionId == membersubscriptionId);

        }

/*        public async Task RemoveUnPaidLeases()
        {
            foreach (var item in Leases)
            {
                var paidLease = _appDbContext.Transactions.FirstOrDefault(p => p.LeaseId == item.MemberSubscriptionId);

                if (paidLease == null)
                {
                    _appDbContext.Leases.Remove(item); 
                }
            }

            await _appDbContext.SaveChangesAsync();
        }*/
    }
}
