namespace WebApplication2.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);

    }
}
