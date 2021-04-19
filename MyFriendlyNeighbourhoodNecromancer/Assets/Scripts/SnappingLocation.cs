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
	private GameObject _objname;
	

    // Start is called before the first frame update
    void Start()
    {
        objCheck = false;
		_objname = null;
    }

	private void Update()
	{
		if (_objname != null)
		{
			if (_objname.transform.position != this.gameObject.transform.position)
			{
				Pickup(_objname.GetComponent<Collider>());
			}
		}
	}

	private void OnTriggerStay(Collider other)
    {
		if (other.gameObject.layer == layer)
		{
			//Debug.Log("Layer passed");
			if (other.gameObject.CompareTag(snapTag) && !objCheck && !other.gameObject.GetComponent<Interactable>().attachedToHand)
			{
				//Debug.Log("Checking tag");
				if (_objname == null && other.gameObject.CompareTag(snapTag))
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
		_objname = null;
	}

    public void Place(Collider other)
    {
		Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
		rb.isKinematic = true;
		other.gameObject.transform.position = this.gameObject.transform.position;
        other.gameObject.transform.rotation = this.gameObject.transform.rotation;
		objCheck = true;
		_objname = other.gameObject;
    }

	public bool IsSnapped()
	{
		return objCheck;
	}

	public GameObject GetSnappedObject()
	{
		return _objname;
	}

	public string GetSnappedObjectName()
	{
		return _objname.name;
	}
}
