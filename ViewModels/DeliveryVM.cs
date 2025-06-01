using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RenzGrandWeddingblazor.ph.ViewModels; 
using RenzGrandWeddingblazor.ph.Data.Entities; 
using RenzGrandWeddingblazor.ph.Data; 
using RenzGrandWeddingblazor.ph.Data.Repositories; 
using codegeneratorlib.Helpers; 


namespace RenzGrandWeddingblazor.ph.ViewModels
{
   public class DeliveryVM
   {
		[Display(Name = "Delivery Id")]
		/*[Required(ErrorMessage = "Delivery Id is required")]*/
		public int DeliveryId { get; set; }

		[Display(Name = "Delivery Name")]
		[Required(ErrorMessage = "Delivery Name is required")]
		[MaxLength(25)]
		public string DeliveryName { get; set; }

		[Display(Name = "Delivery Address")]
		[Required(ErrorMessage = "Delivery Address is required")]
		[MaxLength(25)]
		public string DeliveryAddress { get; set; }

		[Display(Name = "Delivery Date")]
		/*[Required(ErrorMessage = "Delivery Date is required")]*/
		public DateTime? DeliveryDate { get; set; }

		[Display(Name = "Business Value")]
		/*[Required(ErrorMessage = "Business Value is required")]*/
		public decimal BusinessValue { get; set; }
   }

   public class DeliverySearch
   {
        public string Keyword { get; set; }
		public string SortOrder { get; set; }
		public string DeliveryName { get; set; }
		public string DeliveryAddress { get; set; }
		public DateTime DeliveryDate { get; set; }
   }
}