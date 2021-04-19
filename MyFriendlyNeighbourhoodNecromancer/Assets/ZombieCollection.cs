using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCollection : MonoBehaviour
{
	[SerializeField]
	List<GameObject> SnapLocations = new List<GameObject>();

	[SerializeField]
	List<GameObject> Torso = new List<GameObject>();

	[SerializeField]
	List<GameObject> Bottom = new List<GameObject>();

	[SerializeField]
	Transform SpawnLegs;

	[SerializeField]
	Transform SpawnTorso;

	private int _legsCount = 0;
	private bool _legs = false;
	private int _torsoCount = 0;
	private bool _torso = false;
	private bool _head = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in this.gameObject.transform)
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
			if (temp.GetSnappedObjectName() == "Leg 1 name" && temp2.GetSnappedObjectName() == "Leg 2 name")
			{
				Instantiate(Bottom[0], SpawnLegs.position, SpawnLegs.rotation);
				_legs = true;
			}
			if (temp.GetSnappedObjectName() == "Leg 1 name red" && temp2.GetSnappedObjectName() == "Leg 2 name red")
			{
				Instantiate(Bottom[1], SpawnLegs.position, SpawnLegs.rotation);
				_legs = true;
			}
		}
		if(SnapLocations[3].GetComponent<SnappingLocation>().IsSnapped() && SnapLocations[4].GetComponent<SnappingLocation>().IsSnapped() && !_torso)
		{
			SnappingLocation temp = SnapLocations[3].GetComponent<SnappingLocation>();
			temp.GetComponent<SnappingLocation>().GetSnappedObject().SetActive(false);
			SnapLocations[3].SetActive(false);
			SnappingLocation temp2 = SnapLocations[4].GetComponent<SnappingLocation>();
			temp2.GetComponent<SnappingLocation>().GetSnappedObject().SetActive(false);
			SnapLocations[4].SetActive(false);
			if (temp.GetSnappedObjectName() == "Arm 1 name" && temp2.GetSnappedObjectName() == "Arm 2 name")
			{
				Instantiate(Torso[0], SpawnTorso.position, SpawnTorso.rotation);
				_torso = true;
			}
			if (temp.GetSnappedObjectName() == "Arm 1 name red" && temp2.GetSnappedObjectName() == "Arm 2 name red")
			{
				Instantiate(Torso[1], SpawnTorso.position, SpawnTorso.rotation);
				_torso = true;
			}
		}
		if (SnapLocations[0].GetComponent<SnappingLocation>().IsSnapped())
		{
			SnapLocations[0].GetComponent<SnappingLocation>().GetSnappedObject().SetActive(false);
			SnapLocations[0].SetActive(false);
		}

	}
}
