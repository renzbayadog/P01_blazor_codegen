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
    internal class PulloutRepository : RepositoryBase<Pullout>, IPulloutRepository
    {
        private readonly db_ab9d6a_dbrenzContext _context;

        public PulloutRepository(db_ab9d6a_dbrenzContext context) : base(context)
        {
            _context = context;
        }
  
        public async Task<List<Pullout>> GetAllPulloutQry(PulloutSearch searchInfo)
        {
            List<Pullout> pullouts = await _context.Pullouts
						
						.AsNoTracking().ToListAsync();
			if (!String.IsNullOrEmpty(searchInfo.Keyword))
			{
				
				pullouts = pullouts.Where(p => 
									p.PulloutName.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| p.PulloutDescription.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| p.PulloutDate.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| p.BusinessValue.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| p.PulloutPrice.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
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
            
            return pullouts;
        }

    }
}