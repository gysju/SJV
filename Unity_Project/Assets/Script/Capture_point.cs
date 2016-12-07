using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Capture_point : MonoBehaviour 
{
    public enum Capture_Type { Capture_type_Tactical, Capture_type_Ressources, Capture_type_Health }
    public Capture_Type type = Capture_Type.Capture_type_Tactical;

	public enum Capture_Mode { Capture_Mode_TriggerZone, Capture_Mode_Activation }
	public Capture_Mode mode = Capture_Mode.Capture_Mode_TriggerZone;
    
    protected SphereCollider m_captureZone;

	[Range(0.1f, 10.0f)]
	public float m_captureZoneRadius = 5.0f;

	[Range(1.0f, 10.0f)]
	public float m_timeToCapture = 1.0f;

    public Unit.UnitFaction m_faction = Unit.UnitFaction.Neutral;

    [SerializeField]
    private List<MobileGroundUnit> m_allyUnits = new List<MobileGroundUnit>();
    [SerializeField]
    private List<MobileGroundUnit> m_enemyUnits = new List<MobileGroundUnit>();

    [SerializeField]
    private float m_currentCaptureValue = 0f;

	private CombatUnit combatUnitOnTarget;

	private IACommander m_enemyCommander;

    #region Initialization
    void Start () 
	{
        SetCollider(mode);
        m_captureZone = GetComponent<SphereCollider>();
        m_captureZone.isTrigger = true;
        UpdateRadarRange();
		m_enemyCommander = GameObject.Find ("Enemy Commander").GetComponent<IACommander>();
    }
    #endregion

    #region State
    public bool IsSameFaction(Unit.UnitFaction otherFaction)
    {
        return (otherFaction == m_faction);
    }
    #endregion

    #region Actions
    void AddBuffByType( CombatUnit combatUnit )
    {
        switch ( type )
        {
            case Capture_Type.Capture_type_Tactical:
                // Call a GameManager.
                break;
            case Capture_Type.Capture_type_Ressources:
                // reload weapons, call a method in combat unit.
                break;
            case Capture_Type.Capture_type_Health:
                combatUnit.Repair(100);
                break;
        }
    }
    #endregion

    #region Radar Related
    protected void UpdateRadarRange()
    {
        m_captureZone.radius = m_captureZoneRadius;
    }

    public void CapturingUnitDestroyed(MobileGroundUnit destroyedUnit)
    {
        switch (destroyedUnit.m_faction)
        {
            case Unit.UnitFaction.Ally:
                m_allyUnits.Remove(destroyedUnit);
                break;
            case Unit.UnitFaction.Neutral:
                break;
            case Unit.UnitFaction.Enemy:
                m_enemyUnits.Remove(destroyedUnit);
                break;
            default:
                break;
        }
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (!col.isTrigger)
        {
            MobileGroundUnit detectedUnit = col.GetComponentInParent<MobileGroundUnit>();
            if (detectedUnit != null && !detectedUnit.IsDestroyed())
            {
                switch (detectedUnit.m_faction)
                {
                    case Unit.UnitFaction.Ally:
                        m_allyUnits.Add(detectedUnit);
                        break;
                    case Unit.UnitFaction.Neutral:
                        break;
                    case Unit.UnitFaction.Enemy:
                        m_enemyUnits.Add(detectedUnit);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    protected virtual void OnTriggerExit(Collider col)
    {
        if (!col.isTrigger)
        {
			MobileGroundUnit detectedUnit = col.GetComponentInParent<MobileGroundUnit>();
            if (detectedUnit != null && !detectedUnit.IsDestroyed())
            {
                switch (detectedUnit.m_faction)
                {
                    case Unit.UnitFaction.Ally:
                        m_allyUnits.Remove(detectedUnit);
                        break;
                    case Unit.UnitFaction.Neutral:
                        break;
                    case Unit.UnitFaction.Enemy:
                        m_enemyUnits.Remove(detectedUnit);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    #endregion

    #region Collider
    void SetCollider(Capture_Mode mode)
    {
        if (mode == Capture_Mode.Capture_Mode_TriggerZone)
        {
            SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = m_captureZoneRadius;
            sphereCollider.isTrigger = true;
        }
        else
        {
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = Vector3.one * m_captureZoneRadius / 2.0f;
            boxCollider.center += transform.forward * (boxCollider.size.x / 2.0f) + new Vector3(0, boxCollider.size.x / 2.0f, 0);
            boxCollider.isTrigger = true;
        }
    }
    #endregion

    #region Capture Related
    private void Capture(int multiplicator)
    {
        if ((multiplicator > 0 && m_faction != Unit.UnitFaction.Ally) || (multiplicator < 0 && m_faction != Unit.UnitFaction.Enemy))
        {
            m_currentCaptureValue += multiplicator * (Time.deltaTime / m_timeToCapture);
            Mathf.Clamp(m_currentCaptureValue, -1f, 1f);


            if (m_currentCaptureValue >= 1f)
            {
                m_faction = Unit.UnitFaction.Ally;
                m_currentCaptureValue = 1f;
				m_enemyCommander.ReactionDroneSquadron ();
            }
            else if (m_currentCaptureValue <= -1f)
            {
                m_faction = Unit.UnitFaction.Enemy;
                m_currentCaptureValue = -1f;
            }
        }
    }
    #endregion

    #region Updates
    void UpdateCapturePoint()
    {
        int allyNumber = m_allyUnits.Count;
        int enemyNumber = m_enemyUnits.Count;
        float previousCaptureValue = m_currentCaptureValue;
        
        Capture(allyNumber - enemyNumber);

        if ((previousCaptureValue > 0 && m_currentCaptureValue <= 0) || (previousCaptureValue < 0 && m_currentCaptureValue >= 0))
            m_faction = Unit.UnitFaction.Neutral;
    }

    void Update()
    {
        UpdateCapturePoint();
    }
    #endregion
}