using System.Collections.Generic;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.Toolbox;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.RenderEngine; 

public class MasterRenderer {
    private readonly DisplayManager _displayManager;

    private const float NearPlane = 0.1f;
    private const float FarPlane = 1000;
    private Matrix _projectionMatrix;
    
    private readonly EntityRenderer _entityRenderer;
    private readonly Dictionary<Model, List<Entity>> _entities;

    private readonly GuiRenderer _guiRenderer;
    private readonly Dictionary<Texture2D, List<GuiElement>> _guiElements;

    public MasterRenderer(DisplayManager displayManager, SpriteBatch spriteBatch) {
        _displayManager = displayManager;
        CreateProjectionMatrix();
        
        _entityRenderer = new EntityRenderer(_projectionMatrix, _displayManager);
        _entities = new Dictionary<Model, List<Entity>>();

        _guiRenderer = new GuiRenderer(spriteBatch);
        _guiElements = new Dictionary<Texture2D, List<GuiElement>>();
    }

    public void Render(Camera camera) {
        _displayManager.GetGraphicsDevice().Clear(Color.CornflowerBlue);
        Matrix viewMatrix = Maths.CreateViewMatrix(camera);
        
        _entityRenderer.Render(_entities, viewMatrix);
        _guiRenderer.Draw(_guiElements);
        _entities.Clear();
        _guiElements.Clear();
    }

    public void AddEntityRange(List<Entity> entities) {
        foreach (Entity entity in entities) {
            AddEntity(entity);
        }
    }
    
    public void AddEntity(Entity entity) {
        Model model = entity.Model;
        if (_entities.ContainsKey(model)) {
            _entities[model].Add(entity);
        } else {
            List<Entity> entityList = new List<Entity> { entity };
            _entities[model] = entityList;
        }
    }
    
    public void AddGuiElementRange(List<GuiElement> elements) {
        foreach (GuiElement element in elements) {
            AddGuiElement(element);
        }
    }

    public void AddGuiElement(GuiElement element) {
        Texture2D texture = element.GetTexture();
        if (_guiElements.ContainsKey(texture)) {
            _guiElements[texture].Add(element);
        } else {
            List<GuiElement> textureList = new List<GuiElement> { element };
            _guiElements[texture] = textureList;
        }
    }
    
    private void CreateProjectionMatrix() {
        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45f),
            _displayManager.GetGraphicsDevice().DisplayMode.AspectRatio,
            NearPlane,
            FarPlane
        );
    }

    public Matrix GetProjectionMatrix() {
        return _projectionMatrix;
    }
}