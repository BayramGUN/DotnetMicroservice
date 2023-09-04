using System.ComponentModel.DataAnnotations;

namespace CommandsService.Api.DTOs;

public class PlatformPublishedDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Event { get; set; }
}