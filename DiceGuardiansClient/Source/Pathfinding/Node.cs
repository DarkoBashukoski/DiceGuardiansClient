using Microsoft.Xna.Framework;

namespace DiceGuardiansClient.Source.Pathfinding; 

public class Node {
    public Vector2 position;
    public readonly int gCost;
    private readonly int hCost;
    public int fCost => gCost + hCost;
    public readonly Node parent;

    public Node(Vector2 position, int gCost, int hCost, Node parent) {
        this.position = position;
        this.gCost = gCost;
        this.hCost = hCost;
        this.parent = parent;
    }
}