using System;
using System.Collections.Generic;
using System.Text;
using DiceGuardiansClient.Source.Collection;

namespace DiceGuardiansClient.Source.World.Player; 

public class CrestPool {
    private readonly Dictionary<Crest, int> _pool;

    public CrestPool() {
        _pool = new Dictionary<Crest, int> {
            [Crest.SUMMON] = 0,
            [Crest.ATTACK] = 0,
            [Crest.MOVEMENT] = 0,
            [Crest.DEFENSE] = 0,
            [Crest.MAGIC] = 0,
            [Crest.TRAP] = 0
        };
    }

    public int getCrests(Crest type) {
        return _pool[type];
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        sb.Append("Summon: ").Append(_pool[Crest.SUMMON]).Append('\n');
        sb.Append("Attack: ").Append(_pool[Crest.ATTACK]).Append('\n');
        sb.Append("Movement: ").Append(_pool[Crest.MOVEMENT]).Append('\n');
        sb.Append("Defense: ").Append(_pool[Crest.DEFENSE]).Append('\n');
        sb.Append("Magic: ").Append(_pool[Crest.MAGIC]).Append('\n');
        sb.Append("Trap: ").Append(_pool[Crest.TRAP]).Append('\n');

        return sb.ToString();
    }

    public void AddCrests(string crests) {
        string[] split = crests.Split('x');
        Crest type = split[1] switch {
            "s" => Crest.SUMMON,
            "r" => Crest.MOVEMENT,
            "a" => Crest.ATTACK,
            "d" => Crest.DEFENSE,
            "m" => Crest.MAGIC,
            "t" => Crest.TRAP,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        _pool[type] += Int32.Parse(split[0]);
    }
}