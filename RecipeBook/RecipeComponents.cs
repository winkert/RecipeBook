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

    public static class Utilities
    {
        public static string Pluralize(string s)
        {
            if (s.Length < 1)
                return s;
            char ultimateLetter = s.Last();
            string penultimateLetter = s.Substring(s.Length - 2, 1);
            if (ultimateLetter == 'y' && "aeiou".IndexOf(penultimateLetter) < 0)
            {
                return s.Substring(0, s.Length - 1) + "ies";
            }
            else
                if (ultimateLetter == 's')
            {
                return s + "es";
            }
            else
            {
                return s + "s";
            }
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
            string IngName = Ingredient.ing_Name;
            string MesName = Measurement.mes_Name;
            if(Amount > 1)
            {
                IngName = Utilities.Pluralize(IngName);
                MesName = Utilities.Pluralize(MesName);
            }
            if (Measurement.mes_Type == (int)MeasurementTypes.count)
            {
                return Amount.ToString() + " " + IngName;
            }
            return Amount.ToString() + " " + MesName + " of " + IngName;
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
