using Microsoft.AspNetCore.Mvc;
using FumicertiApi.Models;
using FumicertiApi.DTOs.Certi;
using FumicertiApi.Data;
using Sieve.Services;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Microsoft.AspNetCore.Authorization;


namespace FumicertiApi.Controllers
{
        [Authorize]
        [ApiController]
        [Route("api/[controller]")]
        public class CertiController : BaseController
        {
            private readonly AppDbContext _context;
            private readonly ISieveProcessor _sieveProcessor;

            public CertiController(AppDbContext context, ISieveProcessor sieveProcessor)
                {
                    _context = context;
                _sieveProcessor = sieveProcessor;
            }

            [HttpGet]
            public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
            {
                var currentPage = sieveModel.Page ?? 1;
                var pageSize = sieveModel.PageSize ?? 10;

                var query = _context.Certi.AsNoTracking();

                // Apply filtering and sorting without pagination first
                var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

                var totalRecords = await filteredQuery.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                var pagedCertis = await filteredQuery
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new CertiReadDto
                    {
                        CertiId = c.CertiId,
                        CertiPhyto = c.CertiPhyto,
                        CertiJobType = c.CertiJobType,
                        CertiOrderId = c.CertiOrderId,
                        CertiBranchId = c.CertiBranchId,
                        CertiProductType = c.CertiProductType,
                        CertiType = c.CertiType,
                        CertiJobfor = c.CertiJobfor,
                        CertiNo = c.CertiNo,
                        CertiDate = c.CertiDate,
                        CertiFumidate = c.CertiFumidate,
                        CertiPol = c.CertiPol,
                        CertiPod = c.CertiPod,
                        CertiImpcountry = c.CertiImpcountry,
                        CertiFumiplace = c.CertiFumiplace,
                        CertiUndersheet = c.CertiUndersheet,
                        CertiFumiduration = c.CertiFumiduration,
                        CertiDoseRate = c.CertiDoseRate,
                        CertiPresserTested = c.CertiPresserTested,
                        CertiTemperature = c.CertiTemperature,
                        CertiHumidity = c.CertiHumidity,
                        CertiContainers = c.CertiContainers,
                        CertiContainerCount = c.CertiContainerCount,
                        CertiContainerSize = c.CertiContainerSize,
                        CertiInvoiceNo = c.CertiInvoiceNo,
                        CertiInvoiceDate = c.CertiInvoiceDate,
                        CertiAfoName = c.CertiAfoName,
                        CertiRemarks = c.CertiRemarks,
                        CertiExpName = c.CertiExpName,
                        CertiExpAddress = c.CertiExpAddress,
                        CertiExpEmail = c.CertiExpEmail,
                        CertiConsignee = c.CertiConsignee,
                        CertiConsigneeAddress = c.CertiConsigneeAddress,
                        CertiNotifyParty = c.CertiNotifyParty,
                        CertiNotifyAddress = c.CertiNotifyAddress,
                        CertiCargoDesc = c.CertiCargoDesc,
                        CertiNetQty = c.CertiNetQty,
                        CertilGrossQty = c.CertilGrossQty == null ? null : c.CertilGrossQty.Value,
                        CertiNetUnit = c.CertiNetUnit,
                        CertiGrossUnit = c.CertiGrossUnit,
                        CertiNoBags = c.CertiNoBags,
                        CertiPackingDesc = c.CertiPackingDesc,
                        CertiShippingMark = c.CertiShippingMark,
                        CertiRefBy = c.CertiRefBy,
                        CertiCountryDest = c.CertiCountryDest,
                        CertiTgPacking = c.CertiTgPacking,
                        CertiTgCommodity = c.CertiTgCommodity,
                        CertiTgPackComm = c.CertiTgPackComm,
                        CertiSurfaceThickness = c.CertiSurfaceThickness,
                        CertiStack = c.CertiStack,
                        CertiContainer = c.CertiContainer,
                        CertiChamber = c.CertiChamber,
                        CertiTestedContainer = c.CertiTestedContainer,
                        CertiUnsheetedContainer = c.CertiUnsheetedContainer,
                        CertiAppliedRate = c.CertiAppliedRate,
                        CertiFinalReading = c.CertiFinalReading,
                        CertiCreateUid = c.CertiCreateUid,
                        CertiEditedUid = c.CertiEditedUid,
                        CertiCreated = c.CertiCreated,
                        CertiUpdated = c.CertiUpdated,
                        CertiBillId = c.CertiBillId,
                        CertiLockedBy = c.CertiLockedBy
                    })

