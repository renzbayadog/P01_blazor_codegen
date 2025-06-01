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
	public class TitlesController : ControllerBase
	{
		private readonly ITitleRepository _titleRepository;

		public TitlesController(IRepositoryWrapper repoWrapper)
		{
			_titleRepository = repoWrapper.Title_Repository;
		}

		[HttpGet]
        [Route("List/Page{currPage:int}/PageSize{pageSize:int}")]
        [Route("List")]
		public async Task<IActionResult> GetAllTitle([FromQuery] TitleSearch searchInfo,int currPage = 1, int pageSize = 10)
		{
			if(!ModelState.IsValid) return BadRequest();

			List<Title> titles = await _titleRepository.GetAllTitleQry(searchInfo);

			// Map entity model to view model
			List<TitleVM> titlesVM = new List<TitleVM>();
			
			foreach(Title title in titles)
			{
				titlesVM.Add(new TitleVM()
				{
					TitleId = title.TitleId,
					TitleName = title.TitleName,
					TItleDescription = title.TItleDescription,
					ProductLineId = title.ProductLine?.ProductLineId,
					ProductLineCode = title.ProductLine?.ProductLineCode,
					ProductLineName = title.ProductLine?.ProductLineName
				});
			}

			Pagination<TitleVM> pagination = new Pagination<TitleVM>(titlesVM, currPage, pageSize);

			return Ok(pagination);
		}

		[HttpGet]
		[Route("GetById/{id:int}")]
		public async Task<IActionResult> GetTitleById(int id)
		{
			if (id == 0)
			{
				return NotFound("Not Found");
			}

			try
            {
				Title title = await _titleRepository.GetTitleById(id);

                if (title == null)
				{
					return NotFound("Not Found");
				}

				var otitle = new TitleVM()
				{
					TitleId = title.TitleId,
					TitleName = title.TitleName,
					TItleDescription = title.TItleDescription,
					ProductLineId = title.ProductLine?.ProductLineId,
					ProductLineCode = title.ProductLine?.ProductLineCode,
					ProductLineName = title.ProductLine?.ProductLineName
				};

				return Ok(otitle);

            }
            catch (Exception ex)
            {
				AppHelper.LogMessage(ex.InnerException.ToString());
				return BadRequest(ex.InnerException.ToString());
            }
		}

		[HttpPost("Add")]
		public async Task<IActionResult> PostTitle(TitleVM title)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			var cdnftpconfig = AppHelper.CDNFTPConfiguration;
            cdnftpconfig.FTPFolderVM = AppHelper.CDNFTPFolder;
			

			Title titleToAdd = new Title()
			{
				TitleName = title.TitleName,
				TItleDescription = title.TItleDescription,
				ProductLineId = title.ProductLineId
			};

            _titleRepository.Add(titleToAdd);

            try
            {
                await _titleRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
				AppHelper.LogMessage(ex.InnerException.ToString());
				return BadRequest(ex.InnerException.ToString());
            }

            return Ok();
		}

		[HttpPost("Update")]
		public async Task<IActionResult> PutTitle(TitleVM title)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			var cdnftpconfig = AppHelper.CDNFTPConfiguration;
            cdnftpconfig.FTPFolderVM = AppHelper.CDNFTPFolder;
			

			Title titleToUpdate = new Title()
			{
				TitleId = title.TitleId,
				TitleName = title.TitleName,
				TItleDescription = title.TItleDescription,
				ProductLineId = title.ProductLineId
			};

            _titleRepository.Update(titleToUpdate);

			try
            {
                await _titleRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
				AppHelper.LogMessage(ex.InnerException.ToString());
				return BadRequest(ex.InnerException.ToString());
            }
            return Ok();
		}

		[HttpDelete("{id}/Delete")]
        public async Task<IActionResult> DeleteTitle(int id)
		{
			Title titleToDelete = await _titleRepository.FindFirstAsync(m => m.TitleId == id);
			
			if(titleToDelete == null)
				return BadRequest("Not Found");
			
            _titleRepository.Delete(titleToDelete);

			try
			{
                await _titleRepository.SaveChangesAsync();
			}
			catch (Exception ex)
            {
				AppHelper.LogMessage(ex.InnerException.ToString());
				return BadRequest(ex.InnerException.ToString());
            }
            
			return Ok();
		}

		[HttpPost("delete/bulk")]
        public async Task<IActionResult> DeleteBulk([FromBody]List<int> titles)
        {
            if (titles.Count > 0)
            {
                foreach (int title in titles)
                {
					Title titleToDelete = await _titleRepository.FindFirstAsync(m => m.TitleId == title);
					_titleRepository.Delete(titleToDelete);
                }
				try
				{
					await _titleRepository.SaveChangesAsync();
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
        public async Task<IActionResult> ExportTitle([FromQuery] TitleSearch searchInfo)
        {
			List<Title> titles = await _titleRepository.GetAllTitleQry(searchInfo);

			// Map entity model to view model
			List<TitleVM> titlesVM = new List<TitleVM>();
			
			foreach(Title title in titles)
			{
				titlesVM.Add(new TitleVM()
				{
					TitleId = title.TitleId,
					TitleName = title.TitleName,
					TItleDescription = title.TItleDescription,
					ProductLineId = title.ProductLine?.ProductLineId,
					ProductLineCode = title.ProductLine?.ProductLineCode,
					ProductLineName = title.ProductLine?.ProductLineName
				});
			}
 
            DataTable dt = new DataTable("Title");
            dt.Columns.Add("TitleId", typeof(string));
						dt.Columns.Add("TitleName", typeof(string));
						dt.Columns.Add("TItleDescription", typeof(string));
						dt.Columns.Add("ProductLineId", typeof(string));
						dt.Columns.Add("ProductLineCode", typeof(string));
						dt.Columns.Add("ProductLineName", typeof(string));

            DataRow dr;

            foreach (var item in titlesVM)
            {
                dr = dt.NewRow();

                dr[0] = item.TitleId;
						dr[1] = item.TitleName;
						dr[2] = item.TItleDescription;
						dr[3] = item.ProductLineId;
						dr[4] = item.ProductLineCode;
						dr[5] = item.ProductLineName;

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