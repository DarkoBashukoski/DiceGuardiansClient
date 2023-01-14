using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.Gui; 

public class Label : GuiElement {
    private string _text;
    private SpriteFont _font;
    private Color _color;
    private Color _outlineColor;
    private float _scale;

    public Label(SpriteFont font, Vector2 position, GraphicsDevice graphicsDevice) {
        _texture = new Texture2D(graphicsDevice, 1, 1);
        _text = "";
        _font = font;
        _position = position;
        _color = Color.White;
        _outlineColor = Color.Black;
        _scale = 1;
    }
    
    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.DrawString(_font, _text, _position + new Vector2(1 * _scale, 1 * _scale), _outlineColor, 0, Vector2.Zero, _scale, SpriteEffects.None, 1f);
        spriteBatch.DrawString(_font, _text, _position + new Vector2(-1 * _scale, 1 * _scale), _outlineColor, 0, Vector2.Zero, _scale, SpriteEffects.None, 1f);
        //spriteBatch.DrawString(_font, _text, _position + new Vector2(-1 * _scale, -1 * _scale), _outlineColor, 0, Vector2.Zero, _scale, SpriteEffects.None, 1f);
        //spriteBatch.DrawString(_font, _text, _position + new Vector2(1 * _scale, -1 * _scale), _outlineColor, 0, Vector2.Zero, _scale, SpriteEffects.None, 1f);
        
        spriteBatch.DrawString(_font, _text, _position, _color, 0, Vector2.Zero, _scale, SpriteEffects.None, 1f);
    }

    public void SetText(string text) {
        _text = text;
    }

    public void SetScale(float scale) {
        _scale = scale;
    }

    public void SetColor(Color color) {
        _color = color;
    }

    public void SetOutlineColor(Color color) {
        _outlineColor = color;
    }
}