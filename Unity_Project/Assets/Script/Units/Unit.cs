using UnityEngine;
using System.Collections;

[AddComponentMenu("MechaVR/Units/DEV/Unit")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshObstacle))]
public class Unit : GraphicalElement
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

    [Header("Unit's armor")]
    [Tooltip("Unit's maximum armor value between 0 and 100.")]
    [Range(MIN_ARMOR, MAX_ARMOR)]
    public int m_armor;

    [Header("Unit's point to target")]
    [Tooltip("Unit's point an IA will aim to.")]
    public Transform m_targetPoint;

    protected bool m_vulnerable = true;

    protected override void Awake()
    {
        base.Awake();
        m_navMeshObstacle = GetComponent<NavMeshObstacle>();
		if(m_navMeshObstacle == null)
		{
			m_navMeshObstacle = GetComponentInParent<NavMeshObstacle> ();
		}
    }

    protected override void Start()
    {
        base.Start();
        m_currentHitPoints = m_startingHitPoints;
        CheckHitPoints();
    }

    #region Faction Related
    /// <summary>A utiliser pour changer la faction de l'unité.</summary>
    /// <param name ="newFaction">Nouvelle faction à appliquer.</param>
    protected void ChangeFaction(UnitFaction newFaction)
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

    /// <summary>A appeler à la mort de l'unité.</summary>
    protected void Die()
    {
        m_destroyed = true;
        StartFade(INVISIBLE, TIME_TO_DIE);
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

    #region Updates
    protected override void Update()
    {
        base.Update();
	}
    #endregion
}
