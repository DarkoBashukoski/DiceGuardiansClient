using System;
using System.Collections.Generic;
using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.Gui;
using DiceGuardiansClient.Source.Networking;
using DiceGuardiansClient.Source.RenderEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Riptide;

namespace DiceGuardiansClient.Source.GameStates; 

public class Collection : State {
    private bool _loading;
    
    private readonly User _user;
    private readonly CollectionManager _collectionManager;

    private Image _background;
    private ScalingImage _collectionContainer;
    private ScalingImage _deckContainer;

    private AnimatedImage _loadingCircle;
    private Label _waitingText;
    private SpriteFont _font;

    private Image[] _cardsToDraw;
    private Image[] _cardQuantities;
    private ScrollBar _scrollBar;

    private List<Image> _deckContainers;
    private List<Label> _deckLabels;

    private Button _back;
    private Button _save;

    public Collection(DisplayManager displayManager, User user) : base(displayManager) {
        _loading = true;
        _user = user;
        _collectionManager = new CollectionManager(displayManager, user);

        LoadElements();
        
        _loadingCircle.SetAnimationSpeed(0.25f);
        _waitingText.SetCentered(true);
        _waitingText.SetText("Waiting for a response from the main server");
        
        _back.SetText("Back");
        _save.SetText("Save");

        Message m = Message.Create(MessageSendMode.Reliable, ClientToServerId.GetCollection);
        m.AddLong(_user.GetUserId());
        NetworkManager.GetClient().Send(m);
    }

    public override List<GuiElement> GetGuiElements() {
        if (_loading) {
            return new List<GuiElement> {
                _background,
                _collectionContainer,
                _deckContainer,
                _loadingCircle,
                _waitingText
            };
        }
        
        List<GuiElement> output = new List<GuiElement> {
            _background,
            _collectionContainer,
            _deckContainer,
            _scrollBar,
            _save,
            _back
        };
        
        output.AddRange(_cardsToDraw);
        output.AddRange(_cardQuantities);
        output.AddRange(_deckContainers);
        output.AddRange(_deckLabels);

        return output;
    }

    public override List<Entity> Get3dSprites() {
        return new List<Entity>();
    }

    public override void Update(GameTime gameTime) {
        if (_loading) {
            _loadingCircle.StepAnimation();
        }
        else {
            _back.Update(gameTime);
            _save.Update(gameTime);
            _scrollBar.Update(gameTime);
            if (_scrollBar.hasChanged()) {ManageScroll();}

            foreach (Image i in _cardsToDraw) {
                i.Update(gameTime);
            }
        }
    }

    private void LoadElements() {
        Texture2D loadingTexture = DisplayManager.GetContent().Load<Texture2D>("LoadingScreen/Loading");
        Texture2D containerTexture = DisplayManager.GetContent().Load<Texture2D>("MenuContainer");
        Texture2D backgroundTexture = DisplayManager.GetContent().Load<Texture2D>("LoadingScreen/MenuBackground");
        Texture2D scrollBarTexture = DisplayManager.GetContent().Load<Texture2D>("ScrollBar");
        Texture2D scrollThumbTexture = DisplayManager.GetContent().Load<Texture2D>("ScrollThumb");
        Texture2D buttonTexture = DisplayManager.GetContent().Load<Texture2D>("LoadingScreen/Button");

        _font = DisplayManager.GetContent().Load<SpriteFont>("arial");
        
        _background = new Image(backgroundTexture, new Vector2(0, 0), new Vector2(DisplayManager.GetWidth(), DisplayManager.GetHeight()));
        _collectionContainer = new ScalingImage(containerTexture, new Vector2(16, 16), new Vector2(984, 690), new Vector2(32, 32));
        _deckContainer = new ScalingImage(containerTexture, new Vector2(1000, 16), new Vector2(264, 690), new Vector2(32, 32));
        _loadingCircle = new AnimatedImage(loadingTexture, new Vector2(4, 4), new Vector2(395, 200), new Vector2(200, 200));
        _waitingText = new Label(_font, new Vector2(495, 450), DisplayManager.GetGraphicsDevice());

        _scrollBar = new ScrollBar(scrollBarTexture, scrollThumbTexture, new Vector2(956, 48), 624);

        _back = new Button(buttonTexture, _font, new Vector2(1032, 614), new Vector2(98, 56));
        _save = new Button(buttonTexture, _font, new Vector2(1134, 614), new Vector2(98, 56));
    }

