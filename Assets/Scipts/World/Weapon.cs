using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Grabbable))]
public class Weapon : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private Grabbable _grabbable;
    private bool _waitForReset;

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
            return;

        _grabbable = GetComponent<Grabbable>();

        TryShoot();
    }

    private void TryShoot()
    {
        VRHand primaryHand = _grabbable.GetHandForTriggerId(0);
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
    }

    [Command]
    public void Shoot()
    {
        GameObject obj = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        NetworkServer.Spawn(obj);
        StartCoroutine(TactileVibrationTest());
    }

    IEnumerator TactileVibrationTest()
    {
        VRHand primaryHand = _grabbable.GetHandForTriggerId(0);
        
        primaryHand.Vibrate(.7f, 0.05f);
        yield return new WaitForSeconds(0.1f);
        primaryHand.Vibrate(1, 0.1f);
        yield return new WaitForSeconds(0.2f);
        primaryHand.Vibrate(.5f, 0.05f);
    }
}
