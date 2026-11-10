using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class MuerteJugador : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public MovimientoJugador movimiento;
    public AnimacionJugador animacionMovimiento;

    private PlayerHealth vida;
    private Rigidbody2D rb;

    private void Awake()
    {
        vida = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (movimiento == null)
        {
            movimiento = GetComponent<MovimientoJugador>();
        }

        if (animacionMovimiento == null)
        {
            animacionMovimiento = GetComponent<AnimacionJugador>();
        }
    }

    private void OnEnable()
    {
        vida.OnDied += AlMorir;
    }

    private void OnDisable()
    {
        vida.OnDied -= AlMorir;
    }

    private void AlMorir()
    {
        // Bloquea caminar y saltar.
        if (movimiento != null)
        {
            movimiento.enabled = false;
        }

        // Evita que siga enviando parámetros de caminar o saltar.
        if (animacionMovimiento != null)
        {
            animacionMovimiento.enabled = false;
        }

        // Detiene al jugador incluso si muere en el aire.
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // El Animator permanece activo para mostrar la muerte.
        if (animator != null)
        {
            animator.SetTrigger("Muerte");
        }
    }
}