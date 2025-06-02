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
   public class ProductLineVM
   {
		[Display(Name = "Product Line Id")]
		/*[Required(ErrorMessage = "Product Line Id is required")]*/
		public int ProductLineId { get; set; }

		[Display(Name = "Product Line Code*")]
		[Required(ErrorMessage = "Product Line Code is required")]
		[MaxLength(100)]
		public string ProductLineCode { get; set; }

		[Display(Name = "Product Line Name*")]
		[Required(ErrorMessage = "Product Line Name is required")]
		[MaxLength(100)]
		public string ProductLineName { get; set; }
   }

   public class ProductLineSearch
   {
        public string Keyword { get; set; }
		public string SortOrder { get; set; }
		public string ProductLineCode { get; set; }
		public string ProductLineName { get; set; }
   }
}