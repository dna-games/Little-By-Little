using UnityEngine;

public class GoToNextLevel : MonoBehaviour
{
    public int currentSceneIndex;
    public bool isLastLevel;
    public bool lockDoor;
    public Sprite lockedDoorSprite;
    public Sprite unlockedDoorSprite;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (lockDoor)
        {
            spriteRenderer.sprite = lockedDoorSprite;
        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!lockDoor)
        {
            if (collision.CompareTag("Player"))
            {
                int nextSceneIndex = currentSceneIndex + 1;

                if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
                {
                    FadeSceneManager.Instance.LoadScene(nextSceneIndex);
                }
                else
                {
                    if (isLastLevel)
                    {
                        FadeSceneManager.Instance.LoadScene(0);
                    }
                    else
                    {
                        Debug.Log("No more levels");
                    }
                }
            }
        }
    }
    public void UnlockDoor()
    {
        lockDoor = false;
        spriteRenderer.sprite = unlockedDoorSprite;
    }
    public void LockDoor()
    {
        lockDoor = true;
        spriteRenderer.sprite = lockedDoorSprite;
    }
}
