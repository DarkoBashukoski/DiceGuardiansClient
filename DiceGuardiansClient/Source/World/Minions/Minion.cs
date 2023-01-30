using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Collisions;
using DiceGuardiansClient.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.World.Minions; 

public class Minion : Entity { //TODO
    private long _cardId;
    private int _maxHealth;
    private int _currentHealth;
    private int _attack;
    private int _defense;
    private AABB _collisionBox;

    public Minion(Card card, Model m, Vector3 pos, Vector3 rot, float scale) : base(m, pos, rot, scale) {
        _cardId = card.GetCardId();
        _attack = card.GetAttack();
        _maxHealth = card.GetHealth();
        _currentHealth = card.GetHealth();
        _defense = card.GetDefense();
        _collisionBox = new AABB(new Vector3(pos.X - 0.3f, pos.Y-1, pos.Z - 0.3f), new Vector3(0.6f, 1.2f, 0.6f));
    }

    public AABB GetAABB() {
        return _collisionBox;
    }

    public Card GetCard() {
        return AllCards.GetCardData(_cardId);
    }

    public int GetAttack() {
        return _attack;
    }

    public int GetHealth() {
        return _currentHealth;
    }

    public int GetDefense() {
        return _defense;
    }
}