using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    private Transform destinoActual;

    void Start()
    {
        destinoActual = puntoB;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            destinoActual.position,
            velocidad * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, destinoActual.position) < 0.1f)
        {
            destinoActual = (destinoActual == puntoA) ? puntoB : puntoA;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.transform.SetParent(null);
        }
    }
}