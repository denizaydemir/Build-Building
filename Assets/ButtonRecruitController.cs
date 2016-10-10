using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ButtonRecruitController : MonoBehaviour
{
    private Button _thisButton;
    public int MyBuildingID;
    private string _myBuildingName;

    void Awake()
    {
        _thisButton = gameObject.GetComponent<Button>();
    }

    void Start()
    {
        _myBuildingName = GameObject.Find("Library").GetComponent<BuildingCreator>().ListOfCreatedBuildingString[MyBuildingID] + MyBuildingID.ToString();
        string myUnitName = transform.GetComponentInChildren<Text>().text;

        _thisButton.onClick.AddListener(() =>
        {

            GameObject.Find(_myBuildingName).GetComponent<LocalBuildingManager>().SetCurrentBuildingForSpawn();
            GameObject.Find(_myBuildingName).GetComponent<LocalBuildingManager>().UnitToSpawn = myUnitName;



            GameObject.Find("ImageUnitSpawnForeground").GetComponent<SpawnProgress>().enabled = true;
            GameObject.Find("ImageUnitSpawnForeground").GetComponent<SpawnProgress>().EnabledByOtherUnit = true;
            GameObject.Find("TextCurrentSpawningUnit").GetComponent<Text>().text = "Currently recruiting: \n" + myUnitName;

            GameObject.Find(_myBuildingName).GetComponent<LocalBuildingManager>().StartSpawning();
        });
    }

}
