using ITL_MakeId.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ITL_MakeId.Model.DomainModel;

namespace ITL_MakeId.Web.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardsController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.IdentityCards.ToListAsync());
        }


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
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Designation,BloodGroup,CardNumber,ImagePathOfUser,ImagePathOfUserSignature,ImagePathOfAuthorizedSignature,CompanyName,CompanyAddress,CompanyLogoPath,CardInfo")] IdentityCard identityCard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(identityCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(identityCard);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Designation,BloodGroup,CardNumber,ImagePathOfUser,ImagePathOfUserSignature,ImagePathOfAuthorizedSignature,CompanyName,CompanyAddress,CompanyLogoPath,CardInfo")] IdentityCard identityCard)
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
