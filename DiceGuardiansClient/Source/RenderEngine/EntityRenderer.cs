using System.Collections.Generic;
using System.Linq;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.Toolbox;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.RenderEngine; 

public class EntityRenderer {
    private readonly Matrix _projectionMatrix;
    private readonly DisplayManager _displayManager;
    
    public EntityRenderer(Matrix projectionMatrix, DisplayManager displayManager) {
        _projectionMatrix = projectionMatrix;
        _displayManager = displayManager;
    }

    public void Render(Dictionary<Model, List<Entity>> entities, Matrix viewMatrix) {
        _displayManager.GetGraphicsDevice().DepthStencilState = DepthStencilState.Default;
        foreach (Model model in entities.Keys) {
            foreach (Entity entity in entities[model]) {
                Matrix worldMatrix = Maths.CreateWorldMatrix(entity.Position, entity.Rotation, entity.Scale);
                foreach (ModelMesh mesh in model.Meshes) {
                    foreach (var basicEffect in mesh.Effects.Cast<BasicEffect>()) {
                        basicEffect.View = viewMatrix;
                        basicEffect.World = worldMatrix;
                        basicEffect.Projection = _projectionMatrix;

                        basicEffect.EnableDefaultLighting();
                        mesh.Draw();
                    }
                }
            }
        }
    }
}