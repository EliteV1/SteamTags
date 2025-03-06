using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "Game Database")]
public class GameDatabase : ScriptableObject
{
    public List<GameData> games;
}