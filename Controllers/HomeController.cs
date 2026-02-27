using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibrariesWebApp.Models;

namespace LibrariesWebApp.Controllers;

/// <summary>
/// Контроллер для обработки домашних страниц и общих действий.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Отображает главную страницу сайта.
    /// </summary>
    /// <returns>Представление Index.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Отображает страницу с политикой конфиденциальности.
    /// </summary>
    /// <returns>Представление Privacy.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Отображает страницу с информацией об ошибке.
    /// Метод помечен атрибутом <see cref="ResponseCacheAttribute"/> с нулевой длительностью,
    /// чтобы браузеры и прокси-серверы не кэшировали страницу ошибки.
    /// </summary>
    /// <returns>Представление Error с моделью <see cref="ErrorViewModel"/>.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}