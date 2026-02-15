using Microsoft.JSInterop;

namespace RecipeWeb;

public class ThemeService
{
    private readonly IJSRuntime _js;
    public bool IsDarkMode { get; private set; }
    
    // This event notifies the Layout and NavMenu to refresh when the toggle is clicked
    public event Action? OnThemeChanged;

    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }

    /// <summary>
    /// Switches the theme and saves the preference to the browser's local storage.
    /// </summary>
    public async Task ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        
        // Save the string "dark" or "light" to the browser's local storage
        await _js.InvokeVoidAsync("localStorage.setItem", "theme", IsDarkMode ? "dark" : "light");
        
        NotifyThemeChanged();
    }

    /// <summary>
    /// Reads the saved preference from the browser on startup.
    /// </summary>
    public async Task InitializeTheme()
    {
        try
        {
            var savedTheme = await _js.InvokeAsync<string>("localStorage.getItem", "theme");
            IsDarkMode = (savedTheme == "dark");
            NotifyThemeChanged();
        }
        catch
        {
            // Default to light mode if something goes wrong or no preference is found
            IsDarkMode = false;
        }
    }

    /// <summary>
    /// Returns the CSS class name to be applied to the main container.
    /// </summary>
    public string GetThemeClass() => IsDarkMode ? "dark-mode" : "light-mode";

    private void NotifyThemeChanged() => OnThemeChanged?.Invoke();
}