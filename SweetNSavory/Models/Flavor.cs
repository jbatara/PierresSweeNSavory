using System.Collections.Generic;
namespace SweetNSavory.Models
{
  public class Flavor
  {
    public Flavor()
    {
      this.TreatFlavors = new HashSet<TreatFlavor>();
    }
    public int FlavorId {get;set;}
    public string FlavorName { get; set; }
    public ICollection<TreatFlavor> TreatFlavors { get; set; }
  }
}