using Microsoft.Xna.Framework;

namespace DiceGuardiansClient.Source.Collisions; 

public class AABB {
    private Vector3 _position;
    private Vector3 _size;

    public AABB(Vector3 position, Vector3 size) {
        _position = position;
        _size = size;
    }

    public Vector3 GetPosition() {
        return _position;
    }

    public Vector3 GetSize() {
        return _size;
    }
}