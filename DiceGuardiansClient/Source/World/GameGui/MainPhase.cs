#nullable enable
using System;
using System.Collections.Generic;
using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Collisions;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.GameStates;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.Networking;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.UserInput;
using DiceGuardiansClient.Source.World.Minions;
using DiceGuardiansClient.Source.World.Player;
using DiceGuardiansClient.Source.World.Summoning;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Riptide;

namespace DiceGuardiansClient.Source.World.GameGui;

public enum MainState {
    IDLE, CHOOSING_MONSTER, SUMMONING
}

public class MainPhase : State {
    private MainState _state;
    
    private readonly GameInstance _game;
    private readonly SpriteFont _largeArial;
    private readonly SpriteFont _arial;
    private readonly SpriteFont _mediumArial;

    private readonly Model _playerTile;
    private readonly Model _playerMinion;
    private readonly Dictionary<string, Texture2D> _textures;

    public MainPhase(DisplayManager displayManager, GameInstance game, Dictionary<string, Texture2D> textures, SpriteFont large, SpriteFont normal, SpriteFont medium) : base(displayManager) {
        _textures = textures;
        _largeArial = large;
        _arial = normal;
        _mediumArial = medium;
        _game = game;
        _playerTile = DisplayManager.GetContent().Load<Model>("BlueTile");
        _playerMinion = DisplayManager.GetContent().Load<Model>("PlayerMinion");
        
        ChangeState(MainState.IDLE);
    }

    private void ChangeState(MainState state) {
        _state = state;
        switch (_state) {
            case MainState.IDLE:
                SetUpIdle();
                break;
            case MainState.CHOOSING_MONSTER:
                SetUpChoosing();
                break;
            case MainState.SUMMONING:
                SetUpSummoning();
                break;
        }
    }

