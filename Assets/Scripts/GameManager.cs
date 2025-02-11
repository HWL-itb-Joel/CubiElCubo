using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public bool normalView;

    [Header("Transition Config")]
    public Animator transitionAnimator;
    public TextMeshProUGUI transitionText;
    public Image backgroundTransition;
    [SerializeField] float transitionTime;

    [Header("Colors Transitions")]
    public Color Level1;
    public Color TextL1;
    public Color Level2;
    public Color TextL2;
    public Color Level3;
    public Color TextL3;

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

    public void LoadScene(int sceneNumber)
    {
        StartCoroutine(SceneLoad(sceneNumber));
    }

    public IEnumerator SceneLoad(int sceneNumber)
    {
        if (sceneNumber == 0)
        {
            backgroundTransition.color = Color.black;
            transitionText.color = Color.black;
            transitionText.text = "";
        }
        else if (sceneNumber == 1)
        {
            backgroundTransition.color = Level1;
            transitionText.color = TextL1;
            transitionText.text = "Nivel 01 - Deuteronomia";
        }
        else if (sceneNumber == 2)
        {
            backgroundTransition.color = Level2;
            transitionText.color = TextL2;
            transitionText.text = "Nivel 02 - Tritanopia";
        }
        else if (sceneNumber == 3)
        {
            backgroundTransition.color = Level3;
            transitionText.color = TextL3;
            transitionText.text = "Nivel 03 - Protanopia";
        }
        transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(sceneNumber);
    }
}
