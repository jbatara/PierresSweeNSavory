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
  public class FlavorController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public FlavorController (ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db) {
      _logger = logger;
      _db = db;
      _userManager = userManager;
    }

    public IActionResult Index () {
      return View ();
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