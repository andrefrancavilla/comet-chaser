using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float seconds;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
