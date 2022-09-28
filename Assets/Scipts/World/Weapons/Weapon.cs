using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : NetworkBehaviour
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
    
    public void Shoot(ActivateEventArgs args)
    {
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
            StartCoroutine(TactileVibrationTest(controllerInteractor));
        
        SpawnBullet();
        
    }
    
    [Command]
    public void SpawnBullet()
    {
        GameObject obj = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        NetworkServer.Spawn(obj);
            
        SoundManager.instance.Play("Shoot", bulletSpawn.position);
    }

    IEnumerator TactileVibrationTest(XRBaseControllerInteractor controllerInteractor)
    {
        controllerInteractor.SendHapticImpulse(.7f, 0.05f);
        yield return new WaitForSeconds(0.1f);
        controllerInteractor.SendHapticImpulse(1, 0.1f);
        yield return new WaitForSeconds(0.2f);
        controllerInteractor.SendHapticImpulse(.5f, 0.05f);
    }
}
