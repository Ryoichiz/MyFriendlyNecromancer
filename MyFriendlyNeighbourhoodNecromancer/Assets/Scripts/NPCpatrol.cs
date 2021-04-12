using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCpatrol : MonoBehaviour
{
	[SerializeField]
	LayerMask PlayerLayer;

	// Shows if agent is going to stop on each waypoint
	[SerializeField]
	bool _patrolStationary;

	// How long agent is supposed to wait at waypoint
	[SerializeField]
	float _waitingTime = 2f;

	// Chance of agent changing directions
	[SerializeField]
	float _changingProbability = 0.1f;

	// Waypoints for agent to follow
	[SerializeField]
	List<WayPoint> _points;

	// Animations for character
	[SerializeField]
	Animator _animate;

	// Detection distance
	[SerializeField]
	float _maxDistance = 1f;

	// Character stopping distance
	[SerializeField]
	float _stopDistance = 1.0f;

	NavMeshAgent _navMeshCharacter;
	Transform Player;
	int _currentPoint;
	bool _stationaryState;
	bool _walkingState;
	bool _patrolForwardState;
	bool _attackingState;
	bool _swingState;
	float _Timer;
	// Start is called before the first frame update
	void Start()
	{
		_navMeshCharacter = this.GetComponent<NavMeshAgent>();
		_animate = this.GetComponent<Animator>();
		Player = GameObject.Find("Player").transform;
		if (_navMeshCharacter == null)
		{
			Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
		}
		else
		{
			if (_points != null && _points.Count >= 2)
			{
				_currentPoint = 0;
				SetDestination();
			}
			else
			{
				Debug.Log("Not enough of waypoints for an agent");
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		//Player detection
		RaycastHit hit;
		Vector3 direction = Player.position - this.gameObject.transform.position;
		if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
		{
			if (hit.distance < _maxDistance && hit.transform.gameObject.layer == PlayerLayer)
			{
				float angel = Vector3.Angle(transform.position, direction);
				Debug.DrawRay(transform.position, direction, Color.red);
				if (Mathf.Abs(angel) > 90)
				{
					Debug.Log(hit.distance.ToString());
					Debug.DrawRay(transform.position, direction, Color.green);
					SetAttackDestination(hit.point);
				}
			}
			else
			{
				_animate.SetBool("IsSprinting", false);
				Debug.DrawRay(transform.position, direction, Color.red);
			}
			Debug.DrawRay(transform.position, direction, Color.red);
		}
		//Checking if patrol is close enough to player
		if (_attackingState && _navMeshCharacter.remainingDistance <= _stopDistance)
		{
			_attackingState = false;
			_swingState = true;
			_animate.SetBool("IsAttacking", true);
		}
		else
		{
			_swingState = false;
			_animate.SetBool("isAttacking", false);
		}
		//Checking distance from waypoint
		if (_walkingState && _navMeshCharacter.remainingDistance <= _stopDistance && !_attackingState && !_swingState)
		{
			_walkingState = false;

			//Checking if agent needs to wait
			if (_patrolStationary)
			{
				_animate.SetBool("IsStanding", true);
				_stationaryState = true;
				_Timer = 0f;
			}
			else
			{
				_animate.SetBool("IsWalking", true);
				ChangeDestination();
				SetDestination();
			}
		}

		//State if we are waiting
		if (_stationaryState && !_attackingState)
		{
			_Timer += Time.deltaTime;
			_animate.SetBool("IsWalking", false);
			if (_Timer >= _waitingTime)
			{

				_stationaryState = false;
				_animate.SetBool("IsStanding", false);
				ChangeDestination();
				SetDestination();
			}
		}
	}

	private void SetDestination()
	{
		if (_points != null)
		{
			Vector3 targetVector = _points[_currentPoint].transform.position;
			_navMeshCharacter.SetDestination(targetVector);
			_walkingState = true;
			_animate.SetBool("IsWalking", true);
		}
	}

	private void SetAttackDestination(Vector3 target)
	{
		_animate.SetBool("IsStanding", false);
		_animate.SetBool("IsWalking", false);
		_navMeshCharacter.SetDestination(target);
		_attackingState = true;
		_animate.SetBool("IsSprinting", true);
	}

	private void ChangeDestination()
	{
		if (UnityEngine.Random.Range(0f, 1f) <= _changingProbability)
		{
			_patrolForwardState = !_patrolForwardState;
		}

		if (_patrolForwardState)
		{
			_currentPoint = (_currentPoint + 1) % _points.Count;
		}
		else
		{
			if (--_currentPoint < 0)
			{
				_currentPoint = _points.Count - 1;
			}
		}
	}
}
