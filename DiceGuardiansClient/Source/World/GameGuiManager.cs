#nullable enable
using System;
using System.Collections.Generic;
using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.Networking;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.World.Minions;
using DiceGuardiansClient.Source.World.Opponent;
using DiceGuardiansClient.Source.World.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Riptide;

namespace DiceGuardiansClient.Source.World; 

public class GameGuiManager {
    private readonly GameInstance _game;
    private Dictionary<string, Texture2D> _textures = null!;

    private SpriteFont _largeArial = null!;
    private SpriteFont _arial = null!;
    private readonly DisplayManager _displayManager;

    private Label _turnLabel = null!;
    private Label _stateLabel = null!;

    public GameGuiManager(DisplayManager displayManager, GameInstance game) {
        _displayManager = displayManager;
        _game = game;
        LoadElements();
        
        InitPlayerProfiles();
    }

    private void LoadElements() {
        _largeArial = _displayManager.GetContent().Load<SpriteFont>("fontLarge");
        _arial = _displayManager.GetContent().Load<SpriteFont>("arial");

        _textures = new Dictionary<string, Texture2D> {
            ["GameHud"] = _displayManager.GetContent().Load<Texture2D>("GameHud"),
            ["DiceSelection"] = _displayManager.GetContent().Load<Texture2D>("DiceSelection"),
            ["RadioButton"] = _displayManager.GetContent().Load<Texture2D>("RadioButton"),
            ["DiceRack"] = _displayManager.GetContent().Load<Texture2D>("DiceRack"),
            ["DiceIcons"] = _displayManager.GetContent().Load<Texture2D>("DiceIcons"),
            ["Button"] = _displayManager.GetContent().Load<Texture2D>("ButtonInGame"),
            ["Waiting"] = _displayManager.GetContent().Load<Texture2D>("Waiting")
        };
    }

    public void Update(GameTime gameTime) {
        UpdatePlayerProfiles();
        if (_game.IsMyTurn()) {
            switch (_game.GetStep()) {
                case Step.SELECT_DICE:
                    UpdateDiceSelection(gameTime);
                    break;
            }
        } else {
            UpdateWaitingForOpponent(gameTime);
        }
    }

    public List<GuiElement> GetGuiElements() {
        List<GuiElement> output = new List<GuiElement>();
        output.AddRange(DrawPlayerProfiles());

        if (_game.IsMyTurn()) {
            switch (_game.GetStep()) {
                case Step.SELECT_DICE:
                    output.AddRange(DrawDiceSelection());
                    break;
            }
        }
        else {
            output.AddRange(DrawWaitingForOpponent());
        }

        output.Add(_stateLabel);
        output.Add(_turnLabel);
        
        return output;
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
        _opponentDeckSize.SetText(_game.GetOpponent().getDeckSize() + "");
    }
    
