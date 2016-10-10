using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildingCreator : MonoBehaviour
{
    public List<Buildings> ListOfBuildings;
    public List<string> ListOfCreatedBuildingString;
    public List<int> ListOfCreatedBuildingIndex;
    public /*static*/ List<Tile> _tileList;
    //static is better, singleton is much better.
    public GameObject BuildingPrefab;
    public bool _isRayOn;
    public string currentRayHit;
    public string _tmpClickedButtonName;
    public int BuiltCount;
    public Color32 green;
    public Color32 yellow;
    public Color32 gray;



    private List<int> _tmpBuildSize;
    private List<int> tmpIndexListForColor;
    private int layerMask = 1 << 8;
    private bool _tmpIsEmpty;




    void Awake()
    {
        ListOfBuildings = new List<Buildings>();
        AddBuildingToListWithDefaultSoldiers("Barracks", 3, 3);
        AddBuildingToListWithDefaultSoldiers("Cave", 1, 1);
        AddBuildingToListWithDefaultSoldiers("Archery", 1, 2);
        AddBuildingToListWithDefaultSoldiers("Cami", 2, 2);
        AddBuildingToListWithDefaultSoldiers("Sanayi", 5, 5);
        AddBuildingToListWithDefaultSoldiers("Otopark", 7, 7);

        //List<Buildings> ListOfCreatedBuildings = new List<Buildings>();
        ListOfCreatedBuildingIndex = new List<int>();

        BuiltCount = 0;
        currentRayHit = "";
        _tmpClickedButtonName = "";
    }

    ///If wanted to change/add defaults, change/add inside if statements
    public void AddBuildingToListWithDefaultSoldiers(string name, int sizeX, int sizeY)
    {
        List<Unit> defaultUnits = new List<Unit>();
        if (name == "Barracks")
        {
            defaultUnits.Add(transform.GetComponent<UnitManager>().UnitList.Find(x => x.GetName() == "Pikeman"));
            defaultUnits.Add(transform.GetComponent<UnitManager>().UnitList.Find(x => x.GetName() == "Archer"));
        }
        else if (name == "Cave")
        {
            defaultUnits.Add(transform.GetComponent<UnitManager>().UnitList.Find(x => x.GetName() == "Homo Erectus"));
        }
        else if (name == "Archery")
        {
            defaultUnits.Add(transform.GetComponent<UnitManager>().UnitList.Find(x => x.GetName() == "Archer"));
        }
        else if (name == "Cami")
        {
            defaultUnits.Add(transform.GetComponent<UnitManager>().UnitList.Find(x => x.GetName() == "Feto"));
        }
        else if (name == "Sanayi")
        {
            defaultUnits.Add(transform.GetComponent<UnitManager>().UnitList.Find(x => x.GetName() == "Tank"));
        }
        else if (name == "Otopark")
        {
            defaultUnits.Add(transform.GetComponent<UnitManager>().UnitList.Find(x => x.GetName() == "Mafia"));
        }

        Buildings newBuilding = new Buildings(name, sizeX, sizeY, defaultUnits);
        ListOfBuildings.Add(newBuilding);
    }



    public void StartRaycastFromMouse()
    {
        if (!_isRayOn)
        {
            _isRayOn = true;
            Debug.Log("RayStarted");
        }
    }

    void Start()
    {
        _isRayOn = false;
        _tmpIsEmpty = false;
        _tmpBuildSize = new List<int>();
        _tileList = transform.GetComponent<MapCreateController>().TileList;
        green = new Color32(29, 244, 0, 255);
        yellow = new Color32(255, 216, 0, 255);
        gray = new Color32(131, 156, 92, 255);
        tmpIndexListForColor = new List<int>();

    }


    void Update()
    {

        if (_isRayOn)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200, layerMask))
            {
                if (hit.transform.GetComponent<TileInfo>().BuildingType == "arsa")
                {
                    if (currentRayHit != hit.transform.name)
                    {
                        ClearListAndColor();


                        int sizeX = _tmpBuildSize[0];
                        int sizeY = _tmpBuildSize[1];

                        int tilePosX = hit.transform.GetComponent<TileInfo>().PositionInfo[0];
                        int tilePosY = hit.transform.GetComponent<TileInfo>().PositionInfo[1];

                        int curIndex = hit.transform.GetComponent<TileInfo>().InitQ;

                        int countEmpty = 0;

                        if (tilePosX > sizeX - 2 && tilePosY > sizeY - 2)
                        {


                            for (int i = 0; i < sizeX; i++)
                            {
                                for (int j = 0; j < sizeY; j++)
                                {
                                    if (_tileList[curIndex].isEmpty() && (_tileList[curIndex].GetX() < 31 && _tileList[curIndex].GetX() >= sizeX - i ) &&
                                        (_tileList[curIndex].GetY() < 31 && _tileList[curIndex].GetY() >= sizeY - j ) && _tileList[curIndex].isEmpty())
                                    {
                                        GameObject.Find(curIndex.ToString()).GetComponent<Renderer>().material.color = yellow;
                                        tmpIndexListForColor.Add(curIndex);
                                        countEmpty++;
                                    }
                                    else
                                    {
                                        countEmpty = 0;
                                    }


                                    tilePosY--;
                                    curIndex--;

                                }
                                tilePosX -= i;
                                curIndex = curIndex - 32 + sizeY;
                            }
                        }

                        if (sizeX * sizeY == countEmpty)
                        {
                            _tmpIsEmpty = true;
                        }
                        else
                        {
                            _tmpIsEmpty = false;
                        }

                        if (_tmpIsEmpty && Input.GetMouseButtonUp(0))
                        {
                            string newBuildingName = _tmpClickedButtonName;

                            GameObject newBuildingParent = Instantiate(BuildingPrefab, transform.position, transform.rotation) as GameObject;
                            newBuildingParent.transform.parent = GameObject.Find("TileContainer").transform;
                            newBuildingParent.transform.name = newBuildingName + BuiltCount.ToString();

                            for (int i = 0; i < tmpIndexListForColor.Count; i++)
                            {
                                GameObject.Find(tmpIndexListForColor[i].ToString()).GetComponent<TileInfo>().BuildingType = _tmpClickedButtonName;
                                _tileList[tmpIndexListForColor[i]].SetFull();
                                GameObject.Find(tmpIndexListForColor[i].ToString()).GetComponent<Renderer>().material.color = gray;
                                GameObject.Find(tmpIndexListForColor[i].ToString()).GetComponent<Transform>().parent = newBuildingParent.transform;
                            }
                            ListOfCreatedBuildingIndex.Add(tmpIndexListForColor[0]);
                            ListOfCreatedBuildingString.Add(newBuildingName);

                        }
                    }

                }
                else
                {
                    //Mouse hovering on building while ray is on
                    //Debug.Log("Mouse on tile no"+hit.transform.name);

                }
                //hit.transform.GetComponent<TileInfo>().PositionInfo[0]
                //Debug.Log("x index: " + hit.transform.GetComponent<TileInfo>().PositionInfo[0]);
                //Debug.Log(hit.transform.name);
            }

        }

        if (Input.GetKeyDown("escape"))
        {
            _isRayOn = false;
            ClearListAndColor();
            _tmpClickedButtonName = "";
            try
            {
                foreach (Transform child in GameObject.Find("PanelRecruitButtons").transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            catch (System.Exception)
            {
                //throw;
            }

            GameObject.Find("Library").GetComponent<UnitPathFind>().UnitSelected = false;

            GameObject.Find("PanelSelectedBuildingInfo").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("PanelSelectedUnitInfo").GetComponent<CanvasGroup>().alpha = 0;
        }

    }


    public void ClearListAndColor()
    {
        for (int i = 0; i < tmpIndexListForColor.Count; i++)
        {
            if (_tileList[tmpIndexListForColor[i]].isEmpty())
            {
                GameObject.Find(tmpIndexListForColor[i].ToString()).GetComponent<Renderer>().material.color = green;
            }
        }
        tmpIndexListForColor.Clear();
    }


    public void GetClickedBuildingSizeFromList(string name)
    {
        _tmpBuildSize.Clear();
        _tmpBuildSize.Add(ListOfBuildings.Find(x => x.GetName() == name).GetSizeX());
        _tmpBuildSize.Add(ListOfBuildings.Find(x => x.GetName() == name).GetSizeY());
        _tmpClickedButtonName = name;

    }

}
