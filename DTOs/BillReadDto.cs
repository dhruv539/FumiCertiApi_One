namespace FumicertiApi.DTOs
{
    public class BillReadDto
    {
        public int BillId { get; set; }
        public int BillNo { get; set; }
        public string? BillNoStr { get; set; }
        public DateTime BillDate { get; set; }

        public int BillPartyId { get; set; }
        public string PartyName { get; set; } = string.Empty; // <-- from Notify

        public int BillVoucherId { get; set; }
        public double BillGrossAmt { get; set; }
        public double BillTaxable { get; set; }
        public double BillNetAmt { get; set; }
        public int BillPosId { get; set; }

        public string? BillPrefix { get; set; }
        public string? BillSufix { get; set; }
        public string? BillShipParty { get; set; }
        public string? BillAddress1 { get; set; }
        public string? BillAddress2 { get; set; }
        public string? BillAddress3 { get; set; }
        public string? BillState { get; set; }
        public string? BillGstin { get; set; }
        public string? BillPin { get; set; }
        public string? BillContactNo { get; set; }

        public DateTime? BillDateFrom { get; set; }
        public DateTime? BillDateTo { get; set; }
        public string? BillIrnNo { get; set; }
        public string? BillAckNo { get; set; }
        public DateTime? BillAckDate { get; set; }

        public string BillSupplyType { get; set; } = string.Empty;
        public float BillRatePerCont { get; set; }
        public float BillGstPer { get; set; }
        public int BillGstSlabId { get; set; }
        public float BillSgst { get; set; }
        public float BillCgst { get; set; }
        public float BillIgst { get; set; }

        public string? Remarks { get; set; }
        public string? FilterPartyName { get; set; }
        public float BillRate40Cont { get; set; }
        public int BillCompanyId { get; set; }
        public int BillYearId { get; set; }
    }

}
