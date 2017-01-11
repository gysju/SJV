using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ikLimb : MonoBehaviour {

	public Transform upperArm;
	public Transform forearm;
	public Transform hand;
	public Transform target;
	public Transform elbowTarget;

	public bool IsEnabled = true;
	public bool debug = true;

	private Quaternion upperArmStartRotation;
	private Quaternion forearmStartRotation;
	private Quaternion handStartRotation;

	private Vector3 targetRelativeStartPosition;
	private Vector3 elbowTargetRelativeStartPosition;

	public float transition = 1.0f;	

	void Start () {
		upperArmStartRotation = upperArm.rotation;
		forearmStartRotation = forearm.rotation;
		handStartRotation = hand.rotation;
		targetRelativeStartPosition = target.position - upperArm.position;
		elbowTargetRelativeStartPosition = elbowTarget.position - upperArm.position;
	}
	
	void LateUpdate () {
		if (!IsEnabled){
			return;
		}
		CalculateIK();
	}

	void CalculateIK()
	{
		//Calculate ikAngle variable.
		float upperArmLength = Vector3.Distance(upperArm.position, forearm.position);
		float forearmLength = Vector3.Distance(forearm.position, hand.position);
		float armLength = upperArmLength + forearmLength;
		float hypotenuse = upperArmLength;
		float targetDistance = Vector3.Distance(upperArm.position, target.position);	
		
		targetDistance = Mathf.Min(targetDistance, armLength - 0.0001f); //Do not allow target distance be further away than the arm's length.

		//var adjacent : float = (targetDistance * hypotenuse) / armLength;
		float adjacent = (Mathf.Pow(hypotenuse,2) - Mathf.Pow(forearmLength,2) + Mathf.Pow(targetDistance,2))/(2*targetDistance);
		Debug.Log(adjacent);
		float ikAngle = Mathf.Acos(adjacent/hypotenuse) * Mathf.Rad2Deg;

		//Store pre-ik info.
		Vector3 targetPosition  = target.position;
		Vector3 elbowTargetPosition  = elbowTarget.position;

		Transform upperArmParent = upperArm.parent;
		Transform forearmParent = forearm.parent;
		Transform handParent = hand.parent; 

		Vector3 upperArmScale = upperArm.localScale;
		Vector3 forearmScale = forearm.localScale;
		Vector3 handScale = hand.localScale;

		Vector3 upperArmLocalPosition = upperArm.localPosition;
		Vector3 forearmLocalPosition = forearm.localPosition;
		Vector3 handLocalPosition = hand.localPosition;

		Quaternion upperArmRotation = upperArm.rotation;
		Quaternion forearmRotation = forearm.rotation;
		Quaternion handRotation = hand.rotation;

		//Reset arm.
		target.position = targetRelativeStartPosition + upperArm.position;
		elbowTarget.position = elbowTargetRelativeStartPosition + upperArm.position;
		upperArm.rotation = upperArmStartRotation;
		forearm.rotation = forearmStartRotation;
		hand.rotation = handStartRotation;

		//Work with temporaty game objects and align & parent them to the arm.
		transform.position = upperArm.position;
		transform.LookAt(targetPosition, elbowTargetPosition - transform.position);
		
		GameObject upperArmAxisCorrection = new GameObject("upperArmAxisCorrection");
		GameObject forearmAxisCorrection = new GameObject("forearmAxisCorrection");
		GameObject handAxisCorrection = new GameObject("handAxisCorrection");
		
		upperArmAxisCorrection.transform.position = upperArm.position;
		upperArmAxisCorrection.transform.LookAt(forearm.position, transform.root.up);
		upperArmAxisCorrection.transform.parent = transform;
		upperArm.parent = upperArmAxisCorrection.transform;
		
		forearmAxisCorrection.transform.position = forearm.position;
		forearmAxisCorrection.transform.LookAt(hand.position, transform.root.up);
		forearmAxisCorrection.transform.parent = upperArmAxisCorrection.transform;
		forearm.parent = forearmAxisCorrection.transform;
		
		handAxisCorrection.transform.position = hand.position;
		handAxisCorrection.transform.parent = forearmAxisCorrection.transform;
		hand.parent = handAxisCorrection.transform;

		//Reset targets.
		target.position = targetPosition;
		elbowTarget.position = elbowTargetPosition;	

		//Apply rotation for temporary game objects.
		upperArmAxisCorrection.transform.LookAt(target,elbowTarget.position - upperArmAxisCorrection.transform.position);
		upperArmAxisCorrection.transform.localRotation =  Quaternion.Euler(upperArmAxisCorrection.transform.localRotation.eulerAngles - new Vector3(ikAngle,0,0)); /// before :upperArmAxisCorrection.transform.localRotation.eulerAngles.x -= ikAngle;

		forearmAxisCorrection.transform.LookAt(target,elbowTarget.position - upperArmAxisCorrection.transform.position);
		handAxisCorrection.transform.rotation = target.rotation;

		//Restore limbs.
		upperArm.parent = upperArmParent;
		forearm.parent = forearmParent;
		hand.parent = handParent;
		upperArm.localScale = upperArmScale;
		forearm.localScale = forearmScale;
		hand.localScale = handScale;
		upperArm.localPosition = upperArmLocalPosition;
		forearm.localPosition = forearmLocalPosition;
		hand.localPosition = handLocalPosition;

		//Clean up temporary game objets.
		Destroy(upperArmAxisCorrection);
		Destroy(forearmAxisCorrection);
		Destroy(handAxisCorrection);

		//Transition.
		transition = Mathf.Clamp01(transition);
		upperArm.rotation = Quaternion.Slerp(upperArmRotation, upperArm.rotation, transition);
		forearm.rotation = Quaternion.Slerp(forearmRotation, forearm.rotation, transition);
		hand.rotation = Quaternion.Slerp(handRotation, hand.rotation, transition);

		//Debug.
		if (debug)
		{
			Debug.DrawLine(forearm.position, elbowTarget.position, Color.yellow);
			Debug.DrawLine(upperArm.position, target.position, Color.red);
		}
	}
}
