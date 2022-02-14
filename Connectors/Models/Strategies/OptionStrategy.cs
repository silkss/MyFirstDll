using Connectors.Models.Instruments;
using Connectors.Models.Strategies.Base;

namespace Connectors.Models.Strategies;

public class OptionStrategy : BaseStrategy<Option>
{
    public void OpenPosition()
    {
        if (Instrument == null) return;
        if (Position == Volume) return;

    }
}

