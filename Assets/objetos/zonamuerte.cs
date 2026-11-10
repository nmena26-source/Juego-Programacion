using UnityEngine;

public class CuboDano : MonoBehaviour
{
    [Header("Daño")]
    [Min(1)]
    public int dano = 1000;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth vida =
            other.GetComponentInParent<PlayerHealth>();

        if (vida != null && !vida.IsDead)
        {
            vida.TakeDamage(dano);
        }
    }
}