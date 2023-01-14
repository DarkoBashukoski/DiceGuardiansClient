using System;
using System.Collections.Generic;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.Networking;
using DiceGuardiansClient.Source.RenderEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Riptide;

namespace DiceGuardiansClient.Source.GameStates;

public class LoginScreen : State {
    private SpriteFont _font;
    
    private Image _background;
    private Image _logo;
    private Image _container;

    private Label _usernameLabel;
    private Label _passwordLabel;
    private Label _validatingLogin;

    private Button _loginButton;
    private Button _registerMenuButton;

    private TextBox _username;
    private TextBox _password;

    private Label _usernameError;
    private Label _passwordError;

    private Button _registerButton;
    private Button _backButton;
    
    private bool _waitingForLoginResponse;
    private bool _registering;
        
    private AnimatedImage _loadingCircle;

    public LoginScreen(DisplayManager displayManager) : base(displayManager){
        LoadElements(DisplayManager);

        _usernameLabel.SetText("Username");
        _passwordLabel.SetText("Password");
        
        _username.CenterText(true);
        _password.CenterText(true);
        
        _loginButton.SetText("Login");
        _registerMenuButton.SetText("Create New Account");
        
        _registerButton.SetText("Register");
        _backButton.SetText("Back");
        
        _waitingForLoginResponse = false;
        _registering = false;
        
        _loadingCircle.SetAnimationSpeed(0.25f);
        _validatingLogin.SetText("Validating Credentials...");
        
        _usernameError.SetColor(Color.OrangeRed);
        _passwordError.SetColor(Color.OrangeRed);

        SetListeners();
    }

    public override List<Entity> Get3dSprites() {
        return new List<Entity>();
    }

    public override void Update(GameTime gameTime) {
        if (_waitingForLoginResponse) {
            _loadingCircle.StepAnimation();
            return;
        }

        _username.Update(gameTime);
        _password.Update(gameTime);
        
        if (_registering) {
            _registerButton.Update(gameTime);
            _backButton.Update(gameTime);
        } else {
            _loginButton.Update(gameTime);
            _registerMenuButton.Update(gameTime);
        }
    }

    public override List<GuiElement> GetGuiElements() {
        if (_waitingForLoginResponse) {
            return new List<GuiElement> {
                _background,
                _container,
                _logo,
                _loadingCircle,
                _validatingLogin
            };
        }

        if (_registering) {
            return new List<GuiElement> {
                _background,
                _container,
                _logo,
                _usernameLabel,
                _username,
                _passwordLabel,
                _password,
                _usernameError,
                _passwordError,
                _registerButton,
                _backButton
            };
        }
        
        return new List<GuiElement> {
            _background,
            _container,
            _logo,
            _usernameLabel,
            _username,
            _passwordLabel,
            _password,
            _loginButton,
            _registerMenuButton,
            _usernameError,
            _passwordError
        };
    }

    private void LoadElements(DisplayManager displayManager) {
        Texture2D backgroundTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/MenuBackground");
        Texture2D logoTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/Logo");
        Texture2D containerTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/MenuContainer");
        Texture2D buttonTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/Button");
        Texture2D loadingTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/Loading");
        Texture2D textBoxTexture = displayManager.GetContent().Load<Texture2D>("LoadingScreen/TextBox");
        Texture2D caretTexture = displayManager.GetContent().Load<Texture2D>("textCaret");
        _font = displayManager.GetContent().Load<SpriteFont>("arial");
        
        _usernameLabel = new Label(_font, new Vector2(DisplayManager.GetWidth() / 2 - 95, 280), displayManager.GetGraphicsDevice());
        _passwordLabel = new Label(_font, new Vector2(DisplayManager.GetWidth() / 2 - 95, 380), displayManager.GetGraphicsDevice());
        _username = new TextBox(textBoxTexture, caretTexture, _font, new Vector2(DisplayManager.GetWidth()/2-100, 300), new Vector2(200, 50));
        _password = new TextBox(textBoxTexture, caretTexture, _font, new Vector2(DisplayManager.GetWidth()/2-100, 400), new Vector2(200, 50));
        _loginButton = new Button(buttonTexture, _font, new Vector2(DisplayManager.GetWidth() / 2 - 100, 500), new Vector2(200, 50));
        _registerMenuButton = new Button(buttonTexture, _font, new Vector2(DisplayManager.GetWidth() / 2 - 150, 630), new Vector2(300, 50));
        
        _registerButton = new Button(buttonTexture, _font, new Vector2(DisplayManager.GetWidth() / 2 - 100, 500), new Vector2(200, 50));
        _backButton = new Button(buttonTexture, _font, new Vector2(DisplayManager.GetWidth() / 2 - 150, 630), new Vector2(300, 50));

        _validatingLogin = new Label(_font, new Vector2(DisplayManager.GetWidth() / 2 - 75, 400), displayManager.GetGraphicsDevice());
        
        _background = new Image(backgroundTexture, new Vector2(0, 0), new Vector2(DisplayManager.GetWidth(), DisplayManager.GetHeight()));
        float ratio = (float) logoTexture.Height / logoTexture.Width;
        _logo = new Image(logoTexture, new Vector2(DisplayManager.GetWidth()/2-175, 25), new Vector2(350, 350*ratio));
        _container = new Image(containerTexture, new Vector2(DisplayManager.GetWidth()/2-150, 185), new Vector2(300, 420));
        _loadingCircle = new AnimatedImage(loadingTexture, new Vector2(4, 4), new Vector2(DisplayManager.GetWidth() / 2 - 75, 250), new Vector2(150, 150));

        _usernameError = new Label(_font, new Vector2(DisplayManager.GetWidth() / 2 - 95, 350), displayManager.GetGraphicsDevice());
        _passwordError = new Label(_font, new Vector2(DisplayManager.GetWidth() / 2 - 95, 450), displayManager.GetGraphicsDevice());
    }

