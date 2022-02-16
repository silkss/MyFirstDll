namespace WebUi.Services;

public class MyDependency : IMyDependecy
{
    public void Some()
    {
        return;
    }
}

public interface IMyDependecy
{
    void Some();
}
