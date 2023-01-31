#nullable enable
using System;
using System.Collections.Generic;
using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.GameStates;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.World.Opponent;
using DiceGuardiansClient.Source.World.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.World.GameGui; 

public class GameGuiManager {
    private readonly GameInstance _game;
    private Dictionary<string, Texture2D> _textures = null!;

    private SpriteFont _largeArial = null!;
    private SpriteFont _arial = null!;
    private SpriteFont _mediumArial = null!;
    private readonly DisplayManager _displayManager;

    private Label _turnLabel = null!;
    private Label _stateLabel = null!;

    private State? _state;

    private Step _step;
    private bool _isMyTurn;

    public GameGuiManager(DisplayManager displayManager, GameInstance game) {
        _displayManager = displayManager;
        _game = game;
        
        LoadElements();
        InitPlayerProfiles();
        ChangeState();
    }

    private void ChangeState() {
        _step = _game.GetStep();
        _isMyTurn = _game.IsMyTurn();
        
        SetUpStateLabels();

        if (_isMyTurn == false) {
            _state = new WaitingForOpponent(_displayManager, _game, _textures, _largeArial, _arial);
            return;
        }

        switch (_step) {
            case Step.STANDBY:
                break;
            case Step.SELECT_DICE:
                _state = new DiceSelectionPhase(_displayManager, _game, _textures, _largeArial, _arial);
                break;
            case Step.MAIN:
                _state = new MainPhase(_displayManager, _game, _textures, _largeArial, _arial, _mediumArial);
                break;
            case Step.END:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void SetUpStateLabels() {
        _turnLabel.SetText(_isMyTurn ? "Your Turn" : "Opponent's Turn");

        string text = _step switch {
            Step.STANDBY => "Standby Phase",
            Step.SELECT_DICE => "Dice Selection Phase",
            Step.MAIN => "Main Phase",
            Step.END => "End Phase",
            _ => throw new ArgumentOutOfRangeException()
        };
        _stateLabel.SetText(text);
    }

    private void LoadElements() {
        _largeArial = _displayManager.GetContent().Load<SpriteFont>("fontLarge");
        _arial = _displayManager.GetContent().Load<SpriteFont>("arial");
        _mediumArial = _displayManager.GetContent().Load<SpriteFont>("fontMedium");

        _textures = new Dictionary<string, Texture2D> {
            ["GameHud"] = _displayManager.GetContent().Load<Texture2D>("GameHud"),
            ["DiceSelection"] = _displayManager.GetContent().Load<Texture2D>("DiceSelection"),
            ["RadioButton"] = _displayManager.GetContent().Load<Texture2D>("RadioButton"),
            ["DiceRack"] = _displayManager.GetContent().Load<Texture2D>("DiceRack"),
            ["DiceIcons"] = _displayManager.GetContent().Load<Texture2D>("DiceIcons"),
            ["Button"] = _displayManager.GetContent().Load<Texture2D>("ButtonInGame"),
            ["Waiting"] = _displayManager.GetContent().Load<Texture2D>("Waiting"),
            ["Choosing"] = _displayManager.GetContent().Load<Texture2D>("Summoning"),
            ["CardNameSlot"] = _displayManager.GetContent().Load<Texture2D>("CardNameSlot")
        };
    }

    public void Update(GameTime gameTime) {
        if (_game.GetStep() != _step || _game.IsMyTurn() != _isMyTurn) {
            ChangeState();
        }
        
        UpdatePlayerProfiles();
        _state!.Update(gameTime);
    }

    public List<GuiElement> GetGuiElements() {
        List<GuiElement> output = new List<GuiElement>();
        output.AddRange(DrawPlayerProfiles());

        output.AddRange(_state!.GetGuiElements());

        output.Add(_stateLabel);
        output.Add(_turnLabel);
        
        return output;
    }
    
    public List<Entity> GetEntities() {
        return new List<Entity>(_state?.Get3dSprites()!);
    }
    
    #region PlayerProfiles
    
    private Image _leftGameHud = null!;
    private Label[] _playerCrests = null!;
    private Label[] _opponentCrests = null!;
    
    private Label _playerName = null!;
    private Label _opponentName = null!;

    private Label _playerMmr = null!;
    private Label _playerDeckSize = null!;

    private Label _opponentMmr = null!;
    private Label _opponentDeckSize = null!;

    private void InitPlayerProfiles() {
        _leftGameHud = new Image(_textures["GameHud"], new Vector2(0, 0), new Vector2(1280, 720));
        
        _playerName = new Label(_arial, new Vector2(223, 494), _displayManager.GetGraphicsDevice());
        _opponentName = new Label(_arial, new Vector2(223, 162), _displayManager.GetGraphicsDevice());

        _opponentMmr = new Label(_arial, new Vector2(180, 208), _displayManager.GetGraphicsDevice());
        _opponentDeckSize = new Label(_arial, new Vector2(256, 208), _displayManager.GetGraphicsDevice());
        
        _playerMmr = new Label(_arial, new Vector2(180, 542), _displayManager.GetGraphicsDevice());
        _playerDeckSize = new Label(_arial, new Vector2(256, 542), _displayManager.GetGraphicsDevice());

        _turnLabel = new Label(_arial, new Vector2(1118, 10), _displayManager.GetGraphicsDevice());
        _stateLabel = new Label(_arial, new Vector2(1118, 30), _displayManager.GetGraphicsDevice());
        
        _turnLabel.SetCentered(true);
        _stateLabel.SetCentered(true);
        
        _playerCrests = new Label[6];
        _opponentCrests = new Label[6];

        for (int i = 0; i < 6; i++) {
            _playerCrests[i] = new Label(_largeArial, new Vector2(34 + i*52, 666), _displayManager.GetGraphicsDevice());
            _opponentCrests[i] = new Label(_largeArial, new Vector2(34 + i*52, 63), _displayManager.GetGraphicsDevice());
            
            _playerCrests[i].SetText("0");
            _playerCrests[i].SetCentered(true);
            _playerCrests[i].SetColor(Color.Black);
            
            _opponentCrests[i].SetText("0");
            _opponentCrests[i].SetCentered(true);
            _opponentCrests[i].SetColor(Color.Black);
        }
        
        _playerName.SetCentered(true);
        _playerName.SetText(_game.GetPlayer().GetUser().GetUserName());
        
        _opponentName.SetCentered(true);
        _opponentName.SetText(_game.GetOpponent().GetUser().GetUserName());
        
        _playerMmr.SetText(_game.GetPlayer().GetUser().GetMmr() + "");
        _playerDeckSize.SetText(_game.GetPlayer().GetDeckManager().GetDeckSize() + "");
        
        _opponentMmr.SetText(_game.GetOpponent().GetUser().GetMmr() + "");
        _opponentDeckSize.SetText(_game.GetOpponent().GetDeckSize() + "");
    }
    
    private void UpdatePlayerProfiles() {
        HumanPlayer player = _game.GetPlayer();
        HumanOpponent opponent = _game.GetOpponent();
        
        CrestPool playerPool = player.GetCrestPool();
        CrestPool opponentPool = opponent.GetCrestPool();

        _playerCrests[0].SetText(playerPool.GetCrests(Crest.TRAP) + "");
        _playerCrests[1].SetText(playerPool.GetCrests(Crest.MAGIC) + "");
        _playerCrests[2].SetText(playerPool.GetCrests(Crest.DEFENSE) + "");
        _playerCrests[3].SetText(playerPool.GetCrests(Crest.ATTACK) + "");
        _playerCrests[4].SetText(playerPool.GetCrests(Crest.MOVEMENT) + "");
        _playerCrests[5].SetText(playerPool.GetCrests(Crest.SUMMON) + "");
        
        _opponentCrests[0].SetText(opponentPool.GetCrests(Crest.TRAP) + "");
        _opponentCrests[1].SetText(opponentPool.GetCrests(Crest.MAGIC) + "");
        _opponentCrests[2].SetText(opponentPool.GetCrests(Crest.DEFENSE) + "");
        _opponentCrests[3].SetText(opponentPool.GetCrests(Crest.ATTACK) + "");
        _opponentCrests[4].SetText(opponentPool.GetCrests(Crest.MOVEMENT) + "");
        _opponentCrests[5].SetText(opponentPool.GetCrests(Crest.SUMMON) + "");

        _playerDeckSize.SetText(player.GetDeckManager().GetDeckSize() + "");
        _opponentDeckSize.SetText(opponent.GetDeckSize() + "");
    }

    private List<GuiElement> DrawPlayerProfiles() {
        List<GuiElement> output = new List<GuiElement> {
            _leftGameHud,
            _playerName, 
            _opponentName,
            _playerMmr,
            _opponentMmr,
            _playerDeckSize,
            _opponentDeckSize,
        };
        output.AddRange(_playerCrests);
        output.AddRange(_opponentCrests);

        return output;
    }

    #endregion
}