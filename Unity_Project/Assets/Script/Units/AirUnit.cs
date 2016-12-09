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
    }
    #endregion

    #region Hit Points Related
    protected override void StartDying()
    {
        base.StartDying();

        GetComponentInChildren<MeshCollider>().convex = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    protected override void FinishDying()
    {
        base.FinishDying();
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponentInChildren<MeshCollider>().convex = false;
    }
    #endregion

    #region Updates
    protected override void Update()
    {
        base.Update();
    }
    #endregion
}
