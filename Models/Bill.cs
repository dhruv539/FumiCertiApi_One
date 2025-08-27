using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.Models
{
    [Table("bills")]
    public class Bill
    {
        [Key]
        [Column("bill_id")]
        public int BillId { get; set; }

        [Column("bill_no")]
        public int BillNo { get; set; }

        [Column("bill_no_str")]
        public string? BillNoStr { get; set; }

        [Column("bill_date")]
        public DateTime BillDate { get; set; }

        [Column("bill_party_id")]
        public int BillPartyId { get; set; }

     
        [Column("bill_voucher_id")]
        public int BillVoucherId { get; set; }

     

        [Column("bill_gross_amt")]
        public double BillGrossAmt { get; set; }

        [Column("bill_taxable")]
        public double BillTaxable { get; set; }

        [Column("bill_net_amt")]
        public double BillNetAmt { get; set; }

        
        [Column("bill_pos_id")]
        public int BillPosId { get; set; }

       

        [Column("bill_prefix")]
        public string? BillPrefix { get; set; }

        [Column("bill_sufix")]
        public string? BillSufix { get; set; }

        [Column("bill_ship_party")]
        public string? BillShipParty { get; set; }

        [Column("bill_address1")]
        public string? BillAddress1 { get; set; }

        [Column("bill_address2")]
        public string? BillAddress2 { get; set; }

        [Column("bill_address3")]
        public string? BillAddress3 { get; set; }

        [Column("bill_state")]
        public string? BillState { get; set; }

        [Column("bill_gstin")]
        public string? BillGstin { get; set; }

        [Column("bill_pin")]
        public string? BillPin { get; set; }

        [Column("bill_contact_no")]
        public string? BillContactNo { get; set; }

        [Column("bill_date_from")]
        public DateTime? BillDateFrom { get; set; }

        [Column("bill_date_to")]
        public DateTime? BillDateTo { get; set; }

        [Column("bill_irn_no")]
        public string? BillIrnNo { get; set; }

        [Column("bill_ack_no")]
        public string? BillAckNo { get; set; }

        [Column("bill_ack_date")]
        public DateTime? BillAckDate { get; set; }

        [Column("bill_supply_type")]
        public string? BillSupplyType { get; set; }

        [Column("bill_rate_per_cont")]
        public float BillRatePerCont { get; set; }

        [Column("bill_gst_per")]
        public float BillGstPer { get; set; }

        [Column("bill_gst_slab_id")]
        public int BillGstSlabId { get; set; }

        [Column("bill_sgst")]
        public float BillSgst { get; set; }

        [Column("bill_cgst")]
        public float BillCgst { get; set; }

        [Column("bill_igst")]
        public float BillIgst { get; set; }

        [Column("bill_filterpartyname")]
        public string? FilterPartyName { get; set; }

        [Column("bill_remarks")]
        public string? Remarks { get; set; }

        [Column("bill_rate_40cont")]
        public float BillRate40Cont { get; set; }

        [Column("bill_company_id")]
        public int BillCompanyId { get; set; }

    }
}
