﻿using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Repositories
{
    public class PurchasePaymentMethodRepository : IPurchasePayementMethodRepository
    {
        private readonly AppDbContext _appDbContext;

        public PurchasePaymentMethodRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddPurchasePaymentMethod(MemberSubscription purchasePaymentMethod)
        {
            _appDbContext.AddAsync(purchasePaymentMethod);
            _appDbContext.SaveChanges();
        }

        public MemberSubscription GetPurchasePaymentMethodByPurchaseId(int purchaseId)
        {
            throw new NotImplementedException();
        }
    }
}
