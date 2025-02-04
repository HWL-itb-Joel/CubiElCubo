using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisColor : MonoBehaviour
{
    public colores colors;
    SpriteRenderer sp;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (GameManager.gameManager.normalView)
        {
            sp.color = colors.ColorBase;
        }
        else
        {
            sp.color = colors.Daltonico;
        }
    }
}
