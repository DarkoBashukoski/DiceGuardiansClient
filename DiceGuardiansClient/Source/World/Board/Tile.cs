#nullable enable
using DiceGuardiansClient.Source.World.Minions;

namespace DiceGuardiansClient.Source.World.Board; 

public class Tile {
    private Terrain _terrain;
    private Minion? _minion;

    public Tile() {
        _terrain = Terrain.EMPTY;
        _minion = null;
    }

    public Terrain GetTerrain() {
        return _terrain;
    }

    public Minion? GetMinion() {
        return _minion;
    }

    public void SetTerrain(Terrain terrain) {
        _terrain = terrain;
    }

    public void SetMinion(Minion minion) {
        _minion = minion;
    }
}