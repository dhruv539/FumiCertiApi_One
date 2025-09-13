using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.Models
{
    [Table("certi")]
    public class Certi
    {
        [Key]
        [Column("certi_id")]
        public string? CertiId { get; set; } // Made nullable - this was likely the main issue

        [Column("certi_order_id")]
        public int? CertiOrderId { get; set; } // Removed [Required] since it's nullable

        [Column("certi_branch_id")]
        public int? CertiBranchId { get; set; } // Removed [Required] since it's nullable

        [Column("certi_product_type")]
        public string? CertiProductType { get; set; } // Removed [Required] since it's nullable

        [Column("certi_type")]
        public string? CertiType { get; set; } // Removed [Required] since it's nullable

        [Column("certi_jobfor")]
        public string? CertiJobfor { get; set; } // Removed [Required] since it's nullable

        [Column("certi_no")]
        public int? CertiNo { get; set; } // Removed [Required] since it's nullable

        [Column("certi_date")]
        public DateTime? CertiDate { get; set; }

        [Column("certi_fumidate")]
        public DateTime? CertiFumidate { get; set; }

        [Column("certi_pol")]
        public string? CertiPol { get; set; }

        [Column("certi_pod")]
        public string? CertiPod { get; set; }

        [Column("certi_impcountry")]
        public string? CertiImpcountry { get; set; }

        [Column("certi_fumiplace")]
        public string? CertiFumiplace { get; set; }

        [Column("certi_undersheet")]
        public string? CertiUndersheet { get; set; }

        [Column("certi_fumiduration")]
        public string? CertiFumiduration { get; set; }

        [Column("certi_dose_rate")] 
        public double? CertiDoseRate { get; set; }

        [Column("certi_presser_tested")]
        public string? CertiPresserTested { get; set; }

        [Column("certi_humidity")]
        public string? CertiHumidity { get; set; }

        [Column("certi_containers")]
        public string? CertiContainers { get; set; }

        [Column("certi_container_count")]
        public int? CertiContainerCount { get; set; }

        [Column("certi_container_size")]
        public int? CertiContainerSize { get; set; }

        [Column("certi_invoice_no")]
        public string? CertiInvoiceNo { get; set; }

        [Column("certi_invoice_date")]
        public DateTime? CertiInvoiceDate { get; set; }

        [Column("certi_afo_name")]
        public string? CertiAfoName { get; set; }

        [Column("certi_remarks")]
        public string? CertiRemarks { get; set; }

        [Column("certi_exp_name")]
        public string? CertiExpName { get; set; }

        [Column("certi_exp_address")]
        public string? CertiExpAddress { get; set; }

        [Column("certi_exp_email")]
        public string? CertiExpEmail { get; set; }

        [Column("certi_consignee")]
        public string? CertiConsignee { get; set; }

        [Column("certi_consignee_address")]
        public string? CertiConsigneeAddress { get; set; }

        [Column("certi_notify_party")]
        public string? CertiNotifyParty { get; set; }

        [Column("certi_notify_address")]
        public string? CertiNotifyAddress { get; set; }

        [Column("certi_cargo_desc")]
        public string? CertiCargoDesc { get; set; }

        [Column("certi_net_qty")]
        public double? CertiNetQty { get; set; }

        [Column("certil_gross_qty")]
        public double? CertilGrossQty { get; set; }

        [Column("certi_net_unit")]
        public string? CertiNetUnit { get; set; }

        [Column("certi_gross_unit")]
        public string? CertiGrossUnit { get; set; }

        [Column("certi_no_bags")]
        public string? CertiNoBags { get; set; }

        [Column("certi_packing_desc")]
        public string? CertiPackingDesc { get; set; }

        [Column("certi_shipping_mark")]
        public string? CertiShippingMark { get; set; }

        [Column("certi_ref_by")]
        public string? CertiRefBy { get; set; }

        [Column("certi_country_dest")]
        public string? CertiCountryDest { get; set; }

        [Column("certi_tg_packing")]
        public bool? CertiTgPacking { get; set; }

        [Column("certi_tg_commodity")]
        public bool? CertiTgCommodity { get; set; }

        [Column("certi_tg_pack_comm")]
        public bool? CertiTgPackComm { get; set; }

        [Column("certi_surface_thickness")]
        public string? CertiSurfaceThickness { get; set; }

        [Column("certi_stack")]
        public bool? CertiStack { get; set; }

        [Column("certi_container")]
        public bool? CertiContainer { get; set; }

        [Column("certi_chamber")]
        public bool? CertiChamber { get; set; }

        [Column("certi_tested_container")]
        public bool? CertiTestedContainer { get; set; }

        [Column("certi_unsheeted_container")]
        public bool? CertiUnsheetedContainer { get; set; }

        [Column("certi_applied_rate")]
        public float? CertiAppliedRate { get; set; }

        [Column("certi_final_reading")]
        public string? CertiFinalReading { get; set; }

        [Column("certi_create_uid")]
        public string? CertiCreateUid { get; set; }

        [Column("certi_edited_uid")]
        public string? CertiEditedUid { get; set; }

        [Column("certi_created")]
        public DateTime? CertiCreated { get; set; }

        [Column("certi_updated")]
        public DateTime? CertiUpdated { get; set; }

        [Column("certi_bill_id")]
        public int? CertiBillId { get; set; }

        [Column("certi_locked_by")]
        public int? CertiLockedBy { get; set; }

        [Column("certi_phyto")]
        public string? CertiPhyto { get; set; }

        [Column("certi_jobtype")]
        public string? CertiJobType { get; set; }

        [Column("certi_temperature")]
        public double? CertiTemperature { get; set; }

        [Column("certi_2notify")]
        public bool? Certi2Notify { get; set; }

        [Column("certi_companyid")]
        public int CertiCompanyId { get; set; }

        [Column("certi_yearid")]
        public int CertiYearId { get; set; }

        [Column("certi_prefix")]
        public string? Certiprefix { get; set; }

        [Column("certi_suffix")]
        public string? Certisuffix { get; set; }


        [Column("certi_doserate_unit")]
        public string? CertiDoseRateUnit { get; set; }

        

    }
}