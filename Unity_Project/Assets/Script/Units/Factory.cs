using UnityEngine;
using System.Collections;

public class Factory : Unit
{
    //protected IAGeneral m_general;
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
            HoverTank newUnit = (HoverTank) Instantiate(m_produceUnit, m_productionExit.position, m_productionExit.rotation);
            newUnit.ChangeFaction(m_faction);
        }
    }

    protected override void Update()
    {
	    
	}
}
