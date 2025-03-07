using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxEnergy = 100; // Energía máxima
    public int energy = 100; // Energía inicial
    public int energyDecreaseRate = 1; // Energía que se pierde cada ciclo
    public float decreaseInterval = 3f; // Intervalo de reducción de energía en segundos

    public TextMeshProUGUI energyText;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
        InvokeRepeating("DecreaseEnergyOverTime", decreaseInterval, decreaseInterval);
    }

    private void DecreaseEnergyOverTime()
    {
        ChangeEnergy(-energyDecreaseRate);
    }

    public void ChangeEnergy(int amount)
    {
        energy += amount;
        energy = Mathf.Clamp(energy, 0, maxEnergy); // Limita el valor entre 0 y 100
        UpdateUI();

        if (energy == 0)
        {
            GameOver();
        }
    }

    private void UpdateUI()
    {
        if (energyText != null)
        {
            energyText.text = "Energía: " + energy;
        }
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        FindObjectOfType<PlayerController>().DisableMovement();
    }
}
