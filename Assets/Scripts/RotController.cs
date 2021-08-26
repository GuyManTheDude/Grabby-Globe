using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotController : MonoBehaviour
{
    [SerializeField] float startRotationSpeed = 100f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float maxRotationSpeed = 350f;
    bool dragging = false;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        startRotationSpeed = rotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        dragging = Input.GetMouseButton(0);
        if(dragging)
        {
            rotationSpeed = Mathf.Clamp(rotationSpeed + 140 * Time.deltaTime, 100, maxRotationSpeed);
        }
        else
        {
            rotationSpeed = startRotationSpeed;
        }
    }

    private void OnMouseDown()
    {
        rb.freezeRotation = true;
    }

    private void OnMouseDrag()
    {
        dragging = true;
    }

    private void FixedUpdate()
    {
        if (dragging)
        {
            rb.freezeRotation = false;
            rb.angularDrag = 0f;
            float x = Input.GetAxisRaw("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
            float y = Input.GetAxisRaw("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;

            rb.AddTorque(Vector3.down * x, ForceMode.Impulse);
            rb.AddTorque(Vector3.right * y, ForceMode.Impulse);
        }
        else
        {
            rb.angularDrag = 18f;

        }

    }

}
