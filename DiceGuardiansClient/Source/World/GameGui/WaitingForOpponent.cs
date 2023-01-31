#nullable enable
using System.Collections.Generic;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.GameStates;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.World.Minions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.World.GameGui; 

public class WaitingForOpponent : State {
    private readonly GameInstance _game;
    private readonly SpriteFont _largeArial;
    private SpriteFont _arial;
    private readonly Dictionary<string, Texture2D> _textures;

    private readonly Image _rightWaiting;
    private readonly Image _cardImage;

    private readonly Label _health;
    private readonly Label _attack;
    private readonly Label _defense;

    public WaitingForOpponent(DisplayManager displayManager, GameInstance game, Dictionary<string, Texture2D> textures, SpriteFont large, SpriteFont normal) : base(displayManager) {
        _textures = textures;
        _largeArial = large;
        _arial = normal;
        _game = game;
        
        _rightWaiting = new Image(_textures["Waiting"], new Vector2(0, 0), new Vector2(1280, 720));
        _cardImage = new Image(_textures["Waiting"], new Vector2(1014, 62), new Vector2(206, 289));
        _cardImage.enableDraw(false);
            
        _health = new Label(_largeArial, new Vector2(1050, 482), DisplayManager.GetGraphicsDevice());
        _attack = new Label(_largeArial, new Vector2(1119, 482), DisplayManager.GetGraphicsDevice());
        _defense = new Label(_largeArial, new Vector2(1188, 482), DisplayManager.GetGraphicsDevice());
        
        _health.SetText("");
        _attack.SetText("");
        _defense.SetText("");
        
        _health.SetCentered(true);
        _attack.SetCentered(true);
        _defense.SetCentered(true);
        
        _health.SetColor(Color.Black);
        _attack.SetColor(Color.Black);
        _defense.SetColor(Color.Black);
    }

    public override List<GuiElement> GetGuiElements() {
        return new List<GuiElement> {
            _rightWaiting,
            _cardImage,
            _health,
            _attack,
            _defense
        };
    }

    public override List<Entity> Get3dSprites() {
        return new List<Entity>();
    }

    public override void Update(GameTime gameTime) {
        Minion? m = _game.GetHoveringMinion();
        if (m == null) {
            _attack.SetText("");
            _defense.SetText("");
            _health.SetText("");
            _cardImage.enableDraw(false);
            return;
        }
        _attack.SetText(m.GetAttack() + "");
        _defense.SetText(m.GetDefense() + "");
        _health.SetText(m.GetHealth() + "");
        _cardImage.SetTexture(m.GetCard().getTexture());
        _cardImage.enableDraw(true);
    }
}