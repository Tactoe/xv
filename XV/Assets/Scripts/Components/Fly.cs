using UnityEngine;

public class Fly : MonoBehaviour
{
    Rigidbody rigidbody;
    // GroundCheck groundCheck;
    FirstPersonMovement fpm;
    bool fly_check;

    void Awake()
    {
        // Get rigidbody.
        rigidbody = GetComponent<Rigidbody>();
        fpm = GetComponentInChildren<FirstPersonMovement>();
    }

    void LateUpdate()
    {
        Debug.Log("fpm.Is_flying = " + fpm.Is_flying);
        fly_check = fpm.Is_flying;
        // Jump when the Jump button is pressed and we are on the ground.
        if (Input.GetButton("Jump") && fly_check)
        {
            Debug.Log("vouiiiiiii");
            // rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            // Jumped?.Invoke();
            transform.Translate(Vector3.up * 0.05f, Space.World);
        }
        else if (Input.GetKey(KeyCode.E) && fly_check)
        {
            Debug.Log("vouiiiiiii");
            // rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            // Jumped?.Invoke();
            transform.Translate(Vector3.up * -0.05f, Space.World);
        }
    }
}
