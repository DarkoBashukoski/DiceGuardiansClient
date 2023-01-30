using DiceGuardiansClient.Source.Entities;
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
    private readonly Camera _camera;

    public DisplayManager(GraphicsDevice device, GraphicsDeviceManager manager, ContentManager content, Camera camera) {
        _graphicsDevice = device;
        _graphics = manager;
        _content = content;
        _camera = camera;
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

    public Camera GetCamera() {
        return _camera;
    }
}