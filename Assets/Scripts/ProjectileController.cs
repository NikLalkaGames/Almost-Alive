using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    private int projectileCount;
    public int maxProjectileCount;
    private GameObject projectile;
    private PlayerController player;
    private float angle;
    private float projectileRegen = 0f;


    void Start()
    {
        player = GetComponent<PlayerController>();
        projectileCount = maxProjectileCount;
    }


    void Update()
    {
        ProjectileRegen();

        if ( Input.GetButtonDown("Fire2") )
        {
            ShootProjectile();
        }
    }



    private void ShootProjectile()
    {
        if ( projectileCount>0 )
        {
            projectileCount -= 1;
            angle = Mathf.Atan2(player.LookDirection.x, player.LookDirection.y) * Mathf.Rad2Deg;
            GameObject projectile = Instantiate(Resources.Load("Projectile"), transform.position, Quaternion.Euler(0, 0, angle)) as GameObject;
            projectile.GetComponent<Projectile>().SetDirection(player.LookDirection);
            projectile.GetComponent<Projectile>().SetTarget(player.MouseTarget);
            Debug.Log("Projectile has been spawned");
        }
    }


    private void ProjectileRegen()
    {
        if (projectileCount < maxProjectileCount)
        {
            projectileRegen += Time.deltaTime;
            if (projectileRegen >= 2f)
            {
                projectileCount += 1;
                projectileRegen = 0f;
            }
        }
    }





}
