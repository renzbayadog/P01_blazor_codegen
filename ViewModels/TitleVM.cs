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
		public int TitleId { get; set; }

		[Display(Name = "Title Name*")]
		[Required(ErrorMessage = "Title Name is required")]
		[MaxLength(50)]
		public string TitleName { get; set; }

		[Display(Name = "Title Description*")]
		[Required(ErrorMessage = "Title Description is required")]
		[MaxLength(50)]
		public string TItleDescription { get; set; }

		[Display(Name = "Product Line Id")]
		public int? ProductLineId { get; set; }

		[Display(Name = "Particular Id")]
		public int? ParticularId { get; set; }

		[Display(Name = "Product Line Code")]
		[MaxLength(50)]
		public string ProductLineCode { get; set; }

		[Display(Name = "Product Line Name")]
		[MaxLength(100)]
		public string ProductLineName { get; set; }

		[Display(Name = "Particular Name")]
		[MaxLength(100)]
		public string ParticularName { get; set; }

		[Display(Name = "Particular Description")]
		[MaxLength(50)]
		public string ParticularDescription { get; set; }
   }

   public class TitleSearch
   {
        public string Keyword { get; set; }
		public string SortOrder { get; set; }
		public string TitleName { get; set; }
		public string TItleDescription { get; set; }
   }
}