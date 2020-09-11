using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform camera;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float scrollingSpeed = 1f;

    private void Start()
    {
        transform.position = new Vector3Int(Parameters.length / 2, Parameters.length / 2, Parameters.length / 2);
        transform.GetChild(0).transform.localPosition = new Vector3(0, 0, -Parameters.length * 2);
    }

    void Update()
    {
        float newZ = camera.localPosition.z + Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollingSpeed;
        camera.localPosition = new Vector3(0, 0, newZ);

        float angleY = 0, angleX = 0;

        if (Input.GetMouseButton(0))
        {
            angleY = rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
            angleX = rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse Y");
        }

        transform.Rotate(-angleX, angleY, 0);
    }
}
