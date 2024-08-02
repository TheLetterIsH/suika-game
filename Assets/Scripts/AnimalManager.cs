using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animal animalPrefab;
    [SerializeField] private LineRenderer animalDropLine;
    private Animal currentAnimal;

    [Header("Settings")]
    [SerializeField] private float animalSpawnPositionY;
    private bool canHandlePlayerInput;
    private bool isControllingCurrentAnimal;

    [Header("Debug")]
    [SerializeField] private bool enableGizmos;

    private void Start()
    {
        DisableAnimalDropLine();

        canHandlePlayerInput = true;
    }

    private void Update()
    {
        if (canHandlePlayerInput)
        {
            HandlePlayerInput();
        }
    }

    private void HandlePlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseDownCallback();
        }
        else if (Input.GetMouseButton(0))
        {
            if (isControllingCurrentAnimal)
            {
                MouseDragCallback();
            }
            else
            {
                MouseDownCallback();
            }
        }
        else if (Input.GetMouseButtonUp(0) && isControllingCurrentAnimal)
        {
            MouseUpCallback();
        }
    }

    private void MouseDownCallback()
    {
        EnableAnimalDropLine();
        DrawAnimalDropLine();

        SpawnAnimal();

        isControllingCurrentAnimal = true;
    }

    private void MouseDragCallback()
    {
        DrawAnimalDropLine();

        currentAnimal.MoveTo(GetSpawnWorldPosition());
    }

    private void MouseUpCallback()
    {
        DisableAnimalDropLine();

        currentAnimal.EnablePhysics();

        canHandlePlayerInput = false;
        StartInputBufferTimer();

        isControllingCurrentAnimal = false;
    }

    private void SpawnAnimal()
    {
        Vector2 spawnPosition = GetSpawnWorldPosition();

        currentAnimal = Instantiate(animalPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector2 GetClickedWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private Vector2 GetSpawnWorldPosition()
    {
        Vector2 clickedWorldPosition = GetClickedWorldPosition();
        clickedWorldPosition.y = animalSpawnPositionY;
        return clickedWorldPosition;
    }

    private void DisableAnimalDropLine()
    {
        animalDropLine.enabled = false;
    }

    private void DrawAnimalDropLine()
    {
        animalDropLine.SetPosition(0, GetSpawnWorldPosition());
        animalDropLine.SetPosition(1, GetSpawnWorldPosition() + Vector2.down * 12);
    }

    private void EnableAnimalDropLine()
    {
        animalDropLine.enabled = true;
    }

    private void StartInputBufferTimer()
    {
        Invoke("StopInputBufferTimer", 1);
    }

    private void StopInputBufferTimer()
    {
        canHandlePlayerInput = true;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!enableGizmos)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(-5, animalSpawnPositionY), new Vector2(5, animalSpawnPositionY));
    }

#endif
}
