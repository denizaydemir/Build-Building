using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Buildings
{
    private string name;
    private int sizeX;
    private int sizeY;
    private List<Unit> unitList;


    public Buildings(string name, int sizeX, int sizeY, List<Unit> unitList)
    {
        this.name = name;
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.unitList = unitList;
    }

    public string GetName()
    {
        return name;
    }

    public int GetSizeX()
    {
        return sizeX;
    }

    public int GetSizeY()
    {
        return sizeY;
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }
}
