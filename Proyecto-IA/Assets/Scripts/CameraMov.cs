using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    [SerializeField] float dragSpeed = 10;
    private Vector3 dragOrigin;
    private Vector3 movement;
    private Vector3 magnitude;


    void Update() {

        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            movement.y = -10;
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            movement.y = 10;
        } else { movement.y = 0; }

        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.up * dragSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.down * dragSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.left * dragSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * dragSpeed * Time.deltaTime);
        }
        transform.Translate(movement*dragSpeed*Time.deltaTime, Space.World);
    }
}
