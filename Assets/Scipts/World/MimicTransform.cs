using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicTransform : MonoBehaviour
{
    public bool positionX = true, positionY = true, positionZ = true;
    public bool rotationX = true, rotationY = true, rotationZ = true;
    [Space] public Transform transformToMimic;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        if (positionX) newPosition.x = transformToMimic.position.x;
        if (positionY) newPosition.y = transformToMimic.position.y;
        if (positionZ) newPosition.z = transformToMimic.position.z;
        transform.position = newPosition;
        
        Vector3 newEulerRotation = transform.rotation.eulerAngles;
        if (rotationX) newEulerRotation.x = transformToMimic.rotation.eulerAngles.x;
        if (rotationY) newEulerRotation.y = transformToMimic.rotation.eulerAngles.y;
        if (rotationZ) newEulerRotation.z = transformToMimic.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(newEulerRotation);
    }
}
