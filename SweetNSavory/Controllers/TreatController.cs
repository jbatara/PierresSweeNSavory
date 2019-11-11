using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SweetNSavory.Data;
using SweetNSavory.Models;

namespace SweetNSavory.Controllers {
  [Authorize]
  public class TreatController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TreatController (ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db) {
      _logger = logger;
      _db = db;
      _userManager = userManager;
    }

    public async Task<ActionResult> Index () {
      var userId = this.User.FindFirst (ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync (userId);
      var userTreatIds = _db.Treats.Where (entry => entry.User.Id == currentUser.Id)
                                    .Select(entry => entry.TreatId)
                                    .ToList();
      var userFlavorIds = _db.Flavors.Where(entry => entry.User.Id == currentUser.Id)
      .Select(entry => entry.FlavorId)
      .ToList();
      var userTreats = _db.Treats.Where(entry => userTreatIds.Contains(entry.TreatId)).ToList();
      List<TreatViewModel> model = new List<TreatViewModel>();
      foreach(Treat t in userTreats)
      {
          Treat treatModel = t;
          List<Flavor> flavorModel = new List<Flavor>();

          foreach(TreatFlavor tf in t.TreatFlavors)
          {
            Flavor flavor = _db.Flavors.FirstOrDefault(entry => entry.FlavorId == tf.FlavorId);
            if(userFlavorIds.Contains(tf.FlavorId))
            {
              flavorModel.Add(flavor);
            }
          }

          TreatViewModel newTreat = new TreatViewModel(){Treat = treatModel, Flavors = flavorModel};
          model.Add(newTreat);
      }
      
      return View (model);
    }

    [HttpGet]
    public async Task<ActionResult> New () {
      var userId = this.User.FindFirst (ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync (userId);
      return View ();
    }

    [HttpPost]
    public async Task<ActionResult> New (Treat t) {
      var userId = this.User.FindFirst (ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync (userId);
      t.User = currentUser;
      _db.Add (t);
      _db.SaveChanges ();
      return RedirectToAction ("Index", "Treat");
    }

    [HttpGet]
    public async Task<ActionResult> Details (int id) {
      var userId = this.User.FindFirst (ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync (userId);
      var treat = _db.Treats.FirstOrDefault (t => t.TreatId == id);
      List<int> flavorIds = _db.TreatFlavors
                      .Where(tf => tf.TreatId == treat.TreatId)
                      .Select(rf => rf.FlavorId)
                      .ToList();
      var flavors = _db.Flavors
                    .Where(f => flavorIds.Contains(f.FlavorId))
                    .ToList();
      var model = new TreatViewModel(){Treat = treat,Flavors = flavors};
      return View (model);
    }

    [HttpGet]
    public async Task<ActionResult> Edit (int id) {
      var userId = this.User.FindFirst (ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync (userId);
      var model = _db.Treats.FirstOrDefault (t => (t.TreatId == id) & (t.User.Id == currentUser.Id));
      ViewBag.Flavors = _db.Flavors.ToList ();
      ViewBag.TreatId = id;
      return View (model);
    }

    [HttpPost]
    public async Task<ActionResult> Edit (Treat t, int flavId) {
      var userId = this.User.FindFirst (ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync (userId);
      if (flavId != 0) {
        _db.TreatFlavors.Add (new TreatFlavor () { TreatId = t.TreatId, FlavorId = flavId });
      }
      _db.Entry (t).State = EntityState.Modified;
      _db.SaveChanges ();
      return RedirectToAction ("Detail", "Treat", new { id = t.TreatId });
    }

    public IActionResult Privacy () {
      return View ();
    }

    [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error () {
      return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}