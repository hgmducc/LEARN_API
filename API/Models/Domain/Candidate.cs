using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Domain;

[Index("ExperienceLevelId", Name = "IDX_Candidates_ExperienceLevelId")]
[Index("UserId", Name = "IDX_Candidates_UserId")]
public partial class Candidate
{
    [Key]
    public int CandidateId { get; set; }

    public long? UserId { get; set; }

    [StringLength(200)]
    public string? FullName { get; set; }

    [StringLength(250)]
    public string? Address { get; set; }

    public byte? Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Summary { get; set; }

    [StringLength(1000)]
    public string? CvUrl { get; set; }

    public long? SalaryExpectation { get; set; }

    [StringLength(10)]
    public string SalaryCurrency { get; set; } = null!;

    public int? ExperienceLevelId { get; set; }

    public string? Meta { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Candidates")]
    public virtual User? User { get; set; }
}
