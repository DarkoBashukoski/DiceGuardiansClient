using System.Collections.Generic;
using System.Linq;
using DiceGuardiansClient.Source.Collection;

namespace DiceGuardiansClient.Source.World.Player; 

public class DeckManager {
    private readonly List<Card> _deck;
    private readonly List<Card> _graveyard;
    
    public DeckManager(Dictionary<long, int> cards) {
        _deck = new List<Card>();
        _graveyard = new List<Card>();

        foreach (KeyValuePair<long, int> kvp in cards) {
            for (int i = 0; i < kvp.Value; i++) {
                _deck.Add(AllCards.GetCardData(kvp.Key));
            }
        }
    }

    public void MoveToGraveyard(int index) {
        Card c = _deck[index];
        _deck.RemoveAt(index);
        _graveyard.Add(c);
    }

    public Card GetCardAtIndex(int index) {
        return _deck[index];
    }

    public int GetDeckSize() {
        return _deck.Count;
    }

    public int GetGraveyardSize() {
        return _graveyard.Count;
    }

    public List<Card> GetDeck() {
        return _deck;
    }
    
    public void RemoveCard(long cardId) {
        foreach (var c in _deck.Where(c => c.GetCardId() == cardId)) {
            _graveyard.Add(c);
            _deck.Remove(c);
            break;
        }
    }
}