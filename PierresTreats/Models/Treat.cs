using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PierresTreats.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<TreatFlavor> JoinEntities { get; set; }
    public IdentityUser User { get; set; }
  }
}