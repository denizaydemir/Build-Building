using UnityEngine;
using System.Collections;

public class Tile
{
    private int _index;
    private int _x;
    private int _y;
    private bool _isEmpty;

    public Tile(int _index, int _x, int _y, bool _isEmpty)
    {
        this._index = _index;
        this._x = _x;
        this._y = _y;
        this._isEmpty = _isEmpty;
    }

    public int GetIndex()
    {
        return _index;
    }

    public int GetX()
    {
        return _x;
    }

    public int GetY()
    {
        return _y;
    }
    public bool isEmpty()
    {
        return _isEmpty;
    }

    public void SetFull()
    {
        _isEmpty = false;
    }

    public void SetEmpty()
    {
        _isEmpty = true;
    }
}
