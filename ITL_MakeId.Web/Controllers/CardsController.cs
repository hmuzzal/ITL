﻿using ITL_MakeId.Data;
using ITL_MakeId.Model.DomainModel;
using ITL_MakeId.Model.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ITL_MakeId.Web.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string filePath;
        private string filePathSignature;

        public CardsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }



        public async Task<IActionResult> Dashboard()
        {
            ViewBag.RequestedIdentityCards = _context.IdentityCards.Where(c => c.ValidationEndDate == null)
                .Include(c => c.BloodGroup)
                .Include(c => c.Designation).Count();


            ViewBag.ValidatedIdentityCards = _context.IdentityCards.Where(c => c.ValidationEndDate >= DateTime.Now)
                .Include(c => c.BloodGroup)
                .Include(c => c.Designation).Count();

            ViewBag.ExpiredIdentityCards = _context.IdentityCards.Where(c => c.ValidationEndDate < DateTime.Now)
                .Include(c => c.BloodGroup)
                .Include(c => c.Designation).Count();


            return View();
        }




        public async Task<IActionResult> Index()
        {
            var identityCards = await _context.IdentityCards
                .Include(c => c.BloodGroup)
                .Include(c => c.Designation).ToListAsync();


            IdentityCardViewModel model = new IdentityCardViewModel();
            model.IdentityCards = identityCards;

            return View(model);
        }


        public async Task<IActionResult> ValidatedUsers()
        {

            var identityCards = await _context.IdentityCards.Where(c => c.ValidationEndDate >= DateTime.Now)
                .Include(c => c.BloodGroup)
                .Include(c => c.Designation).ToListAsync();


            IdentityCardViewModel model = new IdentityCardViewModel();
            model.IdentityCards = identityCards;
            ViewBag.Title = "Validated Card Users";

            return View(model);
        }



        public async Task<IActionResult> ExpiredUserCards()
        {

            var identityCards = await _context.IdentityCards.Where(c => c.ValidationEndDate < DateTime.Now)
                .Include(c => c.BloodGroup)
                .Include(c => c.Designation).ToListAsync();


            IdentityCardViewModel model = new IdentityCardViewModel();
            model.IdentityCards = identityCards;
            ViewBag.Title = "Expired Card Users";

            return View("ValidatedUsers", model);
        }

        public async Task<IActionResult> UserRequestForCard()
        {

            var identityCards = await _context.IdentityCards.Where(c => c.ValidationEndDate == null)
                .Include(c => c.BloodGroup)
                .Include(c => c.Designation).ToListAsync();


            IdentityCardViewModel model = new IdentityCardViewModel();
            model.IdentityCards = identityCards;
            ViewBag.Title = "Users Requested For Card ";

            return View("ValidatedUsers", model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var identityCard = await _context.IdentityCards.Include(c => c.BloodGroup)
                .Include(c => c.Designation)
                .FirstOrDefaultAsync(m => m.Id == id);

            IdentityCardViewModel model = new IdentityCardViewModel();
            model.IdentityCard = identityCard;

            if (identityCard == null)
            {
                return NotFound();
            }

            return View(model);
        }


        public IActionResult Create()
        {
            IdentityCardViewModel model = new IdentityCardViewModel();
            ViewData["BloodGroupId"] = new SelectList(_context.BloodGroups, "Id", "Name");
            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Title");

            var identitycardInfo = _context.IdentityCards.ToList();
            var cardNumber = identitycardInfo.LastOrDefault()?.CardNumber;

            model.CardNumber = model.GetCardNumber(cardNumber);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DesignationId," +
                                                      "BloodGroupId,CardNumber,CardNumber,ImagePathOfUser,ImagePathOfUserSignature," +
                                                      "ImagePathOfAuthorizedSignature,CompanyName,CompanyAddress," +
                                                      "CompanyLogoPath,CardInfo")] IdentityCardViewModel identityCardViewModel)
        {

            ViewData["BloodGroupId"] = new SelectList(_context.BloodGroups, "Id", "Name");
            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Title");

            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                string uniqueFileNameSignature = null;
                string uniqueFileNameAuthorizedSignature = null;
                string uniqueFileNameCompanyLogo = null;
                //ImagePathOfUserSignature
                // CompanyLogoPath
                if (identityCardViewModel.ImagePathOfUser != null && identityCardViewModel.ImagePathOfUserSignature != null)
                {
                    string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image/user/");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + identityCardViewModel.ImagePathOfUser.FileName;
                    filePath = Path.Combine(uplaodsFolder, uniqueFileName);

                    string uplaodsFolderSignature = Path.Combine(_webHostEnvironment.WebRootPath, "image/sig/");
                    uniqueFileNameSignature = Guid.NewGuid().ToString() + "_" + identityCardViewModel.ImagePathOfUserSignature.FileName;
                    filePathSignature = Path.Combine(uplaodsFolderSignature, uniqueFileNameSignature);

                    string uplaodsFolderAuthSignature = Path.Combine(_webHostEnvironment.WebRootPath, "image/auth/");
                    uniqueFileNameAuthorizedSignature = "9536f8a1-d3ad-41b6-8448-e748bb33f589_authsign.jpg";
                    //filePathAuthSignature = Path.Combine(uplaodsFolderAuthSignature, uniqueFileNameAuthorizedSignature);

                    string uplaodsFolderLogo = Path.Combine(_webHostEnvironment.WebRootPath, "image/logo/");
                    uniqueFileNameCompanyLogo = "68837796-1410-4966-a0bb-562e4b44c854_Logo.jpg";
                    //filePathLogo = Path.Combine(uplaodsFolderLogo, uniqueFileNameCompanyLogo);


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        identityCardViewModel.ImagePathOfUser.CopyTo(stream);
                    }

                    using (var stream = new FileStream(filePathSignature, FileMode.Create))
                    {
                        identityCardViewModel.ImagePathOfUserSignature.CopyTo(stream);
                    }

                }

                identityCardViewModel.IdentityCard.Name = identityCardViewModel.Name;
                identityCardViewModel.IdentityCard.DesignationId = identityCardViewModel.DesignationId;
                identityCardViewModel.IdentityCard.BloodGroupId = identityCardViewModel.BloodGroupId;
                identityCardViewModel.IdentityCard.CardNumber = identityCardViewModel.CardNumber;
                identityCardViewModel.IdentityCard.ImagePathOfUser = uniqueFileName;
                identityCardViewModel.IdentityCard.ImagePathOfUserSignature = uniqueFileNameSignature;
                identityCardViewModel.IdentityCard.ImagePathOfAuthorizedSignature = uniqueFileNameAuthorizedSignature;
                identityCardViewModel.IdentityCard.CompanyLogoPath = uniqueFileNameCompanyLogo;


                _context.Add(identityCardViewModel.IdentityCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(identityCardViewModel);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var identityCard = await _context.IdentityCards.Include(c => c.BloodGroup)
                .Include(c => c.Designation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (identityCard == null)
            {
                return NotFound();
            }
            return View(identityCard);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ValidationStartDate,ValidationEndDate")] IdentityCard identityCard)
        {

            if (id != identityCard.Id)
            {
                return NotFound();
            }


            if (identityCard.ValidationStartDate == null)
            {
                ModelState.AddModelError(string.Empty, "Enter valid start date");
                return View(identityCard);
            }

            if (identityCard.ValidationEndDate == null)
            {
                ModelState.AddModelError(string.Empty, "Enter valid end date");
                //return View(identityCard);
            }

            if (identityCard.ValidationEndDate < identityCard.ValidationStartDate)
            {
                ModelState.AddModelError(string.Empty, "End date is not valid");
                //return View(identityCard);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        IdentityCard model = new IdentityCard();
                        model = await _context.IdentityCards.Include(c => c.BloodGroup)
                            .Include(c => c.Designation)
                            .FirstOrDefaultAsync(m => m.Id == id);

                        model.ValidationStartDate = identityCard.ValidationStartDate;
                        model.ValidationEndDate = identityCard.ValidationEndDate;

                        _context.Update(model);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!IdentityCardExists(identityCard.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(identityCard);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var identityCard = await _context.IdentityCards.FindAsync(id);
            string previousUserImageUrl = identityCard.ImagePathOfUser;
            string previousSignatureUrl = identityCard.ImagePathOfUserSignature;
            _context.IdentityCards.Remove(identityCard);
            await _context.SaveChangesAsync();

            string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image/user/");
            string uplaodsFolderSignature = Path.Combine(_webHostEnvironment.WebRootPath, "image/sig/");

            if (System.IO.File.Exists(Path.Combine(uplaodsFolder, previousUserImageUrl)))
            {

                System.IO.File.Delete(Path.Combine(uplaodsFolder, previousUserImageUrl));
            }

            if (System.IO.File.Exists(Path.Combine(uplaodsFolderSignature, previousSignatureUrl)))
            {

                System.IO.File.Delete(Path.Combine(uplaodsFolderSignature, previousSignatureUrl));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool IdentityCardExists(int id)
        {
            return _context.IdentityCards.Any(e => e.Id == id);
        }

    }
}
