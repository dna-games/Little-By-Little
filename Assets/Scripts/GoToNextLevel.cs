using UnityEngine;

public class GoToNextLevel : MonoBehaviour
{
    public int currentSceneIndex;
    private void OnTriggerEnter2D(Collider2D collision)
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
                Debug.Log("No more levels");
            }
        }
    }
}
