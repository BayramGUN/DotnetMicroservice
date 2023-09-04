using System.ComponentModel.DataAnnotations;

namespace CommandsService.Api.Models;

public class CommandCreateDto
{
    [Required]
    public string HowTo { get; set; } = null!;
    [Required]
    public string CommandLine { get; set; } = null!;
}