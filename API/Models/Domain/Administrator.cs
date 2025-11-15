using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Domain;

public partial class Administrator
{
    [Key]
    public long UserId { get; set; }

    [StringLength(200)]
    public string DisplayName { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Administrator")]
    public virtual User User { get; set; } = null!;
}
