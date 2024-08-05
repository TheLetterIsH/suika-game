using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AnimalManager))]
public class AnimalManagerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image nextAnimalImage;
    private AnimalManager animalManager;

    private void Awake()
    {
        AnimalManager.onNextAnimalIndexSet += UpdateNextAnimalImage;
    }

    private void Start()
    {
        animalManager = GetComponent<AnimalManager>();
    }

    private void UpdateNextAnimalImage()
    {
        if (animalManager == null)
        {
            animalManager = GetComponent<AnimalManager>();
        }

        nextAnimalImage.sprite = animalManager.GetNextAnimalSprite();
        nextAnimalImage.color = animalManager.GetNextAnimalColor();
    }
}
