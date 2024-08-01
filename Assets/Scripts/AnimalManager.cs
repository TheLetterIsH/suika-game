using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject animalPrefab;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandlePlayerInput();
        }
    }

    private void HandlePlayerInput()
    {
        Instantiate(animalPrefab, GetClickedWorldPosition(), Quaternion.identity);
    }

    private Vector2 GetClickedWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
