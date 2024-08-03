using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AnimalManager))]
public class AnimalManagerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI nextAnimalText;
    private AnimalManager animalManager;

    private void Start()
    {
        animalManager = GetComponent<AnimalManager>();
    }

    private void Update()
    {
        nextAnimalText.text = animalManager.GetNextAnimalName();
    }
}
