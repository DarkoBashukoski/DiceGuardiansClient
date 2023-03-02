using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiceGuardiansClient.Source.Gui; 

public class Button : GuiElement {
    private readonly SpriteFont _font;
    private string _text;
    private bool _isHighlighted;
    private bool _isDisabled;
    private SoundEffect _clickSfx;
    
    public event EventHandler Click;

    private MouseState _previousMouse;

    public Button(Texture2D buttonTexture, SpriteFont font, Vector2 position, Vector2 size, SoundEffect clickSfx) {
        _texture = buttonTexture;
        _font = font;
        _position = position;
        _size = size;
        _text = "";
        _isHighlighted = false;
        _previousMouse = Mouse.GetState();
        _isDisabled = false;
        _clickSfx = clickSfx;
    }
    
    public Button(Texture2D buttonTexture, SpriteFont font, Vector2 position, Vector2 size) {
        _texture = buttonTexture;
        _font = font;
        _position = position;
        _size = size;
        _text = "";
        _isHighlighted = false;
        _previousMouse = Mouse.GetState();
        _isDisabled = false;
    }
    
    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(
            _texture,
            CollisionBox,
            new Rectangle(0, _isHighlighted ? (_texture.Height / 2) : 0, _texture.Width, _texture.Height / 2),
            _isDisabled ? Color.DimGray : Color.White);

        if (string.IsNullOrEmpty(_text)) return;
        int x = (int) (CollisionBox.X + CollisionBox.Width / 2 - _font.MeasureString(_text).X / 2);
        int y = (int) (CollisionBox.Y + CollisionBox.Height / 2 - _font.MeasureString(_text).Y / 2);
            
        spriteBatch.DrawString(_font, _text, new Vector2(x, y) + Vector2.One, Color.Black);
        spriteBatch.DrawString(_font, _text, new Vector2(x, y), Color.White);
    }

    public void Update(GameTime gameTime) {
        if (_isDisabled) {
            return;
        }
        
        MouseState mouse = Mouse.GetState();
        Point point = new Point(mouse.X, mouse.Y);

        _isHighlighted = false;

        if (CollisionBox.Contains(point)) {
            _isHighlighted = true;
            if (_previousMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed) {
                _clickSfx?.Play();
                Click?.Invoke(this, EventArgs.Empty);
            }
        }

        _previousMouse = Mouse.GetState();
    }

    public void SetText(string text) {
        _text = text;
    }

    public string GetText() {
        return _text;
    }

    public void SetDisabled(bool isDisabled) {
        _isDisabled = isDisabled;
    }
}