using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 120;

    [Header("Muerte")]
    public float tiempoAntesDeDestruir = 0f;

    private int vidaActual;
    private bool estaMuerto;

    void Start()
    {
        vidaActual = vidaMaxima;
        estaMuerto = false;
    }

    // Llama a esta función desde ataques, balas o el jugador.
    public void RecibirDaño(int cantidad)
    {
        if (estaMuerto)
        {
            return;
        }

        vidaActual -= cantidad;

        // La vida nunca puede ser menor que 0 ni mayor que la vida máxima.
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    // Opcional: para curar al enemigo.
    public void Curar(int cantidad)
    {
        if (estaMuerto)
        {
            return;
        }

        vidaActual += cantidad;

        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
    }

    public int ObtenerVidaActual()
    {
        return vidaActual;
    }

    public int ObtenerVidaMaxima()
    {
        return vidaMaxima;
    }

    public bool EstaMuerto()
    {
        return estaMuerto;
    }

    private void Morir()
    {
        estaMuerto = true;

        // Aquí puedes agregar animación de muerte, sonido o drop de objetos.

        Destroy(gameObject, tiempoAntesDeDestruir);
    }
}