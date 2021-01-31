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

					tankScript.steering = x;
					tankScript.gas = y;

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
					tankScript.steering = leftStick.x;
					tankScript.gas = leftStick.y;

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
}
