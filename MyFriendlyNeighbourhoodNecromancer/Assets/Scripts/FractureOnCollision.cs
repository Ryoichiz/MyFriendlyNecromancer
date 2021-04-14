using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureOnCollision : MonoBehaviour
{
	[SerializeField] GameObject ShardsObject;
	[SerializeField] GameObject[] Parts;
	private float Offset = 0.4f;
    // Start is called before the first frame update

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Shovel")
		{
			this.gameObject.GetComponent<MeshRenderer>().enabled = false;
			this.gameObject.GetComponent<BoxCollider>().enabled = false;
			ShardsObject.SetActive(true);
			SpawnBodyPart();
		}
	}

	private void SpawnBodyPart()
	{
		int Rng = Random.Range(0, Parts.Length - 1);
		Vector3 Position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + Offset,
			this.gameObject.transform.position.z);
		Instantiate(Parts[Rng], Position, Quaternion.identity);
	}
}
