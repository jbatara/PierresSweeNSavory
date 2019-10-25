using System.Collections.Generic;
namespace SS.Models
{
  public class TreatFlavor
  {
    public int TreatId {get;set;}
    public int FlavorId {get;set;}
    public Treat Treat {get;set;}
    public Flavor Flavor {get;set;}
  }
}