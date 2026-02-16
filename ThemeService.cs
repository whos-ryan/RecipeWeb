using Microsoft.JSInterop;

namespace RecipeWeb;

public class ThemeService
{
    private readonly IJSRuntime _js;
    public bool IsDarkMode { get; private set; }
    public event Action? OnThemeChanged;

    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        await _js.InvokeVoidAsync("localStorage.setItem", "theme", IsDarkMode ? "dark" : "light");
        NotifyThemeChanged();
    }

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
            IsDarkMode = false;
        }
    }

    public string GetThemeClass() => IsDarkMode ? "dark-mode" : "light-mode";

    private void NotifyThemeChanged() => OnThemeChanged?.Invoke();
}