    private void UpdatePlayerProfiles() {
        HumanPlayer player = _game.GetPlayer();
        HumanOpponent opponent = _game.GetOpponent();
        
        CrestPool playerPool = player.GetCrestPool();
        CrestPool opponentPool = opponent.GetCrestPool();

        _playerCrests[0].SetText(playerPool.getCrests(Crest.TRAP) + "");
        _playerCrests[1].SetText(playerPool.getCrests(Crest.MAGIC) + "");
        _playerCrests[2].SetText(playerPool.getCrests(Crest.DEFENSE) + "");
        _playerCrests[3].SetText(playerPool.getCrests(Crest.ATTACK) + "");
        _playerCrests[4].SetText(playerPool.getCrests(Crest.MOVEMENT) + "");
        _playerCrests[5].SetText(playerPool.getCrests(Crest.SUMMON) + "");
        
        _opponentCrests[0].SetText(opponentPool.getCrests(Crest.TRAP) + "");
        _opponentCrests[1].SetText(opponentPool.getCrests(Crest.MAGIC) + "");
        _opponentCrests[2].SetText(opponentPool.getCrests(Crest.DEFENSE) + "");
        _opponentCrests[3].SetText(opponentPool.getCrests(Crest.ATTACK) + "");
        _opponentCrests[4].SetText(opponentPool.getCrests(Crest.MOVEMENT) + "");
        _opponentCrests[5].SetText(opponentPool.getCrests(Crest.SUMMON) + "");

        _playerDeckSize.SetText(player.GetDeckManager().GetDeckSize() + "");
        _opponentDeckSize.SetText(opponent.getDeckSize() + "");
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

    #region DiceSelection //TODO add crest amount labels
    
    private Image _rightDiceSelection = null!;
    
    private RadioButton[] _cardSelectionButtons = null!;
    private Image[] _cardSelectionRacks = null!;
    private Image[] _cardCrests = null!;

    private Button _confirmSelection = null!;

    public void InitDiceSelection() {
        _confirmSelection = new Button(_textures["Button"], _arial, new Vector2(1025, 672), new Vector2(186, 32));
        _rightDiceSelection = new Image(_textures["DiceSelection"], new Vector2(0, 0), new Vector2(1280, 720));

        _turnLabel.SetText("Your Turn");
        _stateLabel.SetText("Select 3 Dice");
        
        _confirmSelection.SetText("Confirm Selection");
        
        SetUpDiceSelection();
    }
    
    private void SetUpDiceSelection() {
        HumanPlayer player = _game.GetPlayer();
        int deckSize = player.GetDeckManager().GetDeckSize();

        _cardSelectionButtons = new RadioButton[deckSize];
        for (int i = 0; i < deckSize; i++) {
            _cardSelectionButtons[i] = new RadioButton(
                _textures["RadioButton"],
                new Vector2(974, 62 + i*40),
                new Vector2(36, 36)
            );
        }

        _cardSelectionRacks = new Image[deckSize];
        for (int i = 0; i < deckSize; i++) {
            _cardSelectionRacks[i] = new Image(
                _textures["DiceRack"],
                new Vector2(1032, 62 + i*40),
                new Vector2(216, 36)
            );
        }

        _cardCrests = new Image[deckSize * 6];
        List<Card> deck = player.GetDeckManager().GetDeck();
        for (int i = 0; i < deckSize; i++) {
            DiceFace[] d = deck[i].GetDiceFaces();
            for (int j = 0; j < 6; j++) {
                _cardCrests[6 * i + j] = new Image(
                    _textures["DiceIcons"],
                    new Vector2(1036 + j*36, 66 + i*40),
                    new Vector2(28, 28)
                );
                _cardCrests[6 * i + j].SetSource(new Rectangle(
                    _textures["DiceIcons"].Width / 6 * (int) d[j].GetCrest(),
                    0,
                    _textures["DiceIcons"].Width / 6,
                    _textures["DiceIcons"].Height
                ));
            }
        }

        _confirmSelection.Click += (_, _) => {
            Message m = Message.Create(MessageSendMode.Reliable, ClientToServerId.SelectDiceRoll);
            for (int i = 0; i < deckSize; i++) {
                if (_cardSelectionButtons[i].IsSelected()) {
                    m.AddLong(_game.GetPlayer().GetDeckManager().GetCardAtIndex(i).GetCardId());
                    _cardSelectionButtons[i].SetSelected(false);
                }
            }
            NetworkManager.GetClient().Send(m);
            _confirmSelection.SetDisabled(true);
        };
    }
    
    private void UpdateDiceSelection(GameTime gameTime) {
        int selectedCount = 0;
        foreach (RadioButton radioButton in _cardSelectionButtons) {
            radioButton.Update();
            if (radioButton.IsSelected()) {
                selectedCount += 1;
            }
        }
        
        _confirmSelection.SetDisabled(selectedCount != 3);
        _confirmSelection.Update(gameTime);
    }

    private List<GuiElement> DrawDiceSelection() {
        List<GuiElement> output = new List<GuiElement> {
            _rightDiceSelection,
            _confirmSelection
        };
        
        output.AddRange(_cardSelectionButtons);
        output.AddRange(_cardSelectionRacks);
        output.AddRange(_cardCrests);

        return output;
    }
    
    #endregion

    #region WaitingForOpponnet

    private Image _rightWaiting = null!;
    private Image _cardImage = null!;

    private Label _health = null!;
    private Label _attack = null!;
    private Label _defense = null!;

    public void InitWaitingForOpponent() {
        _rightWaiting = new Image(_textures["Waiting"], new Vector2(0, 0), new Vector2(1280, 720));
        _cardImage = new Image(_textures["Waiting"], new Vector2(1014, 62), new Vector2(206, 289));
        _cardImage.enableDraw(false);
            
        _health = new Label(_largeArial, new Vector2(1050, 482), _displayManager.GetGraphicsDevice());
        _attack = new Label(_largeArial, new Vector2(1119, 482), _displayManager.GetGraphicsDevice());
        _defense = new Label(_largeArial, new Vector2(1188, 482), _displayManager.GetGraphicsDevice());
        
        _health.SetText("");
        _attack.SetText("");
        _defense.SetText("");
        
        _health.SetCentered(true);
        _attack.SetCentered(true);
        _defense.SetCentered(true);
        
        _health.SetColor(Color.Black);
        _attack.SetColor(Color.Black);
        _defense.SetColor(Color.Black);
        
        SetUpStateLabels();
    }

    private void SetUpStateLabels() {
        _turnLabel.SetText("Opponent's Turn");
        string text = _game.GetStep() switch {
            Step.STANDBY => "Standby Phase",
            Step.SELECT_DICE => "Dice Selection Phase",
            Step.MAIN => "Main Phase",
            Step.END => "End Phase",
            _ => throw new ArgumentOutOfRangeException()
        };
        _stateLabel.SetText(text);
    }
    
    private void UpdateWaitingForOpponent(GameTime gameTime) {
        Minion? m = _game.GetHoveringMinion();
        if (m == null) {
            _attack.SetText("");
            _defense.SetText("");
            _health.SetText("");
            _cardImage.enableDraw(false);
            return;
        }
        _attack.SetText(m.GetAttack() + "");
        _defense.SetText(m.GetDefense() + "");
        _health.SetText(m.GetHealth() + "");
        _cardImage.SetTexture(m.GetCard().getTexture());
        _cardImage.enableDraw(true);
    }

    private List<GuiElement> DrawWaitingForOpponent() {
        return new List<GuiElement> {
            _rightWaiting,
            _cardImage,
            _health,
            _attack,
            _defense
        };
    }

    #endregion
}