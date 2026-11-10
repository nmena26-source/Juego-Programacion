using System.Collections.Generic;
using UnityEngine;

public class AtaqueJugador : MonoBehaviour
{
    [Header("Ataque")]
    [Min(1)]
    public int dano = 30;

    [Min(0.1f)]
    public float tiempoEntreAtaques = 0.5f;

    [Header("Hitbox")]
    public Transform puntoGolpe;

    [Min(0.01f)]
    public float radioHitbox = 0.6f;

    public LayerMask capaEnemigos;

    [Header("Animacion")]
    public Animator animator;

    public string triggerAtaque = "Ataque";

    [Header("Estado de ataque")]
    public bool atacando = false;

    [Min(0.05f)]
    public float duracionAtaque = 0.5f;

    [Header("Referencias")]
    public SpriteRenderer spriteRenderer;

    public MovimientoJugador movimientoJugador;

    private PlayerHealth vidaJugador;

    private float proximoAtaque;

    private void Awake()
    {
        vidaJugador = GetComponent<PlayerHealth>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (movimientoJugador == null)
        {
            movimientoJugador = GetComponent<MovimientoJugador>();
        }
    }

    private void LateUpdate()
    {
        if (vidaJugador != null && vidaJugador.IsDead)
        {
            return;
        }

        ActualizarPuntoGolpe();

        if (
            Input.GetMouseButtonDown(0) &&
            Time.time >= proximoAtaque &&
            movimientoJugador != null &&
            movimientoJugador.estaEnSuelo &&
            !atacando
        )
        {
            Atacar();
        }
    }

    private void ActualizarPuntoGolpe()
    {
        if (puntoGolpe == null || spriteRenderer == null)
        {
            return;
        }

        Vector3 posicion = puntoGolpe.localPosition;

        float direccion = spriteRenderer.flipX ? -1f : 1f;

        posicion.x = Mathf.Abs(posicion.x) * direccion;

        puntoGolpe.localPosition = posicion;
    }

    private void Atacar()
    {
        if (puntoGolpe == null)
        {
            Debug.LogWarning(
                "Asigna PuntoGolpeJugador en AtaqueJugador.",
                this
            );

            return;
        }

        proximoAtaque = Time.time + tiempoEntreAtaques;

        atacando = true;

        if (animator != null)
        {
            animator.SetTrigger(triggerAtaque);
        }

        // Respaldo: si el Animation Event no funciona,
        // el jugador vuelve a poder moverse.
        CancelInvoke("TerminarAtaque");

        Invoke("TerminarAtaque", duracionAtaque);

        Collider2D[] impactos = Physics2D.OverlapCircleAll(
            puntoGolpe.position,
            radioHitbox,
            capaEnemigos
        );

        HashSet<VidaEnemigo> enemigosGolpeados =
            new HashSet<VidaEnemigo>();

        foreach (Collider2D impacto in impactos)
        {
            VidaEnemigo enemigo =
                impacto.GetComponentInParent<VidaEnemigo>();

            if (
                enemigo != null &&
                !enemigo.EstaMuerto() &&
                enemigosGolpeados.Add(enemigo)
            )
            {
                enemigo.RecibirDaño(dano);
            }
        }
    }

    public void TerminarAtaque()
    {
        atacando = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoGolpe == null)
        {
            return;
        }

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            puntoGolpe.position,
            radioHitbox
        );
    }
}