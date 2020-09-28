using System;
using UnityEngine;

public class DriftCamera : MonoBehaviour
{
	[Serializable]
	public class AdvancedOptions
	{
		public bool updateCameraInUpdate;
		public bool updateCameraInFixedUpdate;
		public bool updateCameraInLateUpdate;
	}

	public float smoothing = 15f;
	public Transform lookAtTarget;
	public Transform positionTarget;
    public Transform tr_Camera;
	public AdvancedOptions advancedOptions;

    private void Start()
    {
        
    }

    private void FixedUpdate ()
	{
		if (advancedOptions.updateCameraInFixedUpdate)
			UpdateCamera ();
	}

	private void LateUpdate ()
	{
		if (advancedOptions.updateCameraInLateUpdate)
			UpdateCamera ();
	}

    private void Update()
    {
        if(advancedOptions.updateCameraInUpdate)
        {
            UpdateCamera();
        }
    }

    private void UpdateCamera ()
	{
		transform.position = Vector3.Lerp (transform.position, positionTarget.position, Time.deltaTime * smoothing);
		transform.LookAt (lookAtTarget);
	}
}
