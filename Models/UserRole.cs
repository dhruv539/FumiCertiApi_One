using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FumicertiApi.Models
{
   
    [Table("roles")]
    public class UserRole
    {
        [Key]
        [Column("role_uuid")]
        [StringLength(70)]
        public string RoleUuid { get; set; } = string.Empty;

        [Column("role_company_id")]
        [StringLength(45)]
    
        public  string RoleCompanyId { get; set; }

        [Column("role_create_uid")]
        [StringLength(60)]
       
        public  string RoleAddedByUserId { get; set; }

        [Column("role_edited_uid")]
        [StringLength(60)]
        public string? RoleUpdatedByUserId { get; set; }


        [Column("role_name")]
        [StringLength(220)]
        [Required(ErrorMessage="Enter Role Name")]
        //[Sieve(CanFilter = true, CanSort = true)]
        public required string RoleName { get; set; }

        [Column("role_status")]
        public int RoleStatus { get; set; } = 1; // 1 => active, 0 => deactivate, 2 => delete

        [Column("role_created")]
        public DateTime RoleCreated { get; set; } = DateTime.UtcNow;

        [Column("role_updated")]
        public DateTime RoleUpdated { get; set; } = DateTime.UtcNow;

        
    }
}
