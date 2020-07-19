using System;
using System.IO;
using ITL_MakeId.Data;
using ITL_MakeId.Model.DomainModel;
using ITL_MakeId.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace ITL_MakeId.Web.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string filePath;


        public CardsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index()
        {

            var validateUsers = await _context.IdentityCards.ToListAsync();

            IdentityCardViewModel model = new IdentityCardViewModel();
            

            return View();
        }


        //public async Task<IActionResult> ValidateUsers()
        //{
        //    return View(await _context.IdentityCards.ToListAsync().);
        //}

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var identityCard = await _context.IdentityCards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (identityCard == null)
            {
                return NotFound();
            }

            return View(identityCard);
        }


        public IActionResult Create()
        {
            IdentityCardViewModel model = new IdentityCardViewModel();
            ViewData["BloodGroupId"] = new SelectList(_context.BloodGroups, "Id", "Name");
            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Title");

            var identitycardInfo = _context.IdentityCards.ToList();
            var cardNumber = identitycardInfo.LastOrDefault()?.CardNumber;

            model.CardNumber = model.GetCardNumber(cardNumber);
            //model.CardNumber = model.GetCardNumber(cardNumber ?? "ITL-0000");

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
                if (identityCardViewModel.ImagePathOfUser != null)
                {
                    string uplaodsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image/user/");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + identityCardViewModel.ImagePathOfUser.FileName;
                    filePath = Path.Combine(uplaodsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        identityCardViewModel.ImagePathOfUser.CopyTo(stream);
                    }

                }

                identityCardViewModel.IdentityCard.Name = identityCardViewModel.CardNumber;
                identityCardViewModel.IdentityCard.DesignationId = identityCardViewModel.DesignationId;
                identityCardViewModel.IdentityCard.BloodGroupId = identityCardViewModel.BloodGroupId;
                identityCardViewModel.IdentityCard.CardNumber = identityCardViewModel.CardNumber;
                identityCardViewModel.IdentityCard.ImagePathOfUser = uniqueFileName;


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

            var identityCard = await _context.IdentityCards.FindAsync(id);
            if (identityCard == null)
            {
                return NotFound();
            }
            return View(identityCard);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Designation,BloodGroup,CardNumber," +
                                                            "ImagePathOfUser,ImagePathOfUserSignature,ImagePathOfAuthorizedSignature," +
                                                            "CompanyName,CompanyAddress,CompanyLogoPath,CardInfo")] IdentityCard identityCard)
        {
            if (id != identityCard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(identityCard);
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
            return View(identityCard);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var identityCard = await _context.IdentityCards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (identityCard == null)
            {
                return NotFound();
            }

            return View(identityCard);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var identityCard = await _context.IdentityCards.FindAsync(id);
            _context.IdentityCards.Remove(identityCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdentityCardExists(int id)
        {
            return _context.IdentityCards.Any(e => e.Id == id);
        }
    }
}
