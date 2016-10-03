using UnityEngine;
using System.Collections;

[AddComponentMenu("MecaVR/Ammo")]
public class Ammo : MonoBehaviour
{
    public GameObject m_metalHit;
    public GameObject m_dirtHit;

    [Header("Damages")]
    [Tooltip("Ammo's damages on hit.")]
    public int m_directDamages;
    [Tooltip("Armor the ammo ignore.")]
    public int m_armorPenetration;

    [Header("Ammo's specs")]
    [Tooltip("Is the ammo explosive.")]
    public bool m_explosive;
    public GameObject m_explosionPrefab;

    [Header("Life Time")]
    const float MIN_LIFE_TIME = 0f;
    const float MAX_LIFE_TIME = 10f;

    [Tooltip("The ammo is lost at after that time (seconds).")]
    [Range(MIN_LIFE_TIME, MAX_LIFE_TIME)]
    public float m_lifeTime = 10f;
	protected virtual void Awake()
    { 
        StartCoroutine(LifeTimer(m_lifeTime));
    }

    protected virtual void DestroyAmmo()
    {

    }

    IEnumerator LifeTimer(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Lost();
    }

    protected virtual void MetalHit()
    {
        Debug.Log("METAL HIT");
        Instantiate(m_metalHit, transform.position, transform.rotation);
    }

    protected virtual void DirtHit()
    {
        Debug.Log("DIRT HIT");
        Instantiate(m_dirtHit, transform.position, transform.rotation);
    }

    protected void HitAnimation(PhysicMaterial physicMaterial)
    {
        if (physicMaterial.name == "Metal (Instance)")
            MetalHit();
        else
            DirtHit();
    }

    protected virtual void Lost()
    {
        Debug.Log("LOST AMMO");
        Destroy(gameObject);
    }

    protected virtual void CheckExplosion()
    {
        if (m_explosive) Explode();
        else DestroyAmmo();
    }

    private void Explode()
    {
        Debug.Log("EXPLOSION");
        Instantiate(m_explosionPrefab, transform.position, transform.rotation);
        DestroyAmmo();
    }
}
