using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Models;
using COHApp.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace COHApp.Controllers
{
    public class LeaseController : Controller
    {
        private readonly IMemberSubscriptionRepository _leaseRepository;
        private readonly UserManager<MemberUser> _vendorUserManager;


        public LeaseController(IMemberSubscriptionRepository leaseRepository, UserManager<MemberUser> userManager)
        {
            _leaseRepository = leaseRepository;
            _vendorUserManager = userManager;
        }


        [HttpGet]
        public IActionResult MakeBooking(int rentalAssetId)
        {
            ViewBag.RentalAssetId = rentalAssetId;
            return View();
        }


        [HttpGet]
        public IActionResult CashBooking(int rentalAssetId)
        {
            ViewBag.RentalAssetId = rentalAssetId;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CashBooking(AddLeaseViewModel model)
        {

            MemberUser vendorUser = await _vendorUserManager.FindByPhoneNumber(ProcessPhoneNumber(model.VendorPhone));

            // add the to the lease 
            if (ModelState.IsValid)
            {
                if (vendorUser == null)
                {
                    ModelState.AddModelError("", "The phone number "+ model.VendorPhone + " is not a registered Vendor");
                    return View();
                }

                MemberSubscription subscription = new MemberSubscription
                {
                    UserId = vendorUser.Id,
                    /*RentalAssetId = model.RentalAssetId,*/
                    From = model.leaseFrom,
                    To = model.leaseTo
                };
                var success =  await _leaseRepository.AddAsync(subscription);
                return RedirectToAction("Checkout", "Transaction", new {leaseId = success.MemberSubscriptionId });
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MakeBooking(MemberSubscription lease)
        {
            // add the to the lease 
            if (ModelState.IsValid)
            {
                var success = await _leaseRepository.AddAsync(lease);

                return RedirectToAction("Checkout", "Transaction", new { leaseId = success.MemberSubscriptionId });
            }

            return View(lease);
        }

        private string ProcessPhoneNumber(string phoneNumber)
        {
            string processedNumber = null;
            string extension = "+263";

            if (phoneNumber != null)
            {
                string startingNum = phoneNumber.Substring(0, 1);

                //not in E.164 format
                if (startingNum != "+")
                {
                    if (startingNum == "0")
                    {
                        processedNumber = extension + phoneNumber.Substring(1);
                    }
                }
            }
            return processedNumber;
        }
    }
}
