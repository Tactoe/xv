using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public bool IsExploreController;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public bool IsFlying { get; private set; }
    
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;
    [SerializeField]
    float m_RotationSpeed;

    Rigidbody m_RB;
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();
    
    void Awake()
    {
        m_RB = GetComponent<Rigidbody>();
        IsFlying = false;
    }

    void Update(){
    }

    void FixedUpdate()
    {
        IsRunning = canRun && Input.GetKey(runningKey);

        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        Vector2 targetVelocity = new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        m_RB.velocity = transform.rotation * new Vector3(targetVelocity.x, m_RB.velocity.y, targetVelocity.y);
        if (IsExploreController)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                IsFlying = !IsFlying;
                m_RB.useGravity = !IsFlying;
                m_RB.velocity = Vector3.zero;
            }

            if ((Input.GetButton("Jump") || Input.GetKey(KeyCode.E)) && IsFlying)
            {
                Vector3 direction = Input.GetKey(KeyCode.E) ? Vector3.down : Vector3.up;
                transform.Translate(direction * 0.05f, Space.World);
            }
        }
        else
        {
            if ((Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)))
            {
                float direction = Input.GetKey(KeyCode.E) ? 1 : -1; 
                transform.RotateAround(transform.position, Vector3.up, Time.unscaledDeltaTime * m_RotationSpeed * direction);
            }
        }
    }
}