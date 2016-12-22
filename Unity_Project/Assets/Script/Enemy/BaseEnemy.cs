using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour
{
    const int MIN_HIT_POINTS = 0;
    const int MAX_HIT_POINTS = 100;

    const int MIN_ARMOR = 0;
    const int MAX_ARMOR = 100;

    protected Transform m_transform;
    protected Renderer m_model;


    //public ZAManager m_ZAManager;

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

    [Header("Weapons")]
    [Tooltip("Unit's Weapons list.")]
    public List<Weapon> m_weapons = new List<Weapon>();


    protected enum EnemyState
    {
        EnemyState_Sleep,
        EnemyState_Moving,
        EnemyState_Attacking
    }
    protected EnemyState m_enemyState = EnemyState.EnemyState_Sleep;

    protected Vector3? m_attackPosition = null;

    [Header("Attack")]
    [Tooltip("Time the unit will take to shoot.")]
    [ContextMenuItem("Test Unit", "TestUnit")]
    [Range(1f, 5f)]
    public float m_timeToAttack = 2f;
    protected float m_currentTimeToAttack;

    protected Transform m_target;

    #region Initialization
    protected virtual void Awake()
    {
        m_transform = transform;
        m_model = GetComponentInChildren<MeshRenderer>();
        m_currentTimeToAttack = m_timeToAttack;
        //if(!m_ZAManager) m_ZAManager = FindObjectOfType<BattleManager>();
    }

    protected virtual void Start()
    {
        m_currentHitPoints = m_startingHitPoints;
        CheckHitPoints();
    }

    public virtual void ResetUnit(Vector3 spawn, Vector3 movementTarget, Transform target)
    {
        m_transform.position = spawn;

        m_attackPosition = movementTarget;

        m_target = target;

        m_destroyed = false;

        m_enemyState = EnemyState.EnemyState_Sleep;
    }

    public virtual void TestUnit()
    {
        ResetUnit(new Vector3(15f,0f, 120f), new Vector3(5f, 0f, 50f), FindObjectOfType<Player>().transform);
    }
    #endregion

    #region HitPoints Related
    /// <summary>Renvoie true si l'unité est détruite</summary>
    public bool IsDestroyed()
    {
        return m_destroyed;
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

        m_enemyState = EnemyState.EnemyState_Sleep;

        GetComponentInChildren<Collider>().enabled = false;

        StartCoroutine(Dying());
    }

    protected virtual void FinishDying()
    {
        //m_ZAManager.PoolUnit(this);
    }

    /// <summary>Vérifie si les hit points ne sont pas inférieurs à 0 ou supérieurs au maximum.</summary>
    private void CheckHitPoints()
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
    public bool ReceiveDamages(int damages, int armorPenetration = 0)
    {
        if (!m_destroyed)
        {
            int actualArmor = Mathf.Max((m_armor - armorPenetration), 0); //réduit l'armure de l'unité par la pénétration d'armure de l'attaque, avec un minimum de 0.

            int actualDamages = Mathf.Max((damages - actualArmor), 0); //réduit les dégâts par l'armure, avec un minimum de 0 (pour ne pas soigner...).

            m_currentHitPoints -= actualDamages;

            CheckHitPoints();

            if (actualDamages > 0) return true;
            else return false;
        }
        else return false;
    }
    #endregion

    #region Movement Related
    public virtual void StartMovement()
    {
        m_enemyState = EnemyState.EnemyState_Moving;
    }
    #endregion

    #region Attack Related
    public virtual void AimWeaponAt(Vector3 target)
    {
        foreach (Weapon weapon in m_weapons)
        {
            weapon.transform.LookAt(target);
        }
    }

    protected void FireWeapon(int weaponID)
    {
        m_weapons[weaponID].FireWeapon();
    }

    public void PressWeaponTrigger(int weaponID)
    {
        m_weapons[weaponID].TriggerPressed();
    }

    public void ReleaseWeaponTrigger(int weaponID)
    {
        m_weapons[weaponID].TriggerReleased();
    }

    public void CeaseFire()
    {
        foreach (Weapon weapon in m_weapons)
        {
            weapon.TriggerReleased();
        }
    }
    #endregion

    #region Updates
    protected virtual void Update()
    {
        if (!m_destroyed)
        {

        }
    }
    #endregion
}
