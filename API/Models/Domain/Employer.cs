using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Domain;

public partial class Employer
{
    [Key]
    public long UserId { get; set; }

    [StringLength(200)]
    public string? FullName { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    [StringLength(1000)]
    public string? AvatarUrl { get; set; }

    [StringLength(300)]
    public string? CompanyName { get; set; }

    public string? CompanyDescription { get; set; }

    [StringLength(50)]
    public string? CompanySize { get; set; }

    [StringLength(500)]
    public string? CompanyAddress { get; set; }

    [StringLength(50)]
    public string? TaxCode { get; set; }

    [StringLength(500)]
    public string? BusinessLicenseUrl { get; set; }

    public string? Meta { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Employer")]
    public virtual User User { get; set; } = null!;
}
