using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class VilligerPatrol : MonoBehaviour
{
	[SerializeField]
	LayerMask IgnoreLayer;

	[SerializeField]
	int ZombieLayer = 15;

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
	float _stopDistance = 2.0f;

	[SerializeField]
	float rotationSpeed = 10f;

	NavMeshAgent _navMeshCharacter;
	ZombieController Zombie;
	Transform Player;
	int _currentPoint;
	bool _stationaryState;
	bool _walkingState;
	bool _patrolForwardState;
	bool _attackingState;
	bool _swingState;
	bool _isDead;
	float _Timer;

	private float _health = 100;

	private float _attackLength = 1f;

	private float _attackDamage = 25f;
	// Start is called before the first frame update
	void Start()
    {
		_isDead = false;
		_navMeshCharacter = this.GetComponent<NavMeshAgent>();
		_animate = this.GetComponent<Animator>();
		Player = GameObject.Find("Zombie_1").GetComponent<Collider>().transform;
		Zombie = GameObject.Find("Zombie_1").GetComponent<ZombieController>();
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
		if (!_isDead)
		{
			RaycastHit hit;
			Vector3 direction = Player.position - this.gameObject.transform.position;
			if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, ~IgnoreLayer))
			{
				//Debug.Log(hit.transform.gameObject.layer);
				//Debug.Log(hit.distance.ToString());
				if (hit.distance < _maxDistance && hit.transform.gameObject.layer == ZombieLayer)
				{
					if (!_swingState && hit.distance >= _stopDistance)
					{
						//Debug.Log(hit.distance.ToString());
						Debug.DrawRay(transform.position, direction, Color.green);
						SetAttackDestination(hit.point);
					}
				}
				else
				{
					_animate.SetBool("IsSprinting", false);
					Debug.DrawRay(transform.position, direction, Color.red);
				}

			}
			if (!Zombie.CheckIfDead())
			{
				if (_attackingState && _navMeshCharacter.remainingDistance <= _stopDistance && !_swingState)
				{
					RotateTowards(Player);
					_swingState = true;
					_animate.SetBool("IsSprinting", false);
					_animate.SetBool("IsAttacking", true);
					_navMeshCharacter.speed = 0.75f;
					_Timer = 0f;
				}
				else
				{
					//_attackingState = false;
					//_swingState = false;
					_walkingState = true;
					_animate.SetBool("isAttacking", false);
				}
				if (_swingState)
				{
					_Timer += Time.deltaTime;
					if (_Timer >= _attackLength)
					{
						_swingState = false;
						Zombie.TakeDamage(_attackDamage);
					}
				}
			}
			else
			{
				_swingState = false;
				_attackingState = false;
				_walkingState = true;
			}
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

			if (_health <= 0)
			{
				_isDead = true;
			}
		}
		else
		{
			_navMeshCharacter.ResetPath();
			_animate.SetBool("IsWalking", false);
			_animate.SetBool("IsDying", true);
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
		RotateTowards(Player);
		_navMeshCharacter.SetDestination(target);
		_attackingState = true;
		_animate.SetBool("IsSprinting", true);
		_navMeshCharacter.speed = 0.3f;
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

	private void RotateTowards(Transform target)
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

	}

	public float GetHealth()
	{
		return _health;
	}

	public void SetHealth(float value)
	{
		_health = value;
	}

	public void TakeDamage(float value)
	{
		_health -= value;
		//Debug.Log(_health);
	}

	public bool CheckIfDead()
	{
		return _isDead;
	}
}
