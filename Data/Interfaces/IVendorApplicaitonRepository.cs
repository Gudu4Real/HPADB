using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IMemberApplicaitonRepository
    {
        IEnumerable<MemberApplication> vendorApplications { get; }

        Task<MemberApplication> AddAsync(MemberApplication application);

        Task<MemberApplication> GetApplicationByIdAsync(int applicationId);

        Task UpdateApplicationAsync(MemberApplication application);

    }
}
