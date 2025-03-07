using UnityEngine;

public class EnergyPickup : MonoBehaviour
{
    public int energyAmount = 10; // Cantidad de energía a sumar

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ChangeEnergy(energyAmount);
            Destroy(gameObject);
        }
    }
}
