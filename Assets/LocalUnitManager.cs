using UnityEngine;
using System.Collections;

public class LocalUnitManager : MonoBehaviour
{

    public string Name;
    public float Health;
    public float Attack;

    public bool isMoving;



    void Start()
    {
        isMoving = false;
    }

}
