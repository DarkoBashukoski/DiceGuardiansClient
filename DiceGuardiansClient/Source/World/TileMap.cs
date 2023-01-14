using System.Collections.Generic;
using DiceGuardiansClient.Source.Collisions;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.RenderEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.World; 

public class TileMap {
    private int[,] _board;
    private readonly List<Entity> _drawables;

    private readonly Model _playerTile;
    private readonly Model _opponentTile;

    public TileMap(DisplayManager displayManager) {
        _drawables = new List<Entity>();
        _playerTile = displayManager.GetContent().Load<Model>("BlueTile");
        _opponentTile = displayManager.GetContent().Load<Model>("RedTile");
        
        _board = new int[13, 19];
        _board[6, 0] = 2;
        _board[6, 18] = 1;
        
        UpdateDrawables();
    }

    private void UpdateDrawables() {
        _drawables.Clear();
        for (int i = 0; i < _board.GetLength(0); i++) {
            for (int j = 0; j < _board.GetLength(1); j++) {
                if (_board[i, j] == 1) {
                    _drawables.Add(new Entity(
                        _playerTile,
                        new Vector3(i, 0, j),
                        new Vector3(-90, 0, 0),
                        1));
                }
                else if (_board[i, j] == 2) {
                    _drawables.Add(new Entity(
                        _opponentTile,
                        new Vector3(i, 0, j),
                        new Vector3(-90, 0, 0),
                        1));
                }
            }
        }
    }

    public List<Entity> GetDrawables() {
        return _drawables;
    }

    public bool PlaceTile(int[,] pathPiece, Vector2 mapPos, Vector2 pieceCenter, int playerIndex) {
        if (!BoardCollisions.IsPlacementPossible(_board, pathPiece, mapPos, pieceCenter, playerIndex)) {
            return false;
        }
        Vector2 topLeft = Vector2.Subtract(mapPos, pieceCenter);
        for (int i = 0; i < pathPiece.GetLength(0); i++) {
            for (int j = 0; j < pathPiece.GetLength(1); j++) {
                if (pathPiece[i, j] == 0) {continue;}
                Vector2 mapCoords = Vector2.Add(topLeft, new Vector2(i, j));
                _board[(int)mapCoords.X, (int)mapCoords.Y] = playerIndex;
            }
        }
        UpdateDrawables();
        //SendMessage();
        return true;
    }

    //private void SendMessage() {
    //    Message m = Message.Create(MessageSendMode.Reliable, ClientToServerId.PlaceTile);
    //    for (int i = 0; i < _board.GetLength(0); i++) {
    //        for (int j = 0; j < _board.GetLength(1); j++) {
    //            m.AddInt(_board[i, j]);
    //        }
    //    }
    //    NetworkManager.GetClient().Send(m);
    //}

    public void SetBoard(int[,] board) {
        _board = board;
        UpdateDrawables();
    }
}