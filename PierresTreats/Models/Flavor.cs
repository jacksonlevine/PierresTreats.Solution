using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PierresTreats.Models
{
  public class Flavor
    {
        public int FlavorId { get; set; }
        public string Name { get; set; }
        public List<TreatFlavor> JoinEntities { get;}
        public IdentityUser User { get; set; }
    }
}