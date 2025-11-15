using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Domain;

[Index("Role", Name = "IDX_Users_Role")]
[Index("Email", Name = "UQ_Users_Email", IsUnique = true)]
[Index("Phone", Name = "UQ_Users_Phone", IsUnique = true)]
public partial class User
{
    [Key]
    public long Id { get; set; }

    [StringLength(320)]
    public string Email { get; set; } = null!;

    [MaxLength(512)]
    public byte[] PasswordHash { get; set; } = null!;

    [StringLength(20)]
    public string Role { get; set; } = null!;

    [StringLength(30)]
    public string Phone { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("User")]
    public virtual Administrator? Administrator { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    [InverseProperty("User")]
    public virtual Employer? Employer { get; set; }
}
