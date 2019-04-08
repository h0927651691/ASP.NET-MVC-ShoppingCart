using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCarts.Models
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
        public class OrderMetadata
        {
            [Display(Name = "Id")]
            public int Id { get; set; }
            [Display(Name = "UserId")]
            public string UserId { get; set; }
            [Display(Name = "ReceiverName")]
            public string ReceiverName { get; set; }
            [Display(Name = "ReceiverPhone")]
            public string ReceiverPhone { get; set; }
            [Display(Name = "ReceiverAddress")]
            public string ReceiverAddress { get; set; }
           
        }
    }
}