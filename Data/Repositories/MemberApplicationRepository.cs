using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class MemberApplicationRepository : IMemberApplicaitonRepository
    {
        private readonly AppDbContext _appDbContext;
        //private readonly IHubContext<SignalServer> _hubContext;

        public MemberApplicationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<MemberApplication> vendorApplications => _appDbContext.MemberApplications;

        public async Task<MemberApplication> AddAsync(MemberApplication application)
        {
            await _appDbContext.MemberApplications.AddAsync(application);
            await _appDbContext.SaveChangesAsync();
            return application;
        }

        public async Task<MemberApplication> GetApplicationByIdAsync(int applicationId)
        {           
            return await _appDbContext.MemberApplications.Include(p => p.ApplicationUser).FirstOrDefaultAsync(p => p.MemberApplicationId == applicationId);
        }

        public async Task UpdateApplicationAsync(MemberApplication application)
        {
            _appDbContext.MemberApplications.Update(application);
            await _appDbContext.SaveChangesAsync();

            //var ApplicationCount = (vendorApplications.Where(p => p.Status == "Pending")).Count();
            //await _hubContext.Clients.All.SendAsync("updatePurchase", OrderCount);
        }
    }
}
