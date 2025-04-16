using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.Animator;

public class Animator
{
    private GameServiceContainer _services;
    private List<Texture2D> _textures;
    private float _cooldown = 0.5f;
    private int _currentIndex;
    
    public Animator(GameServiceContainer services, Position offset, int count)
    {
        _services = services;
        _textures = new List<Texture2D>();
        LoadTextures(offset, count);
    }

    private void LoadTextures(Position offset, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Position position = offset.Copy().Add(new Position(i * 8, 0));
            _textures.Add(CreateTexture(position));
        }
    }

    public void Update(float deltaTime)
    {
        if (_cooldown <= 0)
        {
            _currentIndex++;
            if (_currentIndex >= _textures.Count) _currentIndex = 0;
            _cooldown = 0.5f;
        }
        _cooldown -= deltaTime;   
    }
    
    private Texture2D CreateTexture(Position position)
    {
        Texture2D spriteSheet = _services.GetService<Texture2D>();
        int playerWidth = 64;
        int playerHeight = 64;

        RenderTarget2D renderTarget = new RenderTarget2D(
            _services.GetService<GraphicsDevice>(), playerWidth, playerHeight);
        
        _services.GetService<GraphicsDevice>().SetRenderTarget(renderTarget);
        _services.GetService<GraphicsDevice>().Clear(Color.Transparent);

        SpriteBatch spriteBatch = _services.GetService<SpriteBatch>();
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Rectangle sourceRectangle = new Rectangle(position.IntX(), position.IntY(), 8, 8);
        spriteBatch.Draw(spriteSheet, Vector2.Zero, sourceRectangle, Color.White, 0f, Vector2.Zero, 8f, SpriteEffects.None, 0f);

        spriteBatch.End();

        _services.GetService<GraphicsDevice>().SetRenderTarget(null);
        
        return renderTarget;
    }

    public Texture2D GetCurrentTexture()
    {
        return _textures[_currentIndex];
    }
}