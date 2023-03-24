using Microsoft.AspNetCore.Mvc;
using PierresTreats.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace PierresTreats.Controllers
{
  [Authorize]
  public class HomeController : Controller
  {
    private readonly PierresTreatsContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(UserManager<IdentityUser> userManager, PierresTreatsContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous]
    [HttpGet("/")]
    public ActionResult Index()
    {
      Dictionary<string, object[]> model = new Dictionary<string, object[]>();
      Treat[] treats = _db.Treats.ToArray();
      model.Add("treats", treats);
      Flavor[] flavors = _db.Flavors.ToArray();
      model.Add("flavors", flavors);
      return View(model);
    }
  }
}