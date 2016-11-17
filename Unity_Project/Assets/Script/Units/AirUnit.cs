using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Units/DEV/Air Unit")]
public class AirUnit : HoverTank
{

    #region Initialization
    protected override void Reset()
    {
        base.Reset();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void ResetUnit()
    {
        base.ResetUnit();

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponentInChildren<MeshCollider>().convex = false;
    }
    #endregion

    #region Hit Points Related
    protected override void Die()
    {
        base.Die();

        GetComponentInChildren<MeshCollider>().convex = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    #endregion

    #region Updates
    protected override void Update()
    {
        base.Update();
    }
    #endregion
}
