using Microsoft.Xna.Framework;

namespace DiceGuardiansClient.Source.Pathfinding; 

public class Node {
    public Vector2 position;
    public int gCost;
    public int hCost;
    public int fCost {
        get {
            return gCost + hCost;
        }
    }
    public Node parent;

    public Node(Vector2 position, int gCost, int hCost, Node parent) {
        this.position = position;
        this.gCost = gCost;
        this.hCost = hCost;
        this.parent = parent;
    }
}