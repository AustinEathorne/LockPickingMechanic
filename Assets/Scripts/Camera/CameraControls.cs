using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic camera controller for orthographic camera

public class CameraControls : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private float minSize;
	[SerializeField]
	private float maxSize;
	[SerializeField]
	private float zoomStep;

	[SerializeField]
	private float minPosX;

	public float MinPosX {
		get {
			return minPosX;
		}

		set { this.minPosX = value; }
	}

	[SerializeField]
	private float maxPosX;

	public float MaxPosX {
		get {
			return maxPosX;
		}

		set { this.maxPosX = value; }
	}

	[SerializeField]
	private float minPosY;

	public float MinPosY {
		get {
			return minPosY;
		}

		set { this.minPosY = value; }
	}

	[SerializeField]
	private float maxPosY;

	public float MaxPosY {
		get {
			return maxPosY;
		}

		set { this.maxPosY = value; }
	}

	[SerializeField]
	private float moveStep;

	float posX;
	float posY;
	float posZ;

	private void Start()
	{
		this.posX = this.transform.position.x;
		this.posY = this.transform.position.y;
		this.posZ = this.transform.position.z;
	}

	private void Update()
	{
		this.UpdateZoom();
		this.UpdatePosition();
	}


	private void UpdateZoom()
	{
		if(Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			cam.orthographicSize -= zoomStep;
		}
		if(Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			cam.orthographicSize += zoomStep;
		}

		cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, this.minSize, this.maxSize);
	}

	private void UpdatePosition()
	{
		if(Input.GetKey(KeyCode.W))
		{
			this.posY += this.moveStep;
		}
		if(Input.GetKey(KeyCode.S))
		{
			this.posY -= this.moveStep;
		}
		if(Input.GetKey(KeyCode.A))
		{
			this.posX -= this.moveStep;
		}
		if(Input.GetKey(KeyCode.D))
		{
			this.posX += this.moveStep;
		}

		this.posY = Mathf.Clamp(this.posY, this.minPosY, this.maxPosY);
		this.posX = Mathf.Clamp(this.posX, this.minPosX, this.maxPosX);

		this.cam.transform.position = new Vector3(this.posX, this.posY, this.posZ);
	}
}
