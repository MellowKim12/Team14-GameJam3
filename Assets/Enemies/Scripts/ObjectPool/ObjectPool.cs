using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private PoolableObject prefab;
    private List<PoolableObject> AvailableObjects;

    private ObjectPool(PoolableObject prefab, int size)
    {
        this.prefab = prefab;
        AvailableObjects = new List<PoolableObject>(size);
    }

    public static ObjectPool CreateInstance(PoolableObject prefab, int size)
    {
        ObjectPool pool = new ObjectPool(prefab, size);

        GameObject poolObject = new GameObject(prefab.name + " Pool");
        pool.CreateObjects(poolObject.transform, size);

        return pool;
    }

    private void CreateObjects(Transform parent, int size)
    {
        for (int i = 0; i < size; i++)
        {
            PoolableObject poolableObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObject.Parent = this;
            poolableObject.gameObject.SetActive(false);
        }
    }
    
    public void ReturnObjectToPool(PoolableObject poolableObject)
    {
        AvailableObjects.Add(poolableObject);
    }


    public PoolableObject GetObject()
    {
        if (AvailableObjects.Count > 0)
        {
            PoolableObject instance = AvailableObjects[0];
            AvailableObjects.RemoveAt(0);

            instance.gameObject.SetActive(true);

            return instance;
        }
        else
        {
            // if we do not want to autoexpand the size of the ppol, return null. Or expand size of pool
            Debug.LogError($"Could not get an object from pool\"{prefab.name}\" Pool. Probably a configuration issue.");
            return null;
        }
    }



}
