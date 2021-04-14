using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SnappingLocation : MonoBehaviour
{
    public int layer = 12;

	public string snapTag;
    private Interactable m_CurrentInteractable = null;
    private bool objCheck;
	private string _objname;
	

    // Start is called before the first frame update
    void Start()
    {
        objCheck = false;
		_objname = "";
    }

    private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.layer == layer)
		{
			//Debug.Log("Layer passed");
			if (other.gameObject.CompareTag(snapTag) && !objCheck && !other.gameObject.GetComponent<Interactable>().attachedToHand)
			{
				//Debug.Log("Checking tag");
				if (_objname.Equals("") && other.gameObject.CompareTag(snapTag))
				{
					//Debug.Log("Tag passed");
					Place(other);
				}
			}
		}
    }

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == layer && objCheck)
		{
			Pickup(other);
		}
	}

    public void Pickup(Collider other)
    {
		Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		objCheck = false;
		_objname = "";
	}

    public void Place(Collider other)
    {
		Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
		rb.isKinematic = true;
		other.gameObject.transform.position = this.gameObject.transform.position;
        other.gameObject.transform.rotation = this.gameObject.transform.rotation;
		objCheck = true;
		_objname = other.gameObject.name;
    }

	public bool IsSnapped()
	{
		return objCheck;
	}

}
