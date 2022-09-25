using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkXRGrab : NetworkBehaviour
{
    private NetworkIdentity _identity;
    
    private void Start()
    {
        //Get Identity in this object
        _identity = GetComponent<NetworkIdentity>();
        foreach (var interactable in GetComponentsInChildren<XRGrabInteractable>())
        {
            interactable.selectEntered.AddListener(Grabbed);
        }
    }

    private void Grabbed(SelectEnterEventArgs args)
    {
        ClaimAuthority();
    }

    [Command(requiresAuthority = false)]
    public void ClaimAuthority(NetworkConnectionToClient sender = null)
    {
        //Return authority to server
        _identity.RemoveClientAuthority();
        //Give authority to client who called command
        _identity.AssignClientAuthority(sender);
    }

    [Command(requiresAuthority = false)]
    public void ResignAuthority(NetworkConnectionToClient sender = null)
    {
        //Return authority to server
        _identity.RemoveClientAuthority();
    }
}
