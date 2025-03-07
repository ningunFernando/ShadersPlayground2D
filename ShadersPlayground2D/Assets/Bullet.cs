using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3f; // Tiempo antes de destruirse

    void Start()
    {
        Destroy(gameObject, lifetime); // Se autodestruye despu�s del tiempo asignado
    }
}
