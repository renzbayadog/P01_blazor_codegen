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
    public interface IParticularRepository : IRepositoryBase<Particular>
    {
        Task<List<Particular>> GetAllParticularQry(ParticularSearch searchInfo);
        Task<Particular> GetParticularById(int id);
    }
}