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

	private float _stopDistance = 3f;

	private GameObject _currentTarget;
	private float shortestDistance = Mathf.Infinity;

	private bool _walkingState;
	private bool _attackingState;

    // Start is called before the first frame update
    void Start()
    {
		_navMeshAgent = this.GetComponent<NavMeshAgent>();
		_animate = this.GetComponent<Animator>();
		CollectVilligers();
    }

    // Update is called once per frame
    void Update()
    {
		if (_walkingState && _navMeshAgent.remainingDistance <= _stopDistance)
		{
			_walkingState = false;
			_attackingState = true;
			{
				_animate.SetBool("IsWalking", true);
			}
		}
		if (_attackingState && _navMeshAgent.remainingDistance <= _stopDistance)
		{
			CalculateShortestDistance();
			SetTarget();
		}
	}

	private void CalculateShortestDistance()
	{
		foreach (GameObject other in _villigers)
		{
			float tempDistance = Vector3.Distance(other.transform.position, this.gameObject.transform.position);
			if(tempDistance < shortestDistance)
			{
				shortestDistance = tempDistance;
				_currentTarget = other;
			}
		}
	}

	private void CollectVilligers()
	{
		_villigers = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "WeakViligerFemale(Clone)").ToList();
		var _tempList = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "WeakViligerMale(Clone)").ToList();
		_villigers.AddRange(_tempList);
	}

	private void SetTarget()
	{
		Vector3 targetVector = _currentTarget.transform.position;
		_navMeshAgent.SetDestination(targetVector);
		//_attackingState = true;
	}

	public void ZombieAwake()
	{
		Vector3 targetVector = StartingPoint.transform.position;
		_navMeshAgent.SetDestination(targetVector);
		_walkingState = true;
	}
}
