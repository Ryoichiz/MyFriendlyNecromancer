using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
	public float m_Sensitivity = 0.1f;
	public float m_MaxSpeed = 1.0f;

	public SteamVR_Action_Boolean m_MovesPress = null;
	public SteamVR_Action_Vector2 m_MoveValue = null;

	private float m_Speed = 0.0f;

	private CharacterController m_CharacterController = null;
	private Transform m_CameraRig = null;
	private Transform m_Head = null;

	private void Awake()
	{
		m_CharacterController = GetComponent<CharacterController>();
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		CalculateMovement();
    }

	private void CalculateMovement()
	{
		// Movement
		Vector3 orientationEuler = new Vector3(0, transform.eulerAngles.y, 0);
		Quaternion orientation = Quaternion.Euler(orientationEuler);
		Vector3 movement = Vector3.zero;

		// No movement
		if (m_MovesPress.GetStateUp(SteamVR_Input_Sources.Any))
			m_Speed = 0;

		// Button press
		if(m_MovesPress.state)
		{
			// Add
			m_Speed += m_MoveValue.axis.y * m_Sensitivity;
			m_Speed = Mathf.Clamp(m_Speed, -m_MaxSpeed, m_MaxSpeed);

			// Orientation
			movement += orientation * (m_Speed * Vector3.forward) * Time.deltaTime;

		}

		m_CharacterController.Move(movement);
		// Apply
	}

	private void HandleController()
	{

	}


}
