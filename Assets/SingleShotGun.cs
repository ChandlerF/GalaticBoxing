using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] private Camera _cam;

    public  override void Use()
    {
        Shoot();
    }


    private void Shoot()
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)ItemInfo).Damage);
        }
    }
}
