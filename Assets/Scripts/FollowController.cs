using UnityEngine;

public class FollowController : MonoBehaviour
{
    [SerializeField] private Transform m_target;
    [SerializeField] private bool m_usePhysics = false;
    [SerializeField] private Rigidbody m_rb;

    [Header("Movement")]
    [SerializeField] private float m_minSpeed = 1f;
    [SerializeField] private float m_maxSpeed = 10f;
    [SerializeField] private float m_minDistance = 2.5f;
    [SerializeField] private float m_maxDistance = 10f;
    [SerializeField] private AnimationCurve m_speedCurve;

    [Header("Rotation")]
    [SerializeField] private float m_rotationSpeed;
    void Start()
    {
        
    }
    void Update()
    {
        if (!m_usePhysics)
        {
            float distance = Vector3.Distance(transform.position, m_target.position);
            if (distance > m_minDistance)
            {
                float distanceRatio = Remap(distance, m_minDistance, m_maxDistance, 0, 1);
                Debug.Log("test" + distanceRatio);
                distanceRatio = Mathf.Clamp(distanceRatio, 0, 1);

                float speedRatio = m_speedCurve.Evaluate(distanceRatio);
                float wantedSpeed = Remap(speedRatio, 0, 1, m_minSpeed, m_maxSpeed);

                transform.position = Vector3.MoveTowards(transform.position, m_target.position, wantedSpeed * Time.deltaTime);
            }

            // Rotation

        }
        Vector3 direction = (m_target.position - transform.position).normalized;
        transform.forward = Vector3.MoveTowards(transform.forward, direction, m_rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void FixedUpdate()
    {
        if (!m_usePhysics) return;

        Vector3 direction = (m_target.position - transform.position).normalized;

        float distance = Vector3.Distance(transform.position, m_target.position);
        if (distance > m_minDistance)
        {
            float distanceRatio = Remap(distance, m_minDistance, m_maxDistance, 0, 1);
            distanceRatio = Mathf.Clamp(distanceRatio, 0, 1);

            float speedRatio = m_speedCurve.Evaluate(distanceRatio);
            float wantedSpeed = Remap(speedRatio, 0, 1, m_minSpeed, m_maxSpeed);

            direction = new Vector3(direction.x, 0, direction.z);

            m_rb.AddForce(direction * wantedSpeed, ForceMode.Acceleration);
        }
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_target.position, m_maxDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_target.position, m_minDistance);
    }
}
