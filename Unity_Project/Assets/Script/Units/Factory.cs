using UnityEngine;
using System.Collections;

public class Factory : Unit
{
    public bool m_producing;
    [ContextMenuItem("Produce Squadron", "ProduceSquadron")]
    public Unit m_produceUnit;
    public float m_productionTime;
    public Transform m_productionExit;

    public enum SpawnType { SpawnType_ByConstanteTime = 0, SpawnType_ByCurve }
    public SpawnType spawnType = SpawnType.SpawnType_ByConstanteTime;

    [Header("Wave system")]
    public AnimationCurve SpawnTimeline;
    private float currentTime;

    protected override void Start()
    {
        base.Start();
        currentTime = Time.deltaTime;
        if (m_producing) StartContinuousProduction();
    }

    private void CreateUnit(Capture_point order = null)
    {
        Unit newUnit = null;

        if (m_produceUnit is AirUnit)
        {
            newUnit = (m_battleManager.IsThereUnusedDrones()) ? m_battleManager.GetUnusedDrone(m_productionExit.position, m_productionExit.rotation).GetComponent<Unit>() : (Unit)Instantiate(m_produceUnit, m_productionExit.position, m_productionExit.rotation);
        }
        else if (m_produceUnit is HoverTank)
        {
            newUnit = (m_battleManager.IsThereUnusedTanks()) ? m_battleManager.GetUnusedTank(m_productionExit.position, m_productionExit.rotation).GetComponent<Unit>() : (Unit)Instantiate(m_produceUnit, m_productionExit.position, m_productionExit.rotation);
        }

        newUnit.ChangeFaction(m_faction);
        if (order) newUnit.GetComponent<IA>().GiveCaptureOrder(order);
    }

    IEnumerator ContinuousProduction()
    {
        while (m_producing)
        {
            yield return new WaitForSeconds(m_productionTime);
            CreateUnit();
        }
    }

    protected void StartContinuousProduction()
    {
        if(spawnType == SpawnType.SpawnType_ByConstanteTime)
            StartCoroutine(ContinuousProduction());
        //else
        //    SpawnTimeline.
    }

    IEnumerator Production(float timeBetweenSpawns, int spawnsCount, Capture_point order = null)
    {
        for (int i = 0 ; i < spawnsCount; i++)
        {
            CreateUnit(order);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public void ProduceUnit(float timeBetweenSpawns, int spawnsCount, Capture_point order = null)
    {
        StartCoroutine(Production(timeBetweenSpawns, spawnsCount, order));
    }

    public void ProduceSquadron(Capture_point order = null)
    {
        ProduceUnit(1f, 5, order);
    }

    protected override void Update()
    {
	    
	}
}
