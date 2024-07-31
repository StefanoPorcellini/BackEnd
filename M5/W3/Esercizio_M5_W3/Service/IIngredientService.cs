using Esercizio_Pizzeria_In_Forno.Models;

namespace Esercizio_Pizzeria_In_Forno.Service
{
    public interface IIngredientService
    {
        Task<Ingredient> CreateIngredientAsync(Ingredient ingredient);
        Task<Ingredient> GetIngredientByIdAsync(int ingredientId);
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        Task<Ingredient> UpdateIngredientAsync(Ingredient ingredient);
        Task DeleteIngredientAsync(int ingredientId);

    }
}
