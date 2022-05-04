using UnityEngine;

public class Jump : MonoBehaviour
{
    Rigidbody m_RB;
    public float JumpStrength = 2;
    public event System.Action Jumped;
    bool m_IsFlying;
    PlayerMovement m_Fpm;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;


    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        // Get rigidbody.
        m_RB = GetComponent<Rigidbody>();
        m_Fpm = GetComponentInChildren<PlayerMovement>();

    }

    void LateUpdate()
    {
        m_IsFlying = m_Fpm.IsFlying;
        // Jump when the Jump button is pressed and we are on the ground.
        if (Input.GetButtonDown("Jump") && (!groundCheck || groundCheck.isGrounded) && !m_IsFlying)
        {
            m_RB.AddForce(Vector3.up * 100 * JumpStrength);
            Jumped?.Invoke();
        }
    }
}
