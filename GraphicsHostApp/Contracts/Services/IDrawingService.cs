namespace GraphicsHostApp.Contracts.Services;

public interface IDrawingService
{
    void Load(object[] args);

    void Update(double deltaSeconds);

    void Render(double deltaSeconds);
}
