using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiceGuardiansClient.Source.Gui; 

public class Image : GuiElement {
    private Color _color;
    private Rectangle _source;
    private Rectangle _destination;
    private Vector2 _translation;
    private bool _drawEnabled;
    private MouseState _previousMouse;
    public event EventHandler Click;
    
    public Image(Texture2D texture, Vector2 position, Vector2 size) {
        _texture = texture;
        _position = position;
        _size = size;
        _color = Color.White;
        _translation = new Vector2(0, 0);
        _source = new Rectangle(0, 0, _texture.Width, _texture.Height);
        _destination = new Rectangle((int) (_position.X + _translation.X), (int) (_position.Y + _translation.Y), (int)_size.X, (int)_size.Y);
        _drawEnabled = true;
        _previousMouse = Mouse.GetState();
    }
    
    public override void Draw(SpriteBatch spriteBatch) {
        if (_drawEnabled) {
            spriteBatch.Draw(_texture, _destination, _source, _color);
        }
    }
    
    public void Update(GameTime gameTime) {
        MouseState mouse = Mouse.GetState();
        Point point = new Point(mouse.X, mouse.Y);

        if (_destination.Contains(point)) {
            if (_previousMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed) {
                Click?.Invoke(this, EventArgs.Empty);
            }
        }

        _previousMouse = Mouse.GetState();
    }

    public void SetColor(Color color) {
        _color = color;
    }

    public void enableDraw(bool enable) {
        _drawEnabled = enable;
    }

    public void SetSource(Rectangle source) {
        _source = source;
    }

    public void SetTranslation(Vector2 translation) {
        _translation = translation;
        _destination = new Rectangle((int) (_position.X + _translation.X), (int) (_position.Y + _translation.Y), (int)_size.X, (int)_size.Y);
    }

    public Rectangle GetDestination() {
        return _destination;
    }

    public void SetDestination(Rectangle destination) {
        _destination = destination;
    }

    public void SetTexture(Texture2D texture) {
        _texture = texture;
        _source = new Rectangle(0, 0, _texture.Width, _texture.Height);
    }
}