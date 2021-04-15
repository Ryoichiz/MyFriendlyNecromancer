using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BackPackSnapping : MonoBehaviour
{
	public int layer = 12;

	private Interactable m_CurrentInteractable = null;
	private string _objname;
	private int _currentSlots = 4;
	private List<GameObject> _container = new List<GameObject>();
	private GameObject _previous = null;
	private Collider currentOther;

	// Start is called before the first frame update
	void Start()
	{
		_objname = "";
	}

	private void Update()
	{
		if(_container.Count != 0)
		{
			if(_container[_container.Count - 1].transform.position != this.gameObject.transform.position)
			{
				Pickup(_container[_container.Count - 1].GetComponent<Collider>());
				if (_container.Count != 0)
				{
					_previous = _container[_container.Count - 1];
				}
			}
		}
		if (currentOther.gameObject.layer == layer && !currentOther.gameObject.GetComponent<Interactable>().attachedToHand)
		{
			//Debug.Log("Checking tag");
			if (_container.Count <= _currentSlots && !_container.Contains(currentOther.gameObject))
			{
				//Debug.Log("Tag passed");
				Place(currentOther);
				if (_container.Count != 0)
				{
					_previous = _container[_container.Count - 1];
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<Interactable>())
		{
			currentOther = other;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == layer)
		{
			if (_objname == other.gameObject.name && other.gameObject.GetComponent<Interactable>().attachedToHand)
			{
				Debug.Log("Changing back");
				Pickup(other);
				if(_container.Count != 0)
				{
					_previous = _container[_container.Count - 1];
					currentOther = _previous.gameObject.GetComponent<Collider>();
				}
				else
				{
					currentOther = null;
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
		Vector3 scaledown = new Vector3(0.009999998f, 0.01f, 0.009999998f);
		other.gameObject.transform.localScale = scaledown;
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
