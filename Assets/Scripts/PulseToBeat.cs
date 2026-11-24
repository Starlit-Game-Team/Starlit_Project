using UnityEngine;

public class PulseToBeat : MonoBehaviour
{
    [SerializeField] private float pulseSize;
    [SerializeField] private float pulseSpeed;
    private Vector3 originalScale;
    private void Start()
    {
        originalScale = transform.localScale;
    }
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * pulseSpeed);
    }
    public void Pulse()
    {
        transform.localScale = originalScale * pulseSize;
    }
}
