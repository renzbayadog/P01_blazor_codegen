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
    internal class ParticularRepository : RepositoryBase<Particular>, IParticularRepository
    {
        private readonly db_ab9d6a_dbrenzContext _context;

        public ParticularRepository(db_ab9d6a_dbrenzContext context) : base(context)
        {
            _context = context;
        }
  
        public async Task<List<Particular>> GetAllParticularQry(ParticularSearch searchInfo)
        {
            List<Particular> particulars = await _context.Particulars
						
						.AsNoTracking().ToListAsync();
			if (!String.IsNullOrEmpty(searchInfo.Keyword))
			{
				
				particulars = particulars.Where(p => 
									p.ParticularName.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| p.ParticularDescription.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
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
            
            return particulars;
        }

		public async Task<Particular> GetParticularById(int id)
        {
            Particular particular = await _context.Particulars
                        
                        .AsNoTracking().FirstOrDefaultAsync(m => m.ParticularId == id);

            return particular;
        }

    }
}