using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PeopleGeneration : MonoBehaviour
{
    public int Amount = 5;
    public GameObject[] NPCs;
    public GameObject Spot;
    private Vector3[] SpawningSpots;
    // Start is called before the first frame update
    void Start()
    {
        int j = 0;
        int RngH;
        int RngNPC;
        SpawningSpots = new Vector3[Spot.transform.childCount];
        foreach (Transform child in Spot.transform)
        {
            Debug.Log(child.localPosition.x);
            SpawningSpots[j] = new Vector3(child.position.x, child.position.y, child.position.z);
            j++;
        }
        for(int i = 0; i < Amount; i++)
        {
            RngH = Random.Range(0, SpawningSpots.Length);
            RngNPC = Random.Range(0, NPCs.Length);
            Instantiate(NPCs[RngNPC], SpawningSpots[RngH], Quaternion.identity);
        }
    }
}
