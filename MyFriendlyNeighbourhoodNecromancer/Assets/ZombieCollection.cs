using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCollection : MonoBehaviour
{
	[SerializeField]
	List<GameObject> SnapLocations = new List<GameObject>();

	[SerializeField]
	GameObject Torso;

	[SerializeField]
	GameObject Bottom;

	[SerializeField]
	Transform SpawnLegs;

	[SerializeField]
	Transform SpawnTorso;

	private bool _legs = false;
	private bool _torso = false;
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
			SnapLocations[1].GetComponent<SnappingLocation>().GetSnappedObject().SetActive(false);
			SnapLocations[1].SetActive(false);
			SnapLocations[2].GetComponent<SnappingLocation>().GetSnappedObject().SetActive(false);
			SnapLocations[2].SetActive(false);
			Instantiate(Bottom, SpawnLegs.position, SpawnLegs.rotation);
			_legs = true;
		}
		if(SnapLocations[3].GetComponent<SnappingLocation>().IsSnapped() && SnapLocations[4].GetComponent<SnappingLocation>().IsSnapped() &&
			SnapLocations[5].GetComponent<SnappingLocation>().IsSnapped() && !_torso)
		{
			SnapLocations[3].GetComponent<SnappingLocation>().GetSnappedObject().SetActive(false);
			SnapLocations[3].SetActive(false);
			SnapLocations[4].GetComponent<SnappingLocation>().GetSnappedObject().SetActive(false);
			SnapLocations[4].SetActive(false);
			SnapLocations[5].GetComponent<SnappingLocation>().GetSnappedObject().SetActive(false);
			SnapLocations[5].SetActive(false);
			Instantiate(Torso, SpawnTorso.position, SpawnTorso.rotation);
			_torso = true;
		}

	}
}
