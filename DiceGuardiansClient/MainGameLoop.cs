using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Collisions;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.GameStates;
using DiceGuardiansClient.Source.Networking;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.UserInput;
using DiceGuardiansClient.Source.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiceGuardiansClient;

public class MainGameLoop : Game {
    private static GraphicsDeviceManager _graphics;
    private static DisplayManager _displayManager;
    private SpriteBatch spriteBatch;

    private Camera _camera;
    private MasterRenderer _renderer;

    //private MainMenu _menu;

    //private int[,] pathPiece = {
    //    { 0, 1, 0 },
    //    { 1, 1, 1 },
    //    { 0, 1, 0 },
    //    { 0, 1, 0 }
    //};

    //private Entity e1, e2, e3, e4, e5, e6;
    //private int playerIndex = 1;
    //private static bool isMyTurn = true;
    
    public MainGameLoop() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        _camera = new Camera(new Vector3(6f, 21f, 17.8f), new Vector3(70f, 0f, 0f));
        Content = new ContentManager(Services, "Content");
        _displayManager = new DisplayManager(GraphicsDevice, _graphics, Content, _camera);
        spriteBatch = new SpriteBatch(GraphicsDevice);
        
        _renderer = new MasterRenderer(_displayManager, spriteBatch);
        AllCards.InitializeCardInfo(_displayManager); //TODO make server responsible
        MousePicker.Start(_camera, _renderer.GetProjectionMatrix());

        //Model red = Content.Load<Model>("RedTile");
        //Model blue = Content.Load<Model>("BlueTile");

        //e1 = new Entity((playerIndex == 1) ? blue : red, new Vector3(0, 0, 0), new Vector3(-90, 0, 0), 1);
        //e2 = new Entity((playerIndex == 1) ? blue : red, new Vector3(0, 0, 0), new Vector3(-90, 0, 0), 1);
        //e3 = new Entity((playerIndex == 1) ? blue : red, new Vector3(0, 0, 0), new Vector3(-90, 0, 0), 1);
        //e4 = new Entity((playerIndex == 1) ? blue : red, new Vector3(0, 0, 0), new Vector3(-90, 0, 0), 1);
        //e5 = new Entity((playerIndex == 1) ? blue : red, new Vector3(0, 0, 0), new Vector3(-90, 0, 0), 1);
        //e6 = new Entity((playerIndex == 1) ? blue : red, new Vector3(0, 0, 0), new Vector3(-90, 0, 0), 1);

        NetworkManager.Start();
        StateManager.Start(_displayManager);
        StateManager.SetState(new LoginScreen(_displayManager));

        base.Initialize();
    }

    protected override void Update(GameTime gameTime) {
        NetworkManager.Update();
        KeyboardParser.Update();
        StateManager.GetState().Update(gameTime);

        MousePicker.Update();

        if (StateManager.GetState() is GameInstance) {
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                _camera.SetPosition(_camera.GetPosition().X - 1, _camera.GetPosition().Y, _camera.GetPosition().Z);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _camera.SetPosition(_camera.GetPosition().X + 1, _camera.GetPosition().Y, _camera.GetPosition().Z);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                _camera.SetPosition(_camera.GetPosition().X, _camera.GetPosition().Y, _camera.GetPosition().Z - 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _camera.SetPosition(_camera.GetPosition().X, _camera.GetPosition().Y, _camera.GetPosition().Z + 1);
            }
        }

        //Vector2 coords = BoardCollisions.GetTileCoords(_camera, _mousePicker.GetRay());
        //e1.Position = new Vector3(coords.X, 0, coords.Y);
        //e2.Position = new Vector3(coords.X-1, 0, coords.Y);
        //e3.Position = new Vector3(coords.X, 0, coords.Y+1);
        //e4.Position = new Vector3(coords.X, 0, coords.Y-1);
        //e5.Position = new Vector3(coords.X+1, 0, coords.Y);
        //e6.Position = new Vector3(coords.X+2, 0, coords.Y);
        //if (_gameInstance != null) {
        //    _gameInstance.Update(pathPiece, coords, new Vector2(1, 1), playerIndex);
        //}
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Aqua);

        _renderer.AddGuiElementRange(StateManager.GetState().GetGuiElements());
        _renderer.AddEntityRange(StateManager.GetState().Get3dSprites());
        _renderer.Render(_camera);
        base.Draw(gameTime);
    }
}