using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Référence Data")]
    [SerializeField] private PlayerData data;

    // ==== Stats runtime ====
    public int CurrentHealth { get; private set; }
    public int Money { get; private set; }
    public float MoveSpeed { get; private set; }

    public int Strength { get; private set; }
    public int Agility { get; private set; }
    public int Intelligence { get; private set; }

    private void Awake()
    {
        InitializeFromData();
    }

    private void InitializeFromData()
    {
        if (data == null)
        {
            Debug.LogError("Aucune PlayerData assignée !");
            return;
        }

        CurrentHealth = data.maxHealth;
        Money = data.startingMoney;
        MoveSpeed = data.moveSpeed;

        Strength = data.strength;
        Agility = data.agility;
        Intelligence = data.intelligence;
    }

    // =======================
    //       MÉTHODES UTILES
    // =======================

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, data.maxHealth);

        if (CurrentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, data.maxHealth);
    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public bool SpendMoney(int amount)
    {
        if (Money < amount) return false;

        Money -= amount;
        return true;
    }

    private void Die()
    {
        Debug.Log("Le joueur est mort !");
        // TODO : gérer respawn, game over, etc.
    }
}
