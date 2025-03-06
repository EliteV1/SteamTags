using UnityEngine;
using UnityEngine.UI;

public class GameEntry : MonoBehaviour
{
    private GameFilter gameFilter;
    private GameData gameData;

    public Button compareButton;

    public void Initialize(GameData game, GameFilter filter)
    {
        gameData = game;
        gameFilter = filter;
        compareButton.onClick.AddListener(() => gameFilter.SelectGameForComparison(gameData));
    }
}