using UnityEngine;

public class AmmoPickUp : PickUp
{
   [SerializeField] private int ammoAmount = 100;

   protected override void OnPickUp(Collider other)
   {
    int currentAmmo = 0;
 
    var currWeapon = other.GetComponentInChildren<ActiveWeapon>();
    currentAmmo = currWeapon.currentAmmo;
 
    if (currentAmmo >= currWeapon.activeWeapon.weaponStats.magazineSize) return;


    other.GetComponentInChildren<ActiveWeapon>().AdjustAmmo(ammoAmount);
    Destroy(gameObject);
  }
}
