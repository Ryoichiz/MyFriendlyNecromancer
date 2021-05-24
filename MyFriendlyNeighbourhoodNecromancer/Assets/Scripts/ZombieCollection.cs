using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CartoonHeroes;

public class ZombieCollection : MonoBehaviour
{
	[SerializeField]
	List<GameObject> SnapLocations = new List<GameObject>();

	[SerializeField]
	List<GameObject> Torso = new List<GameObject>();

	[SerializeField]
	List<GameObject> Bottom = new List<GameObject>();

	[SerializeField]
	List<GameObject> IGameObjects = new List<GameObject>();

	[SerializeField]
	Transform SpawnLegs;

	[SerializeField]
	Transform SpawnTorso;

	[SerializeField]
	SetCharacter characterselection;

	SetCharacter.ItemGroup[] items = null;

	private int _legsCount = 0;
	private bool _legs = false;
	private int _torsoCount = 0;
	private bool _torso = false;
	private bool _head = false;
	private bool _collected = false;
	private bool _oneTimeRun = false;

	private ZombieController _zombieStatus;

    // Start is called before the first frame update
    void Start()
    {
		_zombieStatus = GameObject.Find("Zombie_1").GetComponent<ZombieController>();
		items = characterselection.GetAllItems();
		//		characterselection = 
		foreach (Transform child in this.gameObject.transform)
		{
			SnapLocations.Add(child.gameObject);
		}
    }

    // Update is called once per frame
    void Update()
    {
        if(SnapLocations[1].GetComponent<SnappingLocation>().IsSnapped() && SnapLocations[2].GetComponent<SnappingLocation>().IsSnapped() && !_legs)
		{
			SnappingLocation temp = SnapLocations[1].GetComponent<SnappingLocation>();
			temp.GetSnappedObject().SetActive(false);
			SnapLocations[1].SetActive(false);
			SnappingLocation temp2 = SnapLocations[2].GetComponent<SnappingLocation>();
			temp2.GetSnappedObject().SetActive(false);
			SnapLocations[2].SetActive(false);
			if (temp.GetSnappedObjectName() == "M_LeftLeg_Zombie_1")
			{
				_legsCount++;
			}
			if (temp2.GetSnappedObjectName() == "M_RightLeg_Zombie_1")
			{
				_legsCount++;
			}
			if (_legsCount >= 1)
			{
				IGameObjects.Add(Instantiate(Bottom[0], SpawnLegs.position, SpawnLegs.rotation));
			}
			else
			{
				IGameObjects.Add(Instantiate(Bottom[1], SpawnLegs.position, SpawnLegs.rotation));
			}
			_legs = true;
		}
		if(SnapLocations[3].GetComponent<SnappingLocation>().IsSnapped() && SnapLocations[4].GetComponent<SnappingLocation>().IsSnapped() && SnapLocations[5].GetComponent<SnappingLocation>().IsSnapped() && !_torso)
		{
			SnappingLocation temp = SnapLocations[3].GetComponent<SnappingLocation>();
			temp.GetSnappedObject().SetActive(false);
			SnapLocations[3].SetActive(false);
			SnappingLocation temp2 = SnapLocations[4].GetComponent<SnappingLocation>();
			temp2.GetSnappedObject().SetActive(false);
			SnapLocations[4].SetActive(false);
			SnappingLocation temp3 = SnapLocations[5].GetComponent<SnappingLocation>();
			temp3.GetSnappedObject().SetActive(false);
			SnapLocations[5].SetActive(false);
			if (temp.GetSnappedObjectName() == "M_LeftArm 1")
			{
				_torsoCount++;
			}
			if (temp2.GetSnappedObjectName() == "M_RightArm 1")
			{
				_torsoCount++;
			}
			if (temp3.GetSnappedObjectName() == "Zombie_Chest_1")
			{
				_torsoCount++;
			}
			if (_torsoCount >= 2)
			{
				IGameObjects.Add(Instantiate(Torso[0], SpawnTorso.position, SpawnTorso.rotation));
			}
			else
			{
				IGameObjects.Add(Instantiate(Torso[1], SpawnTorso.position, SpawnTorso.rotation));
			}
			_torso = true;
		}
		if (SnapLocations[0].GetComponent<SnappingLocation>().IsSnapped() && !_head)
		{
			SnapLocations[0].SetActive(false);
			_head = true;
		}
		if(_collected = true && CheckIfCollected() && !_oneTimeRun)
		{
			SnapLocations[0].GetComponent<SnappingLocation>().GetSnappedObject().SetActive(false);
			foreach(GameObject other in IGameObjects)
			{
				other.SetActive(false);
			}
			if (_legsCount >= 1)
			{
				//IGameObjects.Add(Instantiate(Bottom[0], SpawnLegs.position, SpawnLegs.rotation));
				characterselection.AddItem(items[0], 0);
				_zombieStatus.AddHealth(50);
			}
			else
			{
				//IGameObjects.Add(Instantiate(Bottom[1], SpawnLegs.position, SpawnLegs.rotation));
				characterselection.AddItem(items[0], 1);
				_zombieStatus.AddHealth(100);
			}
			if (SnapLocations[0].GetComponent<SnappingLocation>().GetSnappedObjectName() == "Male_Head_Blender")
			{
				characterselection.AddItem(items[2], 0);
				_zombieStatus.AddHealth(50);
			}
			else
			{
				characterselection.AddItem(items[2], 1);
				_zombieStatus.AddHealth(100);
			}
			if (_torsoCount >= 2)
			{
				//IGameObjects.Add(Instantiate(Torso[0], SpawnTorso.position, SpawnTorso.rotation));
				characterselection.AddItem(items[1], 0);
				_zombieStatus.AddHealth(50);
			}
			else
			{
				//IGameObjects.Add(Instantiate(Torso[1], SpawnTorso.position, SpawnTorso.rotation));
				characterselection.AddItem(items[1], 1);
				_zombieStatus.AddHealth(100);
			}
			_oneTimeRun = true;
		}

	}

	public bool CheckIfCollected()
	{
		if(_head && _torso && _legs)
		{
			_collected = true;
			return true;
		}
		else
		{
			return false;
		}
	}
}
