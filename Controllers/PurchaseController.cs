﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BataCMS.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly Checkout _checkout;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly AppDbContext _appDbContext;

        public PurchaseController(IPurchaseRepository purchaseRepository, Checkout checkout, IPaymentMethodRepository paymentMethodRepository, AppDbContext appDbContext)
        {
            _purchaseRepository = purchaseRepository;
            _checkout = checkout;
            _paymentMethodRepository = paymentMethodRepository;
            _appDbContext = appDbContext; 
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [Authorize] 
        public IActionResult Checkout(Purchase purchase, PaymentMethod paymentMethod)
        {
            var items = _checkout.GetCheckoutItems();
            _checkout.CheckoutItems = items;

            if (_checkout.CheckoutItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty");
            }
            if (ModelState.IsValid)
            {
                _purchaseRepository.CreatePurchase(purchase);
                _paymentMethodRepository.CreatePaymentMethod(paymentMethod);

                var purchasePaymentMethod = new PurchasePaymentMethod
                {
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    PurchaseId =  purchase.PurchaseId
                };

                _appDbContext.Add(purchasePaymentMethod);
                _appDbContext.SaveChanges();
                _checkout.ClearCheckout();
                return RedirectToAction("CheckoutComplete"); 
            }

            return View(purchase);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckkoutComplete = "The purchase has been made";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListPurchases()
        {
            IEnumerable<Purchase> purchases = _appDbContext.Purchases.OrderBy(p => p.PurchaseDate);
            var vm = new ListPurchaseViewModel
            { 
                Purchases = purchases
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PurchaseDetail(int purchaseId)
        {
            var purchaseObjects = new List<PurchaseObject>();

            Purchase purchase = _appDbContext.Purchases.FirstOrDefault(p => p.PurchaseId == purchaseId);
            IEnumerable<PurchasedItem> purchasedItems = _appDbContext.PurchasedItems.Where(p => p.PurchaseId == purchaseId);

            foreach (var item in purchasedItems)
            {
                unitItem unit = _appDbContext.UnitItems.FirstOrDefault(p => p.unitItemId == item.unitItemId);

                var purchaseObject = new PurchaseObject { 
                    PurchaseAmount = item.Amount,
                    Price = item.Price,
                    ItemName = unit.Name
                };

                purchaseObjects.Add(purchaseObject);
            }
     

            PurchasePaymentMethod purchasePaymentMethod = _appDbContext.PurchasePaymentMethod.FirstOrDefault(p => p.PurchaseId == purchaseId);
            PaymentMethod paymentMethod = _appDbContext.PaymentMethods.FirstOrDefault(p => p.PaymentMethodId == purchasePaymentMethod.PaymentMethodId);

            var vm = new PurchaseDetailViewModel
            {
                PurchaseId = purchase.PurchaseId,
                PurchasedItems = purchaseObjects,
                PaymentMethod = paymentMethod,
                Purchase = purchase
            }; 

            return View(vm);
        }
    }
}