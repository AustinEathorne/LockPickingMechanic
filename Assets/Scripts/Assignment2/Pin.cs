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

    [SerializeField]
    private Vector3 resetPosition;
    [SerializeField]
    private Vector3 resetEulerAngles;

    private Quaternion resetRotation;

    [SerializeField]
    private Rigidbody pinRigidBody;
    [SerializeField]
    private Vector3 forcePosition;
    [SerializeField]
    private Vector3 launchDirection;
    [SerializeField]
    [Range(0, 500)]
    private float launchForce;



	public void Start()
	{
		aSource.clip = aClip;
        this.resetRotation.eulerAngles = this.resetEulerAngles; 
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

    public void LaunchPin()
    { 
        this.pinRigidBody.useGravity = true;
        this.pinRigidBody.AddForceAtPosition(this.launchDirection * this.launchForce, this.forcePosition);
    }

    public void ResetPin()
    {
        this.pinRigidBody.useGravity = false;
        this.pinRigidBody.velocity = Vector3.zero;
        this.pinRigidBody.angularVelocity = Vector3.zero;
        this.pinRigidBody.transform.localPosition = this.resetPosition;
        this.pinRigidBody.transform.localRotation = this.resetRotation;
    }
}
