using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public bool normalView;

    private void Awake()
    {
        if (GameManager.gameManager != null && GameManager.gameManager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            GameManager.gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ChangeColor()
    {
        if (normalView)
        {
            normalView = false;
        }
        else
        {
            normalView = true;
        }
    }
}
