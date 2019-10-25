using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
      var userTreats = _db.Treats.Where (entry => entry.User.Id == currentUser.Id);
      return View (userTreats);
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

    

    public IActionResult Privacy () {
      return View ();
    }

    [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error () {
      return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}