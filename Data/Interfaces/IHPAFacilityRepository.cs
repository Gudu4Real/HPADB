using BataCMS.Data.Models;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IHPAFacilityRepository
    {
        IEnumerable<HPAFacility> HPAFacilities { get; }

        Task<HPAFacility> GetItemByIdAsync(int itemId);

        Task<int> DeleteItem(int itemId);

        Task<HPAFacility> AddAsync(HPAFacility item);

        Task EditItemAsync(HPAFacility updatedItem);

        public Task BookAsset(DateTime bookedTill, int assetID);

        public Task EndBooking(int assetId);
    }
}
