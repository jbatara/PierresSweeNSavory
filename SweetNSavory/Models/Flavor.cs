using System.Collections.Generic;
namespace SS.Models
{
  public class Flavor
  {
    public Flavor()
    {
      this.Treats = new HashSet<TreatFlavor>();
    }
    public int FlavorId {get;set;}
    public string FlavorName { get; set; }
    public ICollection<TreatFlavor> Treats { get; set; }
  }
}