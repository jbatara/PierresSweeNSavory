using System.Collections.Generic;
namespace SS.Models
{
  public class Treat
  {
    public Treat()
    {
      this.Flavors = new HashSet<TreatFlavor>();
    }
    public int TreatId { get; set; }
    public string TreatName { get; set; }
    public ICollection<TreatFlavor> Flavors { get; set; }
  }
}
