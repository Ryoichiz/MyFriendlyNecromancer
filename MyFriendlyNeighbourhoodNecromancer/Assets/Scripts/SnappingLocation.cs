using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SnappingLocation : MonoBehaviour
{
    private LayerMask layer = 12;

    public SteamVR_Action_Boolean m_GrabAction = null;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private Interactable m_CurrentInteractable = null;
    private List<Interactable> m_ContactInteractables = new List<Interactable>();
    private bool objCheck;

    // Start is called before the first frame update
    void Start()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        bool objCheck = false;
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        //Down
        if (m_GrabAction.GetStateUp(m_Pose.inputSource) && objCheck)
        {
            Place(other);
        }
        //Up
        if (m_GrabAction.GetStateDown(m_Pose.inputSource) && objCheck)
        {
            Pickup(other);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layer)
        {
            return;
        }
        m_ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layer)
        {
            return;
        }
        m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
        objCheck = true;
    }

    public void Pickup(Collider other)
    {
        m_ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());
    }

    public void Place(Collider other)
    {
        other.gameObject.transform.position = this.gameObject.transform.position;
        other.gameObject.transform.rotation = this.gameObject.transform.rotation;
        other.gameObject.transform.localScale = this.gameObject.transform.localScale;
    }


}
