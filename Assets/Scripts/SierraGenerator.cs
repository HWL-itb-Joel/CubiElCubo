using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SierraGenerator : MonoBehaviour
{
    public GameObject particles;
    public GameObject sierra;

    public float timeMin;
    public float timeMax;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerarSierra());
    }

    IEnumerator GenerarSierra()
    {
        float tiempo = Random.Range(timeMin,timeMax);

        yield return new WaitForSecondsRealtime(tiempo);

        GameObject uwu = Instantiate(particles);
        uwu.transform.position = transform.position;
        Destroy(uwu, 2f);

        yield return new WaitForSecondsRealtime(1f);

        GameObject sierraGenerada = Instantiate(sierra);
        sierraGenerada.transform.position = transform.position;
        Destroy(sierraGenerada, 4);

        StartCoroutine(GenerarSierra());
    }
}
