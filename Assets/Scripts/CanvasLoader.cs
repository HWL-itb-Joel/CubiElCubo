using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLoader : MonoBehaviour
{
    public static CanvasLoader canvasLoader;

    private void Awake()
    {
        if (CanvasLoader.canvasLoader != null && CanvasLoader.canvasLoader != this)
        {
            Destroy(gameObject);
        }
        else
        {
            CanvasLoader.canvasLoader = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
