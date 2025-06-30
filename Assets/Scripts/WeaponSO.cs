using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/WeaponSO", order = 1)]
public class WeaponSO : ScriptableObject
{
   public GameObject weaponPrefab;
  
   public int damage;
   public float fireRate;
   public float range;
   public bool canZoom;
   public float zoomAmount;
   public bool isAutomatic;
   public bool isBurst;
   public int burstCount;
   public float burstInterval; // Time between shots in a burst
   public int magazineSize;
}
