using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private bool autoLoadSceneAfterDelay;
    [SerializeField] private float DelayTimer;
    [SerializeField] private string SceneToLoad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (autoLoadSceneAfterDelay)
        {
            StartCoroutine(DelayLoad(DelayTimer)); // Start the coroutine with a delay
        }
    }

    // The coroutine function
    IEnumerator DelayLoad(float delayTime)
    {
        // Code to execute before the delay
        Debug.Log("Waiting...");

        // Wait for the specified delay time
        yield return new WaitForSeconds(delayTime);

        // Code to execute after the delay
        Load(SceneToLoad);
    }

    public void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
