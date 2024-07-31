using Esercizio_Pizzeria_In_Forno.Context;
using Esercizio_Pizzeria_In_Forno.Models;
using Microsoft.EntityFrameworkCore;

namespace Esercizio_Pizzeria_In_Forno.Service
{
    public class IngredientService : IIngredientService
    {
        private readonly DataContext _context;

        public IngredientService(DataContext context)
        {
            _context = context;
        }

        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }

        public async Task<Ingredient> GetIngredientByIdAsync(int ingredientId)
        {
            return await _context.Ingredients.FindAsync(ingredientId);
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task<Ingredient> UpdateIngredientAsync(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }

        public async Task DeleteIngredientAsync(int ingredientId)
        {
            var ingredient = await _context.Ingredients.FindAsync(ingredientId);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
            }
        }
    }
}
