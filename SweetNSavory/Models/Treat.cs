using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace SweetNSavory.Models
{
  public class Treat
  {
    public Treat()
    {
      this.TreatFlavors = new HashSet<TreatFlavor>();
    }
    public int TreatId { get; set; }
    public string TreatName { get; set; }
    public virtual ApplicationUser User { get; set; }
    public ICollection<TreatFlavor> TreatFlavors { get; set; }

    public SelectListItem ToDropDownItem()
    {
      SelectListItem item = new SelectListItem { Text = $"{TreatName}", Value = $"{ TreatId }" };
      return item;
    }

    public static List<SelectListItem> ListToDropDownItems(List<Treat> treats)
    {
      List<SelectListItem> list = new List<SelectListItem>();
      foreach (Treat t in treats)
      {
        list.Add(t.ToDropDownItem());
      }
      return list;
    }
  }
}
