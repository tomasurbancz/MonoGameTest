using Microsoft.Xna.Framework;

namespace TestGame.Camera;

public class Camera
{
    private GameServiceContainer _services;
    private Position _position;
    private Size _screenSize;

    public Camera(GameServiceContainer services)
    {
        _services = services;
        _screenSize = _services.GetService<Size>();
        _position = new Position(0, 0);
    }

    public void Update()
    {
        Player player = _services.GetService<Player>();
        float cameraX = _screenSize.GetWidth() / 2f - player.GetPosition().GetX() - player.GetSize().GetWidth()/2f;
        float cameraY = _screenSize.GetHeight() /2f - player.GetPosition().GetY() - player.GetSize().GetHeight()/2f;
        
        _position.SetX(cameraX);
        _position.SetY(cameraY);
    }

    public Position GetPosition()
    {
        return _position;
    }
}