using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TestGame.Map;

public class Map
{
    private Tile[,] _tiles;
    private Size _size;
    private GameServiceContainer _services;
    
    public Map(GameServiceContainer services)
    {
        _services = services;
        Size screenSize = services.GetService<Size>();
        _size = new Size(screenSize.GetWidth()/16, screenSize.GetHeight()/16);
        _tiles = new Tile[_size.GetHeight(), _size.GetWidth()];
        CreateEmptyMap();
        CreatePlatform(new Position(0, _size.GetHeight() - 3), _size.GetWidth(), 3);
        CreatePlatform(new Position(8, _size.GetHeight() - 10), 10, 3);
        CreatePlatform(new Position(14, _size.GetHeight() - 17), 10, 3);
        CreatePlatform(new Position(20, _size.GetHeight() - 24), 10, 3);
        
        CreatePlatform(new Position(0, _size.GetHeight() - 6), 3, 3);
        CreatePlatform(new Position(_size.GetWidth() - 3, _size.GetHeight() - 6), 3, 3);
    }

    private void CreateEmptyMap()
    {
        for (int y = 0; y < _size.GetHeight(); y++)
        {
            for (int x = 0; x < _size.GetWidth(); x++)
            {
                _tiles[y, x] = new Tile(null, null, null, Tile.SpriteSheetPosition.Empty,  Tile.SpriteSheetPosition.Empty);
            }
        }
    }

    public void CreatePlatform(Position startPosition, int width, int height = 3)
    {
        int startX = startPosition.IntX();
        int startY = startPosition.IntY();
        for (int y = startY; y < startY + height; y++)
        {
            for (int x = startX; x < startX + width; x++)
            {
                List<Tile.SpriteSheetPosition> spriteSheetPositions = GetSpritePositions(startPosition, width, height, new Position(x, y));
                _tiles[y, x] = new Tile(_services, new Position(x, y).Multiply(16), new Size(16, 16), spriteSheetPositions[0], spriteSheetPositions[1]);
            }
        }
    }

    public List<Tile.SpriteSheetPosition> GetSpritePositions(Position startPosition, int width, int height, Position position)
    {
        Tile.SpriteSheetPosition xPosition;
        Tile.SpriteSheetPosition yPosition;
        if (startPosition.IntX() == position.IntX())
        {
            xPosition = Tile.SpriteSheetPosition.START;
        }
        else if (startPosition.IntX() + width - 1 == position.IntX())
        {
            xPosition = Tile.SpriteSheetPosition.END;
        }
        else
        {
            xPosition = Tile.SpriteSheetPosition.MIDDLE;
        }
        
        if (startPosition.IntY() == position.IntY())
        {
            yPosition = Tile.SpriteSheetPosition.START;
        }
        else if (startPosition.IntY() + height - 1 == position.IntY())
        {
            yPosition = Tile.SpriteSheetPosition.END;
        }
        else
        {
            yPosition = Tile.SpriteSheetPosition.MIDDLE;
        }
        List<Tile.SpriteSheetPosition> spritePositions = new List<Tile.SpriteSheetPosition>();
        spritePositions.Add(xPosition);
        spritePositions.Add(yPosition);
        return spritePositions;
    }

    public void Draw()
    {
        for (int y = 0; y < _size.GetHeight(); y++)
        {
            for (int x = 0; x < _size.GetWidth(); x++)
            {
                _tiles[y, x].Draw();
            }
        }
    }

    public void ClampPlayer(Position position, Size size, ref float velocityY, ref bool isOnGround)
    {
        for (int y = 0; y < _size.GetHeight(); y++)
        {
            for (int x = 0; x < _size.GetWidth(); x++)
            {
                _tiles[y, x].ClampPlayer(position, size, ref velocityY, ref isOnGround);
            }
        }
    }
}