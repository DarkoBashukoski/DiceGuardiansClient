#nullable enable
using System;
using System.Collections.Generic;
using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.Pathfinding;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.World.Minions;
using DiceGuardiansClient.Source.World.Summoning;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.World.Board;

public class GameBoard : Entity {
    private const int WIDTH = 13;
    private const int HEIGHT = 19;

    private readonly GameInstance _game;

    private readonly Model _playerTile;
    private readonly Model _opponentTile;

    private readonly Model _playerMinionModel;
    private readonly Model _opponentMinionModel;

    private readonly List<Minion> _playerMinions;
    private readonly List<Minion> _opponentMinions;

    private readonly Tile[,] _board;

    public GameBoard(DisplayManager displayManager, Model model, bool playerGoesFirst, GameInstance game) : base(model, new Vector3(6, 0, 9),
        new Vector3(-90, 0, 90), 1) {
        _playerTile = displayManager.GetContent().Load<Model>("BlueTile");
        _opponentTile = displayManager.GetContent().Load<Model>("RedTile");
        _playerMinionModel = displayManager.GetContent().Load<Model>("PlayerMinion");
        _opponentMinionModel = displayManager.GetContent().Load<Model>("PlayerMinion"); //TODO change to opponent minion when it works
        _game = game;
        
        _board = new Tile[WIDTH, HEIGHT];
        for (int i = 0; i < WIDTH; i++) {
            for (int j = 0; j < HEIGHT; j++) {
                _board[i, j] = new Tile();
            }
        }

        if (playerGoesFirst) {
            _board[6, 0].SetTerrain(Terrain.PLAYER_TILE);
            _board[6, 0].SetMinion(new Minion(AllCards.GetDiceGuardian(), _playerMinionModel, new Vector3(6, 1, 0), new Vector3(-90, 0, 0), 1));
        
            _board[6, 18].SetTerrain(Terrain.OPPONENT_TILE);
            _board[6, 18].SetMinion(new Minion(AllCards.GetDiceGuardian(), _playerMinionModel, new Vector3(6, 1, 18), new Vector3(-90, 0, 0), 1));

            _playerMinions = new List<Minion> { _board[6, 0].GetMinion()! };
            _opponentMinions = new List<Minion> { _board[6, 18].GetMinion()! };
        } else {
            _board[6, 0].SetTerrain(Terrain.OPPONENT_TILE);
            _board[6, 0].SetMinion(new Minion(AllCards.GetDiceGuardian(), _opponentMinionModel, new Vector3(6, 1, 0), new Vector3(-90, 0, 0), 1));
        
            _board[6, 18].SetTerrain(Terrain.PLAYER_TILE);
            _board[6, 18].SetMinion(new Minion(AllCards.GetDiceGuardian(), _playerMinionModel, new Vector3(6, 1, 18), new Vector3(-90, 0, 0), 1));

            _playerMinions = new List<Minion> { _board[6, 18].GetMinion()! };
            _opponentMinions = new List<Minion> { _board[6, 0].GetMinion()! };
        }
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
    
    public bool IsPlacementPossible(int[,] pathPiece, Vector2 pieceCenter, Vector2 mapPos) {
        Vector2 mapLimits = new Vector2(WIDTH - 1, HEIGHT - 1);
        Vector2 pieceLimits = new Vector2(pathPiece.GetLength(0) - 1, pathPiece.GetLength(1) - 1);
        Vector2 topLeft = Vector2.Subtract(mapPos, pieceCenter);
        Vector2 bottomRight = Vector2.Add(topLeft, pieceLimits);

        bool connected = false;
        if (topLeft.X < 0 || topLeft.Y < 0 || bottomRight.X > mapLimits.X || bottomRight.Y > mapLimits.Y) {return false;}

        for (int i = 0; i < pathPiece.GetLength(0); i++) {
            for (int j = 0; j < pathPiece.GetLength(1); j++) {
                if (pathPiece[i, j] == 0) {continue;}
                Vector2 mapCoords = Vector2.Add(topLeft, new Vector2(i, j));
                if (_board[(int) mapCoords.X, (int) mapCoords.Y].GetTerrain() != Terrain.EMPTY) {return false;}
                
                if (connected) {continue;}
                try {
                    if (_board[(int)mapCoords.X + 1, (int)mapCoords.Y].GetTerrain() == Terrain.PLAYER_TILE) {connected = true; continue;}
                    if (_board[(int)mapCoords.X - 1, (int)mapCoords.Y].GetTerrain() == Terrain.PLAYER_TILE) {connected = true; continue;}
                    if (_board[(int)mapCoords.X, (int)mapCoords.Y + 1].GetTerrain() == Terrain.PLAYER_TILE) {connected = true; continue;}
                    if (_board[(int)mapCoords.X, (int)mapCoords.Y - 1].GetTerrain() == Terrain.PLAYER_TILE) {connected = true;}
                }
                catch (IndexOutOfRangeException) {}
            }
        }
        return connected;
    }

    public List<Minion> GetPlayerMinions() {
        return _playerMinions;
    }

    public List<Minion> GetOpponentMinions() {
        return _opponentMinions;
    }

    public void PlaceTile(Piece piece, Vector2 mapPos, long cardId) {
        Vector2[] relativeCoords = piece.GetRelativeCoords(mapPos);

        foreach (Vector2 coords in relativeCoords) {
            _board[(int) coords.X, (int) coords.Y].SetTerrain(_game.IsMyTurn() ? Terrain.PLAYER_TILE : Terrain.OPPONENT_TILE);
        }

        Minion m = new Minion(AllCards.GetCardData(cardId), _playerMinionModel, new Vector3(mapPos.X, 1, mapPos.Y), new Vector3(-90, 0, 0), 1);
        _board[(int) mapPos.X, (int) mapPos.Y].SetMinion(m);
        
        if (_game.IsMyTurn()) {
            _playerMinions.Add(_board[(int) mapPos.X, (int) mapPos.Y].GetMinion()!); 
        }
        else {
            _opponentMinions.Add(_board[(int) mapPos.X, (int) mapPos.Y].GetMinion()!); 
        }
    }

    public int[,] GetAllReachableTiles(int maxDepth, Vector2 start) {
        int depth = 0;
        int elementsUntilNextDepth = 1;
        
        int[,] depths = new int[WIDTH, HEIGHT];
        for (int i = 0; i < WIDTH; i++) {
            for (int j = 0; j < HEIGHT; j++) {
                depths[i, j] = -1;
            }
        }
        bool[,] visited = new bool[WIDTH, HEIGHT];

        depths[(int)start.X, (int)start.Y] = 0;
        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(start);
        
        if (maxDepth < 0) {
            return depths;
        }

        while (queue.Count > 0) {
            Vector2 currentNode = queue.Dequeue();
            depths[(int) currentNode.X, (int) currentNode.Y] = depth;

            Vector2 v = new Vector2(currentNode.X + 1, currentNode.Y);
            if (IsInBounds(v) && IsTraversable(v) && !visited[(int) v.X, (int) v.Y]) {
                queue.Enqueue(v);
                visited[(int) v.X, (int) v.Y] = true;
            }
            
            v = new Vector2(currentNode.X - 1, currentNode.Y);
            if (IsInBounds(v) && IsTraversable(v) && !visited[(int) v.X, (int) v.Y]) {
                queue.Enqueue(v);
                visited[(int) v.X, (int) v.Y] = true;
            }

            v = new Vector2(currentNode.X, currentNode.Y + 1);
            if (IsInBounds(v) && IsTraversable(v) && !visited[(int) v.X, (int) v.Y]) {
                queue.Enqueue(v);
                visited[(int) v.X, (int) v.Y] = true;
            }

            v = new Vector2(currentNode.X, currentNode.Y - 1);
            if (IsInBounds(v) && IsTraversable(v) && !visited[(int) v.X, (int) v.Y]) {
                queue.Enqueue(v);
                visited[(int) v.X, (int) v.Y] = true;
            }
            
            elementsUntilNextDepth -= 1;
            if (elementsUntilNextDepth == 0) {
                depth += 1;
                elementsUntilNextDepth = queue.Count;
            }

            if (depth > maxDepth) {
                return depths;
            }
        }

        return depths;
    }

    private bool IsTraversable(Vector2 nodeCoords) {
        Tile t = _board[(int)nodeCoords.X, (int)nodeCoords.Y];
        return t.GetTerrain() != Terrain.EMPTY && t.GetMinion() == null;
    }

    private bool IsInBounds(Vector2 v) {
        try {
            Tile t = _board[(int)v.X, (int)v.Y];
            return true;
        }
        catch (IndexOutOfRangeException) {
            return false;
        }
    }

    public void MoveMinion(Vector2 start, Vector2 end) {
        Minion? m = _board[(int)start.X, (int)start.Y].GetMinion();
        if (m == null) {
            return;
        }
        
        int[,] aStarGrid = new int[WIDTH, HEIGHT];
        for (int i = 0; i < WIDTH; i++) {
            for (int j = 0; j < HEIGHT; j++) {
                aStarGrid[i, j] = IsTraversable(new Vector2(i, j)) ? 0 : 1;
            }
        }
        
        _board[(int)end.X, (int)end.Y].SetMinion(m);
        _board[(int) start.X, (int) start.Y].SetMinion(null);

        List<Vector2> path = AStar.FindPath(aStarGrid, start, end);
        m.StartMove(path);
    }
}