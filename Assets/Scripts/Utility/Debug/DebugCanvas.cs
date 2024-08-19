using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    void Start()
    {
        if (!Debug.isDebugBuild && !Application.isEditor)
        {
            Destroy(gameObject);
        }
    }
}
