using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class OnlyAuthorityComponents : MonoBehaviour
{
    public List<Behaviour> components;
    private NetworkIdentity _identity;

    // Start is called before the first frame update
    void Awake()
    {
        //Get the network identity on current or parent objects
        _identity = GetComponentInParent<NetworkIdentity>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var component in components)
        {
            component.enabled = _identity.hasAuthority;
        }
    }
}
