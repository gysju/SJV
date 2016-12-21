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
		base.Start ();	
        if (m_producing) StartContinuousProduction();
    }

    private void CreateUnit(Capture_point order = null)
    {
        Unit newUnit = null;

        if (m_produceUnit is AirUnit)
        {
            newUnit = (m_battleManager.IsThereUnusedDrones()) ? m_battleManager.GetUnusedDrone(m_productionExit.position, m_productionExit.rotation).GetComponent<Unit>() : (Unit)Instantiate(m_produceUnit, m_productionExit.position, m_productionExit.rotation);
			BattleManager.Instance.setCurrentNbrDrone ( BattleManager.Instance.getCurrentNbrDrone() + 1 );
			BattleManager.Instance.setcurrentNbrDroneSinceLastWave ( BattleManager.Instance.getcurrentNbrDroneSinceLastWave() + 1 );

			Debug.Log (BattleManager.Instance.getCurrentNbrDrone() + " : " + WaveManager.Instance.getCurrentMaxDrone());
		}
        else if (m_produceUnit is HoverTank)
        {
            newUnit = (m_battleManager.IsThereUnusedTanks()) ? m_battleManager.GetUnusedTank(m_productionExit.position, m_productionExit.rotation).GetComponent<Unit>() : (Unit)Instantiate(m_produceUnit, m_productionExit.position, m_productionExit.rotation);
			BattleManager.Instance.setCurrentNbrTank ( BattleManager.Instance.getCurrentNbrTank() + 1 );
			BattleManager.Instance.setcurrentNbrTankSinceLastWave ( BattleManager.Instance.getcurrentNbrTankSinceLastWave() + 1 );

			Debug.Log (BattleManager.Instance.getCurrentNbrTank() + " : " + WaveManager.Instance.getCurrentMaxTank());
		}

        newUnit.ChangeFaction(m_faction);
        if (order) newUnit.GetComponent<IA>().GiveCaptureOrder(order);
    }

    IEnumerator ContinuousProduction()
    {
        while (m_producing)
        {
            yield return new WaitForSeconds(m_productionTime);
			if ( m_produceUnit is HoverTank && ( WaveManager.Instance.getCurrentMaxTank() > BattleManager.Instance.getcurrentNbrTankSinceLastWave() ))
				CreateUnit();
			else if ( m_produceUnit is AirUnit && ( WaveManager.Instance.getCurrentMaxDrone() > BattleManager.Instance.getcurrentNbrDroneSinceLastWave() ))
				CreateUnit();
        }
    }

    protected void StartContinuousProduction()
    {
        StartCoroutine(ContinuousProduction());
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
