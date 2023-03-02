namespace DiceGuardiansClient; 

public static class DrawCounter {
    private static int total = 0;
    private static int current = 0;
    
    public static void Update() {
        total = 0;
        current = 0;
    }

    public static void AddTotal() {
        total++;
    }

    public static void AddCurrent() {
        current++;
    }

    public static int GetTotal() {
        return total;
    }

    public static int GetCurrent() {
        return current;
    }
}