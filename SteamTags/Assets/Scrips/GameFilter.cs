using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class GameFilter : MonoBehaviour
{
    public GameDatabase gameDatabase;

    // UI Elements
    public TMP_Dropdown genreDropdown;
    public Slider maxPriceSlider;
    public TMP_Text maxPriceText;
    public Slider minSalesSlider;
    public TMP_Text minSalesText;
    public Slider minReviewSlider;
    public TMP_Text minReviewText;
    public Slider releaseYearSlider;
    public TMP_Text releaseYearText;

    public Transform resultContainer; // 🔹 Aquí van los resultados individuales
    public GameObject gameEntryPrefab;
    public GameObject resultsPanel; // 🔹 Panel que se oculta/activa
    public Button closeButton; // 🔹 Botón para cerrar el panel

    private List<GameData> selectedGamesForComparison = new List<GameData>();
    public GameObject comparisonPanel; // Panel donde se mostrará la comparación
    public TMP_Text comparisonText;    // Texto donde se mostrará la comparación

    void Start()
    {
        PopulateGenreDropdown();
        SetupSliders();
        closeButton.onClick.AddListener(CloseResultsPanel);
    }

    void PopulateGenreDropdown()
    {
        List<string> genres = gameDatabase.games
            .Select(g => g.mainGenre)
            .Distinct()
            .ToList();

        genreDropdown.ClearOptions();
        genreDropdown.AddOptions(genres);
    }

    void SetupSliders()
    {
        maxPriceSlider.onValueChanged.AddListener(delegate { UpdateSliderText(); });
        minSalesSlider.onValueChanged.AddListener(delegate { UpdateSliderText(); });
        minReviewSlider.onValueChanged.AddListener(delegate { UpdateSliderText(); });
        releaseYearSlider.onValueChanged.AddListener(delegate { UpdateSliderText(); });

        UpdateSliderText();
    }

    void UpdateSliderText()
    {
        maxPriceText.text = $"${maxPriceSlider.value}";
        minSalesText.text = $"{minSalesSlider.value} M";
        minReviewText.text = $"{minReviewSlider.value}%";
        releaseYearText.text = $"{releaseYearSlider.value}";
    }

    public void ApplyFilters()
    {
        List<GameData> filteredGames = gameDatabase.games;

        string selectedGenre = genreDropdown.options[genreDropdown.value].text;
        if (!string.IsNullOrEmpty(selectedGenre))
        {
            filteredGames = filteredGames
                .Where(game => game.mainGenre == selectedGenre || game.secondaryGenre == selectedGenre)
                .ToList();
        }

        filteredGames = filteredGames.Where(game => game.price <= maxPriceSlider.value).ToList();
        filteredGames = filteredGames.Where(game => game.estimatedSales >= minSalesSlider.value).ToList();
        filteredGames = filteredGames.Where(game => game.positiveReviewsPercentage >= minReviewSlider.value).ToList();
        filteredGames = filteredGames.Where(game => int.Parse(game.releaseDate.Substring(0, 4)) >= releaseYearSlider.value).ToList();

        DisplayResults(filteredGames);
        resultsPanel.SetActive(true); // 🔹 Mostrar el panel al aplicar filtros
    }

    void DisplayResults(List<GameData> games)
    {
        foreach (Transform child in resultContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var game in games)
        {
            GameObject entry = Instantiate(gameEntryPrefab, resultContainer);

            // 🔹 Configurar los datos del juego en el GameEntry
            GameEntry entryScript = entry.GetComponent<GameEntry>();
            entryScript.Initialize(game, this);

            entry.transform.Find("GameName").GetComponent<TMP_Text>().text = game.gameName;
            entry.transform.Find("GameDetails").GetComponent<TMP_Text>().text =
                $"Género: {game.mainGenre}\n" +
                $"Desarrolladora: {game.developer}\n" +
                $"Precio: ${game.price}\n" +
                $"Ventas: {game.estimatedSales}\n" +
                $"Reseñas Positivas: {game.positiveReviewsPercentage}%\n" +
                $"Lanzamiento: {game.releaseDate}";

            entry.transform.Find("GameImage").GetComponent<Image>().sprite = game.gameCover;
        }

        resultsPanel.SetActive(true);
    }

    public void CloseResultsPanel()
    {
        resultsPanel.SetActive(false);
    }
    public void SelectGameForComparison(GameData game)
    {
        if (selectedGamesForComparison.Contains(game))
        {
            Debug.Log("Este juego ya está seleccionado.");
            return;
        }

        selectedGamesForComparison.Add(game);

        if (selectedGamesForComparison.Count == 2)
        {
            ShowComparison();
        }

    
    }
    void ShowComparison()
    {
        GameData game1 = selectedGamesForComparison[0];
        GameData game2 = selectedGamesForComparison[1];

        comparisonText.text =
            $"⚔ {game1.gameName} vs {game2.gameName} ⚔\n\n" +
            $"📌 Gender: {game1.mainGenre} vs {game2.mainGenre}\n" +
            $"🏭 Developer: {game1.developer} vs {game2.developer}\n" +
            $"💲 Price: ${game1.price} vs ${game2.price}\n" +
            $"📈 Owners: {game1.estimatedSales} vs {game2.estimatedSales}\n" +
            $"⭐ Positive Reviews: {game1.positiveReviewsPercentage}% vs {game2.positiveReviewsPercentage}%\n" +
            $"📅 Launch: {game1.releaseDate} vs {game2.releaseDate}";

        comparisonPanel.SetActive(true);
    }

    public void CloseComparisonPanel()
    {
        comparisonPanel.SetActive(false);
        selectedGamesForComparison.Clear();
    }
}