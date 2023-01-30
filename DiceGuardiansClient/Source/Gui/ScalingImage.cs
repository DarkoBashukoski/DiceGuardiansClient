using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.Gui; 

public class ScalingImage : GuiElement {
    private readonly Vector2 _scaleStart;
    
    public ScalingImage(Texture2D texture, Vector2 position, Vector2 size, Vector2 scaleStart) {
        _texture = texture;
        _position = position;
        _size = size;
        _scaleStart = scaleStart;
        Rectangle r1 = new Rectangle((int)_position.X, (int)_position.Y, (int)_scaleStart.X, (int)_scaleStart.Y);
        Rectangle r2 = new Rectangle(0, 0, (int)_scaleStart.X, (int)_scaleStart.Y);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw( //TopLeft
            _texture,
            new Rectangle((int) _position.X, (int) _position.Y, (int) _scaleStart.X, (int) _scaleStart.Y),
            new Rectangle(0, 0, (int) _scaleStart.X, (int) _scaleStart.Y),
            Color.White);
        spriteBatch.Draw( //Top
            _texture,
            new Rectangle((int) (_scaleStart.X + _position.X), (int) _position.Y, (int) (_size.X - 2 * _scaleStart.X), (int) _scaleStart.Y),
            new Rectangle((int) _scaleStart.X, 0, (int) (_texture.Width - 2 * _scaleStart.X), (int) _scaleStart.Y),
            Color.White);
        spriteBatch.Draw( //TopRight
            _texture,
            new Rectangle((int) (_position.X + _size.X - _scaleStart.X), (int) _position.Y, (int) _scaleStart.X, (int) _scaleStart.Y),
            new Rectangle((int) (_texture.Width - _scaleStart.X), 0, (int) _scaleStart.X, (int) _scaleStart.Y),
            Color.White);
        spriteBatch.Draw( //Left
            _texture,
            new Rectangle((int) _position.X, (int) (_scaleStart.Y + _position.Y), (int) _scaleStart.X, (int) (_size.Y - 2 * _scaleStart.Y)),
            new Rectangle(0, (int) _scaleStart.Y, (int) _scaleStart.X, (int) (_texture.Height - 2 * _scaleStart.Y)),
            Color.White);
        spriteBatch.Draw( //Middle
            _texture,
            new Rectangle((int) (_position.X + _scaleStart.X), (int) (_position.Y + _scaleStart.Y), (int) (_size.X - 2 * _scaleStart.X), (int) (_size.Y - 2 * _scaleStart.Y)),
            new Rectangle((int) _scaleStart.X, (int) _scaleStart.Y, (int) (_texture.Width - 2 * _scaleStart.X), (int) (_texture.Height - 2 * _scaleStart.Y)),
            Color.White);
        spriteBatch.Draw( //Right
            _texture,
            new Rectangle((int) (_position.X + _size.X - _scaleStart.X), (int) (_position.Y + _scaleStart.Y), (int) _scaleStart.X, (int) (_size.Y - 2 * _scaleStart.Y)),
            new Rectangle((int) (_texture.Width - _scaleStart.X), (int) _scaleStart.Y, (int) _scaleStart.X, (int) (_texture.Height - 2 * _scaleStart.Y)),
            Color.White);
        spriteBatch.Draw( //BottomLeft
            _texture,
            new Rectangle((int) _position.X, (int) (_position.Y + _size.Y - _scaleStart.Y), (int) _scaleStart.X, (int) _scaleStart.Y),
            new Rectangle(0, (int) (_texture.Height - _scaleStart.Y), (int) _scaleStart.X, (int) _scaleStart.Y),
            Color.White);
        spriteBatch.Draw( //Bottom
            _texture,
            new Rectangle((int) (_position.X + _scaleStart.X), (int) (_position.Y + _size.Y - _scaleStart.Y), (int) (_size.X - 2 * _scaleStart.X), (int) _scaleStart.Y),
            new Rectangle((int) _scaleStart.X, (int) (_texture.Height - _scaleStart.Y), (int) (_texture.Width - 2 * _scaleStart.X), (int) _scaleStart.Y),
            Color.White);
        spriteBatch.Draw( //BottomRight
            _texture,
            new Rectangle((int) (_position.X + _size.X - _scaleStart.X), (int) (_position.Y + _size.Y - _scaleStart.Y), (int) _scaleStart.X, (int) _scaleStart.Y),
            new Rectangle((int) (_texture.Width - _scaleStart.X), (int) (_texture.Height - _scaleStart.Y), (int) _scaleStart.X, (int) _scaleStart.Y),
            Color.White);
    }
}