using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour 
{
	public float minAngle = -90; // furthest angle we can reach -90 & 90
	public float maxAngle = 90;

	public float rotationStep = 0.1f;

	public float currentAngle = 0.0f;

	[SerializeField]
	private AudioClip aClip;
	[SerializeField]
	private AudioSource aSource;

	public void Start()
	{
		aSource.clip = aClip;
	}

	public void SetPinAngle(float angle)
	{
		currentAngle = angle;
		this.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, angle));
	}

	public void RotatePin(bool isClockwise)
	{
		// check if we can rotate (currentAngle) > -90 && < 90??
		// check direction
		if(isClockwise)
		{
			// check if we can move that way
			if(currentAngle < maxAngle)
			{
				if(!aSource.isPlaying)
					aSource.Play();
				currentAngle += rotationStep;
				this.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, currentAngle));
				//Debug.Log("Rotate pin right");
			}
		}
		else
		{
			if (currentAngle > minAngle)
			{
				if(!aSource.isPlaying)
					aSource.Play();
				currentAngle -= rotationStep;
				this.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, currentAngle));
				//Debug.Log("Rotate pin left");
			}
		}

		//Debug.Log("Current angle: " + currentAngle.ToString());
	}
}
