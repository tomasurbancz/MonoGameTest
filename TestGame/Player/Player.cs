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

    private Animator.Animator _idle;
    private Animator.Animator _jumping;
    private Animator.Animator _walkingLeft;
    private Animator.Animator _walkingRight;
    private Animator.Animator _currentAnimation;
    
    public Player(GameServiceContainer services)
    {
        _services = services;
        _position = new Position(0, 2255);
        _size = new Size(64, 64);
        _idle = new Animator.Animator(_services, new Position(0, 0), 2);
        _jumping = new Animator.Animator(_services, new Position(0, 32), 1);
        _walkingRight = new Animator.Animator(_services, new Position(0, 40), 1);
        _walkingLeft = new Animator.Animator(_services, new Position(0, 48), 1);
        _currentAnimation = _idle;
    }

    public void Update(float deltaTime)
    {
        _currentAnimation.Update(deltaTime);
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
        int moved = 0;
        
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            moved -= 1;
            _position.Add(new Position(-_speed * deltaTime, 0));
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            moved += 1;
            _position.Add(new Position(_speed * deltaTime, 0));
        }

        if (moved == 1)
        {
            _currentAnimation = _walkingRight;
        }
        else if (moved == -1)
        {
            _currentAnimation = _walkingLeft;
        }
        else
        {
            _currentAnimation = _idle;
        }

        if (!_isOnGround)
        {
            _currentAnimation = _jumping;
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
        SpriteBatch spriteBatch = _services.GetService<SpriteBatch>();
        
        Camera.Camera camera = _services.GetService<Camera.Camera>();
        
        spriteBatch.Begin();
        spriteBatch.Draw(_currentAnimation.GetCurrentTexture(), _position.Copy().Add(camera.GetPosition()).ToVector2(), Color.White);
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