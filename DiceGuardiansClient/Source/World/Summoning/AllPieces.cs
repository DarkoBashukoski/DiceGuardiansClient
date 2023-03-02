using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DiceGuardiansClient.Source.World.Summoning; 

public static class AllPieces {
    private static List<List<Piece>> _allPieces;

    private static int _piece;
    private static int _rotation;

    public static void Start() {
        _piece = 0;
        _rotation = 0;
        
        List<Piece> piece1 = new List<Piece> {
            new Piece(
                new[,] {
                    { 0, 1, 0 },
                    { 1, 1, 1 },
                    { 0, 1, 0 },
                    { 0, 1, 0 }
                },
                new Vector2(1, 1)
            ),
            new Piece(
                new[,] {
                    { 0, 1, 0, 0 },
                    { 1, 1, 1, 1 },
                    { 0, 1, 0, 0 }
                },
                new Vector2(1, 1)
            ),
            new Piece(
                new[,] {
                    { 0, 1, 0 },
                    { 0, 1, 0 },
                    { 1, 1, 1 },
                    { 0, 1, 0 }
                },
                new Vector2(2, 1)
            ),
            new Piece(
                new[,] {
                    { 0, 0, 1, 0 },
                    { 1, 1, 1, 1 },
                    { 0, 0, 1, 0 }
                },
                new Vector2(1, 2)
            )
        };
        List<Piece> piece2 = new List<Piece> {
            new Piece(
                new[,] {
                    { 0, 1, 0 },
                    { 1, 1, 0 },
                    { 0, 1, 1 },
                    { 0, 1, 0 }
                },
                new Vector2(1, 1)
            ),
            new Piece(
                new[,] {
                    { 0, 0, 1, 0 },
                    { 1, 1, 1, 1 },
                    { 0, 1, 0, 0 }
                },
                new Vector2(1, 1)
            ),
            new Piece(
                new[,] {
                    { 0, 1, 0 },
                    { 1, 1, 0 },
                    { 0, 1, 1 },
                    { 0, 1, 0 }
                },
                new Vector2(2, 1)
            ),
            new Piece(
                new[,] {
                    { 0, 0, 1, 0 },
                    { 1, 1, 1, 1 },
                    { 0, 1, 0, 0 }
                },
                new Vector2(1, 2)
            )
        };
        List<Piece> piece3 = new List<Piece> {
            new Piece(
                new[,] {
                    { 0, 1 },
                    { 0, 1 },
                    { 1, 1 },
                    { 1, 0 },
                    { 1, 0 }
                },
                new Vector2(2, 0)
            ),
            new Piece(
                new[,] {
                    { 1, 1, 1, 0, 0 },
                    { 0, 0, 1, 1, 1 }
                },
                new Vector2(1, 2)
            ),
            new Piece(
                new[,] {
                    { 0, 1 },
                    { 0, 1 },
                    { 1, 1 },
                    { 1, 0 },
                    { 1, 0 }
                },
                new Vector2(2, 1)
            ),
            new Piece(
                new[,] {
                    { 1, 1, 1, 0, 0 },
                    { 0, 0, 1, 1, 1 }
                },
                new Vector2(0, 2)
            ),
        };
        
        _allPieces = new List<List<Piece>> {
            piece1,
            piece2,
            piece3
        };
    }

    public static void Rotate() {
        _rotation = (_rotation + 1) % 4;
    }

    public static void NextPiece() {
        _piece = (_piece + 1) % _allPieces.Count;
    }

    public static Piece GetCurrentPiece() {
        return _allPieces[_piece][_rotation];
    }

    public static int GetPiece() {
        return _piece;
    }

    public static Piece GetPiece(int piece, int rotation) {
        return _allPieces[piece][rotation];
    }

    public static int GetRotation() {
        return _rotation;
    }

    public static void Reset() {
        _rotation = 0;
        _piece = 0;
    }
}