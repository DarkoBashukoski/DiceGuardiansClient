using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.World.Player;

namespace DiceGuardiansClient.Source.World.Opponent; 

public class HumanOpponent {
    private readonly User _user;
    private CrestPool _crestPool;
    private int _deckSize;

    public HumanOpponent(User user) {
        _user = user;
        _crestPool = new CrestPool();
        _deckSize = 15;
    }

    public User GetUser() {
        return _user;
    }

    public CrestPool GetCrestPool() {
        return _crestPool;
    }

    public int GetDeckSize() {
        return _deckSize;
    }

    public void SetDeckSize(int size) {
        _deckSize = size;
    }
}