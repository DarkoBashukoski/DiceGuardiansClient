using System.Collections.Generic;
using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.World.Minions;
using Microsoft.Xna.Framework;

namespace DiceGuardiansClient.Source.World.Player; 

public class HumanPlayer {
    private readonly User _user;
    private DeckManager _deckManager;
    private CrestPool _crestPool;

    private bool _hasSummoned;

    public HumanPlayer(User user, Dictionary<long, int> deck) {
        _user = user;
        _deckManager = new DeckManager(deck);
        _crestPool = new CrestPool();
        _hasSummoned = false;
    }

    public User GetUser() {
        return _user;
    }

    public DeckManager GetDeckManager() {
        return _deckManager;
    }

    public CrestPool GetCrestPool() {
        return _crestPool;
    }
}