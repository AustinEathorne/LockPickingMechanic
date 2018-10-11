using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour 
{
	// Bobby Pin
	[SerializeField]
	public Pin gamePin;

	[SerializeField]
	private List<float> sweetSpotRangeValues; // sweet spot range value for each difficulty

	private float currentSweetSpotMin;
	private float currentSweetSpotMax;

	// Lock
	private float minAngle = 0.0f;
	private float maxAngle = -90.0f;
	private float currentAngle = 0.0f;
	private float currentMaxAngle = -20.0f;

	[SerializeField]
	private float rotationStep = 1.0f;
	[SerializeField]
	private float resetStep = 0.5f;

	[SerializeField]
	private Transform lockPivot;

	[SerializeField]
	private List<float> proximityRanges; // 20, 40, 60
	[SerializeField]
	private List<float> lockTurnAngleLimits; // -30, -60, -90

	[SerializeField]
	private List<float> forceTimes; // easy/med/hard(10, 5, 2.5)

	public bool isForcing = false; // when the screwdriver is pushed to its limit
	private float forceTime = 0.0f; // time where the screwdrive is held at is maximum

	public LockManager gameManager;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip squeakClip;
	[SerializeField]
	private AudioClip unlockClip;
	[SerializeField]
	private AudioClip breakClip;

	private bool isSqueaking = false;



	private void Update()
	{
		// Constantly rotate the lock back to starting rotation
		if(currentAngle < minAngle)
		{
			currentAngle += this.resetStep;
			this.lockPivot.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, currentAngle));
		}

		// update force time if we are forcing the lock
		if(isForcing)
		{
			forceTime += Time.deltaTime;


			if(forceTime >= this.forceTimes[(int)this.gameManager.gameDifficulty])
			{
				forceTime = 0.0f;
				isForcing = false;
				this.gameManager.StartCoroutine(this.gameManager.BreakPin());
				this.gamePin.SetPinAngle(0.0f);

				this.PlayBreak();
			}
		}
		else
		{
			forceTime = 0.0f;
		}
	}


	public void SetupLock(LockManager.Difficulty _diff)
	{
		// get random point between pin's min and max angle (+ or - the range values to ensure the pickable range stays within pick movement range)
		float ranAngle = Random.Range(gamePin.minAngle + sweetSpotRangeValues[(int)_diff], gamePin.maxAngle - sweetSpotRangeValues[(int)_diff]);

		// Debug.Log("Ran Angle: " + ranAngle.ToString());

		// set our min & max for the sweet spot
		currentSweetSpotMin = ranAngle - sweetSpotRangeValues[(int)_diff];
		currentSweetSpotMax = ranAngle + sweetSpotRangeValues[(int)_diff];

		Debug.Log("Sweet spot: " + currentSweetSpotMin.ToString() + ", " + currentSweetSpotMax.ToString());

		// Reset pin angle
		this.gamePin.SetPinAngle(0.0f);
	}


	public void RotateLock()
	{
		// Find the pin's proximity to the sweet spot
		float distance = 0.0f;
		if(gamePin.currentAngle < currentSweetSpotMin) // left of the sweet spot
		{
			distance = gamePin.currentAngle - currentSweetSpotMin;
		}
		else if(gamePin.currentAngle > currentSweetSpotMax) // right of the sweet spot
		{
			distance = gamePin.currentAngle - currentSweetSpotMax;
		}
		else // in the sweetspot!
		{
			distance = 0.0f;
		}

        //Debug.Log("Distance: " + distance.ToString());

		// check how far we can rotate based off the pin's proximity to the sweet spot
		float absoluteValue = Mathf.Abs(distance);
		if(distance == 0.0f) // within the sweet spot
		{
			currentMaxAngle = lockTurnAngleLimits[3];
			//Debug.Log("Within the sweetspot");
		}
		else if(absoluteValue < proximityRanges[0]) // within the first range
		{
			currentMaxAngle = lockTurnAngleLimits[2];
			//Debug.Log("Within the first proximity range");
		}
		else if(absoluteValue < proximityRanges[1]) // "" second range
		{
			currentMaxAngle = lockTurnAngleLimits[1];
			//Debug.Log("Within the second proximity range");
		}
		else if(absoluteValue < proximityRanges[2]) // "" third range
		{
			currentMaxAngle = lockTurnAngleLimits[0];
			//Debug.Log("Within the third proximity range");
		}

		// Check to see whether we've reached our rotation limit
		if(currentAngle > currentMaxAngle)
		{
			currentAngle -= this.rotationStep;
			this.PlaySqueak();
		}
		else if(currentMaxAngle == lockTurnAngleLimits[3]) // we can't move any more, and our limit is the lock max
		{
			//Debug.Log("Lock Cracked!");
			this.StopSqueak();
			this.PlayUnlock();
			this.gameManager.StartCoroutine(this.gameManager.EndGame(true, false));
		}
		else if(currentAngle <= currentMaxAngle)
		{
			isForcing = true;
			this.StopSqueak();
			//Debug.Log("Is Forcing!");
		}
	}

	public void ReleasePressure()
	{
		this.isForcing = false;
		this.StopSqueak();
		//Debug.Log("Pressure released");s
	}


	// Audio
	public void PlaySqueak()
	{
		if(!isSqueaking)
		{
			isSqueaking = true;
			audioSource.clip = squeakClip;
			audioSource.loop = true;
			audioSource.Play();
		}
	}

	public void StopSqueak()
	{
		if(isSqueaking)
		{
			isSqueaking = false;
			audioSource.loop = false;
			audioSource.Stop();
		}
	}

	public void PlayUnlock()
	{
		audioSource.clip = unlockClip;
		audioSource.loop = false;
		audioSource.Play();
	}

	public void PlayBreak()
	{
		audioSource.clip = breakClip;
		audioSource.loop = false;
		audioSource.Play();
	}
}
