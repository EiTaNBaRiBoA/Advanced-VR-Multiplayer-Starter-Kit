using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour//NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private bool _waitForReset;
    private float _debugTime;

    private void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(Shoot);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!hasAuthority)
        //    return;

        //TryShoot();
    }
/*
    private void TryShoot()
    {
        if (primaryHand == null)
            return;
        if (primaryHand.GetInputScheme().Trigger.ReadValue<float>() < 0.8f)
        {
            _waitForReset = false;
            return;
        }
        if (_waitForReset)
            return;

        _waitForReset = true;
        Shoot();
    }*/

    //[Command]
    public void Shoot(ActivateEventArgs arg)
    {
        GameObject obj = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        //NetworkServer.Spawn(obj);
        
        //SoundManager.instance.Play("Shoot", bulletSpawn.position);
        //StartCoroutine(TactileVibrationTest());
    }

    /*IEnumerator TactileVibrationTest()
    {
        VRHand primaryHand = _grabbable.GetHandForTriggerId(0);
        
        primaryHand.Vibrate(.7f, 0.05f);
        yield return new WaitForSeconds(0.1f);
        primaryHand.Vibrate(1, 0.1f);
        yield return new WaitForSeconds(0.2f);
        primaryHand.Vibrate(.5f, 0.05f);
    }*/
}
