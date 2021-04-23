using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
	List<GameObject> _villigers = new List<GameObject>();
	ZombieController zombie;
    // Start is called before the first frame update
    void Start()
    {
        zombie = GameObject.Find("Zombie_1").GetComponent<ZombieController>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void CollectVilligers()
	{
		_villigers = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "WeakViligerFemale(Clone)").ToList();
		var _tempList = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "WeakVilligerMale(Clone)").ToList();
		_villigers.AddRange(_tempList);
	}
}
