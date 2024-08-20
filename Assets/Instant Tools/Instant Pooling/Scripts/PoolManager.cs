using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InstantTools;

namespace  InstantTools.PoolSystem
{
    public static class PoolManager
    {
        public static InstantPool instantPool;
        private static PoolEntity pool = new PoolEntity();

        //USAGE methods
        public static GameObject GetObject(string poolName)
        {
            if(!instantPool)
            {
                instantPool = GameObject.FindObjectOfType<InstantPool>();
            }
            for(int i = 0; i < instantPool.pools.Count; i++)
            {
                if(instantPool.pools[i].poolName == poolName)
                {
                    for(int j = 0; j < instantPool.pools[i].physicalPool.Count; j++)
                    {
                        if(instantPool.pools[i].physicalPool[j])
                        {
                            if(!instantPool.pools[i].physicalPool[j].activeInHierarchy)
                            {
                                instantPool.pools[i].physicalPool[j].SetActive(true);
                                if(instantPool.pools[i].lifetime > 0)
                                    instantPool.pools[i].physicalPool[j].GetComponent<PoolObject>().Disable();
                                return instantPool.pools[i].physicalPool[j];
                            }
                        }
                        else
                        {
                            instantPool.pools[i].physicalPool[j] = GameObject.Instantiate(instantPool.pools[i].poolObject, Vector2.zero, Quaternion.Euler(Vector2.zero));
                            instantPool.pools[i].physicalPool[j].transform.parent = instantPool.poolOwners[i].transform;

                            if(instantPool.pools[i].lifetime > 0)
                            {
                                PoolObject obj = instantPool.pools[i].physicalPool[j].AddComponent<PoolObject>() as PoolObject;
                                instantPool.pools[i].physicalPool[j].GetComponent<PoolObject>().lifetime = instantPool.pools[i].lifetime;
                            }

                            instantPool.pools[i].physicalPool[j].SetActive(false);
                            Debug.LogWarning("An object inside your pool has been destroyed and a new instance of it has been instantiated to avoid errors. Remember that you must disable the objects inside the pool using SetActive() instead of destroying them, or else your performance will be negatively affected.");
                        }
                    }
                    if(instantPool.pools[i].isDynamic)
                    {
                        instantPool.pools[i].physicalPool.Add(GameObject.Instantiate(instantPool.pools[i].poolObject, Vector2.zero, Quaternion.Euler(Vector2.zero)));
                        instantPool.pools[i].physicalPool[instantPool.pools[i].physicalPool.Count - 1].transform.parent = instantPool.poolOwners[i].transform;

                        if(instantPool.pools[i].lifetime > 0)
                        {
                            PoolObject obj = instantPool.pools[i].physicalPool[instantPool.pools[i].physicalPool.Count - 1].AddComponent<PoolObject>() as PoolObject;
                            instantPool.pools[i].physicalPool[instantPool.pools[i].physicalPool.Count - 1].GetComponent<PoolObject>().lifetime = instantPool.pools[i].lifetime;
                        }

                        return instantPool.pools[i].physicalPool[instantPool.pools[i].physicalPool.Count - 1];
                    }
                    else
                    {
                        Debug.LogError("ERROR! - The object pool's size is not big enough! Set isDynamic to true if you want the object pool to expand when there are not enough GameObjects available in the pool.");
                        return null;
                    }
                }
            }
            Debug.LogError("ERROR! - There are no object pools called " + poolName + ". As a consequence, a NULL GameObject will be returned. Did you misspell the name of the object pool?");
            return null;
        }

