using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadMovimiento = 3f;

    [Header("Patrulla")]
    public float distanciaPatrulla = 4f;

    [Header("Sprite")]
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Vector2 posicionInicial;
    private int direccion = 1;
    private AtaqueEnemigo ataqueEnemigo;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;

        ataqueEnemigo = GetComponent<AtaqueEnemigo>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        ActualizarDireccion();
    }

    private void FixedUpdate()
    {
        // Detiene la patrulla mientras realiza el ataque.
        if (ataqueEnemigo != null && ataqueEnemigo.EstaAtacando())
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            return;
        }

        float distanciaRecorrida =
            rb.position.x - posicionInicial.x;

        // Cambia de dirección al alcanzar los límites.
        if (distanciaRecorrida >= distanciaPatrulla && direccion > 0)
        {
            CambiarDireccion();
        }
        else if (
            distanciaRecorrida <= -distanciaPatrulla &&
            direccion < 0
        )
        {
            CambiarDireccion();
        }

        // Movimiento horizontal.
        rb.linearVelocity = new Vector2(
            direccion * velocidadMovimiento,
            rb.linearVelocity.y
        );
    }

    private void CambiarDireccion()
    {
        direccion *= -1;
        ActualizarDireccion();
    }

    private void ActualizarDireccion()
    {
        // Se asume que el sprite original mira a la derecha.
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direccion < 0;
        }

        // Mueve el punto del golpe al lado hacia el que mira.
        if (ataqueEnemigo != null && ataqueEnemigo.puntoGolpe != null)
        {
            Transform punto = ataqueEnemigo.puntoGolpe;

            Vector3 posicion = punto.localPosition;
            posicion.x = Mathf.Abs(posicion.x) * direccion;

            punto.localPosition = posicion;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 centro = Application.isPlaying
            ? posicionInicial
            : (Vector2)transform.position;

        Vector3 limiteIzquierdo = new Vector3(
            centro.x - distanciaPatrulla,
            centro.y,
            transform.position.z
        );

        Vector3 limiteDerecho = new Vector3(
            centro.x + distanciaPatrulla,
            centro.y,
            transform.position.z
        );

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(limiteIzquierdo, limiteDerecho);
        Gizmos.DrawWireSphere(limiteIzquierdo, 0.2f);
        Gizmos.DrawWireSphere(limiteDerecho, 0.2f);
    }
}