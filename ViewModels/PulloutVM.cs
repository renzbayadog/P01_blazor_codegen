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
   public class PulloutVM
   {
		[Display(Name = "Pullout Id")]
		/*[Required(ErrorMessage = "Pullout Id is required")]*/
		public int PulloutId { get; set; }

		[Display(Name = "Pullout Name")]
		[Required(ErrorMessage = "Pullout Name is required")]
		[MaxLength(25)]
		public string PulloutName { get; set; }

		[Display(Name = "Pullout Description")]
		[Required(ErrorMessage = "Pullout Description is required")]
		[MaxLength(25)]
		public string PulloutDescription { get; set; }

		[Display(Name = "Pullout Date")]
		/*[Required(ErrorMessage = "Pullout Date is required")]*/
		public DateTime? PulloutDate { get; set; }

		[Display(Name = "Receipt Image")]
		/*[Required(ErrorMessage = "Receipt Image is required")]*/
		public string ReceiptImage { get; set; }

		[Display(Name = "Business Value")]
		/*[Required(ErrorMessage = "Business Value is required")]*/
		public decimal? BusinessValue { get; set; }

		[Display(Name = "Pullout Price")]
		/*[Required(ErrorMessage = "Pullout Price is required")]*/
		public decimal? PulloutPrice { get; set; }
   }

   public class PulloutSearch
   {
        public string Keyword { get; set; }
		public string SortOrder { get; set; }
		public string PulloutName { get; set; }
		public string PulloutDescription { get; set; }
		public DateTime PulloutDate { get; set; }
		public decimal BusinessValue { get; set; }
		public decimal PulloutPrice { get; set; }
   }
}