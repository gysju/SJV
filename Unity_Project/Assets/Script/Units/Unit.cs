using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("MechaVR/Units/DEV/Unit")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(BoxCollider))]
public class Unit : MonoBehaviour
{
    const int MIN_HIT_POINTS = 0;
    const int MAX_HIT_POINTS = 100;

    const int MIN_ARMOR = 0;
    const int MAX_ARMOR = 100;

    const float TIME_TO_DIE = 1f;

    public enum UnitFaction
    {
        Ally,
        Neutral,
        Enemy
    };

    protected bool m_destroyed = false;

    protected NavMeshObstacle m_navMeshObstacle;

    [Header("Faction")]
    [Tooltip("Unit's current faction.")]
    public UnitFaction m_faction;

    [Tooltip("Is unit capturable.")]
    public bool m_capturable;

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

    public GameObject m_destructionSpawn;

    [Header("Unit's armor")]
    [Tooltip("Unit's maximum armor value between 0 and 100.")]
    [Range(MIN_ARMOR, MAX_ARMOR)]
    public int m_armor;

    [Header("Unit's point to target")]
    [Tooltip("Unit's point an IA will aim to.")]
    public Transform m_targetPoint;

    protected bool m_vulnerable = true;

    protected List<CombatUnit> m_detectingUnits = new List<CombatUnit>();
    protected List<CombatUnit> m_targetingUnits = new List<CombatUnit>();

    #region Initialization
    protected virtual void Reset()
    {
        GetComponent<BoxCollider>().size = Vector3.zero;
    }

    protected virtual void Awake()
    {
        m_navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    protected virtual void Start()
    {
        m_currentHitPoints = m_startingHitPoints;
        CheckHitPoints();
    }
    #endregion

    #region Faction Related
    /// <summary>A utiliser pour changer la faction de l'unité.</summary>
    /// <param name ="newFaction">Nouvelle faction à appliquer.</param>
    public void ChangeFaction(UnitFaction newFaction)
    {
        m_faction = newFaction;
    }

    /// <summary>A utiliser pour changer la faction de l'unité.</summary>
    /// <param name ="newFaction">Nouvelle faction à appliquer.</param>
    public void CaptureUnit(UnitFaction newFaction)
    {
       if (m_capturable) ChangeFaction(newFaction);
    }

    #endregion

    #region HitPoints Related
    /// <summary>Renvoie true si l'unité est détruite</summary>
    public bool IsDestroyed()
    {
        return m_destroyed;
    }

    IEnumerator Dying()
    {
        yield return new WaitForSeconds(TIME_TO_DIE);
        Destroy(gameObject);
    }

    /// <summary>A appeler à la mort de l'unité.</summary>
    protected void Die()
    {
        m_destroyed = true;

        GetComponent<BoxCollider>().enabled = false;

        for (int i = 0 ; i < transform.childCount ; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        foreach (CombatUnit detectingUnit in m_detectingUnits)
        {
            detectingUnit.DetectedUnitDestroyed(this);
        }

        foreach (CombatUnit targetingUnit in m_targetingUnits)
        {
            targetingUnit.TargetedUnitDestroyed(this);
        }

        Instantiate(m_destructionSpawn, transform.position, transform.rotation);

        StartCoroutine(Dying());
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
            Die();
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
        if (m_vulnerable && !m_destroyed)
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

    #region Radar Related
    public void Detected(CombatUnit detectingUnit)
    {
        m_detectingUnits.Add(detectingUnit);
    }

    public void NoMoreDetected(CombatUnit noMoreDetectingUnit)
    {
        m_detectingUnits.Remove(noMoreDetectingUnit);
    }

    public void Targeted(CombatUnit targetingUnit)
    {
        m_targetingUnits.Add(targetingUnit);
    }

    public void NoMoreTargeted(CombatUnit noMoretargetingUnit)
    {
        m_targetingUnits.Remove(noMoretargetingUnit);
    }
    #endregion

    #region Updates
    protected virtual void Update()
    {

	}
    #endregion
}
