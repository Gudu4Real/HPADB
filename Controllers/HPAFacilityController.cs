using COHApp.Data.Interfaces;
using BataCMS.Data.Models;
using COHApp.Data.Models;
using COHApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;

namespace COHApp.Controllers
{
    public class HPAFacilityController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHPAFacilityRepository _HPAFacilityRepository;
        private readonly IMemberCertificateRepository _activeLeaseRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMemberSubscriptionRepository _memberSubscriptionRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IServiceRequestRepository _oDServiceRequestRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;



        public HPAFacilityController(IServiceTypeRepository serviceTypeRepository,IServiceRequestRepository oDServiceRequestRepository, IInvoiceRepository invoiceRepository, UserManager<ApplicationUser> userManager, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment, IHPAFacilityRepository hPAFacilityRepository, IImageRepository imageRepository, IMemberCertificateRepository activeLeaseRepository, IMemberSubscriptionRepository memberSubscriptionRepository, ITransactionRepository transactionRepository)
        {
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _HPAFacilityRepository = hPAFacilityRepository;
            _imageRepository = imageRepository;
            _activeLeaseRepository = activeLeaseRepository;
            _memberSubscriptionRepository = memberSubscriptionRepository;
            _userManager = userManager;
            _invoiceRepository = invoiceRepository;
            _transactionRepository = transactionRepository;
            _oDServiceRequestRepository = oDServiceRequestRepository;
            _serviceTypeRepository = serviceTypeRepository;

    }


        public ViewResult List(string category, string searchString)
        {
            string _category = category;
            IEnumerable<HPAFacility> facilities;

            string currentCategory = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {
                facilities = _HPAFacilityRepository.HPAFacilities.Where(p => p.Name.Contains(searchString));
            }
            else if (!string.IsNullOrEmpty(category))
            {
                facilities = _HPAFacilityRepository.HPAFacilities.Where(p => p.Category.CategoryName.Equals(_category));
                currentCategory = _category;
            }
            else
            {
                facilities = _HPAFacilityRepository.HPAFacilities.OrderBy(p => p.HPAFacilityId);
                currentCategory = "All Facilities";
            }

            var vm = new ListFacilitiesViewModel
            {
                Facilities = facilities,
                CurrentCategory = currentCategory
            };

            return View(vm);
        }

        public ViewResult BookedList(string category, string searchString)
        {
            string _category = category;
            IEnumerable<HPAFacility> facilities;

            string currentCategory = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {
                facilities = _HPAFacilityRepository.HPAFacilities.Where(p => p.Name.Contains(searchString)).Where(p => p.IsAvailable == false);
            }
            else if (!string.IsNullOrEmpty(category))
            {
                facilities = _HPAFacilityRepository.HPAFacilities.Where(p => p.Category.CategoryName.Equals(_category)).Where(p => p.IsAvailable == false);
                currentCategory = _category;
            }
            else
            {
                facilities = _HPAFacilityRepository.HPAFacilities.Where(p => p.IsAvailable == false).OrderBy(p => p.HPAFacilityId);
                currentCategory = "All Facilities";
            }

            var vm = new ListFacilitiesViewModel
            {
                Facilities = facilities,
                CurrentCategory = currentCategory
            };

            return View(vm);
        }


