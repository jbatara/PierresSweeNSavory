using System.Collections.Generic;
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
    public ICollection<TreatFlavor> TreatFlavors { get; set; }
  }
}
