using System.Collections.Generic;
using System.Linq;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.RenderEngine;

namespace DiceGuardiansClient.Source.Collection; 

public class CollectionManager {
    private DisplayManager _displayManager;
    private const int deckCapacity = 15;
    
    private Dictionary<long, int> _ownedCards;
    private Dictionary<long, int> _deck;
    private User _user;

    public CollectionManager(DisplayManager displayManager, User user) {
        _displayManager = displayManager;
        _ownedCards = new Dictionary<long, int>();
        _deck = new Dictionary<long, int>();
        _user = user;
    }
    
    public void SetOwnedCards(Dictionary<long, int> ownedCards) {
        _ownedCards = ownedCards;
    }

    public void SetDeck(Dictionary<long, int> deck) {
        _deck = deck;
    }

    public static int GetDeckCapacity() {
        return deckCapacity;
    }

    public Dictionary<long, int> GetOwnedCards() {
        return _ownedCards;
    }
    
    public Dictionary<long, int> GetDeck() {
        return _deck;
    }

    public int GetCardCount(long cardId) {
        return _ownedCards.ContainsKey(cardId) ? _ownedCards[cardId] : 0;
    }

    public int GetCardCountInDeck(long cardId) {
        return _deck.ContainsKey(cardId) ? _deck[cardId] : 0;
    }

    public int GetCurrentDeckSize() {
        return _deck.Sum(kvp => kvp.Value);
    }

    public void AddToDeck(long cardId) {
        if (_deck.ContainsKey(cardId)) {
            _deck[cardId] += 1;
        } else {
            _deck[cardId] = 1;
        }
    }

    public void RemoveFromDeck(long cardId) {
        _deck[cardId]--;
        if (_deck[cardId] == 0) {
            _deck.Remove(cardId);
        }
    }
}