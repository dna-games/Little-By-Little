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
            AudioManager.Instance.PlaySFX("Key");
            goToNextLevel.UnlockDoor();
            gameObject.SetActive(false);
        }
    }
    public void RestoreKey()
    {
        gameObject.SetActive(true);
        goToNextLevel.LockDoor();
    }
}
