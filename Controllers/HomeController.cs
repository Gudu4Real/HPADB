using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BataCMS.Data.Interfaces;
using BataCMS.Data.Models;
using BataCMS.Data.Repositories;
using BataCMS.Infrastructure;
using BataCMS.ViewModels;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace BataCMS.Controllers
{
    public class HomeController : Controller
    {

        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IHPAFacilityRepository _HPAFacilityRepository;
        private readonly ICategoryRepository _categoryRepository;




        public HomeController( IServiceTypeRepository serviceTypeRepository, IHPAFacilityRepository hPAFacilityRepository, ICategoryRepository categoryRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _categoryRepository = categoryRepository;
            _HPAFacilityRepository = hPAFacilityRepository;
        }
        public ViewResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                Sectors = _categoryRepository.Categories
            };

            return View(homeViewModel);
        }


        public ViewResult OnDemandIndex()
        {
            var homeViewModel = new HomeViewModel
            {
                Sectors = _categoryRepository.Categories
            };

            return View(homeViewModel);
        }

        public ViewResult BulletinIndex(int categoryId)
        {
            IEnumerable<HPAFacility> companies = new List<HPAFacility>();
            Category sector = new Category();

            if (categoryId != 0)
            {
                sector = _categoryRepository.Categories.FirstOrDefault(p => p.CategoryId == categoryId);
                companies = _HPAFacilityRepository.HPAFacilities.Where(p => p.CategoryId == sector.CategoryId);

            }

            var vm = new BulletinHomeViewModel
            {
                SectorName = sector.CategoryName,
                Companies = companies
            };
            return View(vm);
        }


    }
}