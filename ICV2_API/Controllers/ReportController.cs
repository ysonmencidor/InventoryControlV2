using DataAccessLibrary.Models.QneReportFilter;
using DataAccessLibrary.Models.QneServices;
using ICV2_API.Helpers;
using Microsoft.AspNetCore.Cors;
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
    public class ReportController : ControllerBase
    {
        private readonly QneReportService qneReportService;

        public ReportController(QneReportService qneReportService)
        {
            this.qneReportService = qneReportService;
        }

        #region Will to later
        //[Route("GenerateBatchNoBalDetails")]
        //[HttpPost]
        //public async Task<IActionResult> GenerateBatchNoBal([FromBody] DTParameters dtParameters)
        //{
        //    BATCHNOBALANCEDETAILS filter = new BATCHNOBALANCEDETAILS
        //    {
        //        CompanyCode = dtParameters.AdditionalValues[0],
        //        StockGroup = dtParameters.AdditionalValues[1],
        //        Location = dtParameters.AdditionalValues[2],
        //        IncludeZeroBalance = Convert.ToBoolean(dtParameters.AdditionalValues[3])
        //    };

        //    BATCHNOBALANCEDETAILS filter = new BATCHNOBALANCEDETAILS();
        //    filter.CompanyCode = "01";
        //    filter.IncludeZeroBalance = true;

        //    var result = await qneReportService.GenerateBatchNoBalDetails(filter);

        //    var searchBy = dtParameters.Search?.Value;

        //    var orderCriteria = string.Empty;
        //    var orderAscendingDirection = true;

        //    if (dtParameters.Order != null)
        //    {
        //         in this example we just default sort on the 1st column
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }
        //    else
        //    {
        //        if we have an empty search then just order the results by Id ascending
        //        orderCriteria = "stockCode";
        //        orderAscendingDirection = false;
        //    }

        //    if (!string.IsNullOrEmpty(searchBy))
        //    {
        //        result = result.Where(r => r.Batchno != null && r.Batchno.ToUpper().Contains(searchBy.ToUpper()) ||
        //                                   r.StockCode != null && r.StockCode.ToUpper().Contains(searchBy.ToUpper())).ToList();
        //    }

        //    result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

        //    now just get the count of items(without the skip and take) - eg how many could be returned with filtering
        //  var filteredResultsCount = result.Count();
        //    var totalResultsCount = result.Count();

        //    return new JsonResult(new
        //    {
        //        draw = dtParameters.Draw,
        //        recordsTotal = totalResultsCount,
        //        recordsFiltered = filteredResultsCount,
        //        data = result
        //        .Skip(dtParameters.Start)
        //        .Take(dtParameters.Length)
        //        .ToList()
        //    });


        //}

        #endregion
        [Route("GenerateBatchNoBalDetails")]
        [HttpPost]
        public async Task<IActionResult> GenerateBatchNoBal(BATCHNOBALANCEDETAILSFILTER filter)
        {
            if (filter != null)
            {
                if (filter.CompanyCode != "404")
                {
                    var model = await qneReportService.GenerateBatchNoBalDetails(filter);
                    if (model != null)
                    {
                        return Ok(model);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            return BadRequest();
        }
        [Route("GenerateMaterialConsumptions")]
        [HttpPost]
        public async Task<IActionResult> GenerateMaterialConsumptions(MATERIALCONSUMPTIONFILTER filter)
        {
            if (filter != null)
            {
                if (filter.CompanyCode != "404")
                {
                    var model = await qneReportService.GenerateMaterialConsumptions(filter);
                    if (model != null)
                    {
                        return Ok(model);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            return BadRequest();
        }

        [Route("GenerateCustomerStatement")]
        [HttpPost]
        public async Task<IActionResult> GenerateCustomerStatement(CUSTOMERSTATEMENTFILTER filter)
        {
            if (filter != null)
            {
                if(filter.CompanyCode != "404")
                {
                    var model = await qneReportService.GenerateCustomerStatement(filter);
                    if (model != null)
                    {
                        return Ok(model);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            return BadRequest();
       
        }

       [Route("GenerateSalesAnalysis")]
       [HttpPost]
       public async Task<IActionResult> GenerateSalesAnalysis(SALESANALYSISFILTER filter)
        {
            if (filter != null)
            {
                if (filter.CompanyCode != "404")
                {
                    var model = await qneReportService.GenerateSalesAnalysis(filter);
                    if (model != null)
                    {
                        return Ok(model);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            return BadRequest();
        }
        [Route("GenerateCategorizeReport")]
        [HttpPost]
        public async Task<IActionResult> GenerateCategorizeReport(CATEGORIZEFILTER filter)
        {
            if (filter != null)
            {
                if(filter.CompanyCode != "404")
                {
                    var model = await qneReportService.GenerateCategorizeReport(filter);
                    if(model != null)
                    {
                        return Ok(model);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            return BadRequest();
        }

        [Route("GenerateStockIssueDetail")]
        [HttpPost]
        public async Task<IActionResult> GenerateStockIssueDetail(STOCKISSUEFILTER filter)
        {
            if (filter != null)
            {
                if (filter.CompanyCode != "404")
                {
                    var model = await qneReportService.GenerateStockissuesDetail(filter);
                    if (model != null)
                    {
                        return Ok(model);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }


        [Route("GenerateNearExp")]
        [HttpPost]
        public async Task<IActionResult> GenerateNearExp(NEAREXPIRYFILTER filter)
        {
            if (filter != null)
            {
                if (filter.CompanyCode != "404")
                {
                    var model = await qneReportService.GenerateNearExpiry(filter);
                    if (model != null)
                    {
                        return Ok(model);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [Route("GenerateStockLedgerInq")]
        [HttpPost]
        public async Task<IActionResult> GenerateStockLedgerInq(STOCKLEDGERINQUIRYFILTER filter)
        {
            if (filter != null)
            {
                if (filter.CompanyCode != "404")
                {
                    var model = await qneReportService.GenerateStockLedgerInq(filter);
                    if (model != null)
                    {
                        return Ok(model);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [Route("GenerateStockLedgerWBatch")]
        [HttpPost]
        public async Task<IActionResult> GenerateStockLedgerWBatch(STOCKLEDGERWITHBATCHFILTER filter)
        {
            if (filter != null)
            {
                if (filter.CompanyCode != "404")
                {
                    var model = await qneReportService.GenerateStockLedgerWBatch(filter);

                    if (model != null)
                    {
                        return Ok(model);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [Route("GenerateInvoiceMatchListing")]
        [HttpPost]
        public async Task<IActionResult> GenerateInvoiceMatchListing(INVOICEMATCHLISTINGFILTER filter)
        {
            if (filter != null)
            {
                if (filter.CompanyCode != "404")
                {
                    var model = await qneReportService.GenerateInvoiceMatchListing(filter);

                    if (model != null)
                    {
                        return Ok(model);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [Route("GenerateDepartmentLedger")]
        [HttpPost]
        public async Task<IActionResult> GenerateDepartmentLedger(DEPARTMENTLEDGERFILTER filter)
        {
            if (filter != null)
            {
                if (filter.CompanyCode != "404")
                {
                    var model = await qneReportService.GenerateDepartmentLedger(filter);

                    if (model != null)
                    {
                        return Ok(model);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        
    }
}
