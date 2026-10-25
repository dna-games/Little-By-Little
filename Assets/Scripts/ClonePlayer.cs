using UnityEngine;
using System.Collections;
public class ClonePlayer : MonoBehaviour
{
    public GameObject playerPrefab;
    public float cloneRadius = .5f;
    public float spawnDelay = 1f;
    public KeyCode cloneKey = KeyCode.Q;
    public KeyCode resetKey = KeyCode.R;
    public Transform spawnPosition;
    public ParticleSystem cloneEffect;

    bool canClone = true;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(cloneKey) && canClone)
        {
            ClonePlayerMethode();
        }
        if (Input.GetKeyDown(resetKey))
        {
            ResetClone();
        }
    }
    bool ValidateClonePosition(Transform pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos.position, cloneRadius);
        return colliders.Length == 0;
    }
    IEnumerator ClonePlayerCoroutine()
    {
        if (ValidateClonePosition(transform))
        {
            Instantiate(playerPrefab, transform.position, spawnPosition.rotation);
            transform.position = spawnPosition.position;
            canClone = false;
        }

        yield return new WaitForSeconds(spawnDelay);
        canClone = true;
    }
    void ClonePlayerMethode()
    {
        AudioManager.Instance.PlaySFX("Clone");
        Instantiate(cloneEffect, transform.position, Quaternion.identity);
        StartCoroutine(ClonePlayerCoroutine());
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, cloneRadius);
    }
    void ResetClone()
    {
        GameObject[] clone = GameObject.FindGameObjectsWithTag("Clone");

        for (int i =0; i < clone.Length; i++)
        {
            Destroy(clone[i]);
        }

        transform.position = spawnPosition.position;
    }
}