                    .ToListAsync();

                return Ok(new
                {
                    pagination = new
                    {
                        page = currentPage,
                        pageSize = pageSize,
                        totalRecords = totalRecords,
                        totalPages = totalPages
                    },
                    data = pagedCertis
                });
            }


                // GET: api/Certi/{id}
                [HttpGet("{id}")]
                    public ActionResult<CertiReadDto> GetById(string id)
                    {
                        var certi = _context.Certi.FirstOrDefault(c => c.CertiId == id);
                        if (certi == null) return NotFound();

                        var dto = new CertiReadDto
                        {
                            CertiId = certi.CertiId,
                            CertiPhyto = certi.CertiPhyto,
                            CertiJobType = certi.CertiJobType,
                            CertiOrderId = certi.CertiOrderId,
                            CertiBranchId = certi.CertiBranchId,
                            CertiProductType = certi.CertiProductType,
                            CertiType = certi.CertiType,
                            CertiJobfor = certi.CertiJobfor,
                            CertiNo = certi.CertiNo,
                            CertiDate = certi.CertiDate,
                            CertiFumidate = certi.CertiFumidate,
                            CertiPol = certi.CertiPol,
                            CertiPod = certi.CertiPod,
                            CertiImpcountry = certi.CertiImpcountry,
                            CertiFumiplace = certi.CertiFumiplace,
                            CertiUndersheet = certi.CertiUndersheet,
                            CertiFumiduration = certi.CertiFumiduration,
                            CertiDoseRate = certi.CertiDoseRate,
                            CertiPresserTested = certi.CertiPresserTested,
                            CertiTemperature = certi.CertiTemperature,
                            CertiHumidity = certi.CertiHumidity,
                            CertiContainers = certi.CertiContainers,
                            CertiContainerCount = certi.CertiContainerCount,
                            CertiContainerSize = certi.CertiContainerSize,
                            CertiInvoiceNo = certi.CertiInvoiceNo,
                            CertiInvoiceDate = certi.CertiInvoiceDate,
                            CertiAfoName = certi.CertiAfoName,
                            CertiRemarks = certi.CertiRemarks,
                            CertiExpName = certi.CertiExpName,
                            CertiExpAddress = certi.CertiExpAddress,
                            CertiExpEmail = certi.CertiExpEmail,
                            CertiConsignee = certi.CertiConsignee,
                            CertiConsigneeAddress = certi.CertiConsigneeAddress,
                            CertiNotifyParty = certi.CertiNotifyParty,
                            CertiNotifyAddress = certi.CertiNotifyAddress,
                            CertiCargoDesc = certi.CertiCargoDesc,
                            CertiNetQty = certi.CertiNetQty,
                            CertilGrossQty = certi.CertilGrossQty,
                            CertiNetUnit = certi.CertiNetUnit,
                            CertiGrossUnit = certi.CertiGrossUnit,
                            CertiNoBags = certi.CertiNoBags,
                            CertiPackingDesc = certi.CertiPackingDesc,
                            CertiShippingMark = certi.CertiShippingMark,
                            CertiRefBy = certi.CertiRefBy,
                            CertiCountryDest = certi.CertiCountryDest,
                            CertiTgPacking = certi.CertiTgPacking,
                            CertiTgCommodity = certi.CertiTgCommodity,
                            CertiTgPackComm = certi.CertiTgPackComm,
                            CertiSurfaceThickness = certi.CertiSurfaceThickness,
                            CertiStack = certi.CertiStack,
                            CertiContainer = certi.CertiContainer,
                            CertiChamber = certi.CertiChamber,
                            CertiTestedContainer = certi.CertiTestedContainer,
                            CertiUnsheetedContainer = certi.CertiUnsheetedContainer,
                            CertiAppliedRate = certi.CertiAppliedRate,
                            CertiFinalReading = certi.CertiFinalReading,
                            CertiCreateUid = certi.CertiCreateUid,
                            CertiEditedUid = certi.CertiEditedUid,
                            CertiCreated = certi.CertiCreated,
                            CertiUpdated = certi.CertiUpdated,
                            CertiBillId = certi.CertiBillId,
                            CertiLockedBy = certi.CertiLockedBy,
                            Certi2Notify = certi.Certi2Notify

                        };

                        return Ok(dto);
                    }

