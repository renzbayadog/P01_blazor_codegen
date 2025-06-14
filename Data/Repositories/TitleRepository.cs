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
    internal class TitleRepository : RepositoryBase<Title>, ITitleRepository
    {
        private readonly db_ab9d6a_dbrenzContext _context;

        public TitleRepository(db_ab9d6a_dbrenzContext context) : base(context)
        {
            _context = context;
        }
  
        public async Task<List<Title>> GetAllTitleQry(TitleSearch searchInfo)
        {
            List<Title> titles = await _context.TItles
						.Include(productline => productline.ProductLine)
						.Include(particular => particular.Particular)
						.AsNoTracking().ToListAsync();
			if (!String.IsNullOrEmpty(searchInfo.Keyword))
			{
				
				titles = titles.Where(t => 
									t.TitleName.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| t.TItleDescription.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
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
            
            return titles;
        }

		public async Task<Title> GetTitleById(int id)
        {
            Title title = await _context.TItles
                        .Include(productline => productline.ProductLine)
						.Include(particular => particular.Particular)
                        .AsNoTracking().FirstOrDefaultAsync(m => m.TitleId == id);

            return title;
        }

    }
}