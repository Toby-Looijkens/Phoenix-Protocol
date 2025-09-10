using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class RangedWeapon_Script : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private float MagazineSize;
    [SerializeField] private float HealthDamage;
    [SerializeField] private float StaggerDamage;
    [SerializeField] private float FireRate;

    [Header("Components")]
    [SerializeField] Transform Muzzle;
    [SerializeField] GameObject cam;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject projectilePrefab;

    // Weapon Stats
    private float currentAmmo;
    private float reserveAmmo;
    private bool isReloading = false;
    private bool canFire = true;

    // Cooldowns
    private float fireCooldown = 0;

    // Input Registration
    private InputAction shootAction;
    private InputAction reloadAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = MagazineSize;
        shootAction = InputSystem.actions.FindAction("Shoot");
        reloadAction = InputSystem.actions.FindAction("Reload");
    }

    void Update()
    {
        if (reloadAction.ReadValue<float>() > 0 && !isReloading) Reload();
        Shoot();
    }

    private void Shoot()
    {
        if (fireCooldown > 0)
        {
            fireCooldown -= Time.deltaTime;
            return;
        }

        fireCooldown = 60 / FireRate;

        if (shootAction.ReadValue<float>() == 0) return;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, 1000, mask))
        {
            GameObject projectile = Instantiate(projectilePrefab, Muzzle.position, Quaternion.Euler(0, 0, 0));
            projectile.GetComponent<Projectile_Script>().target = hitInfo.point;
            projectile.GetComponent<Projectile_Script>().speed = 100f;

            if (hitInfo.collider.CompareTag("Enemy"))
            {

            }
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
        if (reserveAmmo >= MagazineSize)
        {
            currentAmmo = MagazineSize;
            reserveAmmo -= MagazineSize;
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
