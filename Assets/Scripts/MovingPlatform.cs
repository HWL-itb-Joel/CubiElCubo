using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Puntos entre los cuales la plataforma se moverá
    public Transform pointA;
    public Transform pointB;

    // Velocidad de movimiento de la plataforma
    public float speed = 2f;

    // Tiempo de desplazamiento (interpolación) entre los puntos
    private float journeyLength;
    private float startTime;

    void Start()
    {
        // Calcula la distancia entre los puntos A y B al iniciar el script
        journeyLength = Vector3.Distance(pointA.position, pointB.position);
        startTime = Time.time;
    }

    void Update()
    {
        // Lerp entre los puntos A y B basándonos en el tiempo transcurrido
        float distanceCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distanceCovered / journeyLength;

        // Mover la plataforma suavemente entre los puntos
        transform.position = Vector3.Lerp(pointA.position, pointB.position, fractionOfJourney);

        // Si la plataforma llega al punto B, reiniciar el movimiento hacia el punto A
        if (fractionOfJourney >= 1)
        {
            // Intercambiamos los puntos A y B para que se mueva en sentido contrario
            Transform temp = pointA;
            pointA = pointB;
            pointB = temp;

            // Reiniciar el tiempo para un nuevo ciclo
            startTime = Time.time;
        }
    }
}
