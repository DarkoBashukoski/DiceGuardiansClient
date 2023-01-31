#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Collisions;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.GameStates;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.UserInput;
using DiceGuardiansClient.Source.World.Board;
using DiceGuardiansClient.Source.World.GameGui;
using DiceGuardiansClient.Source.World.Minions;
using DiceGuardiansClient.Source.World.Opponent;
using DiceGuardiansClient.Source.World.Player;
using DiceGuardiansClient.Source.World.Summoning;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Riptide;

namespace DiceGuardiansClient.Source.World;

public class GameInstance : State {
    private readonly GameBoard _board;

    private readonly HumanPlayer _player;
    private readonly HumanOpponent _opponent;

    private bool _myTurn;
    private Step _step;
    
    private Minion? _hoveringMinion;
    private Minion? _selectedMinion;
    
    private readonly GameGuiManager _guiManager;

    public GameInstance(DisplayManager displayManager, bool playerGoesFirst, User user, User opponent, Dictionary<long, int> deck) : base(displayManager) {
        _myTurn = playerGoesFirst;

        Model boardModel = displayManager.GetContent().Load<Model>("Board");
        _board = new GameBoard(displayManager, boardModel, playerGoesFirst, this);

        _player = new HumanPlayer(user, deck);
        _opponent = new HumanOpponent(opponent);
        _guiManager = new GameGuiManager(displayManager, this);
    }

    public override List<GuiElement> GetGuiElements() {
        return _guiManager.GetGuiElements();
    }

    public override List<Entity> Get3dSprites() {
        List<Entity> output = new List<Entity> { _board };
        output.AddRange(_board.GetEntities());
        output.AddRange(_guiManager.GetEntities());
        return output;
    }

    public override void Update(GameTime gameTime) {
        List<Minion> toUpdate = new List<Minion>();
        toUpdate.AddRange(_board.GetPlayerMinions());
        toUpdate.AddRange(_board.GetOpponentMinions());
        
        _guiManager.Update(gameTime);
        foreach (Minion m in toUpdate) {
            m.Update(gameTime);
        }
        CheckCollisions(toUpdate);
    }

    private void CheckCollisions(List<Minion> toCheck) {
        
        foreach (var m in toCheck.Where(m => BoardCollisions.RayVsAABB(DisplayManager.GetCamera(), MousePicker.GetRay(), m.GetAABB()))) {
            _hoveringMinion = m;
            if (MouseInput.CheckSingleClick()) {
                _selectedMinion = _hoveringMinion;
            }
            return;
        }

        _hoveringMinion = null;
        if (MouseInput.CheckSingleClick()) {
            _selectedMinion = null;
        }
    }

    #region Getters and Setters

    public GameBoard GetBoard() {
        return _board;
    }

    public HumanPlayer GetPlayer() {
        return _player;
    }

    public HumanOpponent GetOpponent() {
        return _opponent;
    }

    public bool IsMyTurn() {
        return _myTurn;
    }

    public Step GetStep() {
        return _step;
    }

    public Minion? GetHoveringMinion() {
        return _hoveringMinion;
    }

    public Minion? GetSelectedMinion() {
        return _selectedMinion;
    }

    #endregion

    #region StateMachine

    public void TriggerBeginStandby(Message m) {
        _myTurn = m.GetBool();
        _player.ResetForTurn();
        _step = Step.STANDBY;
        
        Console.WriteLine("Client: Standby");
    }
    
    public void TriggerBeginDiceSelect(Message m) {
        _step = Step.SELECT_DICE;
        
        Console.WriteLine("Client: Dice Select");
        
    }
    
    public void TriggerBeginMain(Message m) {
        _step = Step.MAIN;
    }
    
    public void TriggerBeginEnd(Message m) {
        Console.WriteLine("Client: End");
        _step = Step.END;
    }

    #endregion

    #region ServerResponses

    public void TriggerDiceRollResult(Message m) {
        bool forMe = m.GetBool();
        
        for (int i = 0; i < 3; i++) {
            string outcome = m.GetString();
            if (forMe) {
                _player.GetCrestPool().AddCrests(outcome);
            } else {
                _opponent.GetCrestPool().AddCrests(outcome);
            }
        }
    }
    
    public void TriggerPlaceTile(Message m) {
        int piece = m.GetInt();
        int rotation = m.GetInt();
        
        Piece p = AllPieces.GetPiece(piece, rotation);
        Vector2 mapPosition = new Vector2(m.GetInt(), m.GetInt());
        long cardId = m.GetLong();
        
        _board.PlaceTile(p, mapPosition, cardId);
        if (IsMyTurn()) {
            _player.GetCrestPool().SpendCrest(Crest.SUMMON, AllCards.GetCardData(cardId).GetCost());
            _player.GetDeckManager().RemoveCard(cardId);
        } else {
            _opponent.GetCrestPool().SpendCrest(Crest.SUMMON, AllCards.GetCardData(cardId).GetCost());
            _opponent.SetDeckSize(_opponent.GetDeckSize() - 1);
        }
        _player.SetHasSummoned(true);
    }
    
    public void TriggerMoveMinion(Message m) {
        Vector2 start = new Vector2(m.GetInt(), m.GetInt());
        Vector2 end = new Vector2(m.GetInt(), m.GetInt());
        int cost = m.GetInt();

        _board.MoveMinion(start, end);
        if (_myTurn) {
            _player.GetCrestPool().SpendCrest(Crest.MOVEMENT, cost);
        } else {
            _opponent.GetCrestPool().SpendCrest(Crest.MOVEMENT, cost);
        }
    }

    #endregion
}