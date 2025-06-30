using UnityEngine;

 public class PickUpWeapon : PickUp
{

  [SerializeField] private WeaponSO weaponSO;


  protected override void OnPickUp(Collider other)
  {
      // Access the active weapon component on the player object
      ActiveWeapon activeWeapon = other.GetComponentInChildren<ActiveWeapon>();

      activeWeapon.SwitchWeapon(weaponSO);

      Debug.Log("Picked up " + weaponSO.name);

      Destroy(gameObject);
  }

}
