using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileInfo : MonoBehaviour
{
    public string BuildingType;
    public List<int> PositionInfo;
    public int InitQ;
    public bool isMoving;

    void Awake()
    {
        BuildingType = "arsa";
        isMoving = false;
    }

    void HitByRay()
    {
        GameObject.Find("Library").GetComponent<BuildingCreator>().currentRayHit = transform.name;
    }


    void OnMouseDown()
    {
        if (BuildingType != "arsa" && BuildingType != "unit")
        {
            //Get parent id here
            //or just run method from here via find
            transform.parent.GetComponent<LocalBuildingManager>().BuildingInfoToGUI();
        }
        else if (BuildingType == "unit" && BuildingType != "enemy")
        {
            //Add soldier info

            GameObject.Find("TextMenu").GetComponent<Text>().text = "Unit Info";
            GameObject.Find("TextUnitName").GetComponent<Text>().text = transform.parent.transform.name;
            GameObject.Find("PanelSelectedBuildingInfo").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("PanelSelectedUnitInfo").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("TextUnitHealth").GetComponent<Text>().text = "Health: " + transform.parent.GetComponent<LocalUnitManager>().Health.ToString();
            GameObject.Find("TextUnitAttack").GetComponent<Text>().text = "Attack Damage: " + transform.parent.GetComponent<LocalUnitManager>().Attack.ToString();


            GameObject.Find("Library").GetComponent<UnitPathFind>().GetUnitLocation(PositionInfo, transform.parent.transform.name);
            GameObject.Find("Library").GetComponent<UnitPathFind>().UnitSelected = true;
        }

        if (BuildingType == "arsa" && GameObject.Find("Library").GetComponent<UnitPathFind>().UnitSelected && BuildingType != "enemy")
        {
            if (GameObject.Find("Library").GetComponent<BuildingCreator>()._tileList[InitQ].isEmpty() && !isMoving)
            {
                GameObject.Find("Library").GetComponent<UnitPathFind>().GetDestinationLocation(PositionInfo);
            }
        }


    }



}