                    // POST: api/Certi
                    [HttpPost]
                    public async Task<ActionResult> Add([FromBody] CertiAddDto dto)
                    {
                       await CheckAndAddNotify(dto.CertiExpName, dto.CertiExpAddress, "Exporter");
                       await CheckAndAddNotify(dto.CertiConsignee, dto.CertiConsigneeAddress, "Consignee");
                       await CheckAndAddNotify(dto.CertiNotifyParty, dto.CertiNotifyAddress, "Notify");
                    var exists = await _context.Certi.AnyAsync(c => c.CertiNo == dto.CertiNo);
                    if (exists)
                    {
                        return Conflict(new { message = "Certificate number already exists." });
                    }
                    var model = new Certi
                        {
                            CertiId = Guid.NewGuid().ToString(),
                            CertiPhyto = dto.CertiPhyto,
                            CertiJobType = dto.CertiJobType,                   
                            CertiOrderId = dto.CertiOrderId,
                            CertiBranchId = dto.CertiBranchId,
                            CertiProductType = dto.CertiProductType,
                            CertiType = dto.CertiType,
                            CertiJobfor = dto.CertiJobfor,
                            CertiNo = dto.CertiNo,
                            CertiDate = dto.CertiDate,
                            CertiFumidate = dto.CertiFumidate,
                            CertiPol = dto.CertiPol,
                            CertiPod = dto.CertiPod,
                            CertiImpcountry = dto.CertiImpcountry,
                            CertiFumiplace = dto.CertiFumiplace,
                            CertiUndersheet = dto.CertiUndersheet,
                            CertiFumiduration = dto.CertiFumiduration,
                            CertiDoseRate = dto.CertiDoseRate,
                            CertiPresserTested = dto.CertiPresserTested,
                            CertiTemperature = dto.CertiTemperature,
                            CertiHumidity = dto.CertiHumidity,
                            CertiContainers = dto.CertiContainers,
                            CertiContainerCount = dto.CertiContainerCount,
                            CertiContainerSize = dto.CertiContainerSize,
                            CertiInvoiceNo = dto.CertiInvoiceNo,
                            CertiInvoiceDate = dto.CertiInvoiceDate,
                            CertiAfoName = dto.CertiAfoName,
                            CertiRemarks = dto.CertiRemarks,
                            CertiExpName = dto.CertiExpName,
                            CertiExpAddress = dto.CertiExpAddress,
                            CertiExpEmail = dto.CertiExpEmail,
                            CertiConsignee = dto.CertiConsignee,
                            CertiConsigneeAddress = dto.CertiConsigneeAddress,
                            CertiNotifyParty = dto.CertiNotifyParty,
                            CertiNotifyAddress = dto.CertiNotifyAddress,
                            CertiCargoDesc = dto.CertiCargoDesc,
                            CertiNetQty = dto.CertiNetQty,
                            CertilGrossQty = dto.CertilGrossQty,
                            CertiNetUnit = dto.CertiNetUnit,
                            CertiGrossUnit = dto.CertiGrossUnit,
                            CertiNoBags = dto.CertiNoBags,
                            CertiPackingDesc = dto.CertiPackingDesc,
                            CertiShippingMark = dto.CertiShippingMark,
                            CertiRefBy = dto.CertiRefBy,
                            CertiCountryDest = dto.CertiCountryDest,
                            CertiTgPacking = dto.CertiTgPacking,
                            CertiTgCommodity = dto.CertiTgCommodity,
                            CertiTgPackComm = dto.CertiTgPackComm,
                            CertiSurfaceThickness = dto.CertiSurfaceThickness,
                            CertiStack = dto.CertiStack,
                            CertiContainer = dto.CertiContainer,
                            CertiChamber = dto.CertiChamber,
                            CertiTestedContainer = dto.CertiTestedContainer,
                            CertiUnsheetedContainer = dto.CertiUnsheetedContainer,
                            CertiAppliedRate = dto.CertiAppliedRate,
                            CertiFinalReading = dto.CertiFinalReading,
                            CertiCreateUid =GetUserId().ToString(),
                            CertiBillId = dto.CertiBillId,
                            CertiLockedBy = dto.CertiLockedBy,
                        Certi2Notify = dto.Certi2Notify,
                        CertiCompanyId = GetCompanyId(),
                        CertiCreated =DateTime.Now 
                    
                   
                        };

                        _context.Certi.Add(model);
                        _context.SaveChanges();

                    return Ok(true);

                }

