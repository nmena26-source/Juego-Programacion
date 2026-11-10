using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadMovimiento = 5f;

    public float fuerzaSalto = 8f;

    [Header("Suelo")]
    public Transform puntoSuelo;

    public float radioSuelo = 0.2f;

    public LayerMask capaSuelo;

    [HideInInspector]
    public bool estaEnSuelo;

    [Header("Animacion")]
    public Animator animator;

    public string animacionQuieto = "Idle";

    public string animacionCorrer = "Run";

    public string animacionSaltar = "Jump";

    [Header("Depuracion")]
    public bool mostrarMensajes = true;

    [Header("Referencias")]
    public SpriteRenderer spriteRenderer;

    public AtaqueJugador ataqueJugador;

    private Rigidbody2D rb;

    private float movimientoHorizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (ataqueJugador == null)
        {
            ataqueJugador = GetComponent<AtaqueJugador>();
        }
    }

    private void Update()
    {
        if (ataqueJugador != null && ataqueJugador.atacando)
        {
            if (mostrarMensajes)
            {
                Debug.Log("El jugador esta atacando: movimiento bloqueado.");
            }

            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            return;
        }

        ComprobarSuelo();

        LeerInput();

        Mover();

        Saltar();

        Girar();

        ActualizarAnimacion();
    }

    private void ComprobarSuelo()
    {
        if (puntoSuelo == null)
        {
            estaEnSuelo = false;

            if (mostrarMensajes)
            {
                Debug.LogWarning("Punto Suelo no esta asignado.");
            }

            return;
        }

        estaEnSuelo = Physics2D.OverlapCircle(
            puntoSuelo.position,
            radioSuelo,
            capaSuelo
        );

        if (mostrarMensajes)
        {
            Debug.Log("¿Esta en suelo?: " + estaEnSuelo);
        }
    }

    public bool EstaEnSuelo()
    {
        return estaEnSuelo;
    }

    private void LeerInput()
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");
    }

    private void Mover()
    {
        rb.linearVelocity = new Vector2(
            movimientoHorizontal * velocidadMovimiento,
            rb.linearVelocity.y
        );
    }

    private void Saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (mostrarMensajes)
            {
                Debug.Log(
                    "Espacio presionado. ¿En suelo?: " + estaEnSuelo
                );
            }
        }

        if (
            Input.GetKeyDown(KeyCode.Space) &&
            estaEnSuelo
        )
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );
        }
    }

    private void Girar()
    {
        if (movimientoHorizontal > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (movimientoHorizontal < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void ActualizarAnimacion()
    {
        if (animator == null)
        {
            return;
        }

        if (!estaEnSuelo)
        {
            animator.Play(animacionSaltar);

            return;
        }

        if (Mathf.Abs(movimientoHorizontal) > 0.01f)
        {
            animator.Play(animacionCorrer);
        }
        else
        {
            animator.Play(animacionQuieto);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoSuelo == null)
        {
            return;
        }

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            puntoSuelo.position,
            radioSuelo
        );
    }
}