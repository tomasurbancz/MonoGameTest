using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame;

public class Player
{
    private GameServiceContainer _services;
    private Position _position;
    
    public Player(GameServiceContainer services)
    {
        _services = services;
        _position = new Position(0, 0);
    }

    public void Draw()
    {
        Texture2D spriteSheet = _services.GetService<Texture2D>();
        Rectangle rect = new Rectangle(0, 0, 8, 8);
        
        SpriteBatch spriteBatch = _services.GetService<SpriteBatch>();
        
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(spriteSheet, _position.ToVector2(), rect, Color.White, 0f, Vector2.Zero, 8f, SpriteEffects.None, 0f);
        spriteBatch.End();
        
    }
}