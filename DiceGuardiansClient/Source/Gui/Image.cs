using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.Gui; 

public class Image : GuiElement {
    public Image(Texture2D texture, Vector2 position, Vector2 size) {
        _texture = texture;
        _position = position;
        _size = size;
    }
    
    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(
            _texture,
            new Rectangle((int) _position.X, (int) _position.Y, (int) _size.X, (int) _size.Y),
            new Rectangle(0, 0, _texture.Width, _texture.Height),
            Color.White);
    }
}