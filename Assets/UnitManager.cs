using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;


public class UnitManager : MonoBehaviour
{
    public GameObject UnitPrefab;
    public GameObject EnemyPrefab;
    public List<Unit> UnitList;
    public List<EnemyDirection> SpawnableEnemyPositionIndexes;
    public Buildings SpawnFrom;
    public int SpawnFromID;
    public Color32 Red;
    public Color32 Blue;
    public int SpawnCounter;
    public string CurrentSpawningUnit;
    public int EnemyCounter;

    public bool IsWaitingEnemySpawn;

    void Awake()
    {
        UnitList = new List<Unit>();
        AddUnitToList("Pikeman", 10, 8, 5);
        AddUnitToList("Homo Erectus", 2, 1, 1);
        AddUnitToList("Archer", 6, 4, 1);
        AddUnitToList("Feto", 15, 12, 10);
        AddUnitToList("Tank", 18, 15, 15);
        AddUnitToList("Mafia", 12, 10, 8);
        IsWaitingEnemySpawn = false;
    }


    void Start()
    {
        Red = new Color32(255, 0, 0, 255);
        Blue = new Color32(0, 55, 255, 255);
        SpawnCounter = -1;
        EnemyCounter = 0;
        CurrentSpawningUnit = "";
        SpawnableEnemyPositionIndexes = new List<EnemyDirection>();
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {

                if (i == 0 || j == 0 || i == 32 - 1 || j == 32 - 1)
                {
                    EnemyDirection enemyInfo;
                    if (i == 0)
                    {
                        enemyInfo = new EnemyDirection(i * 32 + j, 1);
                        SpawnableEnemyPositionIndexes.Add(enemyInfo);
                        //GameObject.Find(SpawnableEnemyPositionIndexes[SpawnableEnemyPositionIndexes.Count-1].ToString()).GetComponent<Renderer>().material.color = Red;
                    }
                    else if (j == 0)
                    {
                        enemyInfo = new EnemyDirection(i * 32 + j, 2);
                        SpawnableEnemyPositionIndexes.Add(enemyInfo);
                    }
                    else if (i == 32 - 1)
                    {
                        enemyInfo = new EnemyDirection(i * 32 + j, 3);
                        SpawnableEnemyPositionIndexes.Add(enemyInfo);
                    }
                    else if (j == 32 - 1)
                    {
                        enemyInfo = new EnemyDirection(i * 32 + j, 4);
                        SpawnableEnemyPositionIndexes.Add(enemyInfo);
                    }
                }
            }
        }

    }


    void Update()
    {
        if (!IsWaitingEnemySpawn)
        {
            float randomTimeToSpawn = UnityEngine.Random.Range((float)3, (float)6);
            IsWaitingEnemySpawn = true;
            StartCoroutine(SpawnEnemy(randomTimeToSpawn));
        }
    }


    public void AddUnitToList(string name, float health, float attack, float spawnTime)
    {
        Unit newUnit = new Unit(name, health, attack, spawnTime);
        UnitList.Add(newUnit);
    }


    public IEnumerator StartRecruitUnit(int buildingID, string unitName)
    {
        CurrentSpawningUnit = unitName;
        int curBuildPositionIndex = GameObject.Find("Library").GetComponent<BuildingCreator>().ListOfCreatedBuildingIndex[buildingID];
        float tmpSpawnTime = SpawnFrom.GetUnitList().Find(x => x.GetName() == unitName).GetSpawnTime();
        GameObject.Find("ImageUnitSpawnForeground").GetComponent<SpawnProgress>().CurrentUnitSpawnTime = tmpSpawnTime;

        yield return new WaitForSeconds(tmpSpawnTime);

        SpawnUnit(buildingID, curBuildPositionIndex, unitName);
        GameObject.Find("ImageUnitSpawnForeground").GetComponent<SpawnProgress>().enabled = false;
    }

    public void SpawnUnit(int buildingID, int curBuildPositionIndex, string unitName)
    {
        bool isSpawned = false;
        string buildToSpawn = transform.GetComponent<BuildingCreator>().ListOfCreatedBuildingString[buildingID];
        int buildingSizeX = transform.GetComponent<BuildingCreator>().ListOfBuildings.Find(x => x.GetName() == buildToSpawn).GetSizeX();
        int buildingSizeY = transform.GetComponent<BuildingCreator>().ListOfBuildings.Find(x => x.GetName() == buildToSpawn).GetSizeY();

        int tmpIndex = curBuildPositionIndex;

        List<int> boundaryIndexList = new List<int>();


        for (int i = 0; i < buildingSizeX; i++)
        {
            for (int j = 0; j < buildingSizeY; j++)
            {

                if ((i == 0 || j == 0 || i == buildingSizeX - 1 || j == buildingSizeY - 1) && !isSpawned)
                {
                    boundaryIndexList.Add(GameObject.Find(tmpIndex.ToString()).GetComponent<TileInfo>().InitQ);
                }

                tmpIndex--;
            }
            tmpIndex = tmpIndex - 32 + buildingSizeY;
        }

        //foreach (int item in boundaryIndexList)
        //{
        //    GameObject.Find(item.ToString()).GetComponent<Renderer>().material.color = Red;
        //}
        //Boundary for debug


        //Check map boundary to spawn


        int tmpIndexForBoundary;
        foreach (int index in boundaryIndexList)
        {
            tmpIndexForBoundary = index;
            tmpIndexForBoundary = tmpIndexForBoundary + 33;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GameObject.Find(tmpIndexForBoundary.ToString()).GetComponent<TileInfo>().BuildingType == "arsa")
                    {
                        SpawnCounter++;

                        GameObject.Find(tmpIndexForBoundary.ToString()).GetComponent<TileInfo>().BuildingType = "unit";
                        GameObject.Find(tmpIndexForBoundary.ToString()).GetComponent<Renderer>().material.color = Red;


                        GameObject unitParent = Instantiate(UnitPrefab, transform.position, transform.rotation) as GameObject;
                        unitParent.transform.parent = GameObject.Find("TileContainer").transform;
                        unitParent.transform.name = unitName + SpawnCounter.ToString();
                        GameObject.Find(tmpIndexForBoundary.ToString()).transform.parent = unitParent.transform;
                        transform.GetComponent<BuildingCreator>()._tileList[tmpIndexForBoundary].SetFull();
                        string buildingNameToSpawn = GameObject.Find("Library").GetComponent<BuildingCreator>().ListOfCreatedBuildingString[SpawnFromID] + SpawnFromID.ToString();
                        //Buildings currentBuilding = GameObject.Find(buildingNameToSpawn).GetComponent<LocalBuildingManager>()._thisBuilding;
                        Unit curSpawningUnit = GameObject.Find(buildingNameToSpawn).GetComponent<LocalBuildingManager>()._thisBuilding.GetUnitList().Find(x => x.GetName() == unitName);


                        float attack = curSpawningUnit.GetAttack();
                        float health = curSpawningUnit.GetHealth();
                        unitParent.GetComponent<LocalUnitManager>().Attack = attack;
                        unitParent.GetComponent<LocalUnitManager>().Health = health;
                        unitParent.GetComponent<LocalUnitManager>().Name = unitParent.transform.name;
                        //unitParent.GetComponent<LocalUnitManager>().MotherBuilding = currentBuilding;



                        isSpawned = true;
                        break;
                    }
                    tmpIndexForBoundary--;

                }
                tmpIndexForBoundary = tmpIndexForBoundary - 32 + 3;
                if (isSpawned)
                {
                    break;
                }
            }
            if (isSpawned)
            {
                break;
            }
        }

        CurrentSpawningUnit = "";
        SpawnFrom = null;
    }

    public IEnumerator SpawnEnemy(float enemySpawnTime)
    {
        //Add to spawned enemy list
        yield return new WaitForSeconds(enemySpawnTime);
        int listIndexForSpawn = UnityEngine.Random.Range(0, SpawnableEnemyPositionIndexes.Count-1);
        int tileIndexForSpawn = SpawnableEnemyPositionIndexes[listIndexForSpawn].Index;

        GameObject newEnemy = Instantiate(EnemyPrefab, transform.position, transform.rotation) as GameObject;
        newEnemy.transform.parent = GameObject.Find("TileContainer").transform;
        newEnemy.transform.name = "Enemy" + EnemyCounter.ToString();

        GameObject.Find(tileIndexForSpawn.ToString()).transform.parent = newEnemy.transform;
        GameObject.Find(tileIndexForSpawn.ToString()).GetComponent<TileInfo>().BuildingType = "enemy";
        GameObject.Find(tileIndexForSpawn.ToString()).GetComponent<Renderer>().material.color = Blue;
        transform.GetComponent<BuildingCreator>()._tileList[tileIndexForSpawn].SetFull();
        //add mechanics
        newEnemy.GetComponent<LocalEnemyManager>().Direction = SpawnableEnemyPositionIndexes[listIndexForSpawn].Direction;


        EnemyCounter++;
        IsWaitingEnemySpawn = false;
    }


}
