using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform animalContainer;
    [SerializeField] private Animal[] spawnableAnimalPrefabs;
    [SerializeField] private Animal[] animalPrefabs;
    [SerializeField] private LineRenderer animalDropLine;
    private Animal currentAnimal;
    private int nextAnimalIndex;

    [Header("Settings")]
    [SerializeField] private float animalSpawnPositionY;
    [SerializeField] private float inputBufferDelay;
    private bool canHandlePlayerInput;
    private bool isControllingCurrentAnimal;

    [Header("Debug")]
    [SerializeField] private bool enableGizmos;

    private void Awake()
    {
        MergeManager.onAnimalsMerged += AnimalsMergedCallback;
    }

    private void Start()
    {
        DisableAnimalDropLine();
        SetNextAnimalIndex();

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

        if (currentAnimal != null)
        {
            currentAnimal.EnablePhysics();
        }

        canHandlePlayerInput = false;
        StartInputBufferTimer();

        isControllingCurrentAnimal = false;
    }

    private void SpawnAnimal()
    {
        Vector2 spawnPosition = GetSpawnWorldPosition();

        Animal animalPrefab = spawnableAnimalPrefabs[nextAnimalIndex];

        currentAnimal = Instantiate(
            animalPrefab,
            spawnPosition,
            Quaternion.identity,
            animalContainer
        );

        SetNextAnimalIndex();
    }

    private void SetNextAnimalIndex()
    {
        nextAnimalIndex = Random.Range(0, spawnableAnimalPrefabs.Length);
    }

    public string GetNextAnimalName()
    {
        return spawnableAnimalPrefabs[nextAnimalIndex].name;
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
        Invoke("StopInputBufferTimer", inputBufferDelay);
    }

    private void StopInputBufferTimer()
    {
        canHandlePlayerInput = true;
    }

    private void AnimalsMergedCallback(AnimalType mergedAnimalType, Vector2 mergedAnimalSpawnPosition)
    {
        for (int i = 0; i < animalPrefabs.Length; i++)
        {
            if (animalPrefabs[i].GetAnimalType() == mergedAnimalType)
            {
                SpawnMergedAnimal(animalPrefabs[i], mergedAnimalSpawnPosition);
                break;
            }
        }
    }

    private void SpawnMergedAnimal(Animal mergedAnimal,  Vector2 mergedAnimalSpawnPosition)
    {
        Animal mergedAnimalInstance = Instantiate(
            mergedAnimal, 
            mergedAnimalSpawnPosition, 
            Quaternion.identity, 
            animalContainer
        );

        mergedAnimalInstance.EnablePhysics();
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
