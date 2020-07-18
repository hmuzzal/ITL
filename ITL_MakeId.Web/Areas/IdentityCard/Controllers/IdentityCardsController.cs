using ITL_MakeId.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ITL_MakeId.Web.Areas.IdentityCard.Controllers
{
    [Area("IdentityCard")]
    public class IdentityCardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IdentityCardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IdentityCard/IdentityCards
        public async Task<IActionResult> Index()
        {
            return View(await _context.IdentityCards.ToListAsync());
        }

        // GET: IdentityCard/IdentityCards/Details/5
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

        // GET: IdentityCard/IdentityCards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IdentityCard/IdentityCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Designation,BloodGroup,CardNumber,ImagePathOfUser,ImagePathOfUserSignature,ImagePathOfAuthorizedSignature,CompanyName,CompanyAddress,CompanyLogoPath,CardInfo")] Model.IdentityCard.IdentityCard identityCard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(identityCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(identityCard);
        }

        // GET: IdentityCard/IdentityCards/Edit/5
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

        // POST: IdentityCard/IdentityCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Designation,BloodGroup,CardNumber,ImagePathOfUser,ImagePathOfUserSignature,ImagePathOfAuthorizedSignature,CompanyName,CompanyAddress,CompanyLogoPath,CardInfo")] Model.IdentityCard.IdentityCard identityCard)
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

        // GET: IdentityCard/IdentityCards/Delete/5
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

        // POST: IdentityCard/IdentityCards/Delete/5
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
