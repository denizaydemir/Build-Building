using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MapCreateController : MonoBehaviour
{
    public GameObject EmpTile;
    public List<Tile> TileList;

    private int _tCount;

    private float _initPos = (float)15.5;

    void Awake()
    {
        GameObject.Find("PanelSelectedBuildingInfo").GetComponent<CanvasGroup>().alpha = 0;
        TileList = new List<Tile>();
        _tCount = 0;
        SpawnMap();
    }

    //void Start()
    //{
        
    //}

    void SpawnMap()
    {
        GameObject tileContainer = new GameObject();
        tileContainer.name = "TileContainer";


        float tmpPosX = _initPos;
        float tmpPosZ = -_initPos;
        for (int x = 0; x < 32; x++)
        {
            for (int z = 0; z < 32; z++)
            {
                //Object prefab = AssetDatabase.LoadAssetAtPath("Assets/something.prefab", typeof(GameObject));
                Vector3 pos = new Vector3(tmpPosX, 0, tmpPosZ);
                GameObject newTile = Instantiate(EmpTile, pos, Quaternion.identity) as GameObject;
                newTile.transform.name = _tCount.ToString();
                newTile.transform.parent = tileContainer.transform;
                SetTileIndex(newTile, x, z);
                newTile.GetComponent<TileInfo>().InitQ = _tCount;

                AddToTileList(_tCount, x, z);

                tmpPosZ++;
                _tCount++;
            }
            tmpPosZ = -_initPos;
            tmpPosX--;
        }
    }

    void SetTileIndex(GameObject tile, int x, int z)
    {
        tile.GetComponent<TileInfo>().PositionInfo.Insert(0, x);
        tile.GetComponent<TileInfo>().PositionInfo.Insert(1, z);
    }


    void AddToTileList(int index, int x, int y)
    {
        Tile tile = new Tile(index, x, y, true);
        TileList.Add(tile);
    }

}