    private void SetListeners() {
        _loginButton.Click += (_, _) => SendLoginMessage();
        _registerButton.Click += (_, _) => SendRegisterMessage();
        _registerMenuButton.Click += (_, _) => OpenRegisterMenu();
        _backButton.Click += (_, _) => CloseRegisterMenu();
    }

    private void SendLoginMessage() {
        bool error = false;
        
        string user = _username.GetText();
        string pass = _password.GetText();

        if (String.IsNullOrEmpty(user)) { _usernameError.SetText("Username is required"); error = true; } else {_usernameError.SetText("");}
        if (String.IsNullOrEmpty(pass)) { _passwordError.SetText("Password is required"); error = true; } else {_passwordError.SetText("");}
        if (error) {return;}

        Message m = Message.Create(MessageSendMode.Reliable, ClientToServerId.Login);
        m.AddString(user);
        m.AddString(pass);
        NetworkManager.GetClient().Send(m);

        _waitingForLoginResponse = true;
    }

    private void SendRegisterMessage() {
        bool error = false;
        
        string user = _username.GetText();
        string pass = _password.GetText();
        
        _usernameError.SetText("");
        _passwordError.SetText("");

        if (String.IsNullOrEmpty(user)) { _usernameError.SetText("Username is required"); error = true; }
        if (String.IsNullOrEmpty(pass)) { _passwordError.SetText("Password is required"); error = true; }
        if (error) {return;}

        Message m = Message.Create(MessageSendMode.Reliable, ClientToServerId.Register);
        m.AddString(user);
        m.AddString(pass);
        NetworkManager.GetClient().Send(m);

        _waitingForLoginResponse = true;
    }

    private void OpenRegisterMenu() {
        _registering = true;
        _username.SetText("");
        _password.SetText("");
        _usernameError.SetText("");
        _passwordError.SetText("");
    }

    private void CloseRegisterMenu() {
        _registering = false;
        _username.SetText("");
        _password.SetText("");
        _usernameError.SetText("");
        _passwordError.SetText("");
    }

    public void TriggerUserDoesNotExistError() {
        _waitingForLoginResponse = false;
        _usernameError.SetText("User does not exist");
    }
    
    public void TriggerInvalidPasswordError() {
        _waitingForLoginResponse = false;
        _passwordError.SetText("Incorrect password");
    }

    public void TriggerSuccessfulLogin(Message m) {
        string username = m.GetString();
        int mmr = m.GetInt();
        int gamesPlayed = m.GetInt();
        int gamesWon = m.GetInt();

        User user = new User(username, mmr, gamesPlayed, gamesWon);

        StateManager.SetState(new MainMenu(DisplayManager, user));
    }

    public void TriggerUsernameAlreadyTakenError() {
        _waitingForLoginResponse = false;
        _passwordError.SetText("Username already taken");
    }

    public void TriggerSuccessfulRegister(Message m) {
        string username = m.GetString();
        int mmr = m.GetInt();
        int gamesPlayed = m.GetInt();
        int gamesWon = m.GetInt();

        User user = new User(username, mmr, gamesPlayed, gamesWon);

        StateManager.SetState(new MainMenu(DisplayManager, user));
    }
}