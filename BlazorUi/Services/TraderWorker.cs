using DataLayer.Models.Strategies;

namespace BlazorUi.Services;

public class TraderWorker
{
    public List<Container> WorkingContainers { get; } = new();
    public void StartContainer(Container item)
    {
        if (WorkingContainers.Contains(item) || item.Started)
        {
            return;
        }
        item.Start();
        WorkingContainers.Add(item);
    }

    public void StopContainer(Container item)
    {
        if (!WorkingContainers.Contains(item))
        {
            item.Stop();
            return;
        }
        item.Stop();
        WorkingContainers.Remove(item);
    }
}
