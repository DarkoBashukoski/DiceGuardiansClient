using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.Gui;

public abstract class GuiElement {
    protected Texture2D _texture;
    protected Vector2 _position;
    protected Vector2 _size;
    protected Rectangle CollisionBox => new((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y);
    
    public abstract void Draw(SpriteBatch spriteBatch);

    public Texture2D GetTexture() {
        return _texture;
    }
}