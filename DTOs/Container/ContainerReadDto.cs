namespace FumicertiApi.DTOs.Container
{
    public class ContainerReadDto
    {
        public int? ContainerCid { get; set; }
        public string? ContainerCertiId { get; set; }
        public string? ContainerContainerNo { get; set; }
        public string? ContainerCsize { get; set; }
        public decimal? ContainerConsumeQty { get; set; }
        public DateTime? ContainerDt1 { get; set; }
        public DateTime? ContainerDt2 { get; set; }
        public DateTime? ContainerDt3 { get; set; }
        public TimeSpan? ContainerTime1 { get; set; }
        public TimeSpan? ContainerTime2 { get; set; }
        public TimeSpan? ContainerTime3 { get; set; }
        public decimal? ContainerFb1 { get; set; }
        public decimal? ContainerFb2 { get; set; }
        public decimal? ContainerFb3 { get; set; }
        public decimal? ContainerMc1 { get; set; }
        public decimal? ContainerMc2 { get; set; }
        public decimal? ContainerMc3 { get; set; }
        public decimal? ContainerTb1 { get; set; }
        public decimal? ContainerTb2 { get; set; }
        public decimal? ContainerTb3 { get; set; }
        public string? ContainerEquilibrium { get; set; }
        public decimal? ContainerVolL { get; set; }
        public decimal? ContainerVolB { get; set; }
        public decimal? ContainerVolH { get; set; }
        public string? ContainerProdID1 { get; set; }
        public string? ContainerProdID2 { get; set; }
        public string? ContainerProdID3 { get; set; }
        public decimal? ContainerQty1 { get; set; }
        public decimal? ContainerQty2 { get; set; }
        public decimal? ContainerWt1 { get; set; }
        public decimal? ContainerWt2 { get; set; }
        public decimal? ContainerWt3 { get; set; }
        public decimal? ContainerFbper1 { get; set; }
        public decimal? ContainerFbper2 { get; set; }
        public decimal? ContainerFbper3 { get; set; }
        public decimal? ContainerMcper1 { get; set; }
        public decimal? ContainerMcper2 { get; set; }
        public decimal? ContainerMcper3 { get; set; }
        public decimal? ContainerTbper1 { get; set; }
        public decimal? ContainerTbper2 { get; set; }
        public decimal? ContainerTbper3 { get; set; }
        public string? ContainerEquipmentType { get; set; }
        public string? ContainerProductname { get; set; }
        public decimal? ContainerActualDoseRate { get; set; }
        public decimal? ContainerFirstTvl { get; set; }
        public decimal? ContainerSecondTlv { get; set; }
        public decimal? ContainerCalculateDose { get; set; }
        public string? ContainerCreateUid { get; set; }
        public string? ContainerEditedUid { get; set; }
        public DateTime? ContainerCreated { get; set; }
        public DateTime? ContainerUpdated { get; set; }
        // 🚨 Extra fields from DB (all varchar)
        public decimal? ContainerVolumecbm { get; set; }
        public string? ContainerQtymbrgram { get; set; }

        // ⚠ special case: column name has `%`
        [System.ComponentModel.DataAnnotations.Schema.Column("container_100%mbrgram")]
        public string? Container100Mbrgram { get; set; }

        public string? ContainerRequredprod1 { get; set; }
        public string? ContainerRequredprod2 { get; set; }
        public string? ContainerReqcylinder { get; set; }
        public string? ContainerP1 { get; set; }
        public string? ContainerP2 { get; set; }
        public string? ContainerTotalqtygram { get; set; }
        public string? ContainerExcessqtygrams { get; set; }
        public string? ContainerTotalqtyconsumed { get; set; }
    }

}
