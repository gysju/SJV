﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("Test/Units")]
public class Unit : GraphicalElement
{
    const int MIN_HIT_POINTS = 0;
    const int MAX_HIT_POINTS = 100;

    const int MIN_ARMOR = 0;
    const int MAX_ARMOR = 100;

    const float TIME_TO_DIE = 1f;

    public enum UnitFaction
    {
        FirstTeam,
        Neutral,
        SecondTeam
    };

    protected bool m_destroyed = false;

    [Header("Faction")]
    [Tooltip("Unit's current faction.")]
    public UnitFaction m_faction;

    [Tooltip("Is unit capturable.")]
    public bool m_capturable;

    [Header("Unit's hit points")]
    [Tooltip("Unit's maximum hit points value between 0 and 100.")]
    [Range(MIN_HIT_POINTS, MAX_HIT_POINTS)]
    public int m_maxHitPoints;
    
    [Tooltip("Unit's starting hit points value between 0 and 100.")]
    [Range(MIN_HIT_POINTS, MAX_HIT_POINTS)]
    public int m_startingHitPoints;

    [ContextMenuItem("Destroy Unit", "Die")]
    [Tooltip("Current hit points value (private).")]
    [SerializeField]
    protected int m_currentHitPoints;

    [Header("Unit's armor")]
    [Tooltip("Unit's maximum armor value between 0 and 100.")]
    [Range(MIN_ARMOR, MAX_ARMOR)]
    public int m_armor;

    protected bool m_vulnerable = true;

    public NavMeshAgent navMeshAgent { get; private set; }
    public Balise targetBalise { get; private set; }
    protected Path path;
    protected bool followTheWay = true;

	protected override void Start()
    {
        base.Start();
        m_currentHitPoints = m_startingHitPoints;
        CheckHitPoints();
        navMeshAgent = GetComponent<NavMeshAgent>();
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
    private void Die()
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

    #region Movement Related

    protected void setDestination(Vector3 pos)
    {
        if (navMeshAgent.destination != pos)
            navMeshAgent.destination = pos;
    }

    protected void PauseNavMesh()
    {
        if (navMeshAgent.hasPath)
            navMeshAgent.Stop();
    }

    protected void ContinueNavMesh()
    {
        if (navMeshAgent.hasPath)
            navMeshAgent.Resume();
    }

    protected void MoveAlongPath(bool nextBalise)
    {
        if (navMeshAgent.destination != targetBalise.transform.position && navMeshAgent.remainingDistance > 0.01 && followTheWay == nextBalise)
        {
            setDestination(targetBalise.transform.position);
        }
        else
        {
            followTheWay = nextBalise;
            if (nextBalise)
            {
                targetBalise = path.NextStep(targetBalise);
                setDestination(targetBalise.transform.position);
            }
            else
            {
                targetBalise = path.PreviousStep(targetBalise);
                setDestination(targetBalise.transform.position);
            }
        }
    }

    protected void MoveToDir(Vector3 dir)
    {
        navMeshAgent.destination = transform.position + dir.normalized;
    }

    #endregion

    #region Updates
    void Update()
    {
	    
	}
    #endregion
}
