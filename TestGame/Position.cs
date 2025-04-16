using System.Numerics;

namespace TestGame;

public class Position
{
    private float _x;
    private float _y;

    public Position(float x, float y)
    {
        _x = x;
        _y = y;
    }

    public float GetX()
    {
        return _x;
    }

    public float GetY()
    {
        return _y;
    }

    public int IntX()
    {
        return (int)_x;
    }

    public int IntY()
    {
        return (int) _y;
    }

    public void Add(Position position)
    {
        _x += position.GetX();
        _y += position.GetY();
    }

    public Vector2 ToVector2()
    {
        return new Vector2(_x, _y);
    }
}