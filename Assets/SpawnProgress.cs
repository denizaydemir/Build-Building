using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnProgress : MonoBehaviour
{

    public Image ForegroundImage;
    public bool EnabledByOtherUnit;
    public float CurrentUnitSpawnTime;

    void Start()
    {
        EnabledByOtherUnit = false;
    }

    void Update()
    {
        if (EnabledByOtherUnit)
        {
            ForegroundImage.fillAmount = 0;
            EnabledByOtherUnit = false;
        }
        ForegroundImage.fillAmount += Time.deltaTime / CurrentUnitSpawnTime;
    }

    //get ongoing spawn times from localbuildingman list
    //Set fillamount accordingly
}