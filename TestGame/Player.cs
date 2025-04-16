using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame;

public class Player
{
    private GameServiceContainer _services;
    private Position _position;
    private float _gravity = 500f;
    private Size _size;
    private float _speed = 250f;
    private bool _isOnGround;
    private float _jumpPower;
    private float _maxJumpPower = 900f;
    
    public Player(GameServiceContainer services)
    {
        _services = services;
        _position = new Position(0, 0);
        _size = new Size(64, 64);
    }

    public void Update(float deltaTime)
    {
        ApplyVerticalMove(deltaTime);

        Move(deltaTime);
        ClampWithinBounds(_position);
        CheckIfIsOnGround(deltaTime);
    }

    public void ApplyVerticalMove(float deltaTime)
    {
        _position.Add(new Position(0, (_gravity + _jumpPower) * deltaTime));
        _jumpPower += _maxJumpPower * deltaTime;
        _jumpPower = Math.Min(0, _jumpPower);
    }

    public void CheckIfIsOnGround(float deltaTime)
    {
        Position position1 = _position.Copy();
        Position position2 = _position.Copy();
        
        position2.Add(new Position(0, _gravity * deltaTime));
        ClampWithinBounds(position2);
        
        if (position1.IsSame(position2))
        {
            _isOnGround = true;
        }
        else
        {
            _isOnGround = false;
        }
    }

    public void Move(float deltaTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            _position.Add(new Position(-_speed * deltaTime, 0));
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            _position.Add(new Position(_speed * deltaTime, 0));
        }

        if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            if (_isOnGround)
            {
                _jumpPower = -_maxJumpPower;
            }
        } 
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

    public void ClampWithinBounds(Position position)
    {
        Size screenSize = _services.GetService<Size>();
        position.SetX(Math.Max(0, position.GetX()));
        position.SetX(Math.Min(screenSize.GetWidth() - _size.GetWidth(), position.GetX()));
        position.SetY(Math.Max(0, position.GetY()));
        position.SetY(Math.Min(screenSize.GetHeight() - _size.GetHeight(), position.GetY()));
    }
}