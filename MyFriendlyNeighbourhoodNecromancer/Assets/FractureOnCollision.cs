using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureOnCollision : MonoBehaviour
{
	[SerializeField] GameObject ShardsObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Shovel")
		{
			this.gameObject.GetComponent<MeshRenderer>().enabled = false;
			this.gameObject.GetComponent<BoxCollider>().enabled = false;
			ShardsObject.SetActive(true);
		}
	}
}
