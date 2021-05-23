using DataAccessLibrary.Models.QneModels;
using DataAccessLibrary.Models.QneReportFilter;
using DataAccessLibrary.Models.QneServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICV2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QneController : ControllerBase
    {
        private readonly QneDataService qneDataService;

        public QneController(QneDataService qneDataService)
        {
            this.qneDataService = qneDataService;
        }

        [Route("GetStockItems")]
        [HttpGet]
        public async Task<IActionResult> GetStocks(string companyCode)
        {
            var data = await qneDataService.GetStocks(companyCode);

            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }
        [Route("GetStockGroups")]
        [HttpGet]
        public async Task<IEnumerable<StockGroups>> GetStockGroup(string companyCode)
        {
            return await qneDataService.GetStockGroup(companyCode);
        }
        [Route("GetStockLocations")]
        [HttpGet]
        public async Task<IEnumerable<StockLocations>> GetStockLocation(string companyCode)
        {
           return await qneDataService.GetStockLocations(companyCode);
        }
        [Route("GetDebtors")]
        [HttpGet]
        public async Task<IEnumerable<Debtors>> GetDebtor(string companyCode)
        {
            return await qneDataService.GetDebtors(companyCode);
        }
        [Route("DebtorCategories")]
        [HttpGet]
        public async Task<IEnumerable<DebtorCategory>> GetDebtorCategory(string companyCode)
        {
            return await qneDataService.GetDebtorCategory(companyCode);
        }

        [Route("GetSalesPersons")]
        [HttpGet]
        public async Task<IEnumerable<SalesPersons>> GetSalesPerson(string companyCode)
        {
            return await qneDataService.GetSalesPersons(companyCode);
        }
        [Route("GetAreas")]
        [HttpGet]
        public async Task<IEnumerable<Areas>> GetArea(string companyCode)
        {
            return await qneDataService.GetAreas(companyCode);
        }
        [Route("GetFG")]
        [HttpGet]
        public async Task<IEnumerable<FGMASTERFILEMODEL>> GetFG(string companyCode)
        {
            return await qneDataService.GetFGMasterFile(companyCode);
        }
        [Route("AddUpdateFG")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateFG(FGMASTERFILEFILTER filter)
        {
            int AffectedRows = await qneDataService.InsertUpdateFGMASTERFILE(filter);

            if (AffectedRows > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
        [Route("FindFGByCode")]
        [HttpGet]
        public async Task<FGMASTERFILEMODEL> FindFGByCode(string companyCode,string stockCode)
        {
            return await qneDataService.GetFGByCode(companyCode,stockCode);
        }

        [Route("StockIssues")]
        [HttpGet]
        public async Task<IEnumerable<StockIssue>> LoadStockIssues(string companyCode)
        {
            return await qneDataService.GetStockIssuesAsync(companyCode);
        }
        [Route("GetBatchByStockId")]
        [HttpGet]
        public async Task<IActionResult> GetBatchByStockId(string companyCode,Guid StockId)
        {
             var model = await qneDataService.GetBatchNumByStockId(companyCode, StockId);
             if (model != null)
             {
                    return Ok(model);
             }
            return BadRequest();
        }
    }
}
