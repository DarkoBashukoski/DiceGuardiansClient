using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.Gui; 

public class AnimatedImage : GuiElement {
    private readonly Vector2 _animationLayout;
    
    private int _currentFrame;
    private float _fractionFrame;
    private float _animationSpeed;
    
    public AnimatedImage(Texture2D texture, Vector2 animationLayout, Vector2 position, Vector2 size) {
        _texture = texture;
        _animationLayout = animationLayout;
        _position = position;
        _size = size;
        _currentFrame = 0;
        _fractionFrame = 0;
        _animationSpeed = 1;
    }
    
    public override void Draw(SpriteBatch spriteBatch) {
        int width = _texture.Width / (int)_animationLayout.X;
        int height = _texture.Height / (int)_animationLayout.Y;
        
        Rectangle source = new Rectangle(
            _currentFrame % (int) _animationLayout.X * width,
            _currentFrame / (int) _animationLayout.Y * height,
            width,
            height
        );
        
        spriteBatch.Draw(_texture, CollisionBox, source, Color.White);
    }
    
    public void StepAnimation() {
        _fractionFrame = (_fractionFrame + _animationSpeed) % (int) (_animationLayout.X * _animationLayout.Y);
        _currentFrame = (int) _fractionFrame;
    }

    public void SetAnimationSpeed(float speed) {
        _animationSpeed = speed;
    }
}