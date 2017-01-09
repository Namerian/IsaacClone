using UnityEngine;
using System.Collections;

public class GuidedProjectileController : ProjectileController
{
    public float _radarRange = 5;

    protected new void MoveProjectile()
    {
        RaycastHit[] raycastHit = Physics.SphereCastAll(this.transform.position, _radarRange, Vector3.zero);

        if (raycastHit != null && raycastHit.Length > 0)
        {

        }
        else
        {
            base.MoveProjectile();
        }
    }
}
