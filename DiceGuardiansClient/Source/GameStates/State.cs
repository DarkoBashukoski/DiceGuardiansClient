using System.Collections.Generic;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.RenderEngine;
using Microsoft.Xna.Framework;

namespace DiceGuardiansClient.Source.GameStates; 

public abstract class State {
    protected readonly DisplayManager DisplayManager;

    protected State(DisplayManager displayManager) {
        DisplayManager = displayManager;
    }

    public abstract List<GuiElement> GetGuiElements();
    public abstract List<Entity> Get3dSprites();
    public abstract void Update(GameTime gameTime);
}