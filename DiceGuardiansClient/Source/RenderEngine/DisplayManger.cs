using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.RenderEngine; 

public class DisplayManager {
    private const int Width = 1280;
    private const int Height = 720;

    private readonly GraphicsDevice _graphicsDevice;
    private readonly GraphicsDeviceManager _graphics;
    private readonly ContentManager _content;

    public DisplayManager(GraphicsDevice device, GraphicsDeviceManager manager, ContentManager content) {
        _graphicsDevice = device;
        _graphics = manager;
        _content = content;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
    }

    public static int GetWidth() {
        return Width;
    }
    
    public static int GetHeight() {
        return Height;
    }

    public ContentManager GetContent() {
        return _content;
    }

    public GraphicsDevice GetGraphicsDevice() {
        return _graphicsDevice;
    }
}