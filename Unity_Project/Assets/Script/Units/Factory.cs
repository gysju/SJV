using UnityEngine;
using System.Collections;

public class Factory : Unit
{
    public bool m_producing;
    [ContextMenuItem("Produce Squadron", "ProduceSquadron")]
    public Unit m_produceUnit;
    public float m_productionTime;
    public Transform m_productionExit;
    
    protected override void Start()
    {
        base.Start();
        if (m_producing) StartContinuousProduction();
    }

    private void CreateUnit()
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
        StartCoroutine(ContinuousProduction());
    }

    IEnumerator Production(float m_timeBetweenSpawns, int m_spawnsCount)
    {
        for (int i = 0 ; i < m_spawnsCount; i++)
        {
            CreateUnit();
            yield return new WaitForSeconds(m_timeBetweenSpawns);
        }
    }

    public void ProduceUnit(float m_timeBetweenSpawns, int m_spawnsCount)
    {
        StartCoroutine(Production(m_timeBetweenSpawns, m_spawnsCount));
    }

    public void ProduceSquadron()
    {
        ProduceUnit(1f, 5);
    }

    protected override void Update()
    {
	    
	}
}