                // PUT: api/Certi/{id}
                [HttpPut("{id}")]
                    public ActionResult Update(string id, [FromBody] CertiUpdateDto dto)
                    {
                    var exists = _context.Certi.Any(c => c.CertiNo == dto.CertiNo && c.CertiId != id);
                    if (exists)
                    {
                        return Conflict(new { message = "Certificate number already exists." });
                    }

                    var certi = _context.Certi.FirstOrDefault(c => c.CertiId == id);
                        if (certi == null) return NotFound();

                        certi.CertiOrderId = dto.CertiOrderId;
                    certi.CertiPhyto = dto.CertiPhyto;
                    certi.CertiJobType = dto.CertiJobType;
                    certi.CertiBranchId = dto.CertiBranchId;
                        certi.CertiProductType = dto.CertiProductType;
                        certi.CertiType = dto.CertiType;
                        certi.CertiJobfor = dto.CertiJobfor;
                        certi.CertiNo = dto.CertiNo;
                        certi.CertiDate = dto.CertiDate;
                        certi.CertiFumidate = dto.CertiFumidate;
                        certi.CertiPol = dto.CertiPol;
                        certi.CertiPod = dto.CertiPod;
                        certi.CertiImpcountry = dto.CertiImpcountry;
                        certi.CertiFumiplace = dto.CertiFumiplace;
                        certi.CertiUndersheet = dto.CertiUndersheet;
                        certi.CertiFumiduration = dto.CertiFumiduration;
                        certi.CertiDoseRate = dto.CertiDoseRate;
                        certi.CertiPresserTested = dto.CertiPresserTested;
                        certi.CertiTemperature = dto.CertiTemperature;
                        certi.CertiHumidity = dto.CertiHumidity;
                        certi.CertiContainers = dto.CertiContainers;
                        certi.CertiContainerCount = dto.CertiContainerCount;
                        certi.CertiContainerSize = dto.CertiContainerSize;
                        certi.CertiInvoiceNo = dto.CertiInvoiceNo;
                        certi.CertiInvoiceDate = dto.CertiInvoiceDate;
                        certi.CertiAfoName = dto.CertiAfoName;
                        certi.CertiRemarks = dto.CertiRemarks;
                        certi.CertiExpName = dto.CertiExpName;
                        certi.CertiExpAddress = dto.CertiExpAddress;
                        certi.CertiExpEmail = dto.CertiExpEmail;
                        certi.CertiConsignee = dto.CertiConsignee;
                        certi.CertiConsigneeAddress = dto.CertiConsigneeAddress;
                        certi.CertiNotifyParty = dto.CertiNotifyParty;
                        certi.CertiNotifyAddress = dto.CertiNotifyAddress;
                        certi.CertiCargoDesc = dto.CertiCargoDesc;
                        certi.CertiNetQty = dto.CertiNetQty;
                        certi.CertilGrossQty = dto.CertilGrossQty;
                        certi.CertiNetUnit = dto.CertiNetUnit;
                        certi.CertiGrossUnit = dto.CertiGrossUnit;
                        certi.CertiNoBags = dto.CertiNoBags;
                        certi.CertiPackingDesc = dto.CertiPackingDesc;
                        certi.CertiShippingMark = dto.CertiShippingMark;
                        certi.CertiRefBy = dto.CertiRefBy;
                        certi.CertiCountryDest = dto.CertiCountryDest;
                        certi.CertiTgPacking = dto.CertiTgPacking;
                        certi.CertiTgCommodity = dto.CertiTgCommodity;
                        certi.CertiTgPackComm = dto.CertiTgPackComm;
                        certi.CertiSurfaceThickness = dto.CertiSurfaceThickness;
                        certi.CertiStack = dto.CertiStack;
                        certi.CertiContainer = dto.CertiContainer;
                        certi.CertiChamber = dto.CertiChamber;
                        certi.CertiTestedContainer = dto.CertiTestedContainer;
                        certi.CertiUnsheetedContainer = dto.CertiUnsheetedContainer;
                        certi.CertiAppliedRate = dto.CertiAppliedRate;
                        certi.CertiFinalReading = dto.CertiFinalReading;
                        certi.CertiEditedUid =GetUserId().ToString();
                         certi.CertiBillId = dto.CertiBillId;
            certi.Certi2Notify = dto.Certi2Notify;
            certi.CertiLockedBy = dto.CertiLockedBy;
                        certi.CertiUpdated = DateTime.Now;
                        _context.SaveChanges();

                    return Ok(true);
                }

