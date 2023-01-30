using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DiceGuardiansClient.Source.Gui; 

public class RadioButton : GuiElement {
    private bool _isSelected;
    private MouseState _previousMouse;

    public RadioButton(Texture2D buttonTexture, Vector2 position, Vector2 size) {
        _texture = buttonTexture;
        _position = position;
        _size = size;
        _isSelected = false;
        _previousMouse = Mouse.GetState();
    }
    
    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(
            _texture,
            CollisionBox,
            new Rectangle(_isSelected ? _texture.Width/2 : 0, 0, _texture.Width / 2, _texture.Height),
            Color.White);
    }

    public void Update() {
        MouseState mouse = Mouse.GetState();
        Point point = new Point(mouse.X, mouse.Y);

        if (CollisionBox.Contains(point)) {
            if (_previousMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed) {
                _isSelected = !_isSelected;
            }
        }

        _previousMouse = Mouse.GetState();
    }

    public bool IsSelected() {
        return _isSelected;
    }

    public void SetSelected(bool selected) {
        _isSelected = selected;
    }
}