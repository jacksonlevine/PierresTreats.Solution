using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PierresTreats.Controllers
{
  [Authorize]
  public class TreatsController : Controller
  {
    private readonly PierresTreatsContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public TreatsController(UserManager<IdentityUser> userManager, PierresTreatsContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous] 
    public ActionResult Index()
    {
      //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      //IdentityUser currentUser = await _userManager.FindByIdAsync(userId);
      // List<Treat> userTreat = _db.Treats.Where(treat => treat.User.Id == currentUser.Id.ToString()).ToList();

      List<Treat> userTreats = _db.Treats.ToList();
      return View(userTreats);
    }

    public async Task<ActionResult> Create()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      IdentityUser currentUser = await _userManager.FindByIdAsync(userId);

      //ViewBag.Flavors = _db.Flavors.ToList();
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      IdentityUser currentUser = await _userManager.FindByIdAsync(userId);
      treat.User = currentUser;
      _db.Treats.Add(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> Details(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      IdentityUser currentUser = await _userManager.FindByIdAsync(userId);

      Treat thisTreat = _db.Treats
          .Include(treat => treat.JoinEntities)
          .ThenInclude(join => join.Flavor)
          .Include(treat => treat.User)
          .FirstOrDefault(treat=> treat.TreatId == id);
      
      return View(thisTreat);
    }

    public ActionResult AddTag(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treats => treats.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Title");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddTag(Treat treat, int tagId)
    {
#nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => (join.FlavorId == tagId && join.TreatId == treat.TreatId));
#nullable disable
      if (joinEntity == null && tagId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { FlavorId = tagId, TreatId = treat.TreatId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treat.TreatId });
    }


    public ActionResult Edit(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Treat treat)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      IdentityUser currentUser = await _userManager.FindByIdAsync(userId);
      if(currentUser.Id == treat.User.Id)
      {
        _db.Treats.Update(treat);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      else
      {
        return RedirectToAction("Index");
      }


    }

    public ActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      TreatFlavor joinEntry = _db.TreatFlavors.FirstOrDefault(entry => entry.TreatFlavorId == joinId);
      _db.TreatFlavors.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}