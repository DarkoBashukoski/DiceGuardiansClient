using System.Collections.Generic;
using System.Linq;
using DiceGuardiansClient.Source.RenderEngine;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.Collection; 

public static class AllCards {
    private static Dictionary<long, Card> _allCards;
    private static Card _diceGuardian;

    public static void InitializeCardInfo(DisplayManager displayManager) { //TODO probably change to read all cards from database
        _allCards = new Dictionary<long, Card>();
        Texture2D cardTexture = displayManager.GetContent().Load<Texture2D>("Cards/test-card");
        for (int i = 0; i < 30; i++) {
            _allCards[i] = new Card(i, $"test-card-{i}", 4, 3, 2, "testString", "1xs 2xr 1xa 1xt 1xd 1xm", cardTexture);
        }

        _diceGuardian = new Card(9999, "Dice Guardian", 1, 0, 30, "", "1xs 1xs 1xs 1xs 1xs 1xs", cardTexture);
    }

    public static Card GetCardData(long cardId) {
        return cardId == 9999 ? _diceGuardian : _allCards[cardId];
    }

    public static int Count() {
        return _allCards.Count;
    }

    public static Card[] SortedById() {
        return _allCards.Values.OrderBy(x => x.GetCardId()).ToArray();
    }

    public static Card GetDiceGuardian() {
        return _diceGuardian;
    }
}