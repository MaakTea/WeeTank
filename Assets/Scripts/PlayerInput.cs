using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Tank tankScript;
    public Turret turretScript;

	public int gamePadIndex = -1;
    public Camera mouseCam;
	public float minTargetDistance = 10;

	public bool hitValid;
    public RaycastHit hit;


    // Use this for initialization
    void Start () 
    {
        if (mouseCam == null)
            mouseCam = Camera.main;
    }

	// Update is called once per frame
	void Update()
	{
		switch (gamePadIndex)
		{
			case -1:
				{
					float x = Input.GetAxis("Horizontal");
					float y = Input.GetAxis("Vertical");

					tankScript.steering = Mathf.Clamp(x, -1, 1);
					tankScript.gas = Mathf.Clamp(y, -1, 1);

					float mouseX = Input.GetAxisRaw("Mouse X");

					Vector3 mousePos = Input.mousePosition;
					Quaternion toMouseDir = Quaternion.Euler(transform.position - mousePos);

					Ray ray = mouseCam.ScreenPointToRay(Input.mousePosition);
					// -- Draw a ray from camera to ground --
					//Debug.DrawRay(ray.origin, ray.direction * mouseCam.farClipPlane, Color.gray);

					hitValid = Physics.Raycast(ray, out hit);

					turretScript.targetValid = hitValid && (hit.point - turretScript.transform.position).magnitude >= minTargetDistance;
					turretScript.targetPos = hit.point;
					turretScript.fireInput = Input.GetMouseButton(0);
				}
				break;

			default:
				{
					Vector2 leftStick = GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.LeftStick, (GamepadInput.GamePad.Index)gamePadIndex);
					//remap circle to square
					leftStick = DiscToSquare(leftStick);
					Vector2 dPad = GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.Dpad, (GamepadInput.GamePad.Index)gamePadIndex);
					if (dPad.sqrMagnitude > 0.1f)
						dPad = DiscToSquare(dPad.normalized);
					Vector2 moveInput = leftStick + dPad;

					tankScript.steering = Mathf.Clamp(moveInput.x, -1, 1);
					tankScript.gas = Mathf.Clamp(moveInput.y, -1, 1);

					Vector2 rightStick = GamepadInput.GamePad.GetAxis(GamepadInput.GamePad.Axis.RightStick, (GamepadInput.GamePad.Index)gamePadIndex);
					//turretScript.targetPos = turretScript.transform.position + //new Vector3(rightStick.x, 0, rightStick.y) * 20.0f;
					Vector3 aimInput = turretScript.targetValid ? turretScript.targetPos - turretScript.transform.position : Vector3.zero;
					aimInput = Vector3.Lerp(aimInput, new Vector3(rightStick.x, 0, rightStick.y) * 20.0f, 0.25f);
					turretScript.targetPos = turretScript.transform.position + aimInput;
					turretScript.targetValid = rightStick.sqrMagnitude > 0.02f;
					turretScript.fireInput = GamepadInput.GamePad.GetTrigger(GamepadInput.GamePad.Trigger.RightTrigger, (GamepadInput.GamePad.Index)gamePadIndex) > 0.2f;
				}
				break;
		}

    }

	//from https://arxiv.org/ftp/arxiv/papers/1509/1509.06344.pdf
	public Vector2 DiscToSquare(Vector2 uv)
	{
		uv = Vector2.ClampMagnitude(uv, 1.0f);
		float u = uv.x;
		float v = uv.y;
		float twosqrtwo = 2.0f * Mathf.Sqrt(2.0f);
		float x = 0.5f * Mathf.Sqrt(2.0f + u * u - v * v + twosqrtwo * u)
			    - 0.5f * Mathf.Sqrt(2.0f + u * u - v * v - twosqrtwo * u);
		float y = 0.5f * Mathf.Sqrt(2.0f - u * u + v * v + twosqrtwo * v)
				- 0.5f * Mathf.Sqrt(2.0f - u * u + v * v - twosqrtwo * v);
		return new Vector2(x, y);
	}

	public Vector2 SquareToDisc(Vector2 xy)
	{
		float x = xy.x;
		float y = xy.y;
		float u = x * Mathf.Sqrt(1.0f - y * y * 0.5f);
		float v = y * Mathf.Sqrt(1.0f - x * x * 0.5f);
		return new Vector2(u, v);
	}
}
