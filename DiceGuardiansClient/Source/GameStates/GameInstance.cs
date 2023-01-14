using System.Collections.Generic;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.GameStates; 

public class GameInstance : State{
    private readonly Entity _gameBoard;
    private readonly TileMap _tileMap;

    public GameInstance(DisplayManager displayManager) : base(displayManager) {
        Model board = displayManager.GetContent().Load<Model>("Board");
        _gameBoard = new Entity(board, new Vector3(6, 0, 9), new Vector3(-90, 0, 90), 1);
        _tileMap = new TileMap(displayManager);
    }

    public void SetBoard(int[,] board) {
        _tileMap.SetBoard(board);
    }

    public override List<GuiElement> GetGuiElements() {
        return new List<GuiElement>();
    }

    public override void Update(GameTime gameTime) {
        
    }

    public override List<Entity> Get3dSprites() {
        List<Entity> drawables = new List<Entity> {_gameBoard};
        drawables.AddRange(_tileMap.GetDrawables());
        return drawables;
    }
}