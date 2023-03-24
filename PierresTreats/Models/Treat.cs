using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PierresTreats.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    //[Required(ErrorMessage = "The treat's name can't be empty!")]
    public string Name { get; set; }
    public string Description { get; set; }
    public List<TreatFlavor> JoinEntities { get; set; }
    public IdentityUser User { get; set; }
  }
}