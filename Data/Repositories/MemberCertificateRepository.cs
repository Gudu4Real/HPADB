using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class MemberCertificateRepository : IMemberCertificateRepository
    {
        private readonly AppDbContext _appDbContext;

        public MemberCertificateRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddActiveLeaseAsync(MemberCertificate activeLease)
        {
            await _appDbContext.AddAsync(activeLease);
            await _appDbContext.SaveChangesAsync();
        }



/*        public ActiveLease GetActiveLeaseByAssetId(int id)
        {
            return _appDbContext.ActiveLeases.FirstOrDefault(p => p.RentalAssetId == id);
        }
*/
        public async Task RemoveLease(MemberCertificate certificate)
        {
            _appDbContext.MemberCertificates.Remove(certificate);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
