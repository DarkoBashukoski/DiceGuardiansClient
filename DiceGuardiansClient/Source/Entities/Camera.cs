using Microsoft.Xna.Framework;

namespace DiceGuardiansClient.Source.Entities; 

public class Camera {
    private Vector3 _position;
    private Vector3 _rotation;
    
    public Camera(Vector3 position, Vector3 rotation) {
        _position = position;
        _rotation = rotation;
    }

    public Vector3 GetPosition() {
        return _position;
    }

    public Vector3 GetRotation() {
        return _rotation;
    }

    public void SetPosition(float x, float y, float z) {
        _position = new Vector3(x, y, z);
    }
    
    public void SetRotation(float x, float y, float z) {
        _rotation = new Vector3(x, y, z);
    }
}