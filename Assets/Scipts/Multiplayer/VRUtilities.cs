#if UNITY_EDITOR
using UnityEngine;
using ParrelSync;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;
 
public class VRUtilities : MonoBehaviour
{
    public bool runVrOnClone;

    void Awake()
    {
        if (ClonesManager.IsClone() == runVrOnClone)
        {
            EnableXR();
        }
        else
        {
            DisableXR();
        }
    }
 
    public void EnableXR()
    {
        StartCoroutine(StartXRCoroutine());
    }
 
    public void DisableXR()
    {
        StartCoroutine(StopXRCoroutine());
    }
 
    public IEnumerator StopXRCoroutine()
    {
        Debug.Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR stopped completely.");
        yield return new WaitForSeconds(.1f);
    }
 
 
    public IEnumerator StartXRCoroutine()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
 
        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }
}
#endif