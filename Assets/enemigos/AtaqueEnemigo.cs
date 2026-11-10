using UnityEngine;

public class AtaqueEnemigo : MonoBehaviour
{
    [Header("Daño")]
    [Min(1)]
    public int daño = 15;

    [Header("Tiempo entre inicios de ataque")]
    [Min(0.1f)]
    public float tiempoEntreAtaques = 1.5f;

    [Header("Distancia para iniciar el ataque")]
    [Min(0.1f)]
    public float distanciaAtaque = 1f;

    [Header("Hitbox")]
    public Transform puntoGolpe;

    [Min(0.01f)]
    public float radioHitbox = 0.6f;

    public LayerMask capaJugador;

    [Header("Referencias")]
    public Animator animator;
    public string parametroAtaque = "Ataque";

    private PlayerHealth jugador;
    private float temporizadorAtaque;
    private bool jugadorDetectado;
    private bool estaAtacando;
    private bool golpeAplicado;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        temporizadorAtaque = tiempoEntreAtaques;
    }

    private void Update()
    {
        temporizadorAtaque -= Time.deltaTime;

        if (estaAtacando)
        {
            return;
        }

        if (!jugadorDetectado || jugador == null || jugador.IsDead)
        {
            return;
        }

        if (temporizadorAtaque > 0f)
        {
            return;
        }

        float distancia = Vector2.Distance(
            transform.position,
            jugador.transform.position
        );

        if (distancia <= distanciaAtaque)
        {
            Atacar();
        }
    }

    private void Atacar()
    {
        if (animator == null)
        {
            Debug.LogWarning("Asigna el Animator en AtaqueEnemigo.", this);
            return;
        }

        estaAtacando = true;
        golpeAplicado = false;
        temporizadorAtaque = tiempoEntreAtaques;

        animator.SetTrigger(parametroAtaque);
    }

    // Evento de animación en el momento del golpe.
    public void AplicarDaño()
    {
        if (!estaAtacando || golpeAplicado)
        {
            return;
        }

        golpeAplicado = true;

        Debug.Log("Llegó el momento del golpe.", this);

        if (puntoGolpe == null)
        {
            Debug.LogWarning("Falta asignar PuntoGolpe.", this);
            return;
        }

        Collider2D[] objetivos = Physics2D.OverlapCircleAll(
            puntoGolpe.position,
            radioHitbox,
            capaJugador
        );

        foreach (Collider2D objetivo in objetivos)
        {
            PlayerHealth vida = objetivo.GetComponentInParent<PlayerHealth>();

            if (vida != null && !vida.IsDead)
            {
                vida.TakeDamage(daño);

                Debug.Log(
                    "Golpe al jugador. Vida restante: " + vida.CurrentHealth,
                    this
                );

                return;
            }
        }

        Debug.Log("El golpe no encontró al jugador en la hitbox.", this);
    }

    // Evento de animación cerca del final del ataque.
    public void TerminarAtaque()
    {
        estaAtacando = false;
    }

    // MovimientoEnemigo consulta este método para detener la patrulla.
    public bool EstaAtacando()
    {
        return estaAtacando;
    }

    // Los llama el detector que está en el objeto hijo.
    public void JugadorEntró(Collider2D collision)
    {
        PlayerHealth vida = collision.GetComponentInParent<PlayerHealth>();

        if (vida == null)
        {
            return;
        }

        jugador = vida;
        jugadorDetectado = true;
    }

    public void JugadorSalió(Collider2D collision)
    {
        PlayerHealth vida = collision.GetComponentInParent<PlayerHealth>();

        if (vida != null && vida == jugador)
        {
            jugadorDetectado = false;
            jugador = null;
        }
    }

    private void OnDisable()
    {
        estaAtacando = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoGolpe == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoGolpe.position, radioHitbox);
    }
}