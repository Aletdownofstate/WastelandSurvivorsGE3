using UnityEngine;

public class CommsTowerTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (!hasTriggered && GameManager.Instance.currentGameState == GameManager.GameState.ChapterThree)
        {
            Debug.Log("Comms tower trigger activated");
            hasTriggered = true;
            GameManager.Instance.chapterThreeGoalOne = true;
            Debug.Log($"{GameManager.Instance.currentGameState} goal complete");
        }
    }
}