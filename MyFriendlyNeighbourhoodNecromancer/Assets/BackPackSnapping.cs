using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BackPackSnapping : MonoBehaviour
{
	public int layer = 12;

	public string snapTag;
	private Interactable m_CurrentInteractable = null;
	private string _objname;
	private int _currentSlots = 4;
	private List<GameObject> _container = new List<GameObject>();
	private GameObject _previous = null;

	// Start is called before the first frame update
	void Start()
	{
		_objname = "";
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == layer)
		{
			Debug.Log("Layer passed");
			if (!other.gameObject.GetComponent<Interactable>().attachedToHand)
			{
				Debug.Log("Checking tag");
				if (other.gameObject.CompareTag(snapTag) && _container.Count <= _currentSlots && !_container.Contains(other.gameObject))
				{
					Debug.Log("Tag passed");
					Place(other);
					if(_container.Count != 0)
					{
						_previous = _container[_container.Count-1];
					}
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == layer)
		{
			if (_objname == other.gameObject.name && other.gameObject.GetComponent<Interactable>().attachedToHand)
			{
				Debug.Log("Changing");
				Pickup(other);
				if(_container.Count != 0)
				{
					_previous = _container[_container.Count - 1];
				}
			}
		}
	}

	public void Pickup(Collider other)
	{
		this.gameObject.transform.DetachChildren();
		Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		_objname = "";
		if (_container.Count != 0)
		{
			GameObject temp = null;
			temp = _container.Find(x => x.name == _previous.name);
			temp.SetActive(true);
			_objname = temp.name;
		}
		_container.Remove(other.gameObject);
	}

	public void Place(Collider other)
	{
		other.gameObject.transform.SetParent(this.gameObject.transform);
		Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
		rb.isKinematic = true;
		other.gameObject.transform.position = this.gameObject.transform.position;
		other.gameObject.transform.rotation = this.gameObject.transform.rotation;
		_objname = other.gameObject.name;
		if (_container.Count != 0)
		{
			GameObject temp = null;
			temp = _container.Find(x => x.name == _previous.name);
			temp.SetActive(false);
		}
		_container.Add(other.gameObject);
	}
}
