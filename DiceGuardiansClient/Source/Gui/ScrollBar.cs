using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiceGuardiansClient.Source.Gui; 

public class ScrollBar : GuiElement {
    private Texture2D _thumbTexture;
    private int _scroll;
    private readonly int _maxScroll;
    private bool _isHighlighted;
    private MouseState _previousMouse;
    private new Rectangle CollisionBox => new((int) _position.X - 8, (int) _position.Y + _scroll, 32, 96);
    private bool _clicked;
    private MouseState _scrollStart;
    private int _oldScroll;
    private bool _hasChanged;

    public ScrollBar(Texture2D texture, Texture2D thumbTexture, Vector2 position, int height) {
        _texture = texture;
        _thumbTexture = thumbTexture;
        _position = position;
        _size = new Vector2(16, height);
        _maxScroll = height - 96;
        _scroll = 0;
        _oldScroll = 0;
        _isHighlighted = false;
        _previousMouse = Mouse.GetState();
        _hasChanged = false;
    }

    public bool hasChanged() {
        bool output = _hasChanged;
        _hasChanged = false;
        return output;
    }

    public float getCurrentScroll() {
        return (float) _scroll / _maxScroll;
    }
    
    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(
            _texture,
            new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y),
            new Rectangle(0, 0, _texture.Width, _texture.Height),
            Color.White);
        
        spriteBatch.Draw(
            _thumbTexture,
            CollisionBox,
            new Rectangle(_isHighlighted ? _thumbTexture.Width/2 : 0, 0, _thumbTexture.Width/2, _thumbTexture.Height),
            Color.White
            );
    }
    
    public void Update(GameTime gameTime) {
        MouseState mouse = Mouse.GetState();
        Point point = new Point(mouse.X, mouse.Y);

        if (CollisionBox.Contains(point)) {
            _isHighlighted = true;
            if (_previousMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed) {
                _clicked = true;
                _scrollStart = mouse;
                _oldScroll = _scroll;
            }
        }

        if (_clicked) {
            int distance = _scrollStart.Y - mouse.Y;
            int newScroll = _oldScroll - distance;

            if (newScroll < 0) {
                _scroll = 0;
            } else if (newScroll > _maxScroll) {
                _scroll = _maxScroll;
            } else {
                _scroll = newScroll;
            }
            
            if (mouse.LeftButton == ButtonState.Released) {
                _clicked = false;
            }
            _hasChanged = true;
        }
        
        _isHighlighted = false || CollisionBox.Contains(point);

        _previousMouse = Mouse.GetState();
    }
}