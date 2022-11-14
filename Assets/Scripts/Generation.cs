using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Generation : MonoBehaviour
{
    public                              List<GameObject> AllGroundBlock;
    public                              List<Transform> AllParalax;
    public                              List<GlobalObjects> MasterGen;
    public                              GameObject SaveHouse;
    [SerializeField] public             float[] ParalaxLayerSpeed;
    [SerializeField] private            Player Player;
    [SerializeField] private            Transform ObjectsParent;
    [SerializeField] private            GameManager GM;
    private                             float PSpeed;
    
    void Start()
    {
        MoveHouse(true);
        StartCoroutine(StartGame());
    }
    
    public IEnumerator StartGame()
    {
        for (int i = 0; i < MasterGen.Count; i++)
        {
            ObjectGen(MasterGen[i].GlobalData, MasterGen[i].GlobalSpawnedObjects);
            yield return new WaitForSeconds(Random.Range(1f,3f));
        }
    }

    public void DestroyAllSpawnedObject()
    {
        for (int i = 0; i < MasterGen.Count; i++)
        {
            MasterGen[i].GlobalSpawnedObjects.Clear();
        }
        for (int i = 0; i < ObjectsParent.transform.childCount; i++)
        {
            Destroy(ObjectsParent.transform.GetChild(i).gameObject);
        }
    }

    void ObjectGen(List<Data> Data, List<SpawnedObjects> AllData)
    {
        if(Data.Count == 0) return; 
        List<Data> NewData = new List<Data>();
        for (int i = 0; i < Data.Count; i++)
        {
            if(Data[i].Difficulty <= GM.Difficulty)
            {
                NewData.Add(Data[i]);
            }
        }
        if(NewData.Count == 0) return;
        int Rand = Random.Range(0, NewData.Count);
        var NewObj = Instantiate(NewData[Rand].Prefab, new Vector3(15, NewData[Rand].Prefab.transform.position.y, 0), Quaternion.identity);
        NewObj.transform.SetParent(ObjectsParent);
        NewObj.GetComponent<Move>().Initialization(GM.GameSpeed, NewData[Rand].MoveSpeed);
        if(NewData[Rand].RandomHeight)
        {
            NewObj.transform.position = new Vector3(15,Random.Range(-1f, 4f), 0);
        }
        /////////////////////////////////////
        SpawnedObjects NewSpawnObject = new SpawnedObjects();
        NewSpawnObject.Distance = Random.Range(NewData[Rand].MinDistanceSpawn,NewData[Rand].MaxDistanceSpawn);
        NewSpawnObject.Obj = NewObj;
        NewSpawnObject.SpawnPoint = NewObj.transform.position;
        AllData.Add(NewSpawnObject);
        /////////////////////////////////////
        if(NewData[Rand].FlipImageRandom)
        {
            float Flip = 0 == Random.Range(0,2) ? NewObj.transform.localScale.x : NewObj.transform.localScale.x * -1;
            NewObj.transform.localScale = new Vector3(Flip, NewObj.transform.localScale.y, NewObj.transform.localScale.z);
        }
    }
    
    void ObjectMove(List<Data> Obj, List<SpawnedObjects> SpawnedObjects)
    {
        if(SpawnedObjects.Count > 0)
        {
            for (int i = 0; i < SpawnedObjects.Count; i++)
            {
                if(Vector3.Distance(SpawnedObjects[i].Obj.transform.position, SpawnedObjects[i].SpawnPoint) > SpawnedObjects[i].Distance)
                {
                    if(!SpawnedObjects[i].Switch)
                    {
                        ObjectGen(Obj, SpawnedObjects);
                        SpawnedObjects[i].Switch = true;
                    }

                    if(SpawnedObjects[i].Obj.transform.position.x < -15f || SpawnedObjects[i].Obj.transform.position.y < -100f)
                    {
                        Destroy(SpawnedObjects[i].Obj);
                        SpawnedObjects.Remove(SpawnedObjects[i]);
                    }
                }
            }
        }
    }

    void BlockMove()
    {
        for (int i = 0; i < AllGroundBlock.Count; i++)
        {
            AllGroundBlock[i].transform.position -= Vector3.right * GM.GameSpeed * Time.deltaTime;
            if(AllGroundBlock[i].transform.position.x <= -12.5f)
            {
                float Diff = Mathf.Abs(AllGroundBlock[i].transform.position.x) - 12.5f;
                AllGroundBlock[i].transform.position = new Vector3(14.5f - Diff, -5.95f, 0);
            }
        }
    }

    public void MoveHouse(bool MoveOnStart)
    {
        SaveHouse.transform.position = MoveOnStart == true ? new Vector3(-10f, 1.17f, 0) : new Vector3(15f, 1.17f, 0);
        SaveHouse.SetActive(true);
    }

    void HouseMove()
    {
        if(SaveHouse.transform.position.x > -22.5f)
        {
            SaveHouse.transform.position -= Vector3.right * GM.GameSpeed * Time.deltaTime;
        }
        else
        {
            SaveHouse.SetActive(false);
        }
    }

    void Paralax()
    {
        for (int i = 0; i < AllParalax.Count; i++)
        {
            switch (AllParalax[i].tag)
            {
                case "Layer1" : PSpeed = ParalaxLayerSpeed[0]; break;
                case "Layer2" : PSpeed = ParalaxLayerSpeed[1]; break;
                case "Layer3" : PSpeed = ParalaxLayerSpeed[2]; break;
                case "Layer4" : PSpeed = ParalaxLayerSpeed[3]; break;
                default: break;
            }
            AllParalax[i].transform.position -= Vector3.right * PSpeed * GM.GameSpeed * Time.deltaTime;
            if(AllParalax[i].transform.localPosition.x <= -5.5f)
            {
                float Diff = Mathf.Abs(AllParalax[i].transform.localPosition.x) - 5.4f;
                AllParalax[i].transform.localPosition = new Vector3(10.84f - Diff, AllParalax[i].transform.localPosition.y, AllParalax[i].transform.localPosition.z);
            }
        }
    }
    void Update()
    {
        Player.SetSpeed(GM.GameSpeed);
        Paralax();
        HouseMove();
        BlockMove();
        for (int i = 0; i < MasterGen.Count; i++)
        {
            ObjectMove(MasterGen[i].GlobalData, MasterGen[i].GlobalSpawnedObjects);
        }
    }
}

[System.Serializable]
public class GlobalObjects
{
    public string Name;
    public List<Data> GlobalData;
    [HideInInspector] public List<SpawnedObjects> GlobalSpawnedObjects;
}

[System.Serializable]
public class Data 
{
    public GameObject Prefab;
    public bool FlipImageRandom;
    public bool RandomHeight;
    [Range(0.1f,2)] public float MoveSpeed = 1f;
    public float MinDistanceSpawn;
    public float MaxDistanceSpawn;
    public int Difficulty;
}

[System.Serializable]
public class SpawnedObjects
{
    public GameObject Obj;
    public Vector3 SpawnPoint;
    [HideInInspector] public float Distance;
    [HideInInspector] public bool Switch;
}