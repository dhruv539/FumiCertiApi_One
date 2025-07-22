using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.Models
{
    public class Container
    {
        [Key]
        [Column("container_cid")]
        public int? ContainerCid { get; set; }

        [Required]
        [Column("container_certi_id")]
        public string? ContainerCertiId { get; set; }

        [Required]
        [Column("container_ContainerNo")]
        public string? ContainerContainerNo { get; set; }

        [Required]
        [Column("container_csize")]
        public string? ContainerCsize { get; set; }

        [Required]
        [Column("container_ConsumeQty")]
        public decimal? ContainerConsumeQty { get; set; }

        [Column("container_dt1")]
        public DateTime? ContainerDt1 { get; set; }

        [Column("container_dt2")]
        public DateTime? ContainerDt2 { get; set; }

        [Column("container_dt3")]
        public DateTime? ContainerDt3 { get; set; }

        [Column("container_time1")]
        public TimeSpan? ContainerTime1 { get; set; } = null;

        [Column("container_time2")]
        public TimeSpan? ContainerTime2 { get; set; } = null;

        [Column("container_time3")]
        public TimeSpan? ContainerTime3 { get; set; } = null;

        [Column("container_fb1")]
        public decimal? ContainerFb1 { get; set; }

        [Column("container_fb2")]
        public decimal? ContainerFb2 { get; set; }

        [Column("container_fb3")]
        public decimal? ContainerFb3 { get; set; }

        [Column("container_mc1")]
        public decimal? ContainerMc1 { get; set; }

        [Column("container_mc2")]
        public decimal? ContainerMc2 { get; set; }

        [Column("container_mc3")]
        public decimal? ContainerMc3 { get; set; }

        [Column("container_tb1")]
        public decimal? ContainerTb1 { get; set; }

        [Column("container_tb2")]
        public decimal? ContainerTb2 { get; set; }

        [Column("container_tb3")]
        public decimal? ContainerTb3 { get; set; }

        [Column("container_Equilibrium")]
        public string? ContainerEquilibrium { get; set; }

        [Column("container_Vol_L")]
        public decimal? ContainerVolL { get; set; }

        [Column("container_Vol_B")]
        public decimal? ContainerVolB { get; set; }

        [Column("container_Vol_H")]
        public decimal? ContainerVolH { get; set; }

        [Column("container_ProdID1")]
        public string? ContainerProdID1 { get; set; }

        [Column("container_ProdID2")]
        public string? ContainerProdID2 { get; set; }

        [Column("container_ProdID3")]
        public string? ContainerProdID3 { get; set; }

        [Column("container_Qty1")]
        public decimal? ContainerQty1 { get; set; }

        [Column("container_Qty2")]
        public decimal? ContainerQty2 { get; set; }

        [Column("container_Wt1")]
        public decimal? ContainerWt1 { get; set; }

        [Column("container_Wt2")]
        public decimal? ContainerWt2 { get; set; }

        [Column("container_Wt3")]
        public decimal? ContainerWt3 { get; set; }

        [Column("container_fbper1")]
        public decimal? ContainerFbper1 { get; set; }

        [Column("container_fbper2")]
        public decimal? ContainerFbper2 { get; set; }

        [Column("container_fbper3")]
        public decimal? ContainerFbper3 { get; set; }

        [Column("container_mcper1")]
        public decimal? ContainerMcper1 { get; set; }

        [Column("container_mcper2")]
        public decimal? ContainerMcper2 { get; set; }

        [Column("container_mcper3")]
        public decimal? ContainerMcper3 { get; set; }

        [Column("container_tbper1")]
        public decimal? ContainerTbper1 { get; set; }

        [Column("container_tbper2")]
        public decimal? ContainerTbper2 { get; set; }

        [Column("container_tbper3")]
        public decimal? ContainerTbper3 { get; set; }

        [Column("container_EquipmentType")]
        public string? ContainerEquipmentType { get; set; }

        [Column("container_Productname")]
        public string? ContainerProductname { get; set; }

        [Column("container_ActualDoseRate")]
        public decimal? ContainerActualDoseRate { get; set; }

        [Column("container_firstTvl")]
        public decimal? ContainerFirstTvl { get; set; }

        [Column("container_secondtlv")]
        public decimal? ContainerSecondTlv { get; set; }

        [Column("container_calculateDose")]
        public decimal? ContainerCalculateDose { get; set; }

        [Column("container_create_uid")]
        public string? ContainerCreateUid { get; set; }

        [Column("container_edited_uid")]
        public string? ContainerEditedUid { get; set; }

        [Column("container_created")]
        public DateTime? ContainerCreated { get; set; }

        [Column("container_updated")]
        public DateTime? ContainerUpdated { get; set; }
    }

}
