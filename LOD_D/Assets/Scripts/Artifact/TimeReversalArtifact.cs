using UnityEngine;

public class TimeReversalArtifact : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float effectRadius = 5f; // รัศมีที่จะแสดง effect
    [SerializeField] private KeyCode activationKey = KeyCode.F;
    [SerializeField] private ParticleSystem reversalEffect; // particle system สำหรับ effect
    [SerializeField] private AudioClip reversalSound; // เสียง effect

    [Header("Status")]
    public bool isCollected = false;
    private bool isInReversalZone = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (isCollected && isInReversalZone && Input.GetKeyDown(activationKey))
        {
            ReverseStructure();
        }
    }

    private void ReverseStructure()
    {
        // หา ReversibleStructure ที่อยู่ใกล้เคียง
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, effectRadius);
        
        foreach (var collider in colliders)
        {
            ReversibleStructure structure = collider.GetComponent<ReversibleStructure>();
            if (structure != null && structure.canBeReversed)
            {
                structure.Reverse();
                PlayEffects();
            }
        }
    }

    private void PlayEffects()
    {
        if (reversalEffect != null)
        {
            reversalEffect.Play();
        }

        if (reversalSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reversalSound);
        }
    }

    public void Collect()
    {
        isCollected = true;
        Debug.Log("Time Reversal Artifact collected!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ReversalZone"))
        {
            isInReversalZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ReversalZone"))
        {
            isInReversalZone = false;
        }
    }

    // แสดง Gizmos สำหรับ debug ในหน้า Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}