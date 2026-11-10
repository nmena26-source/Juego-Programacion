using UnityEngine;

public class DetectorAtaqueEnemigo : MonoBehaviour
{
    public AtaqueEnemigo ataqueEnemigo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ataqueEnemigo != null)
        {
            ataqueEnemigo.JugadorEntró(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ataqueEnemigo != null)
        {
            ataqueEnemigo.JugadorSalió(collision);
        }
    }
}