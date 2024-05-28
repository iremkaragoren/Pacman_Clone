using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameData_SO", menuName = "ThisGame/GameData/GameDataSO", order = 1)]

public class GameData_SO : ScriptableObject
{
    public GameObject Pacman => pacman;
    public int PacManHealth => pacmanHealth;
    
    [SerializeField] private int pacmanHealth = 3;
    [SerializeField] private GameObject pacman;
   


}
