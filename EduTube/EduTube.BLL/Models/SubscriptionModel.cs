using EduTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduTube.BLL.Models
{
    public class SubscriptionModel
    {
        public int Id { get; set; }
        public ApplicationUserModel Subscriber { get; set; }
        public string SubscriberId { get; set; }
        public ApplicationUserModel SubscribedOn { get; set; }
        public string SubscribedOnId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Deleted { get; set; }
    }
}