    public override List<GuiElement> GetGuiElements() {
        return _state switch {
            MainState.IDLE => DrawIdle(),
            MainState.CHOOSING_MONSTER => DrawChoosing(),
            MainState.SUMMONING => DrawSummoning(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override List<Entity> Get3dSprites() {
        return _state switch {
            MainState.IDLE => new List<Entity>(),
            MainState.CHOOSING_MONSTER => new List<Entity>(),
            MainState.SUMMONING => Draw3dObjectsSummoning(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override void Update(GameTime gameTime) {
        switch (_state) {
            case MainState.IDLE:
                UpdateIdle(gameTime);
                break;
            case MainState.CHOOSING_MONSTER:
                UpdateChoosing(gameTime);
                break;
            case MainState.SUMMONING:
                UpdateSummoning(gameTime);
                break;
        }
    }

    #region Idle

    private Image _rightMain = null!;
    private Button _summon = null!;
    private Button _endTurn = null!;

    private Image _cardImageMain = null!;

    private Label _healthMain = null!;
    private Label _attackMain = null!;
    private Label _defenseMain = null!;

    private void SetUpIdle() {
        _rightMain = new Image(_textures["Waiting"], new Vector2(0, 0), new Vector2(1280, 720));
        _summon = new Button(_textures["Button"], _arial, new Vector2(1025, 620), new Vector2(186, 32));
        _endTurn = new Button(_textures["Button"], _arial, new Vector2(1025, 672), new Vector2(186, 32));
        _cardImageMain = new Image(_textures["Waiting"], new Vector2(1014, 62), new Vector2(206, 289));

        _summon.SetText("Summon Minion");
        _endTurn.SetText("End Turn");

        _healthMain = new Label(_largeArial, new Vector2(1050, 482), DisplayManager.GetGraphicsDevice());
        _attackMain = new Label(_largeArial, new Vector2(1119, 482), DisplayManager.GetGraphicsDevice());
        _defenseMain = new Label(_largeArial, new Vector2(1188, 482), DisplayManager.GetGraphicsDevice());

        _healthMain.SetText("");
        _attackMain.SetText("");
        _defenseMain.SetText("");

        _healthMain.SetCentered(true);
        _attackMain.SetCentered(true);
        _defenseMain.SetCentered(true);

        _healthMain.SetColor(Color.Black);
        _attackMain.SetColor(Color.Black);
        _defenseMain.SetColor(Color.Black);

        _endTurn.Click += (_, _) => {
            Message m = Message.Create(MessageSendMode.Reliable, ClientToServerId.EndTurn);
            NetworkManager.GetClient().Send(m);
        };

        _summon.Click += (_, _) => {
            ChangeState(MainState.CHOOSING_MONSTER);
        };

        if (_game.GetPlayer().HasSummoned()) {
            _summon.SetDisabled(true);
        }
    }

    private List<GuiElement> DrawIdle() {
        return new List<GuiElement> {
            _rightMain,
            _cardImageMain,
            _healthMain,
            _attackMain,
            _defenseMain,
            _summon,
            _endTurn
        };
    }

    private void UpdateIdle(GameTime gameTime) {
        _summon.Update(gameTime);
        _endTurn.Update(gameTime);

        Minion? hoveringMinion = _game.GetHoveringMinion();
        Minion? selectedMinion = _game.GetSelectedMinion();
        
        _attackMain.SetText("");
        _defenseMain.SetText("");
        _healthMain.SetText("");
        _cardImageMain.enableDraw(false);

        if (selectedMinion == null) {
            if (hoveringMinion == null) return;
            _attackMain.SetText(hoveringMinion.GetAttack() + "");
            _defenseMain.SetText(hoveringMinion.GetDefense() + "");
            _healthMain.SetText(hoveringMinion.GetHealth() + "");
            _cardImageMain.SetTexture(hoveringMinion.GetCard().getTexture());
            _cardImageMain.enableDraw(true);
            return;
        }
        
        _attackMain.SetText(selectedMinion.GetAttack() + "");
        _defenseMain.SetText(selectedMinion.GetDefense() + "");
        _healthMain.SetText(selectedMinion.GetHealth() + "");
        _cardImageMain.SetTexture(selectedMinion.GetCard().getTexture());
        _cardImageMain.enableDraw(true);

        if (MouseInput.CheckSingleClick()) {
            int movementCrests = _game.GetPlayer().GetCrestPool().GetCrests(Crest.MOVEMENT);
            Vector2 minionPosition = new Vector2(selectedMinion.Position.X, selectedMinion.Position.Z);
            int[,] reachableTiles = _game.GetBoard().GetAllReachableTiles(movementCrests, minionPosition);

            Vector2 clicked = BoardCollisions.GetTileCoords(DisplayManager.GetCamera(), MousePicker.GetRay());
            
            try {
                if (reachableTiles[(int)clicked.X, (int)clicked.Y] == -1 || clicked.Equals(minionPosition)) {
                    return;
                }
            }
            catch (IndexOutOfRangeException) {
                return;
            }
            
            Message m = Message.Create(MessageSendMode.Reliable, ClientToServerId.MoveMinion);
            m.AddInt((int) minionPosition.X);
            m.AddInt((int) minionPosition.Y);
            m.AddInt((int) clicked.X);
            m.AddInt((int) clicked.Y);
            m.AddInt(reachableTiles[(int) clicked.X, (int) clicked.Y]);
            NetworkManager.GetClient().Send(m);
        }
    }

    #endregion

    #region Choosing Monster

    private Image _rightChoosing = null!;
    private Button _cancelChoosing = null!;

    private Image[] _cardRacks = null!;
    private Label[] _names = null!;
    private Label[] _costLabels = null!;

    private void SetUpChoosing() {
        _rightChoosing = new Image(_textures["Choosing"], new Vector2(0, 0), new Vector2(1280, 720));
        _cancelChoosing = new Button(_textures["Button"], _arial, new Vector2(1025, 672), new Vector2(186, 32));
        
        _cancelChoosing.SetText("Cancel");

        HumanPlayer player = _game.GetPlayer();
        int deckSize = player.GetDeckManager().GetDeckSize();
        int availableSummonCrests = player.GetCrestPool().GetCrests(Crest.SUMMON);
        List<Card> deck = player.GetDeckManager().GetDeck();
        
        _cardRacks = new Image[deckSize];
        for (int i = 0; i < deckSize; i++) {
            _cardRacks[i] = new Image(
                _textures["CardNameSlot"],
                new Vector2(984, 62 + i*40),
                new Vector2(264, 36)
            );
            if (deck[i].GetCost() > availableSummonCrests) {
                _cardRacks[i].SetColor(Color.DimGray);
            } else {
                var i1 = i;
                _cardRacks[i].Click += (_, _) => {
                    _toBeSummoned = deck[i1];
                    ChangeState(MainState.SUMMONING);
                };
            }
        }
        
        _names = new Label[deckSize];
        for (int i = 0; i < deckSize; i++) {
            _names[i] = new Label(
                _arial,
                new Vector2(1026, 70 + i*40),
                DisplayManager.GetGraphicsDevice()
            );
            _names[i].SetText(deck[i].GetName());
            _names[i].SetColor(Color.Black);
            _names[i].SetOutlineColor(Color.Black);
        }

        _costLabels = new Label[deckSize];
        for (int i = 0; i < deckSize; i++) {
            _costLabels[i] = new Label(
                _arial,
                new Vector2(1002, 70 + i*40),
                DisplayManager.GetGraphicsDevice()
            );
            _costLabels[i].SetText(deck[i].GetCost() + "");
            _costLabels[i].SetCentered(true);
            _costLabels[i].SetColor(Color.Black);
            _costLabels[i].SetOutlineColor(Color.Black);
        }

        _cancelChoosing.Click += (_, _) => {
            ChangeState(MainState.IDLE);
        };
    }

    private void UpdateChoosing(GameTime gameTime) {
        _cancelChoosing.Update(gameTime);
        foreach (Image i in _cardRacks) {
            i.Update(gameTime);
        }
    }

    private List<GuiElement> DrawChoosing() {
        List<GuiElement> output = new List<GuiElement>() {
            _rightChoosing,
            _cancelChoosing,
        };
        
        output.AddRange(_cardRacks);
        output.AddRange(_names);
        output.AddRange(_costLabels);

        return output;
    }

    #endregion
    
    #region Summoning

    private Image _rightSummoning;
    private Card _toBeSummoned;

    private Label _health;
    private Label _attack;
    private Label _defense;
    
    private Button _backButton;
    private Image _cardImage;

    private Entity[] _tiles;
    private Entity _minion;

    private void SetUpSummoning() {
        _rightSummoning = new Image(_textures["Waiting"], new Vector2(0, 0), new Vector2(1280, 720));
        _cardImage = new Image(_toBeSummoned.getTexture(), new Vector2(1014, 62), new Vector2(206, 289));
        _backButton = new Button(_textures["Button"], _arial, new Vector2(1025, 672), new Vector2(186, 32));
        
        _backButton.SetText("Back");
        
        _health = new Label(_largeArial, new Vector2(1050, 482), DisplayManager.GetGraphicsDevice());
        _attack = new Label(_largeArial, new Vector2(1119, 482), DisplayManager.GetGraphicsDevice());
        _defense = new Label(_largeArial, new Vector2(1188, 482), DisplayManager.GetGraphicsDevice());
        
        _health.SetText(_toBeSummoned.GetHealth() + "");
        _attack.SetText(_toBeSummoned.GetAttack() + "");
        _defense.SetText(_toBeSummoned.GetDefense() + "");
        
        _health.SetCentered(true);
        _attack.SetCentered(true);
        _defense.SetCentered(true);
        
        _health.SetColor(Color.Black);
        _attack.SetColor(Color.Black);
        _defense.SetColor(Color.Black);

        _minion = new Entity(_playerMinion, new Vector3(0, 0, 0), new Vector3(-90, 0, 0), 1);
        _tiles = new Entity[6];
        for (int i = 0; i < 6; i++) {
            _tiles[i] = new Entity(_playerTile, new Vector3(0, 0, 0), new Vector3(-90, 0, 0), 1);
        }
        AllPieces.Reset();
        _backButton.Click += (_, _) => {
            ChangeState(MainState.CHOOSING_MONSTER);
        };
    }

    private void UpdateSummoning(GameTime gameTime) {
        ParseInputs();

        Vector2 currentPosition = BoardCollisions.GetTileCoords(DisplayManager.GetCamera(), MousePicker.GetRay());
        Vector2[] relativeCoords = AllPieces.GetCurrentPiece().GetRelativeCoords(currentPosition);
        for (int i = 0; i < 6; i++) {
            _tiles[i].Position = new Vector3(relativeCoords[i].X, 0, relativeCoords[i].Y);
        }
        
        _minion.Position = new Vector3(currentPosition.X, 0, currentPosition.Y);
        _backButton.Update(gameTime);
    }

    private void ParseInputs() {
        if (KeyboardParser.CheckSinglePress(Keys.R)) {
            AllPieces.Rotate();
        }
        if (KeyboardParser.CheckSinglePress(Keys.E)) {
            AllPieces.NextPiece();
        }

        if (!MouseInput.CheckSingleClick()) {
            return;
        }

        Piece p = AllPieces.GetCurrentPiece();
        Vector2 mapCoords = BoardCollisions.GetTileCoords(DisplayManager.GetCamera(), MousePicker.GetRay());
        
        if (!_game.GetBoard().IsPlacementPossible(p.GetPiece(), p.GetCenter(), mapCoords)) {
            return;
        }
            
        Message m = Message.Create(MessageSendMode.Reliable, ClientToServerId.PlaceTile);
        m.AddInt(AllPieces.GetPiece());
        m.AddInt(AllPieces.GetRotation());
        m.AddInt((int) mapCoords.X);
        m.AddInt((int) mapCoords.Y);
        m.AddLong(_toBeSummoned.GetCardId());
        NetworkManager.GetClient().Send(m);
        
        _game.GetPlayer().SetHasSummoned(true);
        ChangeState(MainState.IDLE);
    }
    
    private List<GuiElement> DrawSummoning() {
        return new List<GuiElement> {
            _rightSummoning,
            _health,
            _attack,
            _defense,
            _backButton,
            _cardImage
        };
    }

    private List<Entity> Draw3dObjectsSummoning() {
        List<Entity> output = new List<Entity>(_tiles) { _minion };
        return output;
    }

    #endregion
}