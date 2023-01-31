using System;
using System.Collections.Generic;
using DiceGuardiansClient.Source.Collection;
using DiceGuardiansClient.Source.Collisions;
using DiceGuardiansClient.Source.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DiceGuardiansClient.Source.World.Minions; 

public class Minion : Entity {
    private long _cardId;
    private int _maxHealth;
    private int _currentHealth;
    private int _attack;
    private int _defense;
    
    //movement
    private bool _moving;
    private List<Vector2> _path;
    private int _targetIndex;
    private const float VELOCITY = 0.005f;
    private Direction _direction;

    public Minion(Card card, Model m, Vector3 pos, Vector3 rot, float scale) : base(m, pos, rot, scale) {
        _cardId = card.GetCardId();
        _attack = card.GetAttack();
        _maxHealth = card.GetHealth();
        _currentHealth = card.GetHealth();
        _defense = card.GetDefense();
        
        _moving = false;
        _path = null;
        _targetIndex = 0;
        _direction = Direction.PLUS_X;
    }

    public void StartMove(List<Vector2> path) {
        _moving = true;
        _path = path;
        _targetIndex = 0;
        foreach (var p in _path) {
            Console.WriteLine(p.ToString());
        }
        Console.WriteLine();

        GetNewTarget();
    }

    private void GetNewTarget() {
        _targetIndex++;
        
        if (_targetIndex >= _path.Count) {
            _moving = false;
            return;
        }

        if (Position.X < _path[_targetIndex].X) {
            _direction = Direction.PLUS_X;
        } else if (Position.X > _path[_targetIndex].X) {
            _direction = Direction.MINUS_X;
        } else if (Position.Z < _path[_targetIndex].Y) {
            _direction = Direction.PLUS_Z;
        } else {
            _direction = Direction.MINUS_Z;
        }
    }

    public void Update(GameTime gameTime) {
        if (!_moving) {
            return;
        }

        Vector3 position;
        switch (_direction) {
            case Direction.PLUS_X:
                position = Position;
                position.X += VELOCITY * gameTime.ElapsedGameTime.Milliseconds;
                Position = position;
                if (Position.X > _path[_targetIndex].X) {
                    Position = new Vector3(_path[_targetIndex].X, 1, _path[_targetIndex].Y);
                    GetNewTarget();
                }
                break;
            case Direction.MINUS_X:
                position = Position;
                position.X -= VELOCITY * gameTime.ElapsedGameTime.Milliseconds;
                Position = position;
                if (Position.X < _path[_targetIndex].X) {
                    Position = new Vector3(_path[_targetIndex].X, 1, _path[_targetIndex].Y);
                    GetNewTarget();
                }
                break;
            case Direction.PLUS_Z:
                position = Position;
                position.Z += VELOCITY * gameTime.ElapsedGameTime.Milliseconds;
                Position = position;
                if (Position.Z > _path[_targetIndex].Y) {
                    Position = new Vector3(_path[_targetIndex].X, 1, _path[_targetIndex].Y);
                    GetNewTarget();
                }
                break;
            case Direction.MINUS_Z:
                position = Position;
                position.Z -= VELOCITY * gameTime.ElapsedGameTime.Milliseconds;
                Position = position;
                if (Position.Z < _path[_targetIndex].Y) {
                    Position = new Vector3(_path[_targetIndex].X, 1, _path[_targetIndex].Y);
                    GetNewTarget();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public AABB GetAABB() {
        return new AABB(new Vector3(Position.X - 0.3f, Position.Y - 1, Position.Z - 0.3f), new Vector3(0.6f, 1.2f, 0.6f));
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