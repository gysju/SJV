using UnityEngine;
using System.Collections;

public class Factory : Unit
{
    public bool m_producing;
    public Unit m_produceUnit;
    public float m_productionTime;
    public Transform m_productionExit;
    
    protected override void Start()
    {
        base.Start();
        StartCoroutine(ProduceUnit());
	}
    
    IEnumerator ProduceUnit()
    {
        while (m_producing)
        {
            yield return new WaitForSeconds(m_productionTime);
            CreateUnit();
        }
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

    protected override void Update()
    {
	    
	}
}