    public void TriggerSuccessfulGetCollection(Message m) {
        Dictionary<long, int> ownedCards = new Dictionary<long, int>();
        Dictionary<long, int> deck = new Dictionary<long, int>();

        int size = m.GetInt();
        for (int i = 0; i < size; i++) {
            ownedCards[m.GetLong()] = m.GetInt();
        }

        int deckSize = m.GetInt();
        for (int i = 0; i < deckSize; i++) {
            deck[m.GetLong()] = m.GetInt();
        }
        
        _collectionManager.SetOwnedCards(ownedCards);
        _collectionManager.SetDeck(deck);
        PrepareCollectionDraw();
        PrepareDeckList();
        _loading = false;
        
        foreach (KeyValuePair<long, int> kvp in _collectionManager.GetDeck()) {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }

    private void PrepareCollectionDraw() {
        Texture2D quantityTexture = DisplayManager.GetContent().Load<Texture2D>("Cards/cardOwnedIndicator");
        Rectangle inDeck = new Rectangle(0, 0, quantityTexture.Width/3, quantityTexture.Height);
        Rectangle owned = new Rectangle(quantityTexture.Width/3, 0, quantityTexture.Width/3, quantityTexture.Height);
        Rectangle missing = new Rectangle(quantityTexture.Width/3 * 2, 0, quantityTexture.Width/3, quantityTexture.Height);
        
        _cardsToDraw = new Image[AllCards.Count()];
        _cardQuantities = new Image[AllCards.Count() * 3];
        int column = 0;
        int row = 0;
        
        foreach (Card c in AllCards.SortedById()) {
            Vector2 cardPos = new Vector2(48 + 226 * column, 48 + 358 * row);
            Vector2 cardSize = new Vector2(210, 294);

            Image i = new Image(c.getTexture(), cardPos, cardSize);
            if (_collectionManager.GetCardCount(c.GetCardId()) == 0) {
                i.SetColor(Color.DimGray);
            }

            Image q1 = new Image(quantityTexture, new Vector2(cardPos.X + 28, cardPos.Y + 302), new Vector2(32, 32));
            Image q2 = new Image(quantityTexture, new Vector2(cardPos.X + 89, cardPos.Y + 302), new Vector2(32, 32));
            Image q3 = new Image(quantityTexture, new Vector2(cardPos.X + 150, cardPos.Y + 302), new Vector2(32, 32));
            
            q1.SetSource(missing);
            q2.SetSource(missing);
            q3.SetSource(missing);

            int quantity = _collectionManager.GetCardCount(c.GetCardId());
            if (quantity > 0) {q1.SetSource(owned);}
            if (quantity > 1) {q2.SetSource(owned);}
            if (quantity > 2) {q3.SetSource(owned);}
            
            int index = row * 4 + column;

            i.Click += (_, _) => TryAddToDeck(c.GetCardId());
            
            _cardsToDraw[index] = i;
            _cardQuantities[3 * index] = q1;
            _cardQuantities[3 * index + 1] = q2;
            _cardQuantities[3 * index + 2] = q3;
            column = (column + 1) % 4;
            row = column == 0 ? row + 1 : row;
        }
    }

    private void PrepareDeckList() {
        int spacing = 4;
        int height = 34;
        int width = 200;
        Vector2 topLeft = new Vector2(1032, 48);
        
        Texture2D containerTexture = DisplayManager.GetContent().Load<Texture2D>("LoadingScreen/TextBox");
        Rectangle source = new Rectangle(0, 0, containerTexture.Width, containerTexture.Height / 2);

        _deckContainers = new List<Image>();
        _deckLabels = new List<Label>();

        int row = 0;

        foreach (KeyValuePair<long, int> kvp in _collectionManager.GetDeck()) {
            Vector2 pos = new Vector2(topLeft.X, topLeft.Y  + (height + spacing) * row);
            Vector2 size = new Vector2(width, height);

            Image i = new Image(containerTexture, pos, size);
            Label l = new Label(_font, new Vector2(pos.X + 16, pos.Y + 8), DisplayManager.GetGraphicsDevice());
            l.SetText($"{kvp.Value}x {AllCards.GetCardData(kvp.Key).GetName()}");
            i.SetSource(source);
            _deckContainers.Add(i);
            _deckLabels.Add(l);
            row++;
        }
        
        
    }

    private void TryAddToDeck(long cardId) {
        Console.WriteLine(AllCards.GetCardData(cardId).GetName());
        //TODO implement actual functionality
    }

    private void ManageScroll() {
        int totalHeight = (int) Math.Ceiling(_cardsToDraw.Length / 4f) * 358;
        int maxTranslation = totalHeight - 642 < 0 ? 0 : totalHeight - 642;
        float scroll = _scrollBar.getCurrentScroll();
        int translation = (int) -(maxTranslation * scroll);

        Rectangle scrollArea = new Rectangle(24, 24, 1000, 673);
        int cardHeight = 294;
        
        foreach (Image i in _cardsToDraw) {
            i.SetTranslation(new Vector2(0, translation));
            Texture2D tex = i.GetTexture();
            Rectangle cardArea = i.GetDestination();
            
            if (!scrollArea.Intersects(cardArea)) {
                i.enableDraw(false);
                continue;
            }
            i.enableDraw(true);
            Rectangle intersection = Rectangle.Intersect(scrollArea, cardArea);
            float ratio = (float) tex.Height / cardHeight;
            i.SetDestination(intersection);
            if (cardArea.Y <= 24) {
                i.SetSource(new Rectangle(0, tex.Height - (int)(intersection.Height * ratio), tex.Width, (int) (intersection.Height * ratio)));
            } else if (cardArea.Y >= 404) {
                i.SetSource(new Rectangle(0, 0, tex.Width, (int) (intersection.Height * ratio)));
            } else {
                i.SetSource(new Rectangle(0, 0, tex.Width, tex.Height));
            }
        }
        foreach (Image i in _cardQuantities) {
            i.SetTranslation(new Vector2(0, translation));
            Rectangle cardArea = i.GetDestination();
            Rectangle intersection = Rectangle.Intersect(scrollArea, cardArea);
            i.SetDestination(intersection);
        }
    }
}