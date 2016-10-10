using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;

public class LocalBuildingManager : MonoBehaviour
{
    public GameObject ButtonUnit;
    public int ID;

    private Object _buttonRecruitPrefab;
    public Buildings _thisBuilding;
    public string UnitToSpawn;

    //add currently spawninglist

    void Awake()
    {
        _buttonRecruitPrefab = AssetDatabase.LoadAssetAtPath("Assets/ButtonRecruitGroupped.prefab", typeof(GameObject));
    }

    void Start()
    {
        ID = GameObject.Find("Library").GetComponent<BuildingCreator>().BuiltCount;
        GameObject.Find("Library").GetComponent<BuildingCreator>().BuiltCount++;

        //_thisBuilding = GameObject.Find("Library").GetComponent<BuildingCreator>().ListOfBuildings.Find(x => x.GetName() == "Barracks");
        _thisBuilding = GameObject.Find("Library").GetComponent<BuildingCreator>().ListOfBuildings.Find(x => x.GetName() == GameObject.Find("Library").GetComponent<BuildingCreator>()._tmpClickedButtonName);
        //transform.GetComponentInChildren<TileInfo>().BuildingType
    }

    //add building state

    public void BuildingInfoToGUI()
    {
        GameObject.Find("TextBuildingName").GetComponent<Text>().text = transform.name + " with id " + ID;
        GameObject.Find("TextMenu").GetComponent<Text>().text = "Building Menu";
        GameObject.Find("PanelSelectedBuildingInfo").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("PanelSelectedUnitInfo").GetComponent<CanvasGroup>().alpha = 0;



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

        for (int i = 0; i < _thisBuilding.GetUnitList().Count; i++)
        {
            GameObject newButton = Instantiate(_buttonRecruitPrefab, transform.position, transform.rotation) as GameObject;
            newButton.transform.SetParent(GameObject.Find("PanelRecruitButtons").GetComponent<Transform>(), false);
            newButton.transform.name = "ButtonRecruitGroupped";
            Unit listItem = _thisBuilding.GetUnitList()[i];
            newButton.GetComponentInChildren<Text>().text = listItem.GetName();
            newButton.GetComponent<ButtonRecruitController>().MyBuildingID = ID;
        }
        GameObject.Find("PanelSelectedBuildingInfo").GetComponent<CanvasGroup>().alpha = 1;

    }


    ///If any upgrade feature comes, dont forget to update createdbuilding list in buildingcreator.
    public void SetCurrentBuildingForSpawn()
    {
        GameObject.Find("Library").GetComponent<UnitManager>().SpawnFromID = ID;
        GameObject.Find("Library").GetComponent<UnitManager>().SpawnFrom = _thisBuilding;
    }

    public void StartSpawning()
    {
        StartCoroutine(GameObject.Find("Library").GetComponent<UnitManager>().StartRecruitUnit(ID, UnitToSpawn));
    }



}
