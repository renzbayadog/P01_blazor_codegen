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
    internal class DeliveryRepository : RepositoryBase<Delivery>, IDeliveryRepository
    {
        private readonly db_ab9d6a_dbrenzContext _context;

        public DeliveryRepository(db_ab9d6a_dbrenzContext context) : base(context)
        {
            _context = context;
        }
  
        public async Task<List<Delivery>> GetAllDeliveryQry(DeliverySearch searchInfo)
        {
            List<Delivery> deliveries = await _context.Deliveries
						
						.AsNoTracking().ToListAsync();
			if (!String.IsNullOrEmpty(searchInfo.Keyword))
			{
				
				deliveries = deliveries.Where(d => 
									d.DeliveryName.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| d.DeliveryAddress.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
									|| d.DeliveryDate.ToString().ToUpper().Contains(searchInfo.Keyword.ToUpper())
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
            
            return deliveries;
        }

    }
}