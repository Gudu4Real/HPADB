using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using COHApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BataCMS.Data.Repositories
{
    public class HPAFacilityRepository : IHPAFacilityRepository

    {
        private readonly AppDbContext _appDbContext;

        public HPAFacilityRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<HPAFacility> HPAFacilities => _appDbContext.HPAFacilities.Include(c => c.Category);

        public async Task<HPAFacility> GetItemByIdAsync(int unitItemId) => await _appDbContext.HPAFacilities/*.Include(c => c.Images)*/.FirstOrDefaultAsync(p => p.HPAFacilityId == unitItemId);


        public async Task<int> DeleteItem(int itemId)
        {
            var unitItem = await _appDbContext.HPAFacilities.FindAsync(itemId);


            _appDbContext.HPAFacilities.Remove(unitItem);
            var result = await _appDbContext.SaveChangesAsync();
            return result;
        }

        public async Task<HPAFacility> AddAsync(HPAFacility item)
        {
            await _appDbContext.HPAFacilities.AddAsync(item);
            await _appDbContext.SaveChangesAsync();
            return item;
        }

        public async Task EditItemAsync(HPAFacility updatedItem)
        {
            _appDbContext.HPAFacilities.Update(updatedItem);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task BookAsset(DateTime bookedTill, int assetId)
        {
            HPAFacility rentalAsset = await _appDbContext.HPAFacilities.FindAsync(assetId);
            rentalAsset.BookTillDate = bookedTill;
            rentalAsset.IsAvailable = false;
            _appDbContext.HPAFacilities.Update(rentalAsset);
            await _appDbContext.SaveChangesAsync(); 
        }

        public async Task EndBooking(int assetId)
        {
            HPAFacility hPAFacility = await _appDbContext.HPAFacilities.FindAsync(assetId);
            hPAFacility.BookTillDate = null;
            hPAFacility.IsAvailable = true;
            _appDbContext.HPAFacilities.Update(hPAFacility);
            await _appDbContext.SaveChangesAsync();

        }
    }

}
