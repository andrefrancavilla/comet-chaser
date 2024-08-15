using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScoreStarsLerp : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private bool _moveAllowed;

    public void AllowMove()
    {
        _moveAllowed = true;
    }

    private void Update()
    {
        if(!_moveAllowed) return;

        foreach (Transform child in transform)
        {
            Vector3 moveDir = UIScore.Instance.transform.position - child.position;
            Vector3 moveDirNormalized = moveDir.normalized;

            child.transform.position += moveDirNormalized * (moveSpeed * Time.deltaTime);
            if (Vector3.Distance(child.position, UIScore.Instance.transform.position) < 10f)
            {
                Destroy(child.gameObject);
                break;
            }
        }

        if (transform.childCount == 0)
            Destroy(gameObject);
    }
}
