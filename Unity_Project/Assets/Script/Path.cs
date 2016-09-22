using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;

[ExecuteInEditMode]
public class Path : MonoBehaviour {

    public List<Balise> Balises { get; private set; }

    void Awake ()
    {
        Balises = GetComponentsInChildren<Balise>().ToList();
    }
	
	void Update ()
    {
	
	}
    void OnDrawGizmosSelected()
    {   
        if (Balises != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < Balises.Count; ++i)
            {
                Gizmos.DrawLine(Balises[i].transform.position, Balises[(i + 1) % Balises.Count].transform.position);
            }
        }
    }

    public Balise GetNearestBalise(Vector3 position)
    {
        Balise currentBalise = null;
        float distance = float.MaxValue;

        if (Balises != null)
        {
            foreach (Balise balise in Balises)
            {
                if (distance > (balise.transform.position - position).magnitude)
                    currentBalise = balise;
            }
        }
        return currentBalise;
    }

    public Balise NextStep(Balise currentBalise)
    {
        var index = Balises.FindIndex(a => a == currentBalise) + 1;

        return Balises[(index) % Balises.Count];
    }

    public Balise PreviousStep(Balise currentBalise)
    {
        var index = Balises.FindIndex(a => a == currentBalise) - 1;

        if (index < 0)
            index = Balises.Count - 1;

        return Balises[index];
    }
}
