using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] private string unitName = "Default Unit Name"; // Default name for the unit
    [SerializeField] private int unitMaxHealth = 100; // Default maximum health for the unit
  [SerializeField] private int unitHealth = 100; // Default health for the unit
    [SerializeField] private int unitDamage = 10; // Default damage for the unit
    [SerializeField] private float unitSpeed = 5f; // Default speed for the unit
    [SerializeField] private ParticleSystem onDeathVFX; // Particle effect to play on death
    [SerializeField] private AudioSource onDeathSFX; // Sound to play on death
    [SerializeField] public GameObject onHitVFX; // Particle effect to play on hit

  private void Start()
  {
        // Initialize unit health
        unitHealth = unitMaxHealth;
  }

  public void TakeDamage(int damageAmount)
    {
        unitHealth -= damageAmount;
        if (unitHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Play death effects
        if (onDeathVFX != null)
        {
            Instantiate(onDeathVFX, transform.position, Quaternion.identity);
        }
        if (onDeathSFX != null)
        {
            onDeathSFX.Play();
        }
        // Destroy the unit game object
        Destroy(gameObject);
  }

}
