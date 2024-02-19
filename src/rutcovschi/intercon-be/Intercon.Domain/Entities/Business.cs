using System.ComponentModel.DataAnnotations;
using Intercon.Domain.Abstractions;
using Intercon.Domain.ComplexTypes;
using Intercon.Domain.Enums;

namespace Intercon.Domain.Entities;

public class Business : Entity
{
    public string Title { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string? FullDescription { get; set; } = null!;
    [Range(0, 5)]
    public float Rating { get; set; }
    public Address Address { get; set; } = null!;
    public uint ReviewsCount { get; set; }
    public BusinessCategory Category { get; set; }

    public int OwnerId { get; set; }
    public int? LogoId { get; set; }

    public virtual User Owner { get; set; } = null!;
    public virtual Image? Logo { get; set; } = null;
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}