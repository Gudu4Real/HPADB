using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;

namespace COHApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IMemberSubscriptionRepository _leaseRepository;
        private readonly IHPAFacilityRepository _HPAFacilityRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMemberCertificateRepository _activeLeaseRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IServiceRequestRepository _oDServiceRequestRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserManager<MemberUser> _vendorUserManager;




        public TransactionController(UserManager<MemberUser> vendorUserManager, IMemberSubscriptionRepository leaseRepository, UserManager<ApplicationUser> userManager, IHPAFacilityRepository hPAFacilityRepository, ITransactionRepository transactionRepository, IMemberCertificateRepository activeLeaseRepository, IServiceRequestRepository oDServiceRequestRepository, IServiceTypeRepository serviceTypeRepository)
        {
            _leaseRepository = leaseRepository;
            _HPAFacilityRepository = hPAFacilityRepository;
            _userManager = userManager;
            _transactionRepository = transactionRepository;
            _activeLeaseRepository = activeLeaseRepository;
            _vendorUserManager = vendorUserManager;
            _serviceTypeRepository = serviceTypeRepository;
            _oDServiceRequestRepository = oDServiceRequestRepository;
        }

        [Authorize]
        public async Task<IActionResult> CheckoutAsync(int leaseId)
        {

            List<SelectListItem> paymentOptions = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Debit Card", Value = "swipe" },
                new SelectListItem() { Text = "Eco Cash", Value = "ecocash" },
            };

            this.ViewData["paymentOptions"] = paymentOptions;

            MemberSubscription lease = await _leaseRepository.GetById(leaseId);
            /*HPAFacility rentalAsset = await _rentalAssetRepository.GetItemByIdAsync(lease.RentalAssetId);*/

            var totalDays = (lease.From - lease.To).TotalDays;
            decimal transactionTotal = 0M;//rentalAsset.Price * (decimal)totalDays;

            TransactionCheckoutViewModel vm = new TransactionCheckoutViewModel()    
            {
                AssetPricing = 0M,//rentalAsset.Price,
                RentalDuration = totalDays,
                TransactionTotal = transactionTotal
            };

            ViewBag.leaseId = leaseId;
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListTransactions(string filter, string type)
        {
            IEnumerable<Transaction> transactions = null;
            decimal totalSales = 0M;
            Dictionary<string, int> saleTypes = new Dictionary<string, int>();

            if (!string.IsNullOrEmpty(type))
            {
                transactions = _transactionRepository.Transactions.Where((p => p.TransactionType == type));

            }
            else
            {
                if (String.IsNullOrEmpty(filter) || filter == "all")
                {
                    transactions = _transactionRepository.Transactions;
                }
                else
                {
                    if (filter == "hour")
                    {
                        transactions = _transactionRepository.Transactions.Where((p => p.TransactionDate >= (DateTime.Now.AddHours(-1))));
                    }
                    if (filter == "day")
                    {
                        transactions = _transactionRepository.Transactions.Where((p => p.TransactionDate >= (DateTime.Now.AddDays(-1))));
                    }
                    if (filter == "week")
                    {
                        transactions = _transactionRepository.Transactions.Where((p => p.TransactionDate >= (DateTime.Now.AddDays(-7))));
                    }
                }

            }

            string[] transactionTypes = new string[] { "Cash", "swipe", "ecocash" };

            foreach (var item in transactionTypes)
            {
                saleTypes.Add(item, _transactionRepository.Transactions.Where((p => p.TransactionType == item)).Count());
            }


            foreach (var item in transactions)
            {
                totalSales += item.TransactionTotal;
            }
            var vm = new ListTransactionsViewModel
            {
                Transactions = transactions,
                TotalSales = totalSales,
                SaleTypes = saleTypes
                
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutAsync(TransactionCheckoutViewModel model)
        {
            //get the lease
            MemberSubscription lease = await _leaseRepository.GetById(model.LeaseId);
            /*HPAFacility rentalAsset = await _rentalAssetRepository.GetItemByIdAsync(lease.RentalAssetId);*/

            string transactionType;

            if (lease.UserId == model.ServerId && model.TransactionType != null)
            {
                transactionType = model.TransactionType;
            }
            else
            {
                transactionType = "Cash";
            }

            var totalDays = (lease.From - lease.To).TotalDays;
            decimal transactionTotal = 0M;//rentalAsset.Price * (decimal)totalDays;

            if (ModelState.IsValid)
            {
                var transaction = new Transaction
                {
                    TransactionTotal = transactionTotal,
                    ServerId = model.ServerId,
                    TransactionDate = DateTime.Now,
                    TransactionNotes = model.TransactionNotes,
                    TransactionType = transactionType,
                    /*LeaseId = lease.MemberSubscriptionId,*/
                    VendorUserId = lease.UserId,
                };

                var ActiveLease = new MemberCertificate
                { 
                    /*RentalAssetId = rentalAsset.HPAFacilityId,*/
                    /*LeaseId = lease.MemberSubscriptionId*/
                };


                var result = await _transactionRepository.CreateTransactionAsync(transaction);

                //success 
                if (result > 0)
                {
                    /*await _rentalAssetRepository.BookAsset(lease.leaseTo, rentalAsset.HPAFacilityId);*/
                    await _activeLeaseRepository.AddActiveLeaseAsync(ActiveLease);
/*                    await _leaseRepository.RemoveUnPaidLeases();*/
                }

                return RedirectToAction("CheckoutComplete");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult PostExportExcel()
        {
            var myTranscations = _transactionRepository.Transactions.ToList();

            var stream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");

                workSheet.Cells[1, 1].Value = "TransactionId";
                workSheet.Cells[1, 2].Value = "TransactionDate";
                workSheet.Cells[1, 3].Value = "Vendor Name";
                workSheet.Cells[1, 4].Value = "Transaction Total";
                workSheet.Cells[1, 5].Value = "Transaction Type";

                workSheet.Row(1).Height = 20;
                workSheet.Column(1).Width = 15;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 15;
                workSheet.Column(4).Width = 15;
                workSheet.Column(5).Width = 16;

                workSheet.Row(1).Style.Font.Bold = true;

                workSheet.Cells["B2:B" + (myTranscations.Count + 1)].Style.Numberformat.Format = "yyyy-mm-dd";

                for (int index = 1; index <= myTranscations.Count; index++)
                {
                    workSheet.Cells[index + 1, 1].Value = myTranscations[index - 1].TransactionId;
                    workSheet.Cells[index + 1, 2].Value = myTranscations[index - 1].TransactionDate;
                    workSheet.Cells[index + 1, 3].Value = myTranscations[index - 1].ServerId;
                    workSheet.Cells[index + 1, 4].Value = myTranscations[index - 1].TransactionTotal;
                    workSheet.Cells[index + 1, 5].Value = myTranscations[index - 1].TransactionType;
                }
                package.Save();
            }
            stream.Position = 0;

            string excelName = $"Transactions-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
            // above I define the name of the file using the current datetime.

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName); // this will be the actual export.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportExcelFileAsync(IFormFile upload)
        {
            //get file name
            var filename = ContentDispositionHeaderValue.Parse(upload.ContentDisposition).FileName.Trim('"');
            //get path
            var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

            if (!Directory.Exists(MainPath))
            {
                Directory.CreateDirectory(MainPath);
            }
            //get file path 
            var filePath = Path.Combine(MainPath, upload.FileName);
            using (System.IO.Stream stream = new FileStream(filePath, FileMode.Create))
            {
                await upload.CopyToAsync(stream);
            }
            //get extension
            string extension = Path.GetExtension(filename);

            string conString = string.Empty;

            switch (extension)
            {
                case ".xls": //Excel 97-03.
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                case ".xlsx": //Excel 07 and above.
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }

            DataTable dt = new DataTable();
            conString = string.Format(conString, filePath);

            using (OleDbConnection connExcel = new OleDbConnection(conString))
            {
                using (OleDbCommand cmdExcel = new OleDbCommand())
                {
                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                    {
                        cmdExcel.Connection = connExcel;

                        //Get the name of First Sheet.
                        connExcel.Open();
                        DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        //Read Data from First Sheet.
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        odaExcel.SelectCommand = cmdExcel;
                        odaExcel.Fill(dt);
                        connExcel.Close();
                    }
                }
            }

            //your database connection string
            conString = @"Server=(localdb)\MSSQLLocalDB;Database=Dev_HPADB_Data;Trusted_Connection=True;MultipleActiveResultSets=true";

            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name.
                    sqlBulkCopy.DestinationTableName = "dbo.Transactions";

                    // Map the Excel columns with that of the database table, this is optional but good if you do
                    // 
                    sqlBulkCopy.ColumnMappings.Add("TransactionTotal", "TransactionTotal");
                    sqlBulkCopy.ColumnMappings.Add("TransactionDate", "TransactionDate");
                    sqlBulkCopy.ColumnMappings.Add("TransactionNotes", "TransactionNotes");
                    sqlBulkCopy.ColumnMappings.Add("TransactionType", "TransactionType");
                    sqlBulkCopy.ColumnMappings.Add("isDelivered", "isDelivered");

                    con.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }
            //if the code reach here means everthing goes fine and excel data is imported into database
            ViewBag.Message = "File Imported and excel data saved into database";

            return RedirectToAction("TransactionsDashboard");

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TransactionDetailAsync(int Id)
        {

            Transaction transaction = await _transactionRepository.GetPurchaseByIdAsync(Id);

            /*MemberSubscription lease = transaction.Lease;*/

            /*HPAFacility rentalAsset = await _rentalAssetRepository.GetItemByIdAsync(lease.RentalAssetId);*/

           /* MemberUser vendor = await _vendorUserManager.FindByIdAsync(lease.UserId);*/

            ApplicationUser server = await _userManager.FindByIdAsync(transaction.ServerId);


            var vm = new TransactionDetailViewModel
            {
               /* RentalAsset = rentalAsset,*/
                Transaction = transaction,
                /*VendorUser = vendor,*/
                Server = server,
                /*Lease = lease */
            };

            return View(vm);

        }


        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckkoutComplete = "The purchase has been made";
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult TransactionsDashboard(string filter, string type)
        {
            IEnumerable<Transaction> transactions = null;
            decimal totalSales = 0M;
            Dictionary<string, int> saleTypes = new Dictionary<string, int>();
            decimal opCosts = 0;
            decimal assetValue2021 = 0;
            decimal assetValue2022 = 0;



            string[] transactionTypes = new string[] { "Cash", "swipe", "ecocash" };

            foreach (var item in transactionTypes)
            {
                saleTypes.Add(item, _transactionRepository.Transactions.Where((p => p.TransactionType == item)).Count());
            }

            if (!string.IsNullOrEmpty(type))
            {
                transactions = _transactionRepository.Transactions.Where((p => p.TransactionType == type));

            }
            else
            {
                if (String.IsNullOrEmpty(filter) || filter == "all")
                {
                    transactions = _transactionRepository.Transactions;
                }
                else
                {
                    if (filter == "hour")
                    {
                        transactions = _transactionRepository.Transactions.Where((p => p.TransactionDate >= (DateTime.Now.AddHours(-1))));
                    }
                    if (filter == "day")
                    {
                        transactions = _transactionRepository.Transactions.Where((p => p.TransactionDate >= (DateTime.Now.AddDays(-1))));
                    }
                    if (filter == "week")
                    {
                        transactions = _transactionRepository.Transactions.Where((p => p.TransactionDate >= (DateTime.Now.AddDays(-7))));
                    }
                }

            }


            foreach (var item in transactions)
            {
                totalSales += item.TransactionTotal;
            }

            //Calculating operating costs

            List<int> serviceRequestID = new List<int>();

            foreach (var item in _oDServiceRequestRepository.ODServiceRequests)
            {
                if (item.Status == "Dispatched")
                {
                    serviceRequestID.Add(item.ServiceTypeId);
                }
            }

            foreach (var item in serviceRequestID)
            {
                var service = _serviceTypeRepository.ServiceTypes.First(p => p.ServiceTypeId == item);
                opCosts += service.Pricing;
            }

            Dictionary<string, decimal> facilityPricing = new Dictionary<string, decimal>();
            IEnumerable<HPAFacility> facilities;

            facilities = _HPAFacilityRepository.HPAFacilities;
            foreach (var item in facilities)
            {
                if (item.DateModified.Year == 2022)
                {
                   assetValue2022 += item.Price;
                }else
                {
                    assetValue2021 += item.Price;
                }
            }

            foreach (var item in facilities)
            {
                facilityPricing.Add(item.Name, item.Price);
            }

            var vm = new TransactionsDashboardViewModel
            {
                /*Transactions = transactions,*/
                FacilityPricing = facilityPricing,
                TotalSales = totalSales,
                SaleTypes = saleTypes,
                UserCount = _userManager.Users.Count(),
                OpCosts = opCosts,
                AssetValue2021 = assetValue2021,
                AssetValue2022 = assetValue2022 + assetValue2021,
            };
            return View(vm);
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









