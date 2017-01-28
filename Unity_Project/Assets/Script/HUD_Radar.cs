using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_Radar : MonoBehaviour {

	public Transform PivotTransfrom;
	public float Radius = 5.0f;
	public GameObject UIPrefab;

	private Transform Mecha;
	private Transform infoParent;

	struct Info
	{
		public GameObject Target;
		public GameObject UI;
	};

	private List<Info> Infos = new List<Info>();

	void Start () 
	{
		Mecha = GetComponentInParent<BaseMecha> ().transform;	
		GetComponent<SphereCollider>().radius = Radius;
		infoParent = PivotTransfrom.FindChild ("InfoParent");
	}
	
	void Update () {
		if (Mecha != null)
			PivotTransfrom.localRotation = Quaternion.Euler ( new Vector3 (PivotTransfrom.localRotation.eulerAngles.x, PivotTransfrom.localRotation.eulerAngles.y, Mecha.localRotation.eulerAngles.y ));

		DisplayUnit ();
	}

	void DisplayUnit()
	{
		foreach(Info info in Infos)
		{
			
		}
	}

	void OnTriggerEnter( Collider col )
	{
		SetInfo (col.gameObject);
	}
		
	void OnTriggerExit( Collider col )
	{
		RemoveInfo (col.gameObject);
	}

	void SetInfo( GameObject obj )
	{
		BaseEnemy unit = obj.GetComponent<BaseEnemy>();

		if (unit != null && !unit.IsDestroyed()) 
		{
			Info info = new Info ();
			
			info.UI = Instantiate (UIPrefab);
			info.Target = obj;
			
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
	}

	void RemoveInfo(GameObject obj)
	{
		foreach(Info info in Infos)
		{
			if (info.Target == obj) 
			{
				Infos.Remove (info);
				Destroy (info.UI);
				break;
			}
		}
	}
}
 