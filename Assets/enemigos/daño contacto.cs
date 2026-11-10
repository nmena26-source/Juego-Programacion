using UnityEngine;

public class DanoContactoEnemigo : MonoBehaviour
{
    [Header("Daño por contacto")]
    [Min(1)]
    public int danoContacto = 10;

    [Header("Segundos entre daños")]
    [Min(0.1f)]
    public float tiempoEntreDanos = 1f;

    private float proximoDano;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IntentarHacerDano(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        IntentarHacerDano(collision);
    }

    private void IntentarHacerDano(Collision2D collision)
    {
        if (!isActiveAndEnabled || Time.time < proximoDano)
        {
            return;
        }

        PlayerHealth jugador =
            collision.gameObject.GetComponentInParent<PlayerHealth>();

        if (jugador == null || jugador.IsDead)
        {
            return;
        }

        proximoDano = Time.time + tiempoEntreDanos;

        jugador.TakeDamage(danoContacto);
    }
}