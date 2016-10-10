using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ButtonBuildingController : MonoBehaviour
{
    private Button _thisButton;

    void Awake()
    {
        _thisButton = gameObject.GetComponent<Button>();
    }

    void Start()
    {
            _thisButton.onClick.AddListener(() =>
            { 
                GameObject.Find("Library").GetComponent<BuildingCreator>().StartRaycastFromMouse();
                GameObject.Find("Library").GetComponent<BuildingCreator>().GetClickedBuildingSizeFromList(gameObject.transform.GetComponentInChildren<Text>().text);
            });
        
    }


}
