using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RenzGrandWeddingblazor.ph.ViewModels; 
using RenzGrandWeddingblazor.ph.Data.Entities; 
using RenzGrandWeddingblazor.ph.Data; 
using RenzGrandWeddingblazor.ph.Data.Repositories; 
using codegeneratorlib.Helpers; 


namespace RenzGrandWeddingblazor.ph.Data.Repositories
{
    internal class ProductLineRepository : RepositoryBase<ProductLine>, IProductLineRepository
    {
        private readonly db_ab9d6a_dbrenzContext _context;

        public ProductLineRepository(db_ab9d6a_dbrenzContext context) : base(context)
        {
            _context = context;
        }
  
        public async Task<List<ProductLine>> GetAllProductLineQry(ProductLineSearch searchInfo)
        {
            List<ProductLine> productlines = await _context.ProductLines
						
						.AsNoTracking().ToListAsync();
			if (!String.IsNullOrEmpty(searchInfo.Keyword))
			{
				
				productlines = productlines.Where(p => 
									p.ProductLineCode.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| p.ProductLineName.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									)
								.ToList();
			}

				//.Where(f => searchInfo.DateFrom == null || f.CreateDate >= searchInfo.DateFrom)
				//.Where(f => searchInfo.DateTo == null || f.CreateDate <= searchInfo.DateTo)
				//.OrderByDescending(s => s.CreateDate).ToList();

				//if (!String.IsNullOrEmpty(searchInfo.SortOrder))
				//{
				//	var sortCurrent = searchInfo.SortOrder.Split("_").Last();
				//	var sortCurrent = searchInfo.SortOrder.Split("_").First();
				//	if (sortCurrent.Equals("DESC"))
				//	{
				//		products.OrderByDescending(a=>a.)
				//	}
				//}
            
            return productlines;
        }

    }
}