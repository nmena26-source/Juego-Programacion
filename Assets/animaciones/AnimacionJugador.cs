using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimacionJugador : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public Rigidbody2D rb;

    [Header("Nombres de parámetros")]
    public string parametroVelocidad = "Velocidad";
    public string parametroEnSuelo = "EnSuelo";
    public string parametroSalto = "Salto";

    private MovimientoJugador movimientoJugador;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        movimientoJugador = GetComponent<MovimientoJugador>();
    }

    void Update()
    {
        if (animator == null || rb == null || movimientoJugador == null)
        {
            return;
        }

        float velocidadHorizontal = Mathf.Abs(rb.linearVelocity.x);

        animator.SetFloat(parametroVelocidad, velocidadHorizontal);

        bool enSuelo = movimientoJugador.EstaEnSuelo();

        animator.SetBool(parametroEnSuelo, enSuelo);

        bool estaSaltando = !enSuelo && rb.linearVelocity.y > 0.1f;

        if (estaSaltando)
        {
            animator.SetTrigger(parametroSalto);
        }
    }
}