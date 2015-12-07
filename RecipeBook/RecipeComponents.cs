using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Components
{
    public static class Recipes
    {
        public static void InsertRecipe(string name, string source, string description, string prep, string cook, List<IngredientEntry> ingredients)
        {
            
        }
        public static void UpdateRecipe(string name, string source, string description, string prep, string cook, List<IngredientEntry> ingredients, int rec_ID)
        {

        }
    }

    public class IngredientEntry
    {
        public IngredientEntry()
        {

        }
        public IngredientEntry(Ingredient i, Measurement m, double amount)
        {
            _ingredient = i;
            _measurement = m;
            _amount = amount;
        }
        public IngredientEntry(Ingredient i, Measurement m, string amount)
        {
            _ingredient = i;
            _measurement = m;
            if(!double.TryParse(amount, out _amount))
            {
                _amount = 0.00;
            }
        }
        private Ingredient _ingredient;
        private Measurement _measurement;
        private double _amount;
        public Ingredient Ingredient { get { return _ingredient; } }
        public Measurement Measurement { get { return _measurement; } }
        public double Amount { get { return _amount; } }

        public override string ToString()
        {

            return Amount.ToString() + " " + Measurement.mes_Name + " of " + Ingredient.ing_Name;
        }
    }

    public enum MeasurementTypes
    {
        _default = 0,
        weight,
        volume,
        count
    }
}
