using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsMenu : MonoBehaviour
{
    public void LoadLevel1()
    {
        GameManager.gameManager.LoadScene(1);
    }
    public void LoadLevel2()
    {
        GameManager.gameManager.LoadScene(2);
    }
    public void LoadLevel3()
    {
        GameManager.gameManager.LoadScene(3);
    }
}