        public static GameObject ActivateObject(string poolName, Vector3 position, Quaternion rotation)
        {
            if(!instantPool)
            {
                instantPool = GameObject.FindObjectOfType<InstantPool>();
            }
            for(int i = 0; i < instantPool.pools.Count; i++)
            {
                if(instantPool.pools[i].poolName == poolName)
                {
                    for(int j = 0; j < instantPool.pools[i].physicalPool.Count; j++)
                    {
                        if(instantPool.pools[i].physicalPool[j])
                        {
                            if(!instantPool.pools[i].physicalPool[j].activeInHierarchy)
                            {
                                instantPool.pools[i].physicalPool[j].SetActive(true);
                                instantPool.pools[i].physicalPool[j].transform.position = position;
                                instantPool.pools[i].physicalPool[j].transform.rotation = rotation;
                                if(instantPool.pools[i].lifetime > 0)
                                    instantPool.pools[i].physicalPool[j].GetComponent<PoolObject>().Disable();
                                return instantPool.pools[i].physicalPool[j];
                            }
                        }
                        else
                        {
                            instantPool.pools[i].physicalPool[j] = GameObject.Instantiate(instantPool.pools[i].poolObject, Vector2.zero, Quaternion.Euler(Vector2.zero));
                            instantPool.pools[i].physicalPool[j].transform.parent = instantPool.poolOwners[i].transform;
                            instantPool.pools[i].physicalPool[j].SetActive(false);
                            Debug.LogWarning("An object inside your pool has been destroyed and a new instance of it has been instantiated to avoid errors. Remember that you must disable the objects inside the pool using SetActive() instead of destroying them, or else your performance will be negatively affected.");
                        }
                    }
                    if(instantPool.pools[i].isDynamic)
                    {
                        instantPool.pools[i].physicalPool.Add(GameObject.Instantiate(instantPool.pools[i].poolObject, position, rotation));
                        instantPool.pools[i].physicalPool[instantPool.pools[i].physicalPool.Count - 1].transform.parent = instantPool.poolOwners[i].transform;

                        if(instantPool.pools[i].lifetime > 0)
                        {
                            PoolObject obj = instantPool.pools[i].physicalPool[instantPool.pools[i].physicalPool.Count - 1].AddComponent<PoolObject>() as PoolObject;
                            instantPool.pools[i].physicalPool[instantPool.pools[i].physicalPool.Count - 1].GetComponent<PoolObject>().lifetime = instantPool.pools[i].lifetime;
                            instantPool.pools[i].physicalPool[instantPool.pools[i].physicalPool.Count - 1].GetComponent<PoolObject>().Disable();
                        }

                        return instantPool.pools[i].physicalPool[instantPool.pools[i].physicalPool.Count - 1];
                    }
                    else
                    {
                        Debug.LogError("ERROR! - The size of the object pool named " + poolName + " is not big enough! Set isDynamic to true if you want the object pool to expand when there are not enough GameObjects available in the pool.");
                        return null;
                    }
                }
            }
            Debug.LogError("ERROR! - There are no object pools called " + poolName + ". As a consequence, a NULL GameObject will be returned. Did you misspell the name of the object pool?");
            return null;
        }

        //MODIFICATION methods
        public static void DeletePool(string poolName, bool autoUpdate)
        {
            if(!instantPool)
            {
                instantPool = GameObject.FindObjectOfType<InstantPool>();
            }
            for(int i = 0; i < instantPool.pools.Count; i++)
            {
                if(instantPool.pools[i].poolName == poolName)
                {
                    instantPool.pools.RemoveAt(i);
                    if(autoUpdate)
                        instantPool.GeneratePools();
                    return;
                }
            }
            Debug.LogError("Did not find any object pool called " + poolName + ". No object pools were deleted.");
        }
        
        public static void AddPool(string poolName, int poolSize, GameObject poolObject, bool isDynamic, bool autoUpdate)
        {
            for(int i = 0; i < instantPool.pools.Count; i++)
            {
                if(instantPool.pools[i].poolName == poolName)
                {
                    Debug.LogError("There's already an object pool named " + poolName + ". The pool has not been added.");
                    return;
                }
            }
            pool.poolName = poolName;
            pool.poolSize = poolSize;
            pool.poolObject = poolObject;
            pool.isDynamic = isDynamic;

            instantPool.pools.Add(pool);

            if(autoUpdate)
                instantPool.GeneratePools();
        }
    }
 
}
