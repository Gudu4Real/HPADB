using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IMemberCertificateRepository
    {
/*        ActiveLease GetActiveLeaseByAssetId(int id);*/

         Task AddActiveLeaseAsync(MemberCertificate activeLease);

        public Task RemoveLease(MemberCertificate activeLease);
    }
}
