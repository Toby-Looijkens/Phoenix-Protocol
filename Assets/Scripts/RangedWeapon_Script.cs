using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedWeapon_Script : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private float magazineSize;
    [SerializeField] private float healthDamage;
    [SerializeField] private float staggerDamage;
    [SerializeField] private float fireRate;

    [Header("Components")]

    // Weapon Stats
    private float currentAmmo;
    private float reserveAmmo;
    private bool isReloading = false;
    private bool canFire = true;

    // Input Registration
    private InputAction shootAction;
    private InputAction reloadAction;

    GameObject cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = magazineSize;
        shootAction = InputSystem.actions.FindAction("Shoot");
        reloadAction = InputSystem.actions.FindAction("Reload");
        cam = FindAnyObjectByType(typeof(CinemachineCamera)) as GameObject;
    }

    void Update()
    {
        if (reloadAction.ReadValue<float>() > 0 && !isReloading) Reload();     
    }

    private void Shoot()
    {
        if (!canFire) return;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, 1000);

        if (hitInfo.collider.CompareTag("Enemy"))
        {
            
        }
    }

    private void Reload()
    {
        if (reserveAmmo == 0) return; 
        canFire = false;
        // trigger animation
    }

    public void OnReloadFinish()
    {
        if (reserveAmmo >= magazineSize)
        {
            currentAmmo = magazineSize;
            reserveAmmo -= magazineSize;
        } 
        else
        {
            currentAmmo += reserveAmmo;
            reserveAmmo = 0;
        }
    }

    private void UpdateUI()
    {

    }
}
