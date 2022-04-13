namespace Connectors.Interfaces;

public interface IOrderHolder
{
    void OnOrderFilled(int orderId);
    void OnCanceled(int orderId);
    void OnSubmit(int orderId);
    void onFilledQunatityChanged(int orderId);
}
