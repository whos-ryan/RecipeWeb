using System.Text.Json;

namespace RecipeWeb;

public class RecipeService
{
    private List<Recipe> _recipes = new();
    // The file will be created in your project's root folder
    private readonly string _filePath = "recipes.json";

    public RecipeService()
    {
        LoadFromFile(); // Load saved recipes as soon as the app starts
    }

    public List<Recipe> GetAll() => _recipes;

    public void Add(Recipe r) 
    {
        _recipes.Add(r);
        SaveToFile();
    }

    public void UpdateRecipe(Recipe oldRecipe, Recipe updatedRecipe)
    {
        var index = _recipes.IndexOf(oldRecipe);
        if (index != -1)
        {
            _recipes[index] = updatedRecipe;
            SaveToFile();
        }
    }

    public void DeleteRecipe(Recipe r)
    {
        if (_recipes.Remove(r))
        {
            SaveToFile();
        }
    }

    private void SaveToFile()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(_recipes, options);
        File.WriteAllText(_filePath, json);
    }

    private void LoadFromFile()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            _recipes = JsonSerializer.Deserialize<List<Recipe>>(json) ?? new();
        }
        else
        {
            // If no file exists, start with a default recipe
            _recipes = new() { new Recipe("Classic Omelette", new() { "Eggs", "Cheese" }, "Fry it.") };
            SaveToFile();
        }
    }
}