using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.DTOs.Certi
{
    public class CertiUpdateDto
    {
        public int? CertiOrderId { get; set; }
        public int? CertiBranchId { get; set; }
        public string? CertiProductType { get; set; }
        public string? CertiType { get; set; }
        public string? CertiJobfor { get; set; }
        public int? CertiNo { get; set; }
        public DateTime? CertiDate { get; set; }
        public DateTime? CertiFumidate { get; set; }
        public string? CertiPol { get; set; }
        public string? CertiPod { get; set; }
        public string? CertiImpcountry { get; set; }
        public string? CertiFumiplace { get; set; }
        public string? CertiUndersheet { get; set; }
        public string? CertiFumiduration { get; set; }
        public double? CertiDoseRate { get; set; }
        public string? CertiPresserTested { get; set; }
        public double? CertiTemperature { get; set; }
        public string? CertiHumidity { get; set; }
        public string? CertiContainers { get; set; }
        public int? CertiContainerCount { get; set; }
        public int? CertiContainerSize { get; set; }
        public string? CertiInvoiceNo { get; set; }
        public DateTime? CertiInvoiceDate { get; set; }
        public string? CertiAfoName { get; set; }
        public string? CertiRemarks { get; set; }
        public string? CertiExpName { get; set; }
        public string? CertiExpAddress { get; set; }
        public string? CertiExpEmail { get; set; }
        public string? CertiConsignee { get; set; }
        public string? CertiConsigneeAddress { get; set; }
        public string? CertiNotifyParty { get; set; }
        public string? CertiNotifyAddress { get; set; }
        public string? CertiCargoDesc { get; set; }
        public double? CertiNetQty { get; set; }
        public double? CertilGrossQty { get; set; }
        public string? CertiNetUnit { get; set; }
        public string? CertiGrossUnit { get; set; }
        public string? CertiNoBags { get; set; }
        public string? CertiPackingDesc { get; set; }
        public string? CertiShippingMark { get; set; }
        public string? CertiRefBy { get; set; }
        public string? CertiCountryDest { get; set; }
        public bool? CertiTgPacking { get; set; }
        public bool? CertiTgCommodity { get; set; }
        public bool? CertiTgPackComm { get; set; }
        public string? CertiSurfaceThickness { get; set; }
        public bool? CertiStack { get; set; }
        public bool? CertiContainer { get; set; }
        public bool? CertiChamber { get; set; }
        public bool? CertiTestedContainer { get; set; }
        public bool? CertiUnsheetedContainer { get; set; }
        public float? CertiAppliedRate { get; set; }
        public string? CertiFinalReading { get; set; }
        public string? CertiEditedUid { get; set; }
        public int? CertiBillId { get; set; }
        public int? CertiLockedBy { get; set; }
        public string? CertiPhyto { get; set; }
        public string? CertiJobType { get; set; }
        public bool? Certi2Notify { get; set; }
     
        public int CertiCompanyId { get; set; }

    }
}