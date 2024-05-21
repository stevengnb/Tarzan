using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private float rotationSpeed = 7f;

    private void Update() {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
