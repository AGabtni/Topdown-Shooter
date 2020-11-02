using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] GameObject pooledObjectPrefab;
    public int pooledAmount;
    private List<GameObject> pooledInstances;

    void Start()
    {
        pooledInstances = new List<GameObject>();

        

        //Fill the pooler with the a copy of each obstacle
        for (int i = 0; i < pooledAmount; i++)
        {


            GameObject obj = (GameObject)Instantiate(pooledObjectPrefab);
            obj.SetActive(false);
            pooledInstances.Add(obj);
        }



    }



    public GameObject GetPooledObject()
    {


        //Search for an available obstacle to respawn
        for (int k = 0; k < pooledInstances.Count; k++)
        {

            if (!pooledInstances[k].activeSelf)
            {

                return pooledInstances[k];
            }

        }


        //Else create a new one and add it to the pooler
        GameObject newObj = (GameObject)Instantiate(pooledObjectPrefab);
        newObj.SetActive(false);
        pooledInstances.Add(newObj);

        return newObj;
    }




}
