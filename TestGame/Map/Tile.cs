using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.DirectWrite;

namespace TestGame.Map;

public class Tile
{
    private Position _position;
    private Size _size;
    private GameServiceContainer _services;
    private SpriteSheetPosition _xSpriteSheetPosition;
    private SpriteSheetPosition _ySpriteSheetPosition;

    public enum SpriteSheetPosition
    {
        START,
        MIDDLE,
        END,
        Empty
    }

    public Tile(GameServiceContainer services, Position position, Size size, SpriteSheetPosition xSpriteSheetPosition,
        SpriteSheetPosition ySpriteSheetPosition)
    {
        _position = position;
        _size = size;
        _services = services;
        _xSpriteSheetPosition = xSpriteSheetPosition;
        _ySpriteSheetPosition = ySpriteSheetPosition;
    }

    private Position GetOffset()
    {
        Position offset = new Position(0, 0);
        switch (_xSpriteSheetPosition)
        {
            case SpriteSheetPosition.MIDDLE:
            {
                offset.Add(new Position(8, 0));
                break;
            }
            case SpriteSheetPosition.END:
            {
                offset.Add(new Position(16, 0));
                break;
            }
        }

        switch (_ySpriteSheetPosition)
        {
            case SpriteSheetPosition.MIDDLE:
            {
                offset.Add(new Position(0, 8));
                break;
            }
            case SpriteSheetPosition.END:
            {
                offset.Add(new Position(0, 16));
                break;
            }
        }

        return offset;
    }

    public void Draw()
    {
        if (!_xSpriteSheetPosition.Equals(SpriteSheetPosition.Empty))
        {
            Texture2D spriteSheet = _services.GetService<Texture2D>();
            Position offset = new Position(0, 8);
            offset.Add(GetOffset());
            Rectangle rect = new Rectangle(offset.IntX(), offset.IntY(), 8, 8);

            SpriteBatch spriteBatch = _services.GetService<SpriteBatch>();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(spriteSheet, _position.Copy().ToVector2(), rect, Color.White, 0f, Vector2.Zero,
                _size.GetWidth() / 8f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }

    public void ClampPlayer(Position position, Size size, ref float velocityY, ref bool isOnGround)
    {
        if (_position == null || _size == null) return;
        float px = position.GetX();
        float py = position.GetY();
        float pw = size.GetWidth();
        float ph = size.GetHeight();

        float tx = _position.GetX();
        float ty = _position.GetY();
        float tw = _size.GetWidth();
        float th = _size.GetHeight();

        if (!IsColliding(position, size, _position, _size)) return;

        float centerX = position.GetX() + size.GetWidth() / 2f;
        float centerY = position.GetY() + size.GetHeight() / 2f;

        float tileCenterX = _position.GetX() + _size.GetWidth() / 2f;
        float tileCenterY = _position.GetY() + _size.GetHeight() / 2f;

        float dx = centerX - tileCenterX;
        float dy = centerY - tileCenterY;

        if (Math.Abs(dx) > Math.Abs(dy))
        {
            if (dx < 0)
                position.SetX(_position.GetX() - size.GetWidth());
            else
                position.SetX(_position.GetX() + _size.GetWidth());
        }
        else
        {
            if (dy < 0)
            {
                position.SetY(_position.GetY() - size.GetHeight());
                if (velocityY > 0)  // Postava padá dolů, takže zpomalujeme
                {
                    velocityY = 0f;  // Zastavení pohybu po kolizi s podlahou
                    isOnGround = true;  // Postava je na zemi
                }
            }
            else
            {
                position.SetY(_position.GetY() + _size.GetHeight());
                velocityY = 0f; // <<< Zastavení výskoku (hlava do bloku)
            }
        }
    }

    private bool IsColliding(Position aPos, Size aSize, Position bPos, Size bSize)
    {
        float ax = aPos.GetX();
        float ay = aPos.GetY();
        float aw = aSize.GetWidth();
        float ah = aSize.GetHeight();

        float bx = bPos.GetX();
        float by = bPos.GetY();
        float bw = bSize.GetWidth();
        float bh = bSize.GetHeight();

        return ax < bx + bw &&
               ax + aw > bx &&
               ay < by + bh &&
               ay + ah > by;
    }
}