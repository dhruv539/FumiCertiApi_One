using FumicertiApi.Data;
using FumicertiApi.DTOs;
using FumicertiApi.DTOs.Container;
using FumicertiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FumicertiApi.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;
        public ReportService(AppDbContext context) => _context = context;

        public async Task<List<CertiContainerReportDto>> GetCertiContainerReportAsync(
            DateTime fromdate,
            DateTime todate,
            string? certiNo,
            string? containerNo,
            string? certiType)
        {
            var query = from c in _context.Certi.AsNoTracking()
                        join cont in _context.Containers.AsNoTracking()
                            on c.CertiId equals cont.ContainerCertiId
                        join afo in _context.afo_members.AsNoTracking()
                            on c.CertiAfoName equals afo.AfoName
                        join comp in _context.companies.AsNoTracking()
                            on cont.ContainerCompanyId equals comp.CompanyId
                        where c.CertiDate >= fromdate && c.CertiDate <= todate
                        select new CertiContainerReportDto
                        {
                            // Certi
                            CertiNo = c.CertiNo,
                            CertiDate = c.CertiDate,
                            CertiType = c.CertiType,
                            CertiJobType = c.CertiJobType,
                            CertiProductType = c.CertiProductType,
                            CertiFumidate = c.CertiFumidate,
                            CertiFumiplace = c.CertiFumiplace,
                            CertiTemperature = c.CertiTemperature,
                            CertiHumidity = c.CertiHumidity,
                            CertiDoseRate = c.CertiDoseRate,
                            CertiRemarks = c.CertiRemarks,
                            CertiFumiduration = c.CertiFumiduration,

                            // Container
                            Container = new ContainerReadDto
                            {
                                ContainerCid = cont.ContainerCid,
                                ContainerCertiId = cont.ContainerCertiId,
                                ContainerContainerNo = cont.ContainerContainerNo,
                                ContainerCsize = cont.ContainerCsize,
                                ContainerConsumeQty = cont.ContainerConsumeQty,
                                ContainerDt1 = cont.ContainerDt1,
                                ContainerDt2 = cont.ContainerDt2,
                                ContainerDt3 = cont.ContainerDt3,
                                ContainerTime1 = cont.ContainerTime1,
                                ContainerTime2 = cont.ContainerTime2,
                                ContainerTime3 = cont.ContainerTime3,
                                ContainerFb1 = cont.ContainerFb1,
                                ContainerFb2 = cont.ContainerFb2,
                                ContainerFb3 = cont.ContainerFb3,
                                ContainerMc1 = cont.ContainerMc1,
                                ContainerMc2 = cont.ContainerMc2,
                                ContainerMc3 = cont.ContainerMc3,
                                ContainerTb1 = cont.ContainerTb1,
                                ContainerTb2 = cont.ContainerTb2,
                                ContainerTb3 = cont.ContainerTb3,
                                ContainerEquilibrium = cont.ContainerEquilibrium,
                                ContainerVolL = cont.ContainerVolL,
                                ContainerVolB = cont.ContainerVolB,
                                ContainerVolH = cont.ContainerVolH,
                                ContainerProductname = cont.ContainerProductname,
                                ContainerCompanyId = cont.ContainerCompanyId
                            },

                            // Company
                            Company = new CompanyDto
                            {
                                CompanyId = comp.CompanyId,
                                Name = comp.Name,
                                Address1 = comp.Address1,
                                Mobile = comp.Mobile,
                                Email = comp.Email,
                                CompanyAddress =
                                    (comp.Address1 ?? "") +
                                    (string.IsNullOrEmpty(comp.Address2) ? "" : ", " + comp.Address2) +
                                    (string.IsNullOrEmpty(comp.Address3) ? "" : ", " + comp.Address3)
                            },

                            // Afo
                            Afo = new AfoDto
                            {
                                AfoMbrNo = afo.AfoMbrNo,
                                AfoName = afo.AfoName,
                                AfoAlpNo = afo.AfoAlpNo
                            }
                        };

            if (!string.IsNullOrWhiteSpace(certiType))
                query = query.Where(x => x.CertiType == certiType);

            if (!string.IsNullOrWhiteSpace(certiNo) && int.TryParse(certiNo, out int certiNoInt))
                query = query.Where(x => x.CertiNo == certiNoInt);

            if (!string.IsNullOrWhiteSpace(containerNo))
                query = query.Where(x => x.Container.ContainerContainerNo == containerNo);

            return await query.ToListAsync();
        }
    }

}
