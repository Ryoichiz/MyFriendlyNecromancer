using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class VilligerPatrol : MonoBehaviour
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

	[SerializeField]
	GameObject ParentPoint;

	// Waypoints for agent to follow
	private List<WayPoint> _points = new List<WayPoint>();

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
		ParentPoint = GameObject.Find("VillageWaypoints");
		foreach (Transform child in ParentPoint.transform)
		{
			_points.Add(child.gameObject.GetComponent<WayPoint>());
		}
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

	// Changes next patrol point into a different one, making agent not always predictable
	// allowing agent to walk forward and backward
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
