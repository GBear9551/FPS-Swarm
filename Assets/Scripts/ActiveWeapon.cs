using UnityEngine;
using System.Collections;
using StarterAssets;
using TMPro;


public class ActiveWeapon : MonoBehaviour
{

  // Handles the active weapon of the player character in a game.
  // Handles the switching of weapons and the firing of the active weapon.
  [SerializeField] public WeaponSO startingWeaponSO; // The weapon stats for the starting weapon
  [SerializeField] public GameObject[] weapons;
  [SerializeField] public GameObject zoomVignette; // UI element for zoom effect when aiming
  [SerializeField] TMP_Text ammoText; // UI text element to display current ammo count
  public int currentAmmo;


  // References - Dependencies - Cache
  public Weapon activeWeapon;
  StarterAssetsInputs starterAssetsInputs;
  Animator animator;
  FirstPersonController firstPersonController;
  float defaultRotationSpeed;
  Cinemachine.CinemachineVirtualCamera virtualCamera;
  Cinemachine.CinemachineImpulseSource impulseSource;
  float defaultZoom; // Default camera field of view (FOV) when not zoomed in

  // State
  bool isOnCooldown = false; // Indicates if the weapon is on cooldown after shooting

  // Constants
  const string CONST_SHOOT_STRING = "Shoot"; // Animation trigger for shooting (if using animations)


  private void Awake()
  {
    animator = GetComponent<Animator>();
    
  }

  private void Start()
  {
    impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
    virtualCamera = FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>();
    defaultZoom = virtualCamera.m_Lens.FieldOfView;
    starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    activeWeapon = GetComponentInChildren<Weapon>();
    firstPersonController = GetComponentInParent<FirstPersonController>();
    defaultRotationSpeed = firstPersonController.RotationSpeed;
    SwitchWeapon(startingWeaponSO);
  }

  private void Update()
  {
    HandleZoom();
    if (isOnCooldown) return;
    HandleShoot();
  }

  public void AdjustAmmo(int amount)
  {

    int newAmmoAmount = currentAmmo + amount;
    if (newAmmoAmount <= activeWeapon.weaponStats.magazineSize)
    {
      currentAmmo = newAmmoAmount;
    }

    else if(newAmmoAmount > activeWeapon.weaponStats.magazineSize)
    {
      currentAmmo = activeWeapon.weaponStats.magazineSize;
    }
    else if (newAmmoAmount < 0)
    {
      currentAmmo = 0;
    }

    if (ammoText != null)
    {
      ammoText.text = currentAmmo.ToString("D2");
    }
    Debug.Log("Ammo adjusted by: " + amount + ". Current ammo: " + currentAmmo);
  }

  public void SwitchWeapon(WeaponSO newWeaponStats)
  {

    if(activeWeapon)
    {
      Destroy(activeWeapon.gameObject);
    }

    Weapon newWeapon = Instantiate(newWeaponStats.weaponPrefab, transform).GetComponent<Weapon>();
    
    activeWeapon = newWeapon;
    activeWeapon.weaponStats = newWeapon.weaponStats;
    currentAmmo = 0; // Reset current ammo when switching weapons
    AdjustAmmo(newWeaponStats.magazineSize); // Reset ammo to new weapon's magazine size

    Debug.LogWarning("Weapon with specified stats found: " + newWeaponStats.name);
  }

  private void HandleShoot()
  {
    if (starterAssetsInputs.shoot && currentAmmo > 0)
    {

      HandleSingleShot();
      /*if (!activeWeapon.weaponStats.isBurst)
      {
        HandleSingleShot();
      }

      else
      {
        StartCoroutine(HandleBurstShotV2Routine());
      }*/
    }

  }

  private IEnumerator HandleBurstShotV2Routine()
  {
    float burstInterval = activeWeapon.weaponStats.burstInterval;
    int burstCount = activeWeapon.weaponStats.burstCount;

    float timeSinceLastShot = 0f;
    int shotsFired = 0;

    while (shotsFired < burstCount)
    {
      timeSinceLastShot += Time.deltaTime;

      if (timeSinceLastShot >= burstInterval)
      {
        timeSinceLastShot = 0f;
        Debug.Log("Burst shot #" + (shotsFired + 1));

        if (animator != null)
        {
          animator.Play(CONST_SHOOT_STRING, 0, 0f);
        }

        activeWeapon.Shoot();
        shotsFired++;
      }

      yield return null;
    }

    starterAssetsInputs.ShootInput(false);
    isOnCooldown = true;
    StartCoroutine(WeaponCooldownRoutine());
  }

  private void HandleBurstShot()
  {
    StartCoroutine(HandleBurstShotRoutine());
  }

  private IEnumerator HandleBurstShotRoutine()
  {
    for (int i = 0; i < activeWeapon.weaponStats.burstCount; i++)
    {
      if (animator != null)
      {
        animator.Play(CONST_SHOOT_STRING, 0, 0f); // Play the shooting animation
        
      }
      activeWeapon.Shoot();
      yield return new WaitForSeconds(activeWeapon.weaponStats.burstInterval);
    }
    // Reset the shoot input to avoid continuous shooting
    starterAssetsInputs.ShootInput(false);
    isOnCooldown = true;
    StartCoroutine(WeaponCooldownRoutine());
  }



  private bool HandleSingleShot()
  {

    AdjustAmmo(-1); // Decrease ammo by 1

    if (animator != null && impulseSource != null)
    {
      animator.Play(CONST_SHOOT_STRING, 0, 0f); // Play the shooting animation
      impulseSource.GenerateImpulse(0.5f);
    }
    else
    {
      Debug.LogWarning("Animator or ImpulseSource component is missing.");
    }

      activeWeapon.Shoot();

    // Reset the shoot input to avoid continuous shooting
    if (!activeWeapon.weaponStats.isAutomatic)
    {
      starterAssetsInputs.ShootInput(false);
    }
    isOnCooldown = true;

    StartCoroutine(WeaponCooldownRoutine());
    return true;
  }


  // handle zoom in and out for weapon
  void HandleZoom()
  {
    if(!activeWeapon.weaponStats.canZoom) return;

    if (starterAssetsInputs.zoom)
    {
       firstPersonController.SetRotationSpeed(0.4f);
       virtualCamera.m_Lens.FieldOfView = activeWeapon.weaponStats.zoomAmount;
       zoomVignette.SetActive(true);
    }
    else
    {
       firstPersonController.SetRotationSpeed(defaultRotationSpeed);
       virtualCamera.m_Lens.FieldOfView = defaultZoom;
       zoomVignette.SetActive(false);
    }
  }

  private IEnumerator WeaponCooldownRoutine()
  {
    // Wait for the duration of the weapon's fire rate before allowing the next shot
    
    yield return new WaitForSeconds(activeWeapon.weaponStats.fireRate);
    isOnCooldown = false;
      
  }


}
