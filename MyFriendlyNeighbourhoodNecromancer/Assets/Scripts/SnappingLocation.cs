using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SnappingLocation : MonoBehaviour
{
    private LayerMask layer = 12;

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

    // Update is called once per frame
    //private void OnTriggerStay(Collider other)
    //{
    //    //Down
    //    if (m_GrabAction.GetStateUp(m_Pose.inputSource) && objCheck)
    //    {
    //        Place(other);
    //    }
    //    //Up
    //    if (m_GrabAction.GetStateDown(m_Pose.inputSource) && objCheck)
    //    {
    //        Pickup(other);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
		if ((other.gameObject.layer == layer) && !objCheck && !other.gameObject.GetComponent<Interactable>().attachedToHand)
		{
			if (_objname.Equals("") && other.gameObject.CompareTag(snapTag))
			{
				Place(other);
			}
		}

    }

	private void OnTriggerExit(Collider other)
	{
		if ((other.gameObject.layer == layer && objCheck))
		{
			if (_objname == other.gameObject.name)
			{
				Pickup(other);
			}
		}
	}

    public void Pickup(Collider other)
    {
		Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
		rb.useGravity = true;
		rb.isKinematic = false;
		objCheck = false;
		_objname = "";
	}

    public void Place(Collider other)
    {
		Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
		rb.useGravity = false;
		rb.isKinematic = true;
		other.gameObject.transform.position = this.gameObject.transform.position;
        other.gameObject.transform.rotation = this.gameObject.transform.rotation;
        //other.gameObject.transform.localScale = this.gameObject.transform.localScale;
		objCheck = true;
		_objname = other.gameObject.name;
    }


}
