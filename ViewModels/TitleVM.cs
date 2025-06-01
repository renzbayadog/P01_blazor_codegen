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
   public class TitleVM
   {
		[Display(Name = "Title Id")]
		/*[Required(ErrorMessage = "Title Id is required")]*/
		public int TitleId { get; set; }

		[Display(Name = "Title Name")]
		[Required(ErrorMessage = "Title Name is required")]
		[MaxLength(50)]
		public string TitleName { get; set; }

		[Display(Name = "T Itle Description")]
		[Required(ErrorMessage = "T Itle Description is required")]
		[MaxLength(50)]
		public string TItleDescription { get; set; }

		[Display(Name = "Product Line Id")]
		/*[Required(ErrorMessage = "Product Line Id is required")]*/
		public int? ProductLineId { get; set; }

		[Display(Name = "Product Line Code")]
		/*[Required(ErrorMessage = "Product Line Code is required")]*/
		[MaxLength(50)]
		public string ProductLineCode { get; set; }

		[Display(Name = "Product Line Name")]
		/*[Required(ErrorMessage = "Product Line Name is required")]*/
		[MaxLength(50)]
		public string ProductLineName { get; set; }
   }

   public class TitleSearch
   {
        public string Keyword { get; set; }
		public string SortOrder { get; set; }
		public string TitleName { get; set; }
		public string TItleDescription { get; set; }
   }
}