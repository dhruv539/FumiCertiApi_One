using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.Models
{
    [Table("eqpmentseriealno")]
    public class EquipmentSerial
    {
        [Key]
        [Column("equipment_id")]
        public int EquipmentId { get; set; }

        [Column("equipment_name")]
        [StringLength(150)]
        public string? EquipmentName { get; set; }

        [Column("equipment_serialNo")]
        [StringLength(60)]
        public string? EquipmentSerialNo { get; set; }

        [Column("equipment_created")]
        public DateTime? EquipmentCreated { get; set; }

        [Column("equipment_createdby")]
        [StringLength(45)]
        public string? EquipmentCreatedBy { get; set; }

        [Column("equipment_updated")]
        public DateTime? EquipmentUpdated { get; set; }

        [Column("equipment_updatedby")]
        [StringLength(45)]
        public string? EquipmentUpdatedBy { get; set; }
    }
}
