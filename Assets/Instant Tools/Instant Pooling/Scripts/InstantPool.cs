using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InstantTools.PoolSystem
{
    [DisallowMultipleComponent]
    public class InstantPool : MonoBehaviour
    {
        [HideInInspector]
        public List<GameObject> poolOwners = new List<GameObject>(); //poolOwners are empty GameObject parents that hold all the elements that populate a pool list
        
        public List<PoolEntity> pools = new List<PoolEntity>(); //actual pool list
        
        GameObject masterParent; //Master parent is an empty game object that holds all of the poolOwners generated from the GeneratePools() method.
        //Duplicate detection
        List<int> duplicateIndexes;
        List<string> duplicateNames = new List<string>();
        List<int> duplicateCounter = new List<int>();
        
        public void GeneratePools()
        {
            duplicateIndexes = CheckForDuplicates();

            //Erase all previously created pools, if they exist
            if(GameObject.Find("Object Pools") && !masterParent)
            {
                masterParent = GameObject.Find("Object Pools");
            }
            DestroyImmediate(masterParent);

            for(int i = 0; i < pools.Count; i++)
            {
                pools[i].physicalPool.Clear();
            }

            if(pools.Count > 0)
            {
                poolOwners = new List<GameObject>();
                masterParent = new GameObject();
                masterParent.name = "Object Pools";
                
                #if UNITY_EDITOR
                EditorUtility.SetDirty(masterParent);
                #endif
                
                int masterIndex = 0;

                //Generate pools
                for(int i = 0; i < pools.Count; i++)
                {
                    if(!duplicateIndexes.Contains(i))
                    {
                        #if UNITY_EDITOR
                        if (!PrefabUtility.IsPartOfAnyPrefab(pools[i].poolObject))
                        {
                            Debug.LogError($"The object pool '{pools[i].poolName}' must have a prefab reference to populate it. Skipping object pool.", gameObject);
                            continue;
                        }
                        #endif

                        poolOwners.Add(new GameObject());
                        poolOwners[masterIndex].name = pools[i].poolName;
                        poolOwners[masterIndex].transform.parent = masterParent.transform;

                        for(int j = 0; j < pools[i].poolSize; j++)
                        {
                            #if UNITY_EDITOR
                            pools[i].physicalPool.Add(PrefabUtility.InstantiatePrefab(pools[i].poolObject) as GameObject);
                            #else
                            pools[i].physicalPool.Add(Instantiate(pools[i].poolObject) as GameObject);        
                            #endif
                            
                            pools[i].physicalPool[j].transform.parent = poolOwners[masterIndex].transform;

                            if(pools[i].lifetime > 0)
                            {
                                PoolObject obj = pools[i].physicalPool[j].AddComponent<PoolObject>() as PoolObject;
                                pools[i].physicalPool[j].GetComponent<PoolObject>().lifetime = pools[i].lifetime;
                            }
                            pools[i].physicalPool[j].SetActive(false);
                        }
                        masterIndex++;
                    }   
                    else
                    {
                        //Keep track of the individual names of duplicates
                        if(!duplicateNames.Contains(pools[i].poolName))
                        {
                            duplicateNames.Add(pools[i].poolName);
                            duplicateCounter.Add(0);
                        }
                    }
                }
                //Count how many duplicates of each type there are
                for(int i = 0; i < pools.Count; i++)
                {
                    for(int j = 0; j < duplicateNames.Count; j++)
                    {
                        if(pools[i].poolName == duplicateNames[j])
                        {
                            duplicateCounter[j]++;
                        }
                    }
                }
                //Print duplicates on screen
                for(int i = 0; i < duplicateNames.Count; i++)
                {
                    Debug.LogWarning("Spotted " + duplicateCounter[i] + " pools called '" + duplicateNames[i] + "'. These pools have not been created." 
                    + " Please consider using different unique names for all pools.");
                }

            }
            else
            {
                Debug.LogWarning("You did not configure any pools!");
            }
        }


        private List<int> CheckForDuplicates()
        {
            List<int> indexes = new List<int>();
            string currentRefName = string.Empty;

            for(int i = 0; i < pools.Count; i++)
            {
                currentRefName = pools[i].poolName;
                for(int j = 0; j < pools.Count; j++)
                {
                    if(j != i)
                    {
                        if(currentRefName == pools[j].poolName)
                        {
                            indexes.Add(j);
                        }
                    }
                }
            }

            return indexes;
        }
    }

    [System.Serializable]
    public class PoolEntity //This class represents the actual object pool that populates the pool list
    {
        public string poolName;
        public int poolSize;
        public bool isDynamic;
        public GameObject poolObject;
        public float lifetime;
        //[HideInInspector]
        public List<GameObject> physicalPool = new List<GameObject>();
    }
}

