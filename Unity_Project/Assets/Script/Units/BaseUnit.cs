using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseUnit : MonoBehaviour
{
    const int MIN_HIT_POINTS = 0;
    const int MAX_HIT_POINTS = 100;

    const int MIN_ARMOR = 0;
    const int MAX_ARMOR = 100;

    [HideInInspector]
    public Transform m_transform;
    protected Renderer m_model;
    protected Animator m_animator;

    protected bool m_destroyed = false;

    [Header("Unit's hit points")]
    [Tooltip("Unit's maximum hit points value between 0 and 100.")]
    [Range(MIN_HIT_POINTS, MAX_HIT_POINTS)]
    public int m_maxHitPoints = 10;

    [Tooltip("Unit's starting hit points value between 0 and 100.")]
    [Range(MIN_HIT_POINTS, MAX_HIT_POINTS)]
    public int m_startingHitPoints = 10;

    [Tooltip("Current hit points value (private).")]
    [SerializeField]
    [ContextMenuItem("Destroy Unit", "Die")]
    protected int m_currentHitPoints;

    public float m_timeToDie = 1f;

    public GameObject m_destructionSpawn;

    [Header("Unit's armor")]
    [Tooltip("Unit's maximum armor value between 0 and 100.")]
    [Range(MIN_ARMOR, MAX_ARMOR)]
    public int m_armor;

    [Header("Unit's point to target")]
    [Tooltip("Unit's point an IA will aim to.")]
    public Transform m_targetPoint;

    [Header("Weapons")]
    [Tooltip("Unit's Weapons list.")]
    public List<BaseWeapon> m_weapons = new List<BaseWeapon>();
    
    protected virtual void Awake()
    {
        m_transform = transform;
        if (!m_targetPoint) m_targetPoint = m_transform;
        m_model = GetComponentInChildren<MeshRenderer>();
        m_animator = GetComponent<Animator>();
        m_currentHitPoints = m_startingHitPoints;
        CheckHitPoints();
    }

    protected virtual void Start()
    {

    }

    #region HitPoints Related
    /// <summary>Renvoie true si l'unité est détruite</summary>
    public bool IsDestroyed()
    {
        return m_destroyed;
    }

    public int GetCurrentHitPoints()
    {
        return m_currentHitPoints;
    }

    protected IEnumerator Dying()
    {
        yield return new WaitForSeconds(m_timeToDie);
        if (m_destructionSpawn) Instantiate(m_destructionSpawn, transform.position, transform.rotation);
        FinishDying();
    }

    /// <summary>A appeler à la mort de l'unité.</summary>
    protected virtual void StartDying()
    {
        m_destroyed = true;
        if (m_animator) m_animator.SetTrigger("Death");
        StartCoroutine(Dying());
    }

    protected virtual void FinishDying()
    {
        m_destroyed = true;
    }

    /// <summary>Vérifie si les hit points ne sont pas inférieurs à 0 ou supérieurs au maximum.</summary>
    protected void CheckHitPoints()
    {
        if (m_currentHitPoints > m_maxHitPoints) m_currentHitPoints = m_maxHitPoints;
        if (m_currentHitPoints <= 0) //Si l'intégrité de l'unité tombe à 0.
        {
#if UNITY_EDITOR
            Debug.Log("Unit '" + name + "' is destroyed.");
#endif
            StartDying();
        }
    }

    /// <summary>A utiliser pour réparer l'unité.</summary>
    /// <param name ="reparations">Montant des réparations.</param>
    public void Repair(int reparations)
    {
        m_currentHitPoints += reparations;
        CheckHitPoints();
    }

    /// <summary>A utiliser pour infliger des dégâts à l'unité.</summary>
    /// <param name ="damages">Montant des dégâts reçus.</param>
    /// <param name ="armorPenetration">Nombre de points d'armure ignorés.</param>
    public virtual bool ReceiveDamages(int damages, int armorPenetration = 0)
    {
        if (!m_destroyed)
        {
            int actualArmor = Mathf.Max((m_armor - armorPenetration), 0); //réduit l'armure de l'unité par la pénétration d'armure de l'attaque, avec un minimum de 0.

            int actualDamages = Mathf.Max((damages - actualArmor), 0); //réduit les dégâts par l'armure, avec un minimum de 0 (pour ne pas soigner...).

            m_currentHitPoints -= actualDamages;

            CheckHitPoints();

            if (m_animator != null)
                m_animator.SetTrigger("Touched");

            if (actualDamages > 0)
                return true;
            else 
                return false;
        }
        else return false;
    }
    #endregion

    public virtual void LaserOn()
    {
        foreach (BaseWeapon weapon in m_weapons)
        {
            weapon.m_showLaser = true;
        }
    }

    public virtual void LaserOff()
    {
        foreach (BaseWeapon weapon in m_weapons)
        {
            weapon.m_showLaser = false;
        }
    }

    void Update ()
	{
		
	}
}
