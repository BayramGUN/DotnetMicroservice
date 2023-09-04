
using System.Net.Http.Headers;
using System.Security.Cryptography;
using AutoMapper;
using CommandsService.Api.Data.Repositories.Interfaces;
using CommandsService.Api.DTOs;
using CommandsService.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/cs/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository _commandRepository;
    private readonly IMapper _mapper;

    public CommandsController(IMapper mapper, ICommandRepository commandRepository)
    {
        _mapper = mapper;
        _commandRepository = commandRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands(int platformId)
    {
        Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");
        if(!_commandRepository.IsPlatformExist(platformId))
            return NotFound();
        var commandItems = _commandRepository.ReadCommandsForPlatform(platformId);
        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
    }
    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");
        if(!_commandRepository.IsPlatformExist(platformId))
            return NotFound();
        var commandItem = _commandRepository.GetCommand(platformId, commandId);
        if(commandItem is null)
            return NotFound();
        return Ok(_mapper.Map<CommandReadDto>(commandItem));
    }
    
    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(
        [FromRoute]int platformId,
        CommandCreateDto commandCreateDto)
    {
        Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");
        if(!_commandRepository.IsPlatformExist(platformId))
            return NotFound();
        
        var commandModel = _mapper.Map<Command>(commandCreateDto);
        var platformModel = _commandRepository.GetPlatform(platformId);
        commandModel.Platform.Name = platformModel.Name;
        commandModel.PlatformId = platformModel.ExternalId;
        _commandRepository.CreateCommand(platformId, commandModel);
        _commandRepository.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

        return CreatedAtRoute(
            nameof(GetCommandForPlatform),
            new { PlatformId = platformId, CommandId = commandReadDto.Id },
            commandReadDto);
    }
}