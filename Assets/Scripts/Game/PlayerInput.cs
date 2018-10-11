using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour 
{
	[SerializeField]
	private LockManager gameManager;

	public bool isInputEnabled = false;
	public bool isForcing = false;

	private void Update()
	{
		if(isInputEnabled)
		{
			// move screwdriver forwards
			if(Input.GetKey(KeyCode.W))
			{
				isForcing = true;
				gameManager.MoveLock();
			}

			if(Input.GetKeyUp(KeyCode.W))
			{
				isForcing = false;
				gameManager.ReleasePressure();
			}

			if(!isForcing)
			{
				// move bobby pin left and right
				if(Input.GetKey(KeyCode.A))
				{
					gameManager.MovePin(false);
				}
				else if(Input.GetKey(KeyCode.D))
				{
					gameManager.MovePin(true);
				}
			}
		}
	}
}
