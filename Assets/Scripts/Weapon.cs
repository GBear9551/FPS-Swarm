using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  [Header("Weapon Settings")]
  [SerializeField] public WeaponSO weaponStats; // Default damage for the weapon
  
  [Header("References - Dependencies - Cache")]
  [SerializeField] private ParticleSystem weaponMuzzleFlashVFX; // Particle effect for the weapon muzzle flash



  public void Shoot()
  {

    // Check if the shoot input is pressed

      // Play the weapon muzzle flash effect
      if (weaponMuzzleFlashVFX != null)
      {
        weaponMuzzleFlashVFX.Play();
      }

      RaycastHit hit;

      // Perform a raycast from the camera's position in the forward direction
      // to detect objects in the scene
      Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity);
    // Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, interactionLayer, QueryTriggerInteraction.ignore);

    // Decrease unit health if it hits a enemy unit
    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
      {
        Unit enemyUnit = hit.collider.GetComponentInParent<Unit>();
        if (enemyUnit != null)
        {
          enemyUnit.TakeDamage(weaponStats.damage);
          Debug.Log("Damage Dealt: " + weaponStats.damage);
          var vfx = Instantiate(enemyUnit.onHitVFX, hit.point, Quaternion.identity, enemyUnit.transform);
          
        }
        else 
        {
          Debug.LogWarning("Enemy unit does not have a Unit component.");
        }
      }

      // DEBUG LOGIC
      if (hit.collider != null)
      {
        Debug.Log("Hit: " + hit.collider.name);
      }
      else
      {
        Debug.Log("No hit detected.");
      }



      
  }
}
