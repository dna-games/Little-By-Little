using UnityEngine;
using UnityEngine.Events;

public class Spike : MonoBehaviour
{
    public UnityEvent OnSpikeCollision;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = collision.gameObject.GetComponent<ClonePlayer>().spawnPosition.position;
            
            OnSpikeCollision.Invoke();
        }
    }
}
