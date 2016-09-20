using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class BaliseManager : MonoBehaviour {

    public static BaliseManager Instance { get; private set; }
    public List<Balise> target { get; private set; }

    public Balise firstBalise { get; private set; }
    public Balise secondeBalise { get; private set; }

    void Start()
    {
        Instance = this;
        target = GetComponentsInChildren<Balise>().ToList();

        firstBalise = Instance.target[0];
        secondeBalise = Instance.target[1];
    }

    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            
            for(int i = 0; i < target.Count; ++i)
            {
                Gizmos.DrawLine(target[i].transform.position, target[ (i + 1) % target.Count].transform.position);
            }
        }
    }

    public void NextStep()
    {
        var index = target.FindIndex(a => a == secondeBalise) + 1;

        firstBalise = secondeBalise;
        secondeBalise = target[(index) % target.Count];
    }

    public void PreviousStep()
    {
        var index = target.FindIndex(a => a == firstBalise) - 1;

        if (index < 0)
            index = target.Count - 1;

        secondeBalise = firstBalise;
        firstBalise = target[index];
    }
}
