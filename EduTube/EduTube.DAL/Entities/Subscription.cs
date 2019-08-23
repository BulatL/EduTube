using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EduTube.DAL.Entities
{
   public class Subscription : Entity
   {
      [ForeignKey("SubscriberId")]
      public ApplicationUser Subscriber { get; set; }
      public string SubscriberId { get; set; }

      [ForeignKey("SubscribedOnId")]
      public ApplicationUser SubscribedOn { get; set; }
      public string SubscribedOnId { get; set; }
      public bool Deleted { get; set; }
   }
}
