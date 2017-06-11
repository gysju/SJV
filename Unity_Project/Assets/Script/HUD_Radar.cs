using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Radar : MonoBehaviour 
{
	public static HUD_Radar Instance;
	public Transform PivotTransfrom;
	public float Radius = 5.0f;
	public GameObject UIPrefab;

	private Transform Mecha;
	private Transform infoParent;

	struct Info
	{
		public BaseEnemy Target;
		public GameObject UI;
	};

	private List<Info> Infos = new List<Info>();

	void Start () 
	{
		if (Instance == null) {
			Instance = this;
			Mecha = BaseMecha.instance.transform;	
			infoParent = PivotTransfrom.FindChild ("InfoParent");
		} 
		else 
		{
			Destroy (gameObject);
		}
	}
	
	void Update () {
		if (Mecha != null)
			PivotTransfrom.localRotation = Quaternion.Euler ( new Vector3 ( PivotTransfrom.localRotation.eulerAngles.x, 
                                                                            PivotTransfrom.localRotation.eulerAngles.y, 
                                                                            MechaTorso.Instance.transform.localRotation.eulerAngles.y ));

		DisplayUnit ();
	}

	void DisplayUnit()
	{
		foreach(Info info in Infos)
		{
			Vector3 Dir = ( info.Target.transform.position - Mecha.position ) / Radius;
			info.UI.transform.localPosition = infoParent.localPosition + ( new Vector3( Dir.x, Dir.z, 0.0f) * 0.2f );
		}
	}

	public void AddInfo( BaseEnemy unit )
	{
		Info info = new Info ();
		
		info.UI = Instantiate (UIPrefab);
		info.Target = unit;
		
		info.UI.transform.parent = infoParent;
		info.UI.transform.localPosition = Vector3.zero;
		info.UI.transform.localRotation = Quaternion.identity;

		if (unit as GroundEnemy) 
		{
			info.UI.GetComponent<MeshRenderer> ().material.SetTexture ("_MainTex", (Texture)Resources.Load ("Radar/Sprite_Tank"));
		} 
		else if (unit as AirEnemy) 
		{
			info.UI.GetComponent<MeshRenderer> ().material.SetTexture ("_MainTex", (Texture)Resources.Load ("Radar/Sprite_Drone"));
		}

		Infos.Add( info );
	}

	public void RemoveInfo(BaseEnemy unit)
	{
		foreach(Info info in Infos)
		{
			if (info.Target == unit) 
			{
				Infos.Remove (info);
				Destroy (info.UI);
				break;
			}
		}
	}

    public void RemoveAllInfos()
    {
        foreach (Info info in Infos)
        {
            Destroy(info.UI);
        }
        Infos.Clear();
    }
}
 