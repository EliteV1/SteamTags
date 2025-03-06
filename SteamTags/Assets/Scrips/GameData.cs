using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    public string gameName;
    public string mainGenre;
    public string secondaryGenre;
    public string releaseDate;
    public string developer;
    public float price;
    public int estimatedSales;
    public float positiveReviewsPercentage;
    public Sprite gameCover;
}