using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
	[SerializeField] Transform orientation;

	[Header("Camera")]
	[SerializeField] private Camera cam;
	[SerializeField] private float fov;
	[SerializeField] private float wallRunFov;
	[SerializeField] private float wallRunFovTime;
	[SerializeField] private float camTilt;
	[SerializeField] private float camTiltTime;

	public float tilt {get; private set;}

	[Header("Detection")]
	[SerializeField] float wallDistance = 0.5f;
	[SerializeField] float minimumJumpHeight = 1.5f;

	bool wallLeft = false;
	bool wallRight = false;

	[Header("Wall Running")]
	[SerializeField] private float wallRunGravity;
	[SerializeField] private float wallRunJumpForce;

	RaycastHit leftWallHit;
	RaycastHit rightWallHit;

	private Rigidbody rb;

	private void Start(){
		rb = GetComponent<Rigidbody>();
	}

	bool CanWallRun()
	{
		return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
	}

	void CheckWall()
	{
		wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit,  wallDistance);
		wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit,  wallDistance);
	}

	private void Update()
	{
		CheckWall();

		if(CanWallRun())
		{
			if(wallLeft)
			{
				StartWallRun();
				Debug.Log("Wall running on the left");
			}
			else if(wallRight)
			{
				StartWallRun();
				Debug.Log("Wall running on the Right");
			}
			else
			{
				StopWallRun();
			}
		}
		else
		{
			StopWallRun();
		}
	}

	void StartWallRun()
	{
		rb.useGravity = false;

		rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

		if(wallLeft)
		{
			tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
		}
		else if(wallRight)
		{
			tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(wallLeft)
			{
				Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
				rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
				rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
			}
			if(wallRight)
			{
				Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
				rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
				rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
			}
		}
	}

	void StopWallRun()
	{
		rb.useGravity = true;
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
		tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
	}
}
