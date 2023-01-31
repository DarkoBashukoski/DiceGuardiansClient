using System.Collections.Generic;
using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.GameStates;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.Networking;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.World.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Riptide;

namespace DiceGuardiansClient.Source.World.GameGui; 

public class DiceSelectionPhase : State {
    private readonly GameInstance _game;
    private readonly SpriteFont _largeArial;
    private readonly SpriteFont _arial;
    private readonly Dictionary<string, Texture2D> _textures;

    private readonly Image _rightDiceSelection;
    
    private RadioButton[] _cardSelectionButtons = null!;
    private Image[] _cardSelectionRacks = null!;
    private Image[] _cardCrests = null!;

    private readonly Button _confirmSelection;
    
    public DiceSelectionPhase(DisplayManager displayManager, GameInstance game, Dictionary<string, Texture2D> textures, SpriteFont large, SpriteFont normal) : base(displayManager) {
        _textures = textures;
        _largeArial = large;
        _arial = normal;
        _game = game;
        
        _confirmSelection = new Button(_textures["Button"], _arial, new Vector2(1025, 672), new Vector2(186, 32));
        _rightDiceSelection = new Image(_textures["DiceSelection"], new Vector2(0, 0), new Vector2(1280, 720));

        _confirmSelection.SetText("Confirm Selection");
        
        SetUpDiceSelection();
    }

    private void SetUpDiceSelection() {
        HumanPlayer player = _game.GetPlayer();
        int deckSize = player.GetDeckManager().GetDeckSize();

        _cardSelectionButtons = new RadioButton[deckSize];
        for (int i = 0; i < deckSize; i++) {
            _cardSelectionButtons[i] = new RadioButton(
                _textures["RadioButton"],
                new Vector2(974, 62 + i*40),
                new Vector2(36, 36)
            );
        }

        _cardSelectionRacks = new Image[deckSize];
        for (int i = 0; i < deckSize; i++) {
            _cardSelectionRacks[i] = new Image(
                _textures["DiceRack"],
                new Vector2(1032, 62 + i*40),
                new Vector2(216, 36)
            );
        }

        _cardCrests = new Image[deckSize * 6];
        List<Card> deck = player.GetDeckManager().GetDeck();
        for (int i = 0; i < deckSize; i++) {
            DiceFace[] d = deck[i].GetDiceFaces();
            for (int j = 0; j < 6; j++) {
                _cardCrests[6 * i + j] = new Image(
                    _textures["DiceIcons"],
                    new Vector2(1036 + j*36, 66 + i*40),
                    new Vector2(28, 28)
                );
                _cardCrests[6 * i + j].SetSource(new Rectangle(
                    _textures["DiceIcons"].Width / 6 * (int) d[j].GetCrest(),
                    0,
                    _textures["DiceIcons"].Width / 6,
                    _textures["DiceIcons"].Height
                ));
            }
        }

        _confirmSelection.Click += (_, _) => {
            Message m = Message.Create(MessageSendMode.Reliable, ClientToServerId.SelectDiceRoll);
            for (int i = 0; i < deckSize; i++) {
                if (_cardSelectionButtons[i].IsSelected()) {
                    m.AddLong(_game.GetPlayer().GetDeckManager().GetCardAtIndex(i).GetCardId());
                    _cardSelectionButtons[i].SetSelected(false);
                }
            }
            NetworkManager.GetClient().Send(m);
            _confirmSelection.SetDisabled(true);
        };
    }

    public override List<GuiElement> GetGuiElements() {
        List<GuiElement> output = new List<GuiElement> {
            _rightDiceSelection,
            _confirmSelection
        };
        
        output.AddRange(_cardSelectionButtons);
        output.AddRange(_cardSelectionRacks);
        output.AddRange(_cardCrests);

        return output;
    }

    public override List<Entity> Get3dSprites() {
        return new List<Entity>();
    }

    public override void Update(GameTime gameTime) {
        int selectedCount = 0;
        foreach (RadioButton radioButton in _cardSelectionButtons) {
            radioButton.Update();
            if (radioButton.IsSelected()) {
                selectedCount += 1;
            }
        }
        
        _confirmSelection.SetDisabled(selectedCount != 3);
        _confirmSelection.Update(gameTime);
    }
}