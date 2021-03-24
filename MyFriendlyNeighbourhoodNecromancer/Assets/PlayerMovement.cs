using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
	public float m_Sensitivity = 0.1f;
	public float m_MaxSpeed = 1.0f;
	public float m_Gravity = 30f;

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
		Quaternion orientation = CalculateOrientation();
		Vector3 movement = Vector3.zero;

		// No movement
		if (m_MoveValue.axis.magnitude == 0)
			m_Speed = 0;

			// Add
			m_Speed += m_MoveValue.axis.magnitude * m_Sensitivity;
			m_Speed = Mathf.Clamp(m_Speed, -m_MaxSpeed, m_MaxSpeed);

			// Orientation
			movement += orientation * (m_Speed * Vector3.forward);

		movement.y -= m_Gravity * Time.deltaTime;

		// Apply
		m_CharacterController.Move(movement * Time.deltaTime);
	}

	private Quaternion CalculateOrientation()
	{
		float rotation = Mathf.Atan2(m_MoveValue.axis.x, m_MoveValue.axis.y);
		rotation *= Mathf.Rad2Deg;

		Vector3 orientationEuler = new Vector3(0, m_Head.eulerAngles.y + rotation, 0);
		return Quaternion.Euler(orientationEuler);
	}

	private void HandleController()
	{

	}


}
