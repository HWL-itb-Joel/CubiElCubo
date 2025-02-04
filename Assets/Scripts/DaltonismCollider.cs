using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaltonismCollider : MonoBehaviour
{
    private BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
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
