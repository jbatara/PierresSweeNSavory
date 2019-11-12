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

namespace SweetNSavory.Controllers
{
  [Authorize]
  public class TreatController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TreatController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
      _logger = logger;
      _db = db;
      _userManager = userManager;
    }

    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userTreatIds = _db.Treats.Where(entry => entry.User.Id == currentUser.Id)
                                    .Select(entry => entry.TreatId)
                                    .ToList();
      var userTreats = _db.Treats.Where(entry => userTreatIds.Contains(entry.TreatId)).ToList();
      
      List<TreatViewModel> model = new List<TreatViewModel>();
      foreach (Treat t in userTreats)
      {
        Treat treatModel = t;
        var flavorIds = _db.TreatFlavors.Where(tf => tf.TreatId == t.TreatId)
          .Select(tf => tf.FlavorId)
          .ToList();

        var flavorsModel = _db.Flavors.Where(f => flavorIds.Contains(f.FlavorId) & (f.User.Id == userId)).ToList();
       
        TreatViewModel newTreat = new TreatViewModel() { Treat = treatModel, Flavors = flavorsModel };
        model.Add(newTreat);
      }

      return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> New()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> New(Treat t)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      t.User = currentUser;
      _db.Add(t);
      _db.SaveChanges();
      return RedirectToAction("Index", "Treat");
    }

    [HttpGet("/treat/{id}")]
    public async Task<ActionResult> Detail(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var treat = _db.Treats.FirstOrDefault(t => t.TreatId == id);
      List<int> flavorIds = _db.TreatFlavors
                      .Where(tf => tf.TreatId == treat.TreatId)
                      .Select(rf => rf.FlavorId)
                      .ToList();
      var flavors = _db.Flavors
                    .Where(f => flavorIds.Contains(f.FlavorId) & (f.User.Id == userId))
                    .ToList();
      var model = new TreatViewModel() { Treat = treat, Flavors = flavors };
      return View(model);
    }

    [HttpGet("/treat/edit/{id}")]
    public async Task<ActionResult> Edit(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var allFlavors = _db.Flavors.Where(f => f.User.Id == userId).ToList();
      var treatModel = _db.Treats.FirstOrDefault(t => (t.TreatId == id) & (t.User.Id == currentUser.Id));
      var flavorsIds = _db.TreatFlavors.Where(tf => tf.TreatId == id)
                                       .Select(tf => tf.FlavorId).ToList();
      List<Flavor> flavorModels = _db.Flavors.Where(tf => flavorsIds.Contains(tf.FlavorId)).ToList();

      ViewBag.FlavorsDropDown = Flavor.ListToDropDownItems(allFlavors);
      TreatViewModel model = new TreatViewModel(){Treat = treatModel, Flavors= flavorModels};
      return View(model);
    }

    [HttpPost("treat/edit/{id}")]
    public async Task<ActionResult> Edit(int treatId, int flavId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      if (flavId != 0)
      {
        _db.TreatFlavors.Add(new TreatFlavor() { TreatId = treatId, FlavorId = flavId });
      }
      _db.SaveChanges();
      return RedirectToAction("Detail", "Treat", new { id = treatId });
    }

    [HttpPost]
    public async Task<ActionResult> Delete(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      if (id != 0)
      {
        Treat treat = _db.Treats.FirstOrDefault(entry => entry.TreatId == id);
        if (treat.User.Id == userId)
        {
          _db.Treats.Remove(treat);

          List<TreatFlavor> treatFlavors = _db.TreatFlavors.Where(entry => entry.TreatId == id).ToList();
          foreach (TreatFlavor tf in treatFlavors)
          {
            _db.TreatFlavors.Remove(tf);
          }
          _db.SaveChanges();
        }
      }
      return RedirectToAction("Index", "Treat");
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}