                    // DELETE: api/Certi/{id}
                    [HttpDelete("{id}")]
                    public ActionResult Delete(string id)
                    {
                        var certi = _context.Certi.FirstOrDefault(c => c.CertiId == id);
                        if (certi == null) return NotFound();

                        _context.Certi.Remove(certi);
                        _context.SaveChanges();

                        return Ok(true);
                    }


                private async Task CheckAndAddNotify(string? name, string? address, string type)
                {
                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
                        return;

                    var existingNotify = await _context.Notifies.FirstOrDefaultAsync(n =>
                        n.NotifyName == name && n.NotifyAddress == address && n.NotifyType == type);

                    if (existingNotify == null)
                    {
                        var newNotify = new Notify
                        {
                            NotifyName = name,
                            NotifyAddress = address,
                            NotifyType = type, 
                            NotifyStatus = true ,
                            NotifyCreated = DateTime.Now,
                            NotifyUpdated = DateTime.Now
                        };

                        _context.Notifies.Add(newNotify);
                        await _context.SaveChangesAsync();
                    }
                }


        [HttpGet("print")]
        public async Task<ActionResult<CertiPrintMbrDto>> GetPrintData([FromQuery] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BadRequest(new { message = "ID or Certificate Number is required." });

            // Select only the fields you need and project to anonymous type first
            var certiData = await _context.Certi
                .AsNoTracking()
                .Where(c => c.CertiId == value || c.CertiNo.ToString() == value)
                .Select(c => new
                {
                    CertiNo = c.CertiNo ?? 0,
                    CertiCompanyId = c.CertiCompanyId,
                    CertiDate = c.CertiDate,
                    CertiJobType = c.CertiJobType ?? "",
                    CertiPhyto = c.CertiPhyto ?? "",
                    CertiExpName = c.CertiExpName ?? "",
                    CertiExpAddress = c.CertiExpAddress ?? "",
                    CertiExpEmail = c.CertiExpEmail ?? "",
                    CertiConsignee = c.CertiConsignee ?? "",
                    CertiConsigneeAddress = c.CertiConsigneeAddress ?? "",
                    CertiNotifyParty = c.CertiNotifyParty ?? "",
                    CertiNotifyAddress = c.CertiNotifyAddress ?? "",
                    CertiCargoDesc = c.CertiCargoDesc ?? "",
                    CertiNetQty = c.CertiNetQty ?? 0,
                    CertilGrossQty = c.CertilGrossQty ?? 0,
                    CertiNetUnit = c.CertiNetUnit ?? "",
                    CertiGrossUnit = c.CertiGrossUnit ?? "",
                    CertiInvoiceNo = c.CertiInvoiceNo ?? "",
                    CertiInvoiceDate = c.CertiInvoiceDate,
                    CertiFumiplace = c.CertiFumiplace ?? "",
                    CertiFumidate = c.CertiFumidate,
                    CertiFumiduration = c.CertiFumiduration ?? "",
                    CertiDoseRate = c.CertiDoseRate ?? 0,
                    CertiContainers = c.CertiContainers ?? "",
                    CertiContainerCount = c.CertiContainerCount ?? 0,
                    CertiContainerSize = c.CertiContainerSize ?? 0,
                    CertiShippingMark = c.CertiShippingMark ?? "",
                    CertiRemarks = c.CertiRemarks ?? "",
                    CertiImpcountry = c.CertiImpcountry ?? "",
                    CertiPol = c.CertiPol ?? "",
                    CertiPod = c.CertiPod ?? "",
                    CertiAfoName = c.CertiAfoName ?? "",
                    CertiNoBags = c.CertiNoBags ?? "",
                    CertiPackingDesc = c.CertiPackingDesc ?? "",
                    CertiProductType = c.CertiProductType ?? "",
                    CertiUndersheet = c.CertiUndersheet ?? "",
                    CertiTemperature = c.CertiTemperature ?? 0,
                    CertiPresserTested = c.CertiPresserTested ?? "",
                    CertiHumidity = c.CertiHumidity ?? "",
                   
                    Certi2Notify = c.Certi2Notify ?? false
                })
                .FirstOrDefaultAsync();

            if (certiData == null)
                return NotFound();

            // Fetch company - assuming single company for now
            var company = await _context.companies
    .AsNoTracking()
    .FirstOrDefaultAsync(c => c.Status == true);


            var dto = new CertiPrintMbrDto
            {
                CertiNo = certiData.CertiNo,
                CertiCompanyId = certiData.CertiCompanyId,
                CertiDate = certiData.CertiDate,
                CertiJobType = certiData.CertiJobType,
                CertiPhyto = certiData.CertiPhyto,
                CertiExpName = certiData.CertiExpName,
                CertiExpAddress = certiData.CertiExpAddress,
                CertiExpEmail = certiData.CertiExpEmail,
                CertiConsignee = certiData.CertiConsignee,
                CertiConsigneeAddress = certiData.CertiConsigneeAddress,
                CertiNotifyParty = certiData.CertiNotifyParty,
                CertiNotifyAddress = certiData.CertiNotifyAddress,
                CertiCargoDesc = certiData.CertiCargoDesc,
                CertiNetQty = certiData.CertiNetQty,
                CertilGrossQty = certiData.CertilGrossQty,
                CertiNetUnit = certiData.CertiNetUnit,
                CertiGrossUnit = certiData.CertiGrossUnit,
                CertiInvoiceNo = certiData.CertiInvoiceNo,
                CertiInvoiceDate = certiData.CertiInvoiceDate,
                CertiFumiplace = certiData.CertiFumiplace,
                CertiFumidate = certiData.CertiFumidate,
                CertiFumiduration = certiData.CertiFumiduration,
                CertiDoseRate = certiData.CertiDoseRate,
                CertiContainers = certiData.CertiContainers,
                CertiContainerCount = certiData.CertiContainerCount,
                CertiContainerSize = certiData.CertiContainerSize,
                CertiShippingMark = certiData.CertiShippingMark,
                CertiRemarks = certiData.CertiRemarks,
                CertiImpcountry = certiData.CertiImpcountry,
                CertiPol = certiData.CertiPol,
                CertiPod = certiData.CertiPod,
                CertiAfoName = certiData.CertiAfoName,
                CertiNoBags = certiData.CertiNoBags,
                CertiPackingDesc = certiData.CertiPackingDesc,
                CertiProductType = certiData.CertiProductType,
                CertiUndersheet = certiData.CertiUndersheet,
                CertiTemperature = certiData.CertiTemperature,
                CertiPresserTested = certiData.CertiPresserTested,
                CertiHumidity = certiData.CertiHumidity,
                Certi2Notify = certiData.Certi2Notify,
                CompanyName = company?.Name ?? "N/A",
                CompanyAddress = string.Join(", ", new[] {
            company?.Address1, company?.Address2, company?.Address3,
            company?.City, company?.StateCode, company?.Country, company?.Pincode
        }.Where(s => !string.IsNullOrWhiteSpace(s)))
            };

            return Ok(dto);
        }


    }

}


