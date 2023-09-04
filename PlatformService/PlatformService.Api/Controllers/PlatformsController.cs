using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Api.AsyncDataServices.Interfaces;
using PlatformService.Api.Data.Repository.Interface;
using PlatformService.Api.DTOs;
using PlatformService.Api.Models;
using PlatformService.Api.SyncDataServices.Http.Interface;

namespace PlatformService.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(
        IPlatformRepository repository,
        IMapper mapper,
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet(Name = "ReadAllPlatforms")]
    public ActionResult<IEnumerable<PlatformReadDto>> ReadAllPlatforms()
    {
        Console.WriteLine("--> getting platforms...");
        var platformItems = _repository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }

    [HttpGet("{id}", Name = "ReadPlatformById")]
    public ActionResult<PlatformReadDto> ReadPlatformById(int id)
    {
        Console.WriteLine("--> getting platform item...");
        var platformItem = _repository.GetPlatformById(id);
        if(platformItem is not null)
            return Ok(_mapper.Map<PlatformReadDto>(platformItem));
        return NotFound($"platform could not found! {id}");
    }
    [HttpPost(Name = "CreatePlatform")]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatformAsync(PlatformCreateDto platformCreateDto)
    {
        Console.WriteLine("--> creating platform item...");
        if(platformCreateDto is not null)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"--> Could not send synchronously!: {exception.Message}");
            }

            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"--> Could not send asynchronously!: {exception.Message}");
            }
            return CreatedAtRoute(nameof(ReadPlatformById), new { Id = platformModel.Id }, platformReadDto);
        }
        return BadRequest($"{nameof(platformCreateDto)} could not create a platform!");
    }
    [HttpPut("[action]",Name = "UpdatePlatform")]
    public ActionResult<PlatformReadDto> UpdatePlatform(PlatformUpdateDto platformUpdateDto)
    {
        Console.WriteLine("--> updating platform item...");
        if(platformUpdateDto is not null && _repository.IsExist(platformUpdateDto.Id))
        {
            var updatePlatformModel = _mapper.Map<Platform>(platformUpdateDto);
            _repository.UpdatePlatform(_mapper.Map<Platform>(platformUpdateDto));
            _repository.SaveChanges();
            return Ok($"Platform created successfully! {_mapper.Map<PlatformReadDto>(updatePlatformModel).Id}");
        }
        return BadRequest($"{nameof(platformUpdateDto)} could not create a platform!");
    }
}