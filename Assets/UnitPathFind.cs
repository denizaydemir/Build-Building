using UnityEngine;
using System.Collections;
using System.Drawing;
using System;
using System.Collections.Generic;

public class UnitPathFind : MonoBehaviour
{
    private bool[,] map;
    private SearchParameters searchParameters;
    private int _unitInitialX;
    private int _unitInitialY;
    private int _unitDestinationX;
    private int _unitDestinationY;
    private SearchParameters _unitCurrentSearchParameters;

    public string SelectedUnitName;
    public bool UnitSelected;

    public Color32 Green;
    public Color32 Red;


    void Start()
    {
        UnitSelected = false;
        _unitInitialX = -1;
        _unitInitialY = -1;
        _unitDestinationX = -1;
        _unitDestinationY = -1;
        Green = new Color32(29, 244, 0, 255);
        Red = new Color32(255, 0, 0, 255);
    }


    private void InitializeMap()
    {

        this.map = new bool[32, 32];
        int listIndex = 0;

        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 32; y++)
            {
                map[x, y] = transform.GetComponent<BuildingCreator>()._tileList[listIndex].isEmpty();
                listIndex++;
            }
        }

        var startLocation = new Point(_unitInitialX, _unitInitialY);

        var endLocation = new Point(_unitDestinationX, _unitDestinationY);
        _unitCurrentSearchParameters = new SearchParameters(startLocation, endLocation, map);
    }

    public void GetUnitLocation(List<int> position, string unitName)
    {
        _unitInitialX = position[0];
        _unitInitialY = position[1];
        SelectedUnitName = unitName;
    }

    public void GetDestinationLocation(List<int> position)
    {
        _unitDestinationX = position[0];
        _unitDestinationY = position[1];

        UnitFindPath();
    }

    public void UnitFindPath()
    {
        InitializeMap();
        PathFinder pathFinder = new PathFinder(_unitCurrentSearchParameters);
        List<Point> path = pathFinder.FindPath();


        ////For visualization
        //Color32 black = new Color32(0, 0, 0, 255);
        //for (int i = 0; i < path.Count; i++)
        //{
        //    int tileIndex = path[i].X * 32 + path[i].Y;
        //    GameObject.Find(tileIndex.ToString()).GetComponent<Renderer>().material.color = black;
        //}
        ////

        int lastTileIndex = path[path.Count - 1].X * 32 + path[path.Count - 1].Y;
        int tileIndex = _unitInitialX * 32 + _unitInitialY;
        try
        {
            if (GameObject.Find("Library").GetComponent<BuildingCreator>()._tileList[lastTileIndex].isEmpty() && !GameObject.Find(tileIndex.ToString()).GetComponent<Transform>().parent.GetComponent<LocalUnitManager>().isMoving)
            {
                GameObject.Find(tileIndex.ToString()).GetComponent<Transform>().parent.GetComponent<LocalUnitManager>().isMoving = true;
                StartCoroutine(UnitMove(path));

            }
        }
        catch (Exception)
        {

            //throw;
        }
        


        _unitCurrentSearchParameters = null;
        UnitSelected = false;
    }

    public IEnumerator UnitMove(List<Point> path)
    {
        string curUnit = SelectedUnitName;
        int lastTileIndex = path[path.Count - 1].X * 32 + path[path.Count - 1].Y;
        GameObject.Find("Library").GetComponent<BuildingCreator>()._tileList[lastTileIndex].SetFull();

        int tileIndex = _unitInitialX * 32 + _unitInitialY;


        for (int i = 0; i < path.Count; i++)
        {
            GameObject.Find(tileIndex.ToString()).GetComponent<TileInfo>().BuildingType = "arsa";
            GameObject.Find(tileIndex.ToString()).GetComponent<Renderer>().material.color = Green;
            GameObject.Find(tileIndex.ToString()).GetComponent<Transform>().parent = GameObject.Find("TileContainer").GetComponent<Transform>();
            GameObject.Find("Library").GetComponent<BuildingCreator>()._tileList[tileIndex].SetEmpty();

            tileIndex = path[i].X * 32 + path[i].Y;
            GameObject.Find(tileIndex.ToString()).GetComponent<TileInfo>().BuildingType = "unit";
            GameObject.Find(tileIndex.ToString()).GetComponent<Renderer>().material.color = Red;
            GameObject.Find(tileIndex.ToString()).GetComponent<Transform>().parent = GameObject.Find(curUnit).GetComponent<Transform>();
            GameObject.Find("Library").GetComponent<BuildingCreator>()._tileList[tileIndex].SetFull();

            yield return new WaitForSeconds((float)0.2);

        }
        GameObject.Find(tileIndex.ToString()).GetComponent<Transform>().parent.GetComponent<LocalUnitManager>().isMoving = false;

    }


}
