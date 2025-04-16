using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame;

public class Player
{
    private GameServiceContainer _services;
    private Position _position;
    private Size _size;
    
    private float _gravity = 500f;
    private float _velocityY = 0f; 
    private float _jumpStrength = -500f;
    private bool _isOnGround = false;
    private float _speed = 250f;
    
    public Player(GameServiceContainer services)
    {
        _services = services;
        _position = new Position(0, 2255);
        _size = new Size(64, 64);
    }

    public void Update(float deltaTime)
    {
        ApplyVerticalMove(deltaTime);
        Move(deltaTime);
        CheckForTileCollision(deltaTime);
    }

    public void CheckForTileCollision(float deltaTime)
    {
        Map.Map map = _services.GetService<Map.Map>();
        map.ClampPlayer(_position, _size, ref _velocityY, ref _isOnGround);
    }

    public void ApplyVerticalMove(float deltaTime)
    {
        _velocityY += _gravity * deltaTime;
        _position.Add(new Position(0, _velocityY * deltaTime));
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
                _velocityY = _jumpStrength;
                _isOnGround = false;
            }
        }
    }
    
    public void Draw()
    {
        Texture2D spriteSheet = _services.GetService<Texture2D>();
        Rectangle rect = new Rectangle(0, 0, 8, 8);
        
        SpriteBatch spriteBatch = _services.GetService<SpriteBatch>();
        
        Camera.Camera camera = _services.GetService<Camera.Camera>();
        
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(spriteSheet, _position.Copy().Add(camera.GetPosition()).ToVector2(), rect, Color.White, 0f, Vector2.Zero, _size.GetWidth()/8f, SpriteEffects.None, 0f);
        spriteBatch.End();
    }

    public Position GetPosition()
    {
        return _position;
    }

    public Size GetSize()
    {
        return _size;
    }
}