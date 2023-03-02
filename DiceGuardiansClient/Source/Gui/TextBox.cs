using DiceGuardiansClient.Source.UserInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiceGuardiansClient.Source.Gui; 

public class TextBox : GuiElement {
    private readonly Texture2D _caretTexture;
    private readonly SpriteFont _font;
    private readonly SoundEffect _clickSfx;

    private string _text;
    
    private bool _isPassword;
    private bool _isHighlighted;
    private bool _isSelected;
    private bool _centerText;

    private MouseState _previousMouse;
    
    public TextBox(Texture2D textBoxTexture, Texture2D caretTexture, SpriteFont font, Vector2 position, Vector2 size, SoundEffect clickSfx) {
        _texture = textBoxTexture;
        _caretTexture = caretTexture;
        _font = font;
        _position = position;
        _size = size;
        _text = "";
        _isHighlighted = false;
        _isSelected = false;
        _isPassword = false;
        _centerText = false;
        _previousMouse = Mouse.GetState();
        _clickSfx = clickSfx;
    }

    public void SetIsPassword(bool isPassword) {
        _isPassword = isPassword;
    }

    public void Update(GameTime gameTime) {
        MouseState mouse = Mouse.GetState();
        Point point = new Point(mouse.X, mouse.Y);
        Rectangle position = new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y);
        if (position.Contains(point)) {
            _isHighlighted = true;
            if (_previousMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed) {
                _isSelected = true;
            }
        } else {
            _isHighlighted = false;
            if (_previousMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed) {
                _isSelected = false;
            }
        }
        
        if (_isSelected && KeyboardParser.TryConvertKeyboardInput(out char c)) {
            _text += c;
            if (c != (char) 0) {
                _clickSfx.Play();
            }
        }

        _previousMouse = Mouse.GetState();
    }

    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(
            _texture,
            new Rectangle((int) _position.X, (int) _position.Y, (int) _size.X, (int) _size.Y),
            new Rectangle(0, _isHighlighted ? (_texture.Height / 2) : 0, _texture.Width, _texture.Height / 2),
            Color.White);
        
        string toDraw = _text;
        if (_isPassword) {
            toDraw = "";
            for (int i = 0; i < _text.Length; i++) {
                toDraw += '*';
            }
        }

        Vector2 textPos = _position;
        
        if (_centerText) {
            int x = (int) (CollisionBox.X + CollisionBox.Width / 2 - _font.MeasureString(_text).X / 2);
            int y = (int) (CollisionBox.Y + CollisionBox.Height / 2 - _font.MeasureString(_text).Y / 2);
            textPos = new Vector2(x, y);
        }

        spriteBatch.DrawString(_font, toDraw, textPos + Vector2.One, Color.Black);
        spriteBatch.DrawString(_font, toDraw, textPos, Color.White);
    }

    public void SetText(string text) {
        _text = text;
    }

    public void CenterText(bool value) {
        _centerText = value;
    }

    public string GetText() {
        return _text;
    }
}