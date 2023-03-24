using Microsoft.AspNetCore.Mvc;
using PierresTreats.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PierresTreats.Controllers
{
  [Authorize]
  public class FlavorsController : Controller
  {
    private readonly PierresTreatsContext _db;
    public FlavorsController(PierresTreatsContext db)
    {
      _db = db;
    }

    [AllowAnonymous] 
    public ActionResult Index()
    {
      return View(_db.Flavors.ToList());
    }

    public ActionResult Details(int id)
    {
      Flavor thisFlavor = _db.Flavors
          .Include(Flavor => Flavor.JoinEntities)
          .ThenInclude(join => join.Treat)
          .FirstOrDefault(Flavor => Flavor.FlavorId == id);
      return View(thisFlavor);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Flavor Flavor)
    {
      _db.Flavors.Add(Flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddTreat(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(Flavors => Flavors.FlavorId == id);
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Description");
      return View(thisFlavor);
    }

    [HttpPost]
    public ActionResult AddItem(Flavor Flavor, int TreatId)
    {
#nullable enable
      TreatFlavor? joinEntity = _db.TreatFlavors.FirstOrDefault(join => (join.TreatId == TreatId && join.FlavorId == Flavor.FlavorId));
#nullable disable
      if (joinEntity == null && TreatId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { TreatId = TreatId, FlavorId = Flavor.FlavorId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = Flavor.FlavorId });
    }

    public ActionResult Edit(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(Flavors => Flavors.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost]
    public ActionResult Edit(Flavor Flavor)
    {
      _db.Flavors.Update(Flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(Flavors => Flavors.FlavorId == id);
      return View(thisFlavor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Flavor thisFlavor = _db.Flavors.FirstOrDefault(Flavors => Flavors.FlavorId == id);
      _db.Flavors.Remove(thisFlavor);
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