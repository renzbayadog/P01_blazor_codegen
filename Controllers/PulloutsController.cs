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
	public class PulloutsController : ControllerBase
	{
		private readonly IPulloutRepository _pulloutRepository;

		public PulloutsController(IRepositoryWrapper repoWrapper)
		{
			_pulloutRepository = repoWrapper.Pullout_Repository;
		}

		[HttpGet]
        [Route("List/Page{currPage:int}/PageSize{pageSize:int}")]
        [Route("List")]
		public async Task<IActionResult> GetAllPullout([FromQuery] PulloutSearch searchInfo,int currPage = 1, int pageSize = 10)
		{
			if(!ModelState.IsValid) return BadRequest();

			List<Pullout> pullouts = await _pulloutRepository.GetAllPulloutQry(searchInfo);

			// Map entity model to view model
			List<PulloutVM> pulloutsVM = new List<PulloutVM>();
			
			foreach(Pullout pullout in pullouts)
			{
				pulloutsVM.Add(new PulloutVM()
				{
					PulloutId = pullout.PulloutId,
					PulloutName = pullout.PulloutName,
					PulloutDescription = pullout.PulloutDescription,
					PulloutDate = pullout.PulloutDate,
					ReceiptImage = pullout.ReceiptImage,
					BusinessValue = pullout.BusinessValue,
					PulloutPrice = pullout.PulloutPrice
				});
			}

			Pagination<PulloutVM> pagination = new Pagination<PulloutVM>(pulloutsVM, currPage, pageSize);

			return Ok(pagination);
		}

		[HttpGet]
		[Route("GetById/{id:int}")]
		public async Task<IActionResult> GetPulloutById(int id)
		{
			if (id == 0)
			{
				return NotFound("Not Found");
			}

			try
            {
				Pullout pullout = await _pulloutRepository.FindFirstAsync(m => m.PulloutId == id);

                if (pullout == null)
				{
					return NotFound("Not Found");
				}

				var opullout = new PulloutVM()
				{
					PulloutId = pullout.PulloutId,
					PulloutName = pullout.PulloutName,
					PulloutDescription = pullout.PulloutDescription,
					PulloutDate = pullout.PulloutDate,
					ReceiptImage = pullout.ReceiptImage,
					BusinessValue = pullout.BusinessValue,
					PulloutPrice = pullout.PulloutPrice
				};

				return Ok(opullout);

            }
            catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }
		}

		[HttpPost("Add")]
		public async Task<IActionResult> PostPullout(PulloutVM pullout)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			var cdnftpconfig = AppHelper.CDNFTPConfiguration;
            cdnftpconfig.FTPFolderVM = AppHelper.CDNFTPFolder;
			pullout.ReceiptImage = UploadFileHelper.UploadFile(pullout.ReceiptImage, cdnftpconfig)?.FilePath;

			Pullout pulloutToAdd = new Pullout()
			{
				PulloutName = pullout.PulloutName,
				PulloutDescription = pullout.PulloutDescription,
				PulloutDate = pullout.PulloutDate,
				ReceiptImage = pullout.ReceiptImage,
				BusinessValue = pullout.BusinessValue,
				PulloutPrice = pullout.PulloutPrice
			};

            _pulloutRepository.Add(pulloutToAdd);

            try
            {
                await _pulloutRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }

            return Ok();
		}

		[HttpPost("Update")]
		public async Task<IActionResult> PutPullout(PulloutVM pullout)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			var cdnftpconfig = AppHelper.CDNFTPConfiguration;
            cdnftpconfig.FTPFolderVM = AppHelper.CDNFTPFolder;
			pullout.ReceiptImage = UploadFileHelper.UploadFile(pullout.ReceiptImage, cdnftpconfig)?.FilePath;

			Pullout pulloutToUpdate = new Pullout()
			{
				PulloutId = pullout.PulloutId,
				PulloutName = pullout.PulloutName,
				PulloutDescription = pullout.PulloutDescription,
				PulloutDate = pullout.PulloutDate,
				ReceiptImage = pullout.ReceiptImage,
				BusinessValue = pullout.BusinessValue,
				PulloutPrice = pullout.PulloutPrice
			};

            _pulloutRepository.Update(pulloutToUpdate);

			try
            {
                await _pulloutRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }
            return Ok();
		}

		[HttpDelete("{id}/Delete")]
        public async Task<IActionResult> DeletePullout(int id)
		{
			Pullout pulloutToDelete = await _pulloutRepository.FindFirstAsync(m => m.PulloutId == id);
			
			if(pulloutToDelete == null)
				return BadRequest("Not Found");
			
            _pulloutRepository.Delete(pulloutToDelete);

			try
			{
                await _pulloutRepository.SaveChangesAsync();
			}
			catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }
            
			return Ok();
		}

		[HttpPost("delete/bulk")]
        public async Task<IActionResult> DeleteBulk([FromBody]List<int> pullouts)
        {
            if (pullouts.Count > 0)
            {
                foreach (int pullout in pullouts)
                {
					Pullout pulloutToDelete = await _pulloutRepository.FindFirstAsync(m => m.PulloutId == pullout);
					_pulloutRepository.Delete(pulloutToDelete);
                }
				try
				{
					await _pulloutRepository.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					AppHelper.LogMessage(ex.Message.ToString());
					return BadRequest(ex.Message.ToString());
				}
            }
            return Ok();
        }

		#region EXPORT TO EXCEL

        [HttpGet("export/report")]
        public async Task<IActionResult> ExportPullout([FromQuery] PulloutSearch searchInfo)
        {
			List<Pullout> pullouts = await _pulloutRepository.GetAllPulloutQry(searchInfo);

			// Map entity model to view model
			List<PulloutVM> pulloutsVM = new List<PulloutVM>();
			
			foreach(Pullout pullout in pullouts)
			{
				pulloutsVM.Add(new PulloutVM()
				{
					PulloutId = pullout.PulloutId,
					PulloutName = pullout.PulloutName,
					PulloutDescription = pullout.PulloutDescription,
					PulloutDate = pullout.PulloutDate,
					ReceiptImage = pullout.ReceiptImage,
					BusinessValue = pullout.BusinessValue,
					PulloutPrice = pullout.PulloutPrice
				});
			}
 
            DataTable dt = new DataTable("Pullout");
            dt.Columns.Add("PulloutId", typeof(string));
						dt.Columns.Add("PulloutName", typeof(string));
						dt.Columns.Add("PulloutDescription", typeof(string));
						dt.Columns.Add("PulloutDate", typeof(string));
						dt.Columns.Add("ReceiptImage", typeof(string));
						dt.Columns.Add("BusinessValue", typeof(string));
						dt.Columns.Add("PulloutPrice", typeof(string));

            DataRow dr;

            foreach (var item in pulloutsVM)
            {
                dr = dt.NewRow();

                dr[0] = item.PulloutId;
						dr[1] = item.PulloutName;
						dr[2] = item.PulloutDescription;
						dr[3] = item.PulloutDate;
						dr[4] = item.ReceiptImage;
						dr[5] = item.BusinessValue;
						dr[6] = item.PulloutPrice;

                dt.Rows.Add(dr);
            }

            var exportExcelHelperService = new ExportExcelHelper();

            var bytes = Convert.ToBase64String(exportExcelHelperService.CreateExcelWorkBook(dt));

            var data = new ExcelData();
			data.Filename = "Pullout Record";
            data.File = bytes;
            return Ok(data);
        }
        #endregion

	}
}