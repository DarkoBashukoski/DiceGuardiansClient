using Microsoft.Xna.Framework;

namespace DiceGuardiansClient.Source.World.Summoning; 

public class Piece {
    private readonly int[,] _piece;
    private readonly Vector2 _center;

    public Piece(int[,] piece, Vector2 center) {
        _piece = piece;
        _center = center;
    }

    public int[,] GetPiece() {
        return _piece;
    }

    public Vector2 GetCenter() {
        return _center;
    }

    public Vector2[] GetRelativeCoords(Vector2 mapPos) {
        Vector2[] output = new Vector2[6];
        int index = 0;
        
        Vector2 topLeft = Vector2.Subtract(mapPos, _center);
        for (int i = 0; i < _piece.GetLength(0); i++) {
            for (int j = 0; j < _piece.GetLength(1); j++) {
                if (_piece[i, j] == 0) {
                    continue;
                }
                
                output[index] = Vector2.Add(topLeft, new Vector2(i, j));
                index++;
            }
        }

        return output;
    }
}