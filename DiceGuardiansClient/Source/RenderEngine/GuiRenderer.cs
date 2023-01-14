using System.Collections.Generic;
using System.Linq;
using DiceGuardiansClient.Source.Gui;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.RenderEngine; 

public class GuiRenderer {
    private readonly SpriteBatch _spriteBatch;

    public GuiRenderer(SpriteBatch spriteBatch) {
        _spriteBatch = spriteBatch;
    }

    public void Draw(Dictionary<Texture2D, List<GuiElement>> elements) {
        _spriteBatch.Begin();
        foreach (GuiElement guiElement in elements.Keys.SelectMany(texture => elements[texture])) {
            guiElement.Draw(_spriteBatch);
        }
        _spriteBatch.End();
    }
}