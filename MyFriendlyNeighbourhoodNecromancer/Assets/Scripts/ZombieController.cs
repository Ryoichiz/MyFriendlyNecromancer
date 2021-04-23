using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
	[SerializeField]
	WayPoint StartingPoint;

	[SerializeField]
	ZombieCollection collection;

	[SerializeField]
	Animator _animate;

	[SerializeField]
	List<GameObject> _villigers = new List<GameObject>();

	NavMeshAgent _navMeshAgent;

	private int oneTimeRun = 0;

	private float _stopDistance = 3f;

	private float rotationSpeed = 10f;

	private GameObject _currentTarget;
	private float shortestDistance = Mathf.Infinity;

	private float _health = 500;

	private float _attackDamage = 20f;

	private float _attackLength = 1f;

	private bool _walkingState;
	private bool _attackingState;
	private bool _swingState;
	private bool _isDead;

	float _Timer;

	// Start is called before the first frame update
	void Start()
    {
		_isDead = false;
		_swingState = false;
		_navMeshAgent = this.GetComponent<NavMeshAgent>();
		_animate = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		if (collection.CheckIfCollected() && oneTimeRun == 0)
		{
			oneTimeRun++;
			CollectVilligers();
			ZombieAwake();
		}
		if (!_isDead && oneTimeRun == 1)
		{
			CalculateShortestDistance();
			if (_walkingState && _navMeshAgent.remainingDistance <= _stopDistance)
			{
				_walkingState = false;
				_attackingState = true;
				//CalculateShortestDistance();
				{
					_animate.SetBool("IsWalking", true);
				}
				SetTarget();
			}
			if (_attackingState && shortestDistance >= _stopDistance)
			{
				SetTarget();
			}
			if (!_walkingState && shortestDistance <= _stopDistance && !_swingState)
			{
				//Debug.Log(_navMeshAgent.remainingDistance);
				_Timer = 0;
				_swingState = true;
			}
			if (_swingState)
			{
				RotateTowards(_currentTarget.transform);
				_Timer += Time.deltaTime;
				if (_Timer >= _attackLength)
				{
					_swingState = false;
					VilligerPatrol temp = _currentTarget.GetComponent<VilligerPatrol>();
					temp.TakeDamage(_attackDamage);
					if (temp.CheckIfDead())
					{
						_villigers.Remove(_currentTarget);
						CalculateShortestDistance();
					}
				}
			}
			if (_health <= 0)
			{
				_isDead = true;
			}
			//RotateTowards(_currentTarget.transform);
		}
		else if (_isDead && oneTimeRun == 1)
		{
			_animate.SetBool("IsDying", true);
		}
	}

	private void CalculateShortestDistance()
	{
		shortestDistance = Mathf.Infinity;
		foreach (GameObject other in _villigers)
		{
			//if (other.GetComponent<VilligerPatrol>().CheckIfDead())
			//{
				float tempDistance = Vector3.Distance(other.transform.position, this.gameObject.transform.position);
				if (tempDistance < shortestDistance)
				{
					shortestDistance = tempDistance;
					_currentTarget = other;
				}
			//}
		}
	}

	private void CollectVilligers()
	{
		_villigers = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "WeakViligerFemale(Clone)").ToList();
		var _tempList = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "WeakVilligerMale(Clone)").ToList();
		_villigers.AddRange(_tempList);
	}

	private void SetTarget()
	{
		Vector3 targetVector = _currentTarget.transform.position;
		_navMeshAgent.SetDestination(targetVector);
		_animate.SetBool("IsWalking", true);
		_animate.SetBool("IsAttacking", false);
		//_attackingState = true;
	}

	public void ZombieAwake()
	{
		Vector3 targetVector = StartingPoint.transform.position;
		_navMeshAgent.SetDestination(targetVector);
		_walkingState = true;
		_animate.SetBool("IsWalking", true);
	}

	private void RotateTowards(Transform target)
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
		_animate.SetBool("IsAttacking", true);
		_animate.SetBool("IsWalking", false);
	}

	public float GetHealth()
	{
		return _health;
	}

	public void SetHealth(float value)
	{
		_health = value;
	}

	public void AddHealth(float value)
	{
		_health += value;
	}

	public void TakeDamage(float value)
	{
		_health -= value;
		Debug.Log(_health);
	}

	public bool CheckIfDead()
	{
		return _isDead;
	}
}
