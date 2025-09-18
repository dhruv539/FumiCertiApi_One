using FumicertiApi.Data;
using FumicertiApi.DTOs;
using FumicertiApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace FumicertiApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyconfigController : BaseController
    {
            private readonly AppDbContext _context;
            private readonly ISieveProcessor _sieveProcessor;

            public CompanyconfigController(AppDbContext context, ISieveProcessor sieveProcessor)
            {
                _context = context;
                _sieveProcessor = sieveProcessor;
            }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            // Base query
            var query = _context.CompanyConfigs.AsNoTracking();

            // Apply filters (without pagination)
            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Join with other tables (Company, User, UserRole)
            var pagedConfigs = await (from c in filteredQuery
                                      join comp in _context.companies on c.CompanyId equals comp.CompanyId into compJoin
                                      from comp in compJoin.DefaultIfEmpty()
                                      join usr in _context.Users on c.UserId equals usr.UserId into userJoin
                                      from usr in userJoin.DefaultIfEmpty()
                                      join role in _context.UserRoles on c.UserRoleId equals role.RoleUuid into roleJoin
                                      from role in roleJoin.DefaultIfEmpty()
                                      select new CompanyConfigDto
                                      {
                                          CompanyConfigId = c.CompanyConfigId,

                                          CompanyId = c.CompanyId,
                                          CompanyName = comp != null ? comp.Name : null,

                                          UserId = c.UserId,
                                          UserName = usr != null ? usr.UserName : null,

                                          UserRoleId = c.UserRoleId,
                                          UserRoleName = role != null ? role.RoleName : null,

                                          AfoVisible = c.AfoVisible,
                                          BranchVisible = c.BranchVisible,
                                          CertImbrVisible = c.CertImbrVisible,
                                          CertIalpVisible = c.CertIalpVisible,
                                          CertIafasVisible = c.CertIafasVisible,
                                          CompanyVisible = c.CompanyVisible,
                                          ContainerListVisible = c.ContainerListVisible,
                                          IndexVisible = c.IndexVisible,
                                          PurchaseInvoiceVisible = c.PurchaseInvoiceVisible,
                                          SellInvoiceVisible = c.SellInvoiceVisible,
                                          LocationVisible = c.LocationVisible,
                                          NotifyVisible = c.NotifyVisible,
                                          ProductVisible = c.ProductVisible,
                                          UserVisible = c.UserVisible,
                                          VoucherConfigVisible = c.VoucherConfigVisible,
                                          YearVisible = c.YearVisible,
                                          AllCertiVisible = c.AllCertiVisible,
                                          GroupHomeVisible = c.GroupHomeVisible,
                                          GroupAdminVisible = c.GroupAdminVisible,
                                          MasterVisible = c.MasterVisible,
                                          VoucherEntryVisible = c.VoucherEntryVisible,
                                          CertificateEntryVisible = c.CertificateEntryVisible,
                                          ReportVisible = c.ReportVisible,
                                          AboutVisible = c.AboutVisible
                                      })
                                      .Skip((currentPage - 1) * pageSize)
                                      .Take(pageSize)
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
                data = pagedConfigs
            });
        }


        [HttpGet("{id}")]
            public async Task<ActionResult<CompanyConfigDto>> Get(int id)
            {
                var config = await _context.CompanyConfigs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CompanyConfigId == id);

                if (config == null) return NotFound();

                return Ok(new CompanyConfigDto
                {
                    CompanyConfigId = config.CompanyConfigId,
                    CompanyId = config.CompanyId,
                    UserId = config.UserId,
                    UserRoleId = config.UserRoleId,
                    AfoVisible = config.AfoVisible,
                    BranchVisible = config.BranchVisible,
                    CertImbrVisible = config.CertImbrVisible,
                    CertIalpVisible = config.CertIalpVisible,
                    CertIafasVisible = config.CertIafasVisible,
                    CompanyVisible = config.CompanyVisible,
                    ContainerListVisible = config.ContainerListVisible,
                    IndexVisible = config.IndexVisible,
                    PurchaseInvoiceVisible = config.PurchaseInvoiceVisible,
                    SellInvoiceVisible = config.SellInvoiceVisible,
                    LocationVisible = config.LocationVisible,
                    NotifyVisible = config.NotifyVisible,
                    ProductVisible = config.ProductVisible,
                    UserVisible = config.UserVisible,
                    VoucherConfigVisible = config.VoucherConfigVisible,
                    YearVisible = config.YearVisible,
                    AllCertiVisible = config.AllCertiVisible,
                    GroupHomeVisible = config.GroupHomeVisible,
                    GroupAdminVisible = config.GroupAdminVisible,
                    MasterVisible = config.MasterVisible,
                    VoucherEntryVisible = config.VoucherEntryVisible,
                    CertificateEntryVisible = config.CertificateEntryVisible,
                    ReportVisible = config.ReportVisible,
                    AboutVisible = config.AboutVisible
                });
            }

            [HttpPost]
            public async Task<IActionResult> Create(CompanyConfigAddDto dto)
            {
                var config = new CompanyConfig
                {
                    CompanyId = dto.CompanyId,
                    UserId = dto.UserId,
                    UserRoleId = dto.UserRoleId,
                    AfoVisible = dto.AfoVisible,
                    BranchVisible = dto.BranchVisible,
                    CertImbrVisible = dto.CertImbrVisible,
                    CertIalpVisible = dto.CertIalpVisible,
                    CertIafasVisible = dto.CertIafasVisible,
                    CompanyVisible = dto.CompanyVisible,
                    ContainerListVisible = dto.ContainerListVisible,
                    IndexVisible = dto.IndexVisible,
                    PurchaseInvoiceVisible = dto.PurchaseInvoiceVisible,
                    SellInvoiceVisible = dto.SellInvoiceVisible,
                    LocationVisible = dto.LocationVisible,
                    NotifyVisible = dto.NotifyVisible,
                    ProductVisible = dto.ProductVisible,
                    UserVisible = dto.UserVisible,
                    VoucherConfigVisible = dto.VoucherConfigVisible,
                    YearVisible = dto.YearVisible,
                    AllCertiVisible = dto.AllCertiVisible,
                    GroupHomeVisible = dto.GroupHomeVisible,
                    GroupAdminVisible = dto.GroupAdminVisible,
                    MasterVisible = dto.MasterVisible,
                    VoucherEntryVisible = dto.VoucherEntryVisible,
                    CertificateEntryVisible = dto.CertificateEntryVisible,
                    ReportVisible = dto.ReportVisible,
                    AboutVisible = dto.AboutVisible,
                    CreateUid = GetUserId().ToString(),
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                };

                _context.CompanyConfigs.Add(config);
                await _context.SaveChangesAsync();

                return Ok(true);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, CompanyConfigDto dto)
            {
                var config = await _context.CompanyConfigs.FindAsync(id);
                if (config == null) return NotFound();

                config.UserId = dto.UserId;
                config.UserRoleId = dto.UserRoleId;
                config.AfoVisible = dto.AfoVisible;
                config.BranchVisible = dto.BranchVisible;
                config.CertImbrVisible = dto.CertImbrVisible;
                config.CertIalpVisible = dto.CertIalpVisible;
                config.CertIafasVisible = dto.CertIafasVisible;
                config.CompanyVisible = dto.CompanyVisible;
                config.ContainerListVisible = dto.ContainerListVisible;
                config.IndexVisible = dto.IndexVisible;
                config.PurchaseInvoiceVisible = dto.PurchaseInvoiceVisible;
                config.SellInvoiceVisible = dto.SellInvoiceVisible;
                config.LocationVisible = dto.LocationVisible;
                config.NotifyVisible = dto.NotifyVisible;
                config.ProductVisible = dto.ProductVisible;
                config.UserVisible = dto.UserVisible;
                config.VoucherConfigVisible = dto.VoucherConfigVisible;
                config.YearVisible = dto.YearVisible;
                config.AllCertiVisible = dto.AllCertiVisible;
                config.GroupHomeVisible = dto.GroupHomeVisible;
                config.GroupAdminVisible = dto.GroupAdminVisible;
                config.MasterVisible = dto.MasterVisible;
                config.VoucherEntryVisible = dto.VoucherEntryVisible;
                config.CertificateEntryVisible = dto.CertificateEntryVisible;
                config.ReportVisible = dto.ReportVisible;
                config.AboutVisible = dto.AboutVisible;
                config.EditedUid = GetUserId().ToString();
                config.Updated = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            return Ok(true);

        }

        [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var config = await _context.CompanyConfigs.FindAsync(id);
                if (config == null) return NotFound();

                _context.CompanyConfigs.Remove(config);
                await _context.SaveChangesAsync();
                return NoContent();
            }
    }
}


