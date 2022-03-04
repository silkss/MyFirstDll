namespace Connectors.Interfaces;

public interface IOrderHolder
{
    void OnOrderFilled();
    void OnCanceled();
    void onFilledQunatityChanged();
}