        public async Task<IActionResult> EndBooking(int id)
        {

            HPAFacility rentalAsset = await _HPAFacilityRepository.GetItemByIdAsync(id);

            /*ActiveLease activeLease = _activeLeaseRepository.GetActiveLeaseByAssetId(rentalAsset.HPAFacilityId);*/

            MemberSubscription lease = await _memberSubscriptionRepository.GetById(id/*activeLease.LeaseId*/);

            ApplicationUser user = await _userManager.FindByIdAsync(lease.UserId);

            if (rentalAsset == null)
            {
                ViewBag.ErrorMessage = $"The rental asset id={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    if (rentalAsset.BookTillDate < DateTime.Now)
                    {
                        // Calculate amount paid

                        var totalDays = (lease.From - lease.To).TotalDays;
                        decimal AmountPaid = rentalAsset.Price * (decimal)totalDays;

                        //Add invoice 
                        var invoice = new Invoice 
                        {
                            ApplicationId = user.Id,
                            AmountDue = AmountPaid
                        };

                       await _invoiceRepository.AddInvoice(invoice);
                        
                    }
                    await _HPAFacilityRepository.EndBooking(id);
                    /*await _activeLeaseRepository.RemoveLease(activeLease);*/
                    return RedirectToAction("BookedList");
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }   
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewAsync(int facilityId)
        {
            IEnumerable<Transaction> transactions = null;
            decimal totalSales = 0M;
            Dictionary<string, int> saleTypes = new Dictionary<string, int>();
            decimal opCosts = 0;
            decimal assetValue2021 = 0;
            decimal assetValue2022 = 0;

            HPAFacility facility = await _HPAFacilityRepository.GetItemByIdAsync(facilityId);


            string[] transactionTypes = new string[] { "Cash", "swipe", "ecocash" };


            transactions = _transactionRepository.Transactions;
            foreach (var item in transactionTypes)
            {
                saleTypes.Add(item, _transactionRepository.Transactions.Where((p => p.TransactionType == item)).Count());
            }

/*            if (!string.IsNullOrEmpty(type))
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

            }*/


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
                }
                else
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
                HPAFacility = facility,
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

        [HttpGet]
        public IActionResult Create()
        {
            this.ViewData["categories"] = _categoryRepository.Categories.Select(x => new SelectListItem
            {
                Value = x.CategoryName,
                Text = x.CategoryName
            }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateFacilityViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<Image> assetImages = new List<Image>();
                String mainPhotoPath = null;

                if (model.Images != null)
                {
                    if (model.Images.Count > 1)
                    {
                        for (int i = 0; i < model.Images.Count; i++)
                        {
                            if (i == 0)
                            {
                                mainPhotoPath = ProcessUploadedImage(model.Images[i]);
                                continue;
                            }

                            string photoPath = ProcessUploadedImage(model.Images[i]);
                            Image image = new Image { ImageName = model.Images[i].FileName, ImageUrl = photoPath };
                            assetImages.Add(image);

                        }

                    }

                }


                var category = _categoryRepository.Categories.FirstOrDefault(p => p.CategoryName == model.Category);

                HPAFacility newFacility = new HPAFacility
                {
                    Name = model.Name,
                    Price = model.Price,
                    IsAvailable = model.IsAvailable,
                    DateModified = DateTime.Today,
                    CategoryId = category.CategoryId,
                    ImageUrl = mainPhotoPath,
                    Description = model.Description,
                    Images = assetImages
                };

                await _HPAFacilityRepository.AddAsync(newFacility);
                return RedirectToAction("List", new { id = newFacility.HPAFacilityId });

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int facilityId)
        {
            this.ViewData["categories"] = _categoryRepository.Categories.Select(x => new SelectListItem
            {
                Value = x.CategoryName,
                Text = x.CategoryName
            }).ToList();

            HPAFacility facility = await _HPAFacilityRepository.GetItemByIdAsync(facilityId);
            Category category = _categoryRepository.Categories.FirstOrDefault(p => p.CategoryId == facility.CategoryId);

            EditFacilityViewModel viewModel = new EditFacilityViewModel
            {
                FacilityId = facility.HPAFacilityId,
                Name = facility.Name,
                Price = facility.Price,
                Location = facility.Location,
                IsAvailable = facility.IsAvailable,
                Category = category.CategoryName,
                ExistingImagePath = facility.ImageUrl,
                Description = facility.Description,
                ExistingImages = facility.Images
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditFacilityViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<Image> assetImages = new List<Image>();
                List<Image> deleteImages = new List<Image>();
                HPAFacility rentalAsset = await _HPAFacilityRepository.GetItemByIdAsync(model.FacilityId);
                Category category = _categoryRepository.Categories.FirstOrDefault(p => p.CategoryName == model.Category);


                rentalAsset.Name = model.Name;
                rentalAsset.Price = model.Price;
                rentalAsset.Location = model.Location;
                rentalAsset.CategoryId = category.CategoryId;
                rentalAsset.IsAvailable = model.IsAvailable;
                rentalAsset.DateModified = DateTime.Today;
                rentalAsset.Description = model.Description;

                if (model.Images != null)
                {

                    if (model.Images.Count > 1)
                    {
                        if (model.ExistingImagePath != null)
                        {
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath + model.ExistingImagePath);
                            System.IO.File.Delete(filePath);
                        }
/*                        foreach (var item in rentalAsset.Images)
                        {
                            deleteImages.Add(item);
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath + item.ImageUrl);
                            System.IO.File.Delete(filePath);
                        }
*/
                        for (int i = 0; i < model.Images.Count; i++)
                        {
                            if (i == 0)
                            {
                                rentalAsset.ImageUrl = ProcessUploadedImage(model.Images[i]);
                                continue;
                            }

                            string photoPath = ProcessUploadedImage(model.Images[i]);
                            Image image = new Image { ImageName = model.Images[i].FileName, ImageUrl = photoPath };
                            assetImages.Add(image);

                        }
                    }

                    /*rentalAsset.Images = assetImages;*/
                }

                await _HPAFacilityRepository.EditItemAsync(rentalAsset);

                foreach (var item in deleteImages)
                {
                    _imageRepository.DeleteImage(item);
                }

                return RedirectToAction("List");

            }
            return View();
        }

        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            var unitItem = _HPAFacilityRepository.GetItemByIdAsync(id);

            if (unitItem == null)
            {
                ViewBag.ErrorMessage = $"Item with  id ={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _HPAFacilityRepository.DeleteItem(id);

                //success
                if (result > 0)
                {
                    return RedirectToAction("List");
                }
                return View("List");

            }
        }


        private string ProcessUploadedImage(IFormFile Image)
        {
            string uniqueFileName = null;

            if (Image != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/rentalAssets");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                Image.CopyTo(fileStream);

            }

            string photoPath = "/images/rentalAssets/" + uniqueFileName;
            return photoPath;
        }

    }
}
