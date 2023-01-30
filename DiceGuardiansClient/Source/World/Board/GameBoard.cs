#nullable enable
using System.Collections.Generic;
using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Collisions;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.World.Minions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.World.Board;

public class GameBoard : Entity {
    private const int WIDTH = 13;
    private const int HEIGHT = 19;

    private readonly Model _playerTile;
    private readonly Model _opponentTile;

    private readonly Model _playerMinionModel;
    private readonly Model _opponentMinionModel;

    private readonly List<Minion> _playerMinions;
    private readonly List<Minion> _opponentMinions;

    private readonly Tile[,] _board;

    public GameBoard(DisplayManager displayManager, Model model) : base(model, new Vector3(6, 0, 9),
        new Vector3(-90, 0, 90), 1) {
        _playerTile = displayManager.GetContent().Load<Model>("BlueTile");
        _opponentTile = displayManager.GetContent().Load<Model>("RedTile");
        _playerMinionModel = displayManager.GetContent().Load<Model>("PlayerMinion");
        _opponentMinionModel = displayManager.GetContent().Load<Model>("PlayerMinion"); //TODO change to opponent minion when it works

        _board = new Tile[WIDTH, HEIGHT];
        for (int i = 0; i < WIDTH; i++) {
            for (int j = 0; j < HEIGHT; j++) {
                _board[i, j] = new Tile();
            }
        }

        _board[6, 0].SetTerrain(Terrain.PLAYER_TILE);
        _board[6, 0].SetMinion(new Minion(AllCards.GetDiceGuardian(), _playerMinionModel, new Vector3(6, 1, 0), new Vector3(-90, 0, 0), 1));
        
        _board[6, 18].SetTerrain(Terrain.OPPONENT_TILE);
        _board[6, 18].SetMinion(new Minion(AllCards.GetDiceGuardian(), _playerMinionModel, new Vector3(6, 1, 18), new Vector3(-90, 0, 0), 1));

        _playerMinions = new List<Minion> { _board[6, 0].GetMinion()! };
        _opponentMinions = new List<Minion> { _board[6, 18].GetMinion()! };
    }

    public List<Entity> GetEntities() {
        List<Entity> output = new List<Entity>();
        for (int i = 0; i < WIDTH; i++) {
            for (int j = 0; j < HEIGHT; j++) {
                Tile t = _board[i, j];

                if (t.GetTerrain() == Terrain.EMPTY) {
                    continue;
                }

                if (t.GetMinion() != null) {
                    output.Add(t.GetMinion()!);
                }

                if (t.GetTerrain() == Terrain.PLAYER_TILE) {
                    output.Add(new Entity(
                        _playerTile,
                        new Vector3(i, 0, j),
                        new Vector3(-90, 0, 0),
                        1)
                    );
                }
                else {
                    output.Add(new Entity(
                        _opponentTile,
                        new Vector3(i, 0, j),
                        new Vector3(-90, 0, 0),
                        1)
                    );
                }
            }
        }
        return output;
    }

    public List<Minion> GetPlayerMinions() {
        return _playerMinions;
    }

    public List<Minion> GetOpponentMinions() {
        return _opponentMinions;
    }
}