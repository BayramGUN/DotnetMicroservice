namespace CommandsService.Api.EvenProcessing.Interfaces;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}