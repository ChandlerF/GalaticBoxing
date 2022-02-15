using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] private Camera _cam;

    private PhotonView _pv;

    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
    }

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
            _pv.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }


    [PunRPC]
    private void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);

        if(colliders.Length != 0)
        {
            GameObject bulletImpactObject = Instantiate(BulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * BulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObject, 10f);
            bulletImpactObject.transform.SetParent(colliders[0].transform);
        }
    }
}
