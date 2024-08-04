using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject limitLine;
    [SerializeField] private Transform animalContainer;

    [Header("Timer")]
    [SerializeField] private float gameOverTimerDuration;
    [SerializeField] private float gameOverTime;
    [SerializeField] private bool isTimerOn;

    private void Update()
    {
        HandleGameOver();
    }

    private void HandleGameOver()
    {
        if (isTimerOn)
        {
            gameOverTime += Time.deltaTime;

            if (!IsAnimalAboveLine())
            {
                StopGameOverTimer();
            }

            if (gameOverTime > gameOverTimerDuration)
            {
                GameOver();
            }
        }
        else
        {
            if (IsAnimalAboveLine())
            {
                StartGameOverTimer();
            }
        }
    }

    private bool IsAnimalAboveLine()
    {
        for (int i = 0; i < animalContainer.childCount; i++)
        {
            Animal animal = animalContainer.GetChild(i).GetComponent<Animal>();

            if (!animal.HasCollided())
            {
                continue;
            }

            if (IsSpecificAnimalAboveLine(animal.transform))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsSpecificAnimalAboveLine(Transform animal)
    {
        if (animal.position.y >= limitLine.transform.position.y)
        {
            return true;
        }

        return false;
    }

    private void StartGameOverTimer()
    {
        gameOverTime = 0;
        isTimerOn = true;
    }

    private void StopGameOverTimer()
    {
        isTimerOn = false;
    }

    private void GameOver()
    {
        Debug.Log("Game Over :(");
    }
}
