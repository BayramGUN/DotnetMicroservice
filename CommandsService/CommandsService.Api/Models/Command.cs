using System.ComponentModel.DataAnnotations;

namespace CommandsService.Api.Models;

public class Command
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string HowTo { get; set; } = null!;
    [Required]
    public string CommandLine { get; set; } = null!;
    [Required]
    public int PlatformId { get; set; }
    public Platform Platform { get; set; } = new();

}