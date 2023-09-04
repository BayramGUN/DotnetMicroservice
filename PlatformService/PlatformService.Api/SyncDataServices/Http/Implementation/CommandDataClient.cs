using System.Text;
using System.Text.Json;
using PlatformService.Api.DTOs;
using PlatformService.Api.SyncDataServices.Http.Interface;

namespace PlatformService.Api.SyncDataServices.Http.Implementation;

public class CommandDataClient : ICommandDataClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public CommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platformReadDto),
            Encoding.UTF8,
            "application/json"
        );
        var response = await _httpClient.PostAsync($"{_configuration["CommandsService"]}", httpContent);

        if(response.IsSuccessStatusCode)
            Console.WriteLine("--> Sync POST to CommandsService was OK!");
        else
            Console.WriteLine("--> Sync POST to CommandsService was NOT OK!");
            
    }
}
