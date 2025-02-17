using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;

    public Vector3 cameraPos1;
    public Vector3 cameraPos2;
    public Vector3 cameraPos3;
    public Vector3 cameraPos4;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x < -26.65f)
        {
            transform.position = cameraPos1;
        }
        else if (player.transform.position.x > -26.65f && player.transform.position.x < -8.89)
        {
            transform.position = cameraPos2;
        }
        else if (player.transform.position.x > -8.89 && player.transform.position.x < 8.89)
        {
            transform.position = cameraPos3;
        }
        else
        {
            transform.position = cameraPos4;
        }
    }
}
