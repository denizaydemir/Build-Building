using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalEnemyManager : MonoBehaviour
{

    public string Name;
    public float Health;
    public float Attack;

    public bool isMoving;
    public bool isInCombat;
    public int Direction;


    private Color32 Green;
    private Color32 Blue;
    //public Buildings MotherBuilding;
    private int _directionIndex;

    private int _curTileIndex;

    private Transform _transform;
    private GameObject _frontTile;

    void Awake()
    {
        _transform = gameObject.GetComponent<Transform>();
    }

    void Start()
    {
        Blue = new Color32(0, 55, 255, 255);
        Green = new Color32(29, 244, 0, 255);

        isMoving = false;
        isInCombat = false;
        //Default enemy
        Health = 4;
        Attack = 4;
        //
        _curTileIndex = transform.GetComponentInChildren<TileInfo>().InitQ;

        if (Direction == 1)
        {
            _directionIndex = 32;
        }
        else if (Direction == 2)
        {
            _directionIndex = 1;
        }
        else if (Direction == 3)
        {
            _directionIndex = -32;
        }
        else if (Direction == 4)
        {
            _directionIndex = -1;
        }
    }

    //event delegate
    void Update()
    {
        if (CanAttackFront())
        {
            AttackFront();
        }
        else if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(EnemyMove());
        }
    }

    public IEnumerator EnemyMove()
    {
        yield return new WaitForSeconds((float)1);

        _transform.GetComponentInChildren<TileInfo>().BuildingType = "arsa";
        _transform.GetComponentInChildren<Renderer>().material.color = Green;
        GameObject.Find(_curTileIndex.ToString()).GetComponent<Transform>().parent = GameObject.Find("TileContainer").GetComponent<Transform>();
        //_transform.GetComponentInChildren<Transform>().parent = GameObject.Find("TileContainer").GetComponent<Transform>();

        GameObject.Find("Library").GetComponent<BuildingCreator>()._tileList[_curTileIndex].SetEmpty();
        //BuildingCreator._tileList[_curTileIndex].SetEmpty();

        _curTileIndex += _directionIndex;
        if (_curTileIndex >= 1023)
        {
            _curTileIndex = _curTileIndex % 1023;
        }
        GameObject nextTile = GameObject.Find(_curTileIndex.ToString());


        nextTile.transform.parent = transform;
        nextTile.GetComponent<TileInfo>().BuildingType = "enemy";
        nextTile.GetComponent<Renderer>().material.color = Blue;
        GameObject.Find("Library").GetComponent<BuildingCreator>()._tileList[_curTileIndex].SetFull();
        _transform = nextTile.transform;

        isMoving = false;
    }

    public bool CanAttackFront()
    {

        _frontTile = GameObject.Find((_curTileIndex + _directionIndex).ToString());

        if (_frontTile.GetComponent<TileInfo>().BuildingType != "enemy" && _frontTile.GetComponent<TileInfo>().BuildingType != "arsa")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AttackFront()
    {
        isInCombat = false;
        isMoving = true;
        if (_frontTile.GetComponent<TileInfo>().BuildingType == "unit")
        {
            float unitHealth = _frontTile.transform.parent.transform.GetComponent<LocalUnitManager>().Health;
            float unitAttack = _frontTile.transform.parent.transform.GetComponent<LocalUnitManager>().Attack;

            unitHealth -= Attack;
            Health -= unitAttack;

            if (unitHealth <= 0)
            {
                _frontTile.GetComponent<TileInfo>().BuildingType = "arsa";
                _frontTile.GetComponent<Renderer>().material.color = Green;
                _frontTile.GetComponent<Transform>().parent = GameObject.Find("TileContainer").GetComponent<Transform>();
                //GameObject.Find(_frontTile.ToString()).GetComponent<Transform>().parent = GameObject.Find("TileContainer").GetComponent<Transform>();
                //Destroy(_frontTile);
            }


            if (Health <= 0)
            {
                transform.GetComponentInChildren<TileInfo>().BuildingType = "arsa";
                transform.GetComponentInChildren<Renderer>().material.color = Green;
                transform.GetComponentInChildren<Transform>().parent = GameObject.Find("TileContainer").GetComponent<Transform>();
                //Destroy(gameObject);

            }
            else
            {
                isInCombat = false;
                isMoving = false;
            }

        }
        else
        {
            //destroy etc
            transform.GetComponentInChildren<TileInfo>().BuildingType = "arsa";
            transform.GetComponentInChildren<Renderer>().material.color = Green;
            transform.GetComponentInChildren<Transform>().parent = GameObject.Find("TileContainer").GetComponent<Transform>();
            _frontTile.GetComponent<TileInfo>().BuildingType = "arsa";
            _frontTile.GetComponent<Renderer>().material.color = Green;
            _frontTile.GetComponent<Transform>().parent = GameObject.Find("TileContainer").GetComponent<Transform>();
        }
    }

}
