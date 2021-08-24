using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Models;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace COHApp.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IHPAFacilityRepository _HPAFacilityRepository;

        public InvoiceController(IInvoiceRepository invoiceRepository, IHPAFacilityRepository hPAFacilityRepository)
        {
            _invoiceRepository = invoiceRepository;
            _HPAFacilityRepository = hPAFacilityRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult List(string userId)
        {
            IEnumerable<Invoice> invoices = null;

            if (String.IsNullOrEmpty(userId) || userId == "all")
            {
                invoices = _invoiceRepository.Invoices;
            }
            else
            {
                invoices = _invoiceRepository.GetUserInvoices(userId);
            }


            var vm = new ListInvoicesViewModel
            {
                Invoices = invoices,
            };
            return View(vm);
        }

        public async Task<IActionResult> View(int Id)
        {

            Invoice invoice =  _invoiceRepository.GetInvoice(Id);


            var vm = new InvoiceDetailViewModel
            {
                InvoiceId = invoice.InvoiceId,
                AmountPaid = invoice.AmountDue
            };

            return View(vm);

        }

    }
}
