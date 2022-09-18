using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class HideOnAuthority : MonoBehaviour
{
    private NetworkIdentity _identity;
    private MeshRenderer _renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _identity = GetComponentInParent<NetworkIdentity>();
        _renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _renderer.enabled = (!_identity.hasAuthority);
    }
}
