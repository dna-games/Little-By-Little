using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    GoToNextLevel goToNextLevel;
    void Start()
    {
        goToNextLevel = GameObject.Find("NextLevelTrigger").GetComponent<GoToNextLevel>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            goToNextLevel.UnlockDoor();
            Destroy(gameObject);
        }
    }
}
