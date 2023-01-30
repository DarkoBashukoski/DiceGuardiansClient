using System;

namespace DiceGuardiansClient.Source.Collection; 

public class DiceFace {
    private Crest _crest;
    private readonly int _count;
    
    public DiceFace(Crest type, int count) {
        _crest = type;
        _count = count;
    }

    public Crest GetCrest() {return _crest;}
    public int GetCount() {return _count;}

    public override string ToString() {
        string type = _crest switch {
            Crest.SUMMON => "s",
            Crest.MOVEMENT => "r",
            Crest.ATTACK => "a",
            Crest.DEFENSE => "d",
            Crest.MAGIC => "m",
            Crest.TRAP => "t",
            _ => throw new ArgumentOutOfRangeException(nameof(_crest), _crest, null)
        };
        
        return $"{_count}x{type}";
    }

    public static Crest getTypeFromLetter(string s) {
        return s switch {
            "s" => Crest.SUMMON,
            "r" => Crest.MOVEMENT,
            "a" => Crest.ATTACK,
            "d" => Crest.DEFENSE,
            "m" => Crest.MAGIC,
            "t" => Crest.TRAP,
            _ => throw new ArgumentOutOfRangeException(nameof(s), s, null)
        };
    }
}