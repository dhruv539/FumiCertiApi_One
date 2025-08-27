﻿using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.DTOs
{
    public class CompanyAddDto
    {
        [Required(ErrorMessage = "Company name is required.")]
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Address1 { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public bool IsGstApplicable { get; set; }
        public string? Gstin { get; set; }
        public bool Status { get; set; }
        public string? Remarks { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? StateId { get; set; }
        public string? Panno { get; set; }

    }
    public class CompanyDto
    {
        public int? CompanyId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Address1 { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public bool IsGstApplicable { get; set; }
        public string? Gstin { get; set; }
        public bool Status { get; set; }
        public string? Remarks { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? StateId { get; set; }
        public string? Panno { get; set; }

    }
}
