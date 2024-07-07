using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    public float expansionSpeed = 5f; // ������ɢ�ٶ�
    public float maxSize = 10f; // �������ߴ�
    public float forceMagnitude = 10f; // ʩ�Ӹ���������Ĵ�С
    public ParticleSystem rippleParticleSystem; // ��������ϵͳ
    public string interactableTag = "Interactable"; // �ɽ�������ı�ǩ

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


