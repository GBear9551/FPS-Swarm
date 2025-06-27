using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{

  [SerializeField] private int weaponDamage = 10; // Default damage for the weapon

  StarterAssetsInputs starterAssetsInputs;

  private void Awake()
  {
    starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
  }

  // Update is called once per frame
  void Update()
  {
    HandleShoot();

  }

  private void HandleShoot()
  {

    // Check if the shoot input is pressed
    if (starterAssetsInputs.shoot)
    {
      RaycastHit hit;

      // Perform a raycast from the camera's position in the forward direction
      // to detect objects in the scene
      Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity);


      // Decrease unit health if it hits a enemy unit
      if (hit.collider != null && hit.collider.CompareTag("Enemy"))
      {
        Unit enemyUnit = hit.collider.GetComponent<Unit>();
        enemyUnit?.TakeDamage(weaponDamage);
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

      // Reset the shoot input to avoid continuous shooting
      starterAssetsInputs.ShootInput(false);

      
    }
  }
}
