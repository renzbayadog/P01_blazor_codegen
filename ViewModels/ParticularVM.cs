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
   public class ParticularVM
   {
		[Display(Name = "Particular Id")]
		public int ParticularId { get; set; }

		[Display(Name = "Particular Name*")]
		[Required(ErrorMessage = "Particular Name is required")]
		[MaxLength(50)]
		public string ParticularName { get; set; }

		[Display(Name = "Particular Description*")]
		[Required(ErrorMessage = "Particular Description is required")]
		[MaxLength(50)]
		public string ParticularDescription { get; set; }
   }

   public class ParticularSearch
   {
        public string Keyword { get; set; }
		public string SortOrder { get; set; }
		public string ParticularName { get; set; }
		public string ParticularDescription { get; set; }
   }
}