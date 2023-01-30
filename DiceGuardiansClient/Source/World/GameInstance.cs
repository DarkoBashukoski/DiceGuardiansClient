#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using DiceGuardiansClient.Source.Collisions;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.GameStates;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.World.Board;
using DiceGuardiansClient.Source.World.Minions;
using DiceGuardiansClient.Source.World.Opponent;
using DiceGuardiansClient.Source.World.Player;
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
    
    private readonly GameGuiManager _guiManager;

    public GameInstance(DisplayManager displayManager, bool playerGoesFirst, User user, User opponent, Dictionary<long, int> deck) : base(displayManager) {
        _myTurn = playerGoesFirst;

        Model boardModel = displayManager.GetContent().Load<Model>("Board");
        _board = new GameBoard(displayManager, boardModel);

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
        return output;
    }

    public override void Update(GameTime gameTime) {
        _guiManager.Update(gameTime);
        CheckCollisions();
    }

    private void CheckCollisions() {
        List<Minion> toCheck = new List<Minion>();
        toCheck.AddRange(_board.GetPlayerMinions());
        toCheck.AddRange(_board.GetOpponentMinions());
        
        foreach (var m in toCheck.Where(m => BoardCollisions.RayVsAABB(DisplayManager.GetCamera(), MousePicker.GetRay(), m.GetAABB()))) {
            _hoveringMinion = m;
            return;
        }

        _hoveringMinion = null;
    }

    #region Getters and Setters

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

    #endregion

    #region StateMachine

    public void TriggerBeginStandby(Message m) {
        _myTurn = m.GetBool();
        _step = Step.STANDBY;
        
        Console.WriteLine("Client: Standby");
    }
    
    public void TriggerBeginDiceSelect(Message m) {
        if (_myTurn) {
            _guiManager.InitDiceSelection();
        }
        else {
            _guiManager.InitWaitingForOpponent();
        }
        Console.WriteLine("Client: Dice Select");
        _step = Step.SELECT_DICE;
    }
    
    public void TriggerBeginMain(Message m) {
        Console.WriteLine("Client: Main");
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

    #endregion
}