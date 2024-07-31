using Esercizio_Pizzeria_In_Forno.Models;
using Esercizio_Pizzeria_In_Forno.Service;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio_Pizzeria_In_Forno.Controllers
{
    public class IngredientController : Controller
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        // Crea un nuovo ingrediente
        [HttpGet]
        public IActionResult CreateIngredient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredient(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                var createdIngredient = await _ingredientService.CreateIngredientAsync(ingredient);
                return RedirectToAction("IngredientDetails", new { id = createdIngredient.Id });
            }
            return View(ingredient);
        }

               // Ottieni tutti gli ingredienti
        [HttpGet]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _ingredientService.GetAllIngredientsAsync();
            return View(ingredients);
        }

        // Aggiorna un ingrediente
        [HttpGet]
        public async Task<IActionResult> UpdateIngredient(int id)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return View(ingredient);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateIngredient(int id, Ingredient updatedIngredientDetails)
        {
            if (ModelState.IsValid)
            {
                var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
                if (ingredient == null)
                {
                    return NotFound();
                }

                // Aggiorna le proprietà necessarie
                ingredient.Name = updatedIngredientDetails.Name;

                var updatedIngredient = await _ingredientService.UpdateIngredientAsync(ingredient);
                return RedirectToAction("IngredientDetails", new { id = updatedIngredient.Id });
            }
            return View(updatedIngredientDetails);
        }

        // Elimina un ingrediente
        [HttpPost]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            await _ingredientService.DeleteIngredientAsync(id);
            return RedirectToAction("GetAllIngredients");
        }
    }
}
