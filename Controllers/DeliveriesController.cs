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
	public class DeliveriesController : ControllerBase
	{
		private readonly IDeliveryRepository _deliveryRepository;

		public DeliveriesController(IRepositoryWrapper repoWrapper)
		{
			_deliveryRepository = repoWrapper.Delivery_Repository;
		}

		[HttpGet]
        [Route("List/Page{currPage:int}/PageSize{pageSize:int}")]
        [Route("List")]
		public async Task<IActionResult> GetAllDelivery([FromQuery] DeliverySearch searchInfo,int currPage = 1, int pageSize = 10)
		{
			if(!ModelState.IsValid) return BadRequest();

			List<Delivery> deliveries = await _deliveryRepository.GetAllDeliveryQry(searchInfo);

			// Map entity model to view model
			List<DeliveryVM> deliveriesVM = new List<DeliveryVM>();
			
			foreach(Delivery delivery in deliveries)
			{
				deliveriesVM.Add(new DeliveryVM()
				{
					DeliveryId = delivery.DeliveryId,
					DeliveryName = delivery.DeliveryName,
					DeliveryAddress = delivery.DeliveryAddress,
					DeliveryDate = delivery.DeliveryDate,
					BusinessValue = delivery.BusinessValue
				});
			}

			Pagination<DeliveryVM> pagination = new Pagination<DeliveryVM>(deliveriesVM, currPage, pageSize);

			return Ok(pagination);
		}

		[HttpGet]
		[Route("GetById/{id:int}")]
		public async Task<IActionResult> GetDeliveryById(int id)
		{
			if (id == 0)
			{
				return NotFound("Not Found");
			}

			try
            {
				Delivery delivery = await _deliveryRepository.FindFirstAsync(m => m.DeliveryId == id);

                if (delivery == null)
				{
					return NotFound("Not Found");
				}

				var odelivery = new DeliveryVM()
				{
					DeliveryId = delivery.DeliveryId,
					DeliveryName = delivery.DeliveryName,
					DeliveryAddress = delivery.DeliveryAddress,
					DeliveryDate = delivery.DeliveryDate,
					BusinessValue = delivery.BusinessValue
				};

				return Ok(odelivery);

            }
            catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }
		}

		[HttpPost("Add")]
		public async Task<IActionResult> PostDelivery(DeliveryVM delivery)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			var cdnftpconfig = AppHelper.CDNFTPConfiguration;
            cdnftpconfig.FTPFolderVM = AppHelper.CDNFTPFolder;
			

			Delivery deliveryToAdd = new Delivery()
			{
				DeliveryName = delivery.DeliveryName,
				DeliveryAddress = delivery.DeliveryAddress,
				DeliveryDate = delivery.DeliveryDate,
				BusinessValue = delivery.BusinessValue
			};

            _deliveryRepository.Add(deliveryToAdd);

            try
            {
                await _deliveryRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }

            return Ok();
		}

		[HttpPost("Update")]
		public async Task<IActionResult> PutDelivery(DeliveryVM delivery)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest();
            }

			var cdnftpconfig = AppHelper.CDNFTPConfiguration;
            cdnftpconfig.FTPFolderVM = AppHelper.CDNFTPFolder;
			

			Delivery deliveryToUpdate = new Delivery()
			{
				DeliveryId = delivery.DeliveryId,
				DeliveryName = delivery.DeliveryName,
				DeliveryAddress = delivery.DeliveryAddress,
				DeliveryDate = delivery.DeliveryDate,
				BusinessValue = delivery.BusinessValue
			};

            _deliveryRepository.Update(deliveryToUpdate);

			try
            {
                await _deliveryRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }
            return Ok();
		}

		[HttpDelete("{id}/Delete")]
        public async Task<IActionResult> DeleteDelivery(int id)
		{
			Delivery deliveryToDelete = await _deliveryRepository.FindFirstAsync(m => m.DeliveryId == id);
			
			if(deliveryToDelete == null)
				return BadRequest("Not Found");
			
            _deliveryRepository.Delete(deliveryToDelete);

			try
			{
                await _deliveryRepository.SaveChangesAsync();
			}
			catch (Exception ex)
            {
                AppHelper.LogMessage(ex.Message.ToString());
				return BadRequest(ex.Message.ToString());
            }
            
			return Ok();
		}

		[HttpPost("delete/bulk")]
        public async Task<IActionResult> DeleteBulk([FromBody]List<int> deliveries)
        {
            if (deliveries.Count > 0)
            {
                foreach (int delivery in deliveries)
                {
					Delivery deliveryToDelete = await _deliveryRepository.FindFirstAsync(m => m.DeliveryId == delivery);
					_deliveryRepository.Delete(deliveryToDelete);
                }
				try
				{
					await _deliveryRepository.SaveChangesAsync();
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
        public async Task<IActionResult> ExportDelivery([FromQuery] DeliverySearch searchInfo)
        {
			List<Delivery> deliveries = await _deliveryRepository.GetAllDeliveryQry(searchInfo);

			// Map entity model to view model
			List<DeliveryVM> deliveriesVM = new List<DeliveryVM>();
			
			foreach(Delivery delivery in deliveries)
			{
				deliveriesVM.Add(new DeliveryVM()
				{
					DeliveryId = delivery.DeliveryId,
					DeliveryName = delivery.DeliveryName,
					DeliveryAddress = delivery.DeliveryAddress,
					DeliveryDate = delivery.DeliveryDate,
					BusinessValue = delivery.BusinessValue
				});
			}
 
            DataTable dt = new DataTable("Delivery");
            dt.Columns.Add("DeliveryId", typeof(string));
						dt.Columns.Add("DeliveryName", typeof(string));
						dt.Columns.Add("DeliveryAddress", typeof(string));
						dt.Columns.Add("DeliveryDate", typeof(string));
						dt.Columns.Add("BusinessValue", typeof(string));

            DataRow dr;

            foreach (var item in deliveriesVM)
            {
                dr = dt.NewRow();

                dr[0] = item.DeliveryId;
						dr[1] = item.DeliveryName;
						dr[2] = item.DeliveryAddress;
						dr[3] = item.DeliveryDate;
						dr[4] = item.BusinessValue;

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