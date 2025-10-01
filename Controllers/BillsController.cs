using FumicertiApi.Data;
using FumicertiApi.DTOs;
using FumicertiApi.DTOs.Certi;
using FumicertiApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FumicertiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : BaseController
    {
        private readonly AppDbContext _context;

        public BillsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Bills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bill>>> GetBills()
        {
            var bills = await FilterByCompany(_context.Bills.AsNoTracking(), "BillCompanyId")
        .Join(_context.Notifies,
              b => b.BillPartyId,
              n => n.NotifyId,
              (b, n) => new BillReadDto
              {
                  BillId = b.BillId,
                  BillNo = b.BillNo,
                  BillNoStr = b.BillNoStr,
                  BillDate = b.BillDate,
                  BillPartyId = b.BillPartyId,
                  PartyName = n.NotifyName, // 👈 mapped from Notify
                  BillVoucherId = b.BillVoucherId,
                  BillGrossAmt = b.BillGrossAmt,
                  BillTaxable = b.BillTaxable,
                  BillNetAmt = b.BillNetAmt,
                  BillPosId = b.BillPosId,
                  BillPrefix = b.BillPrefix,
                  BillSufix = b.BillSufix,
                  BillShipParty = b.BillShipParty,
                  BillAddress1 = b.BillAddress1,
                  BillAddress2 = b.BillAddress2,
                  BillAddress3 = b.BillAddress3,
                  BillState = b.BillState,
                  BillGstin = b.BillGstin,
                  BillPin = b.BillPin,
                  BillContactNo = b.BillContactNo,
                  BillDateFrom = b.BillDateFrom,
                  BillDateTo = b.BillDateTo,
                  BillIrnNo = b.BillIrnNo,
                  BillAckNo = b.BillAckNo,
                  BillAckDate = b.BillAckDate,
                  BillSupplyType = b.BillSupplyType,
                  BillRatePerCont = b.BillRatePerCont,
                  BillGstPer = b.BillGstPer,
                  BillGstSlabId = b.BillGstSlabId,
                  BillSgst = b.BillSgst,
                  BillCgst = b.BillCgst,
                  BillIgst = b.BillIgst,
                  Remarks = b.Remarks,
                  FilterPartyName = b.FilterPartyName,
                  BillRate40Cont = b.BillRate40Cont,
                  BillCompanyId = b.BillCompanyId,
                  BillYearId = b.BillYearId
              })
        .ToListAsync();

            return Ok(bills);
        }

        // GET: api/Bills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bill>> GetBill(int id)
        {
            var bill = await FilterByCompany(_context.Bills.AsNoTracking(), "BillCompanyId")
        .FirstOrDefaultAsync(b => b.BillId == id);
            if (bill == null)
            {
                return NotFound();
            }   
            return bill;
        }

        // POST: api/Bills
        [HttpPost]
        public async Task<ActionResult<Bill>> PostBill(Bill bill)
        {
            bill.BillCompanyId = GetCompanyId();
            var exists = await _context.Bills.AnyAsync(c => c.BillNoStr == bill.BillNoStr && c.BillCompanyId == GetCompanyId() && c.BillYearId== bill.BillYearId);
            if (exists)
            {
                return Conflict(new { message = "Bill number already exists." });
            }
            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBill), new { id = bill.BillId }, bill);
        }

        // PUT: api/Bills/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBill(int id, Bill bill)
        {
            if (id != bill.BillId)
            {
                return BadRequest();
            }
            bill.BillCompanyId = GetCompanyId();
            _context.Entry(bill).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Bills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var bill = await FilterByCompany(_context.Bills.AsNoTracking(), "BillCompanyId").FirstOrDefaultAsync(b => b.BillId == id);
            if (bill == null)
            {
                return NotFound(false);
            }

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();

            return Ok(true);

        }

        [HttpGet("print/{id}")]
        public async Task<ActionResult<PrintBillDto>> GetPrintBill(int id)
        {
            try
            {
                var result = await (
                    from bill in _context.Bills.AsNoTracking()
                    join company in _context.companies.AsNoTracking()
                        on bill.BillCompanyId equals company.CompanyId
                    join notify in _context.Notifies.AsNoTracking()
                        on bill.BillPartyId equals notify.NotifyId
                    join state in _context.States.AsNoTracking() // yeh join add kiya
                        on bill.BillPosId equals state.StateId

                    where bill.BillId == id
                    select new PrintBillDto
                    {
                        // Bill Info
                        BillId = bill.BillId,
                        BillNoStr = bill.BillNoStr,
                        BillPrefix = bill.BillPrefix,
                        BillSufix = bill.BillSufix,
                        BillDate = bill.BillDate,
                        BillPosId=bill.BillPosId,
                        PartyId = bill.BillPartyId,
                        ShipParty = bill.BillShipParty,
                        Address1 = bill.BillAddress1,
                        Address2 = bill.BillAddress2,
                        Address3 = bill.BillAddress3,
                        State = bill.BillState,
                        Gstin = bill.BillGstin,
                        Pin = bill.BillPin,
                        ContactNo = bill.BillContactNo,
                        GrossAmount = bill.BillGrossAmt,
                        TaxableAmount = bill.BillTaxable,
                        NetAmount = bill.BillNetAmt,
                        Sgst = bill.BillSgst,
                        Cgst = bill.BillCgst,
                        Igst = bill.BillIgst,
                        RatePerContainer = bill.BillRatePerCont,
                        Rate40Container = bill.BillRate40Cont,
                        SupplyType = bill.BillSupplyType,
                        IrnNo = bill.BillIrnNo,
                        AckNo = bill.BillAckNo,
                        AckDate = bill.BillAckDate,
                        Remarks = bill.Remarks,
                        BillDateFrom = bill.BillDateFrom,
                        BillDateTo = bill.BillDateTo,

                        // Company Info
                        CompanyId = company.CompanyId,
                        CompanyName = company.Name,
                        CompanyAddress = company.Address1,
                        CompanyGstin = company.Gstin,
                        CompanyState = company.StateId,
                        CompanyContactNo = company.Mobile,
                        CompanyEmail = company.Email,

                        PosStateName = state.StateName,   // yeh naya field tumhare DTO me daal do
                      

                        // Notify Info
                        Notify = new NotifyDto
                        {
                            NotifyId = notify.NotifyId,
                            NotifyName = notify.NotifyName,
                            Address = notify.NotifyAddress,                        
                            State = notify.NotifyState,
                            Gstin = notify.NotifyGstNo,
                            Pin = notify.NotifyPincode,
                            ContactNo = notify.NotifyContactNo,
                            Email = notify.NotifyEmail
                        },

                        // Collect all Certis
                        Certis = _context.Certi
                            .Where(c => c.CertiBillId == bill.BillId)
                            .Select(c => new Certiprintbillbto
                            {
                                CertiNo = c.CertiNo,
                                CertiDate = c.CertiDate,
                                ContainerNo = c.CertiContainers,
                                Commodity = c.CertiTgCommodity,
                                PortOfLoading = c.CertiPol,
                                PortOfDischarge = c.CertiPod,
                                ContainerSize = c.CertiContainerSize,
                                DoseRate = c.CertiDoseRate,
                                InvoiceNo = c.CertiInvoiceNo
                            })
                            .ToList()
                    }
                ).FirstOrDefaultAsync();

                if (result == null) return NotFound();
                return result;
            }
            catch (Exception ex)
            {
                return Problem($"Failed to fetch PrintBill: {ex.Message}");
            }
        }



    }
}
