using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private float firedTime;
    private Grabbable grabbable;

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
            return;

        grabbable = GetComponent<Grabbable>();

        TryShoot();
    }

    private void TryShoot()
    {
        if (grabbable.holdingHand == null)
            return;
        if (grabbable.holdingHand.GetInputScheme().Trigger.ReadValue<float>() < 0.8f)
            return;

        Shoot();
    }

    [Command]
    public void Shoot()
    {
        if (Time.time - firedTime < 0.3f)//TODO semi auto
            return;
        
        GameObject obj = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        NetworkServer.Spawn(obj);
        firedTime = Time.time;
        StartCoroutine(TactileVibrationTest());
    }

    IEnumerator TactileVibrationTest()
    {
        grabbable.holdingHand.Vibrate(.7f, 0.05f);
        yield return new WaitForSeconds(0.1f);
        grabbable.holdingHand.Vibrate(1, 0.1f);
        yield return new WaitForSeconds(0.2f);
        grabbable.holdingHand.Vibrate(.5f, 0.05f);
    }
}
