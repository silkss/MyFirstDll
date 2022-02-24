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

    public void Signal(string symbol, double price, string type)
    {
        if (WorkingContainers.FirstOrDefault(c => c.Future.LocalSymbol == symbol) is Container container)
        {
            if (type == "OPEN")
            {
                container.OpenStraddle(price);
            }
            else if (type == "CLOSE")
            {
                container.CloseStraddle(price);
            }
        }
    }
}
