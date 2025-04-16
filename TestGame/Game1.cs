using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private GameServiceContainer _services;
    private Player _player;
    private Map.Map _map;
    private Camera.Camera _camera;
    
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _services = new GameServiceContainer(); 
        
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        _services.AddService(_graphics);
        _services.AddService(_spriteBatch);

        Texture2D spriteSheet = Content.Load<Texture2D>("spritesheet");
        _services.AddService<Texture2D>(spriteSheet);

        Size screenSize = new Size(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        _services.AddService<Size>(screenSize);
        
        _player = new Player(_services);
        _services.AddService<Player>(_player);
        
        _map = new Map.Map(_services);
        _services.AddService<Map.Map>(_map);

        _camera = new Camera.Camera(_services);
        _services.AddService<Camera.Camera>(_camera);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _player.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
        _camera.Update();
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _player.Draw();
        _map.Draw();

        base.Draw(gameTime);
    }
}
