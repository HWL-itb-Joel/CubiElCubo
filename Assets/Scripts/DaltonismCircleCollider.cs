using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaltonismCircleCollider : MonoBehaviour
{
    private CircleCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManager.normalView)
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }
}
