using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using InstantTools.PoolSystem;

[CustomEditor(typeof(InstantPool))]
public class InstantPoolingEditor : Editor
{

    [MenuItem("Tools/Instant Tools/Instant Pooling/Create Pool System")]
    public static void InitializePoolSystem()
    {
        if(!FindObjectOfType<InstantPool>())
        {
            GameObject poolSystem = new GameObject();
            poolSystem.AddComponent<InstantPool>();
            poolSystem.name = "Instant Pool";
        }
        else
        {
            Debug.Log("You have already created a pool sysyem.");
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        InstantPool poolManager = (InstantPool) target;

        if(poolManager)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            if(GUILayout.Button("Add Pool", EditorStyles.toolbarButton))
            {
                poolManager.pools.Add(new PoolEntity());
                Undo.RecordObject(poolManager, "New Pool");
            }
            //List element handling
            for(int i = 0; i < poolManager.pools.Count; i++)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Pool #" + (i + 1) + " - " + poolManager.pools[i].poolName, EditorStyles.boldLabel);
                
                GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Name", GUILayout.Width(100));
                    EditorGUILayout.LabelField("Size", GUILayout.Width(40));
                    EditorGUILayout.LabelField("Lifetime", GUILayout.Width(60));
                    EditorGUILayout.LabelField("Object");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                    poolManager.pools[i].poolName = GUILayout.TextField(poolManager.pools[i].poolName, GUILayout.Width(100));
                    poolManager.pools[i].poolSize = EditorGUILayout.IntField(poolManager.pools[i].poolSize, GUILayout.Width(40));
                    poolManager.pools[i].lifetime = EditorGUILayout.FloatField(poolManager.pools[i].lifetime, GUILayout.Width(60));
                    poolManager.pools[i].poolObject = (GameObject)EditorGUILayout.ObjectField(poolManager.pools[i].poolObject, typeof(GameObject), true, GUILayout.ExpandWidth(false));
                    if(GUILayout.Button("X", EditorStyles.miniButtonRight))
                    {
                        poolManager.pools.RemoveAt(i);
                        Undo.RecordObject(poolManager, "Remove Pool");
                        return;
                    }

                GUILayout.EndHorizontal();
                poolManager.pools[i].isDynamic = EditorGUILayout.Toggle("Is dynamic: ", poolManager.pools[i].isDynamic);
                GUILayout.EndVertical();
            }
            
            GUILayout.Space(10);

            if(GUILayout.Button("Add Pool", EditorStyles.toolbarButton))
            {
                poolManager.pools.Add(new PoolEntity());
                Undo.RecordObject(poolManager, "New Pool");
            }

            GUILayout.Space(5);
            if(GUILayout.Button("Generate Pools", EditorStyles.toolbarButton))
            {
                poolManager.GeneratePools();
            }
            
            Undo.RecordObject(poolManager, "Pool Operation");
            GUILayout.EndVertical();
        }
        serializedObject.ApplyModifiedProperties();
    }
}

