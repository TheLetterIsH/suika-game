using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [Header("Actions")]
    public static Action<AnimalType, Vector2> onAnimalsMerged;

    [Header("Elements")]
    Animal lastEmitterAnimal;
    
    private void Start()
    {
        Animal.onCollisionWithAnimal += CollisionBetweenAnimalsCallback;
    }

    private void CollisionBetweenAnimalsCallback(Animal emitterAnimal, Animal otherAnimal)
    {
        if (lastEmitterAnimal != null)
        {
            return;
        }

        lastEmitterAnimal = emitterAnimal;

        HandleMerge(emitterAnimal, otherAnimal);
    }

    private void HandleMerge(Animal emitterAnimal, Animal otherAnimal)
    {
        AnimalType mergedAnimalType = emitterAnimal.GetAnimalType() + 1;

        Vector2 mergedAnimalSpawnPosition = (emitterAnimal.transform.position + otherAnimal.transform.position) / 2;

        Destroy(emitterAnimal.gameObject);
        Destroy(otherAnimal.gameObject);

        StartCoroutine(ResetLastEmitterAnimal());

        onAnimalsMerged?.Invoke(mergedAnimalType, mergedAnimalSpawnPosition);
    }

    IEnumerator ResetLastEmitterAnimal()
    {
        yield return new WaitForEndOfFrame();
        lastEmitterAnimal = null;
    }
}
