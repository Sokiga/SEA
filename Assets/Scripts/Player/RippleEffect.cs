using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    public float expansionSpeed = 5f; // 涟漪扩散速度
    public float maxSize = 10f; // 涟漪最大尺寸
    public float forceMagnitude = 10f; // 施加给物体的力的大小
    public ParticleSystem rippleParticleSystem; // 涟漪粒子系统
    public string interactableTag = "Interactable"; // 可交互物体的标签

    private bool isExpanding = false;
    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        if (isExpanding)
        {
            Expand();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRipple();
        }
    }

    private void StartRipple()
    {
        transform.localScale = initialScale;
        isExpanding = true;
        //rippleParticleSystem.Play();
    }

    private void Expand()
    {
        transform.localScale += Vector3.one * expansionSpeed * Time.deltaTime;

        if (transform.localScale.x >= maxSize)
        {
            isExpanding = false;
            //rippleParticleSystem.Stop();
            transform.localScale = initialScale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(interactableTag))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = (other.transform.position - transform.position).normalized;
                rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}


