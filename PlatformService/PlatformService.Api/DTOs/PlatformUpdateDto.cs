using System.ComponentModel.DataAnnotations;

namespace PlatformService.Api.DTOs;

public class PlatformUpdateDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Publisher { get; set; } = null!;
    [Required]
    public string Cost { get; set; } = null!;
}