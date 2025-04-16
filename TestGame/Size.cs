namespace TestGame;

public class Size
{
    private int _width;
    private int _height;

    public Size(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }
}