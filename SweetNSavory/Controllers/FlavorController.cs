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
  public class FlavorController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public FlavorController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
      _logger = logger;
      _db = db;
      _userManager = userManager;
    }

    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);

      var userFlavorIds = _db.Flavors.Where(entry => entry.User.Id == currentUser.Id)
      .Select(entry => entry.FlavorId)
      .ToList();

      var userFlavors = _db.Flavors.Where(entry => userFlavorIds.Contains(entry.FlavorId)).ToList();

      var userTreatIds = _db.Treats.Where(entry => entry.User.Id == currentUser.Id)
                                    .Select(entry => entry.TreatId)
                                    .ToList();

      List<FlavorViewModel> model = new List<FlavorViewModel>();
      foreach (Flavor f in userFlavors)
      {
        Flavor flavorModel = f;
        List<Treat> treatsModel = new List<Treat>();

        foreach (TreatFlavor tf in f.TreatFlavors)
        {
          Treat treat = _db.Treats.FirstOrDefault(entry => entry.TreatId == tf.TreatId);
          if (userTreatIds.Contains(tf.TreatId))
          {
            treatsModel.Add(treat);
          }
        }

        FlavorViewModel newFlavor = new FlavorViewModel() { Flavor = flavorModel, Treats = treatsModel };
        model.Add(newFlavor);
      }
      return View(userFlavors);
    }

    [HttpGet]
    public async Task<ActionResult> New()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> New(Flavor f)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      f.User = currentUser;
      _db.Add(f);
      _db.SaveChanges();
      return RedirectToAction("Index", "Treat");
    }

    [HttpGet]
    public async Task<ActionResult> Details(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);

      var flavorModel = _db.Flavors.FirstOrDefault(entry => entry.FlavorId == id);

      var userTreatIds = _db.Treats.Where(entry => entry.User.Id == currentUser.Id)
                                    .Select(entry => entry.TreatId)
                                    .ToList();

      List<Treat> treatsModel = new List<Treat>();

      foreach (TreatFlavor tf in flavorModel.TreatFlavors)
      {
        Treat treat = _db.Treats.FirstOrDefault(entry => entry.TreatId == tf.TreatId);
        if (userTreatIds.Contains(tf.TreatId))
        {
          treatsModel.Add(treat);
        }
      }
      FlavorViewModel model = new FlavorViewModel() { Flavor = flavorModel, Treats = treatsModel };
      
      return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> Edit(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var model = _db.Flavors.FirstOrDefault(t => (t.FlavorId == id) & (t.User.Id == currentUser.Id));
      return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Flavor f)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      
      _db.Entry(f).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Detail", "Treat", new { id = f.FlavorId });
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