using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildingsToGUI : MonoBehaviour
{
    private List<Buildings> _listOfBuildings;
    public GameObject BuildingButtonPrefab;

    void Start()
    {
        _listOfBuildings = GameObject.Find("Library").GetComponent<BuildingCreator>().ListOfBuildings;
        AddListToGUI(_listOfBuildings);
    }


    private void AddListToGUI(List<Buildings> bList)
    {
        for (int i = 0; i < _listOfBuildings.Count; i++)
        {
            GameObject newButton = Instantiate(BuildingButtonPrefab, transform.position, transform.rotation) as GameObject;
            newButton.transform.SetParent(GameObject.Find("PanelBuildingButtons").GetComponent<Transform>(), false);
            newButton.transform.name = "ButtonBuildingGroupped";
            Buildings listItem = _listOfBuildings[i];
            newButton.GetComponentInChildren<Text>().text = listItem.GetName();
        }
    }

}
