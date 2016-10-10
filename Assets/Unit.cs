using UnityEngine;
using System.Collections;

public class Unit
{
    private string _name;
    private float _health;
    private float _attack;
    private float _spawnTime;
    //Add speed

    public Unit(string _name, float _health, float _attack, float _spawnTime)
    {
        this._name = _name;
        this._health = _health;
        this._attack = _attack;
        this._spawnTime = _spawnTime;
    }

    public string GetName()
    {
        return _name;
    }

    public float GetHealth()
    {
        return _health;
    }

    public float GetAttack()
    {
        return _attack;
    }

    public float GetSpawnTime()
    {
        return _spawnTime;
    }

    //public void SetHealth(float newHealth)
    //{
    //    _health = newHealth;
    //}

    //public void SetAttack(float newAttack)
    //{
    //    _attack = newAttack;
    //}

    //public void SetSpawnTime(float newSpawnTime)
    //{
    //    _spawnTime = newSpawnTime;
    //}
}
