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
	public class ProductLinesController : ControllerBase
	{
		private readonly IProductLineRepository _productlineRepository;

		public ProductLinesController(IRepositoryWrapper repoWrapper)
		{
			_productlineRepository = repoWrapper.ProductLine_Repository;
		}

		[HttpGet]
        [Route("List/Page{currPage:int}/PageSize{pageSize:int}")]
        [Route("List")]
		public async Task<IActionResult> GetAllProductLine([FromQuery] ProductLineSearch searchInfo,int currPage = 1, int pageSize = 10)
		{
			if(!ModelState.IsValid) return BadRequest();

			List<ProductLine> productlines = await _productlineRepository.GetAllProductLineQry(searchInfo);

			// Map entity model to view model
			List<ProductLineVM> productlinesVM = new List<ProductLineVM>();
			
			foreach(ProductLine productline in productlines)
			{
				productlinesVM.Add(new ProductLineVM()
				{
					ProductLineId = productline.ProductLineId,
					ProductLineCode = productline.ProductLineCode,
					ProductLineName = productline.ProductLineName
				});
			}

			Pagination<ProductLineVM> pagination = new Pagination<ProductLineVM>(productlinesVM, currPage, pageSize);

			return Ok(pagination);
		}

		[HttpGet]
		[Route("GetById/{id:int}")]
		public async Task<IActionResult> GetProductLineById(int id)
		{
			if (id == 0)
			{
				return NotFound("Not Found");
			}

			try
            {
				ProductLine productline = await _productlineRepository.FindFirstAsync(m => m.ProductLineId == id);

                if (productline == null)
				{
					return NotFound("Not Found");
				}

				var oproductline = new ProductLineVM()
				{
					ProductLineId = productline.ProductLineId,
					ProductLineCode = productline.ProductLineCode,
					ProductLineName = productline.ProductLineName
				};

				return Ok(oproductline);

            }
            catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }
		}

		[HttpPost("Add")]
		public async Task<IActionResult> PostProductLine(ProductLineVM productline)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			var cdnftpconfig = AppHelper.CDNFTPConfiguration;
            cdnftpconfig.FTPFolderVM = AppHelper.CDNFTPFolder;
			

			ProductLine productlineToAdd = new ProductLine()
			{
				ProductLineCode = productline.ProductLineCode,
				ProductLineName = productline.ProductLineName
			};

            _productlineRepository.Add(productlineToAdd);

            try
            {
                await _productlineRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }

            return Ok();
		}

		[HttpPost("Update")]
		public async Task<IActionResult> PutProductLine(ProductLineVM productline)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			var cdnftpconfig = AppHelper.CDNFTPConfiguration;
            cdnftpconfig.FTPFolderVM = AppHelper.CDNFTPFolder;
			

			ProductLine productlineToUpdate = new ProductLine()
			{
				ProductLineId = productline.ProductLineId,
				ProductLineCode = productline.ProductLineCode,
				ProductLineName = productline.ProductLineName
			};

            _productlineRepository.Update(productlineToUpdate);

			try
            {
                await _productlineRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }
            return Ok();
		}

		[HttpDelete("{id}/Delete")]
        public async Task<IActionResult> DeleteProductLine(int id)
		{
			ProductLine productlineToDelete = await _productlineRepository.FindFirstAsync(m => m.ProductLineId == id);
			
			if(productlineToDelete == null)
				return BadRequest("Not Found");
			
            _productlineRepository.Delete(productlineToDelete);

			try
			{
                await _productlineRepository.SaveChangesAsync();
			}
			catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }
            
			return Ok();
		}

		[HttpPost("delete/bulk")]
        public async Task<IActionResult> DeleteBulk([FromBody]List<int> productlines)
        {
            if (productlines.Count > 0)
            {
                foreach (int productline in productlines)
                {
					ProductLine productlineToDelete = await _productlineRepository.FindFirstAsync(m => m.ProductLineId == productline);
					_productlineRepository.Delete(productlineToDelete);
                }
				try
				{
					await _productlineRepository.SaveChangesAsync();
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
        public async Task<IActionResult> ExportProductLine([FromQuery] ProductLineSearch searchInfo)
        {
			List<ProductLine> productlines = await _productlineRepository.GetAllProductLineQry(searchInfo);

			// Map entity model to view model
			List<ProductLineVM> productlinesVM = new List<ProductLineVM>();
			
			foreach(ProductLine productline in productlines)
			{
				productlinesVM.Add(new ProductLineVM()
				{
					ProductLineId = productline.ProductLineId,
					ProductLineCode = productline.ProductLineCode,
					ProductLineName = productline.ProductLineName
				});
			}
 
            DataTable dt = new DataTable("ProductLine");
            dt.Columns.Add("ProductLineId", typeof(string));
						dt.Columns.Add("ProductLineCode", typeof(string));
						dt.Columns.Add("ProductLineName", typeof(string));

            DataRow dr;

            foreach (var item in productlinesVM)
            {
                dr = dt.NewRow();

                dr[0] = item.ProductLineId;
						dr[1] = item.ProductLineCode;
						dr[2] = item.ProductLineName;

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