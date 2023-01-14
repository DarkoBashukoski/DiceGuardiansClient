using System;
using System.Collections.Generic;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.RenderEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.GameStates;

public class MainMenu : State {
    private readonly User _user;
    
    private SpriteFont _font;
    
    private Image _background;
    private Image _logo;
    private Image _container;

    private Button _play;
    private Button _collection;
    private Button _settings;
    private Button _quit;

    private Image _profileContainer;
    private Image _profileImage;
    private Label _username;
    private Label _mmr;

    private bool _matchmaking;
    
    private AnimatedImage _loadingCircle;
    private Label _matchmakingLabel;
    private Label _timer;
    private Button _cancelButton;
    private Button _debugStartMatch;
    private float _timeWaiting;
    
    public MainMenu(DisplayManager displayManager, User user) : base(displayManager) {
        _user = user;
        LoadElements(DisplayManager);
        
        _play.SetText("Play");
        _collection.SetText("Collection");
        _settings.SetText("Settings");
        _quit.SetText("Quit");
        
        _matchmakingLabel.SetText("Finding Match...");
        _cancelButton.SetText("Cancel");
        _debugStartMatch.SetText("DEBUG: Start Game");
        _timeWaiting = 0;
        _timer.SetText(TimeSpan.FromMilliseconds(_timeWaiting).ToString(@"mm\:ss"));
        
        _username.SetText(_user.GetUserName());
        _mmr.SetText($"MMR: {_user.GetMmr()}");

        _matchmaking = false;
        _loadingCircle.SetAnimationSpeed(0.25f);
        
        SetListeners();
    }
    
    public override List<GuiElement> GetGuiElements() {
        if (_matchmaking) {
            return new List<GuiElement> {
                _background,
                _container,
                _logo,
                _loadingCircle,
                _matchmakingLabel,
                _timer,
                _cancelButton,
                _debugStartMatch
            };
        }
        
        return new List<GuiElement> {
            _background,
            _container,
            _logo,
            _play,
            _collection,
            _settings,
            _quit,
            _profileContainer,
            _profileImage,
            _username,
            _mmr
        };
    }

    public override List<Entity> Get3dSprites() {
        return new List<Entity>();
    }

    public override void Update(GameTime gameTime) {
        if (_matchmaking) {
            UpdateTimer(gameTime);
            _cancelButton.Update(gameTime);
            _loadingCircle.StepAnimation();
            _debugStartMatch.Update(gameTime);
            return;
        }
        
        _play.Update(gameTime);
        _collection.Update(gameTime);
        _settings.Update(gameTime);
        _quit.Update(gameTime);
    }
    
    private void LoadElements(DisplayManager displayManager) {
        Texture2D buttonTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/Button");
        Texture2D backgroundTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/MenuBackground");
        Texture2D logoTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/Logo");
        Texture2D containerTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/MenuContainer");
        Texture2D profileTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/ProfileImage");
        Texture2D loadingTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/Loading");
        _font = displayManager.GetContent().Load<SpriteFont>("arial");

        _background = new Image(backgroundTexture, new Vector2(0, 0), new Vector2(DisplayManager.GetWidth(), DisplayManager.GetHeight()));
        float ratio = (float) logoTexture.Height / logoTexture.Width;
        _logo = new Image(logoTexture, new Vector2(DisplayManager.GetWidth()/2-175, 25), new Vector2(350, 350*ratio));
        _container = new Image(containerTexture, new Vector2(DisplayManager.GetWidth()/2-150, 185), new Vector2(300, 520));
        _profileContainer = new Image(containerTexture, new Vector2(-60, -400), new Vector2(360, 520));
        
        _play = new Button(buttonTexture, _font, new Vector2(DisplayManager.GetWidth() / 2 - 100, 300), new Vector2(200, 50));
        _collection = new Button(buttonTexture, _font, new Vector2(DisplayManager.GetWidth() / 2 - 100, 400), new Vector2(200, 50));
        _settings = new Button(buttonTexture, _font, new Vector2(DisplayManager.GetWidth() / 2 - 100, 500), new Vector2(200, 50));
        _quit = new Button(buttonTexture, _font, new Vector2(DisplayManager.GetWidth() / 2 - 100, 600), new Vector2(200, 50));
        
        _profileImage = new Image(profileTexture, new Vector2(10, 10), new Vector2(90, 90));
        _username = new Label(_font, new Vector2(110, 15), displayManager.GetGraphicsDevice());
        _mmr = new Label(_font, new Vector2(110, 45), displayManager.GetGraphicsDevice());
        
        _loadingCircle = new AnimatedImage(loadingTexture, new Vector2(4, 4), new Vector2(DisplayManager.GetWidth() / 2 - 75, 250), new Vector2(150, 150));
        _matchmakingLabel = new Label(_font, new Vector2(DisplayManager.GetWidth() / 2 - 50, 400), displayManager.GetGraphicsDevice());
        _timer = new Label(_font, new Vector2(DisplayManager.GetWidth() / 2 - 17, 450), displayManager.GetGraphicsDevice());
        _cancelButton = new Button(buttonTexture, _font, new Vector2(DisplayManager.GetWidth() / 2 - 100, 600), new Vector2(200, 50));
        _debugStartMatch = new Button(buttonTexture, _font, new Vector2(1070, 660), new Vector2(200, 50));
    }

    private void SetListeners() {
        _play.Click += (_, _) => StartMatchmaking();
        //_collection TODO
        //_settings TODO
        _quit.Click += (_, _) => Environment.Exit(0);

        _cancelButton.Click += (_, _) => StopMatchmaking();
        _debugStartMatch.Click += (_, _) => StartGame(); //TODO delete this
    }

    private void StartMatchmaking() {
        _matchmaking = true;
        _timeWaiting = 0;
        _timer.SetText(TimeSpan.FromMilliseconds(_timeWaiting).ToString(@"mm\:ss"));
        //TODO send matchmaking message
    }

    private void StopMatchmaking() {
        _matchmaking = false;
        //TODO send cancel message
    }

    private void StartGame() { //TODO delete this
        StateManager.SetState(new GameInstance(DisplayManager));
    }

    private void UpdateTimer(GameTime gameTime) {
        _timeWaiting += gameTime.ElapsedGameTime.Milliseconds;
        _timer.SetText(TimeSpan.FromMilliseconds(_timeWaiting).ToString(@"mm\:ss"));
    }
}