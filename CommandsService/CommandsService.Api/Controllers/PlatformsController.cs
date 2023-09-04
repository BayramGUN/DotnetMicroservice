
using AutoMapper;
using CommandsService.Api.Data.Repositories.Interfaces;
using CommandsService.Api.DTOs;
using CommandsService.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/cs/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepository _commandRepository;
    private readonly IMapper _mapper;

    public PlatformsController(IMapper mapper, ICommandRepository commandRepository)
    {
        _mapper = mapper;
        _commandRepository = commandRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
    {
        Console.WriteLine("--> Getting Platforms form CommandsService");

        var platformItems = _commandRepository.ReadAllPlatforms();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }
    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound POST # Command Service");
        return Ok("Inbound test from PlatformsController!");
    }
}