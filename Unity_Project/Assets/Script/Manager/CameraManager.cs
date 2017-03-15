using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public static CameraManager Instance = null;

    private UnityEngine.PostProcessing.PostProcessingProfile postProcessProfile;
	private UnityEngine.PostProcessing.ChromaticAberrationModel.Settings ChromSettings;

    [Header("ChromaticAberation")]
	public float ChromaticAberationDuration = 1.0f;
	public float ChromaticAberationIntensity = 1.0f;

    private Transform ThisTransform;

    void Start () {
		if (Instance == null) 
		{
			Instance = this;
			ThisTransform = transform;
			UnityEngine.PostProcessing.PostProcessingBehaviour behavior = GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour> ();
			if (behavior != null)
				postProcessProfile = behavior.profile;
			
			ChromSettings = postProcessProfile.chromaticAberration.settings;
		} 
		else 
		{
			Destroy (gameObject);
		}
    }
	
	public IEnumerator ChromaticAberationShake()
	{
		float time = 0;
		while( time < ChromaticAberationDuration)
		{
			float randomPoint = Random.Range(0.0f,1.0f) * ChromaticAberationIntensity;
			ChromSettings.intensity = randomPoint;
			postProcessProfile.chromaticAberration.settings = ChromSettings;
			time += Time.deltaTime;
			yield return null;
		}
		ChromSettings.intensity = 0.0f;
		postProcessProfile.chromaticAberration.settings = ChromSettings;
	}
}