using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

using RenzGrandWeddingblazor.ph.ViewModels; 
using RenzGrandWeddingblazor.ph.Data.Entities; 
using RenzGrandWeddingblazor.ph.Data; 
using RenzGrandWeddingblazor.ph.Data.Repositories; 
using codegeneratorlib.Helpers; 

using RenzGrandWeddingblazor.ph.Middleware;

namespace RenzGrandWeddingblazor.ph.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ParticularsController : ControllerBase
	{
		private readonly IParticularRepository _particularRepository;

		public ParticularsController(IRepositoryWrapper repoWrapper)
		{
			_particularRepository = repoWrapper.Particular_Repository;
		}

		[HttpGet]
        [Route("List/Page{currPage:int}/PageSize{pageSize:int}")]
        [Route("List")]
		public async Task<IActionResult> GetAllParticular([FromQuery] ParticularSearch searchInfo,int currPage = 1, int pageSize = 10)
		{
			if(!ModelState.IsValid) return BadRequest();

			List<Particular> particulars = await _particularRepository.GetAllParticularQry(searchInfo);

			// Map entity model to view model
			List<ParticularVM> particularsVM = new List<ParticularVM>();
			
			foreach(Particular particular in particulars)
			{
				particularsVM.Add(new ParticularVM()
				{
					ParticularId = particular.ParticularId,
					ParticularName = particular.ParticularName,
					ParticularDescription = particular.ParticularDescription
				});
			}

			Pagination<ParticularVM> pagination = new Pagination<ParticularVM>(particularsVM, currPage, pageSize);

			return Ok(pagination);
		}

		[HttpGet]
		[Route("GetById/{id:int}")]
		public async Task<IActionResult> GetParticularById(int id)
		{
			if (id == 0)
			{
				return NotFound("Not Found");
			}

			try
            {
				Particular particular = await _particularRepository.GetParticularById(id);

                if (particular == null)
				{
					return NotFound("Not Found");
				}

				var oparticular = new ParticularVM()
				{
					ParticularId = particular.ParticularId,
					ParticularName = particular.ParticularName,
					ParticularDescription = particular.ParticularDescription
				};

				return Ok(oparticular);

            }
            catch (Exception ex)
            {
				AppHelper.LogMessage(ex.InnerException.ToString());
				return BadRequest(ex.InnerException.ToString());
            }
		}

		[HttpPost("Add")]
		public async Task<IActionResult> PostParticular(ParticularVM particular)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			var cdnftpconfig = AppHelper.CDNFTPConfiguration;
            cdnftpconfig.FTPFolderVM = AppHelper.CDNFTPFolder;
			

			Particular particularToAdd = new Particular()
			{
				ParticularName = particular.ParticularName,
				ParticularDescription = particular.ParticularDescription
			};

            _particularRepository.Add(particularToAdd);

            try
            {
                await _particularRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
				AppHelper.LogMessage(ex.InnerException.ToString());
				return BadRequest(ex.InnerException.ToString());
            }

            return Ok();
		}

		[HttpPost("Update")]
		public async Task<IActionResult> PutParticular(ParticularVM particular)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			var cdnftpconfig = AppHelper.CDNFTPConfiguration;
            cdnftpconfig.FTPFolderVM = AppHelper.CDNFTPFolder;
			

			Particular particularToUpdate = new Particular()
			{
				ParticularId = particular.ParticularId,
				ParticularName = particular.ParticularName,
				ParticularDescription = particular.ParticularDescription
			};

            _particularRepository.Update(particularToUpdate);

			try
            {
                await _particularRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
				AppHelper.LogMessage(ex.InnerException.ToString());
				return BadRequest(ex.InnerException.ToString());
            }
            return Ok();
		}

		[HttpDelete("{id}/Delete")]
        public async Task<IActionResult> DeleteParticular(int id)
		{
			Particular particularToDelete = await _particularRepository.FindFirstAsync(m => m.ParticularId == id);
			
			if(particularToDelete == null)
				return BadRequest("Not Found");
			
            _particularRepository.Delete(particularToDelete);

			try
			{
                await _particularRepository.SaveChangesAsync();
			}
			catch (Exception ex)
            {
				AppHelper.LogMessage(ex.InnerException.ToString());
				return BadRequest(ex.InnerException.ToString());
            }
            
			return Ok();
		}

		[HttpPost("delete/bulk")]
        public async Task<IActionResult> DeleteBulk([FromBody]List<int> particulars)
        {
            if (particulars.Count > 0)
            {
                foreach (int particular in particulars)
                {
					Particular particularToDelete = await _particularRepository.FindFirstAsync(m => m.ParticularId == particular);
					_particularRepository.Delete(particularToDelete);
                }
				try
				{
					await _particularRepository.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					AppHelper.LogMessage(ex.InnerException.ToString());
					return BadRequest(ex.InnerException.ToString());
				}
            }
            return Ok();
        }

		#region EXPORT TO EXCEL

        [HttpGet("export/report")]
        public async Task<IActionResult> ExportParticular([FromQuery] ParticularSearch searchInfo)
        {
			List<Particular> particulars = await _particularRepository.GetAllParticularQry(searchInfo);

			// Map entity model to view model
			List<ParticularVM> particularsVM = new List<ParticularVM>();
			
			foreach(Particular particular in particulars)
			{
				particularsVM.Add(new ParticularVM()
				{
					ParticularId = particular.ParticularId,
					ParticularName = particular.ParticularName,
					ParticularDescription = particular.ParticularDescription
				});
			}
 
            DataTable dt = new DataTable("Particular");
            dt.Columns.Add("ParticularId", typeof(string));
						dt.Columns.Add("ParticularName", typeof(string));
						dt.Columns.Add("ParticularDescription", typeof(string));

            DataRow dr;

            foreach (var item in particularsVM)
            {
                dr = dt.NewRow();

                dr[0] = item.ParticularId;
						dr[1] = item.ParticularName;
						dr[2] = item.ParticularDescription;

                dt.Rows.Add(dr);
            }

            var exportExcelHelperService = new ExportExcelHelper();

            var bytes = Convert.ToBase64String(exportExcelHelperService.CreateExcelWorkBook(dt));

            var data = new ExcelData();
			data.Filename = "Particular Extracted Data";
            data.File = bytes;
            return Ok(data);
        }
        #endregion

	}
}