using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace SweetNSavory.Models {
  public class Flavor {
    public Flavor () {
      this.TreatFlavors = new HashSet<TreatFlavor> ();
    }
    public int FlavorId { get; set; }
    public string FlavorName { get; set; }
    public virtual ApplicationUser User { get; set; }
    public ICollection<TreatFlavor> TreatFlavors { get; set; }


    public SelectListItem ToDropDownItem()
    {
      SelectListItem item = new SelectListItem { Text = $"{FlavorName}", Value = $"{ FlavorId }" };
      return item;
    }

    public static List<SelectListItem> ListToDropDownItems(List<Flavor> flavors)
    {
      List<SelectListItem> list = new List<SelectListItem>();
      foreach (Flavor f in flavors)
      {
        list.Add(f.ToDropDownItem());
      }
      return list;
    }
  }
}