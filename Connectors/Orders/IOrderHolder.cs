namespace Connectors.Orders
{
    public interface IOrderHolder
    {
        void OnOrderFilled();
        void onFilledQunatityChanged();
    }
}
