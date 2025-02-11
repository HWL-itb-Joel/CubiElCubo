using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;

    public Vector3 cameraPos1;
    public Vector3 cameraPos2;
    public Vector3 cameraPos3;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x < -26.5f)
        {
            transform.position = cameraPos1;
        }
        else if (player.transform.position.x > -26.5f && player.transform.position.x < -9)
        {
            transform.position = cameraPos2;
        }
        else
        {
            transform.position = cameraPos3;
        }
    }
}
