using System;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame;

public class Player
{
    private GameServiceContainer _services;
    private Position _position;
    private float gravity = 500f;
    private Size _size;
    
    public Player(GameServiceContainer services)
    {
        _services = services;
        _position = new Position(0, 0);
        _size = new Size(64, 64);
    }

    public void Update(float deltaTime)
    {
        _position.Add(new Position(0, gravity * deltaTime));
        ClampWithinBounds();
    }
    
    public void Draw()
    {
        Texture2D spriteSheet = _services.GetService<Texture2D>();
        Rectangle rect = new Rectangle(0, 0, 8, 8);
        
        SpriteBatch spriteBatch = _services.GetService<SpriteBatch>();
        
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(spriteSheet, _position.ToVector2(), rect, Color.White, 0f, Vector2.Zero, _size.GetWidth()/8f, SpriteEffects.None, 0f);
        spriteBatch.End();
    }

    public void ClampWithinBounds()
    {
        Size screenSize = _services.GetService<Size>();
        _position.SetX(Math.Max(0, _position.GetX()));
        _position.SetX(Math.Min(screenSize.GetWidth() - _size.GetWidth(), _position.GetX()));
        _position.SetY(Math.Max(0, _position.GetY()));
        _position.SetY(Math.Min(screenSize.GetHeight() - _size.GetHeight(), _position.GetY()));
    }
}