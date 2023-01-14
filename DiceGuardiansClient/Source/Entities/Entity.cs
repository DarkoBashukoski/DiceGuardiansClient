using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.Entities; 

public class Entity {
    public Model Model { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Rotation { get; set; }
    public float Scale { get; set; }

    public Entity(Model model, Vector3 position, Vector3 rotation, float scale) {
        Model = model;
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }
    
    public void Move(float x, float y, float z) {
        Position = new Vector3(Position.X + x, Position.Y + y, Position.Z + z);
    }

    public void Rotate(float x, float y, float z) {
        Rotation = new Vector3(Rotation.X + x, Rotation.Y + y, Rotation.Z + z);
    }
}