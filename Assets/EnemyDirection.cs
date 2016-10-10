using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDirection
{
    private int _index;
    private int _direction;

    public EnemyDirection(int _index, int _direction)
    {
        this._index = _index;
        this._direction = _direction;
    }


    public int Index
    {
        get
        {
            return _index;
        }

        set
        {
            _index = value;
        }
    }

    public int Direction
    {
        get
        {
            return _direction;
        }

        set
        {
            _direction = value;
        }
    }

    

    

}
