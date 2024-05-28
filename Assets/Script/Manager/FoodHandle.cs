using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class FoodHandle : MonoBehaviour
{
    public struct FoodInformation
    {
        public Vector3Int foodPosition;
        public TileBase foodTemplate;
    }
    
    private HashSet<FoodInformation> eatenFoods = new HashSet<FoodInformation>();
    
    private int foodCounter = 0;
    private int totalFoodCount; 

    [SerializeField] private Tilemap map;
    [SerializeField] private TileBase normalFood;
    [SerializeField] private TileBase extraFood;
    [SerializeField] private TileBase powerfullFood;
    [SerializeField] private Vector3Int newPowerfullFoodPosition;
    [SerializeField] private Vector3Int initialPowerfullPosition;

    void Awake()
    {
        InitializeFoodCount();
    }
    
    private void OnEnable()
    {
        ExternalEvents.GameOver += OnGameOver;
        ExternalEvents.GameTimer += OnPowerFoodReady;
    }

    private void OnPowerFoodReady(int timer)
    {
        if (timer == 30)
        {
            map.SetTile(newPowerfullFoodPosition, powerfullFood);
            ExternalEvents.PowerfullFoodReady?.Invoke();
            StartCoroutine(WaitForPowerfullFood());
        }
    }

    private IEnumerator WaitForPowerfullFood()
    {
        yield return new WaitForSeconds(6);
        map.SetTile(newPowerfullFoodPosition, null);
    }

    private void OnGameOver()
    {
        ResetAllFoods();
    }

    private void OnDisable()
    {
        ExternalEvents.GameOver -= OnGameOver;
        ExternalEvents.GameTimer -= OnPowerFoodReady;
    }

    private void InitializeFoodCount()
    {
        totalFoodCount = 0;
        foreach (var position in map.cellBounds.allPositionsWithin)
        {
            TileBase tile = map.GetTile(position);
            if (tile == normalFood || tile == extraFood)
            {
                totalFoodCount++;
            }
        }
    }

    private void Update()
    {
        Vector3Int cellPos = map.WorldToCell(transform.position);
        TileBase tileBase = map.GetTile(cellPos);

        if (tileBase != null && (tileBase == normalFood || tileBase == extraFood || tileBase == powerfullFood))
        {
            EatFood(new FoodInformation() { foodPosition = cellPos, foodTemplate = tileBase }); 
        }
    }

    public void EatFood(FoodInformation foodInformation)
    {
        if (!eatenFoods.Contains(foodInformation))
        {
            eatenFoods.Add(foodInformation);
            map.SetTile(foodInformation.foodPosition, null);
            foodCounter++;

            if (foodInformation.foodTemplate == normalFood)
            {
                InternalEvents.NormalFoodEating?.Invoke(foodCounter);
            }
            else if (foodInformation.foodTemplate == extraFood)
            {
                InternalEvents.ExtraFoodEating?.Invoke();
            } 
            else if (foodInformation.foodTemplate == powerfullFood)
            {
                InternalEvents.PowerfullFoodEating?.Invoke();
            }
            if (foodCounter == totalFoodCount)
            {
                ExternalEvents.LevelComplete?.Invoke();
                StartCoroutine(WaitAndResetFoods());
            }
        }
    }

    IEnumerator WaitAndResetFoods()
    {
        yield return new WaitForSeconds(1.3f);
        ResetAllFoods();
    }

    public void ResetAllFoods()
    {
        foreach (var foodInformation in eatenFoods)
        {
            map.SetTile(newPowerfullFoodPosition, null); 
            map.SetTile(foodInformation.foodPosition, foodInformation.foodTemplate);
        }
        eatenFoods.Clear();
        foodCounter = 0;
    }
}
