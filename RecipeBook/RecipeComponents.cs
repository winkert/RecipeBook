using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Components
{
    public static class Recipes
    {
        #region Ingredients
        public static void InsertIngredient(string name, string description)
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    Ingredient i = new Ingredient();
                    i.ing_Name = name;
                    i.ing_Description = description;
                    db.Ingredients.InsertOnSubmit(i);
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void UpdateIngredient(int id, string name, string description)
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    var original = from ing in db.Ingredients where ing.ing_ID == id select ing;
                    foreach (Ingredient m in original)
                    {
                        m.ing_Name = name;
                        m.ing_Description = description;
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void DeleteIngredient(int id)
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    Ingredient i = db.Ingredients.Where(n => n.ing_ID == id).First();
                    db.Ingredients.DeleteOnSubmit(i);
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Measurements
        public static void InsertMeasurement(string name, string description, MeasurementTypes type)
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    Measurement m = new Measurement();
                    m.mes_Name = name;
                    m.mes_Description = description;
                    m.mes_Type = (int)type;
                    db.Measurements.InsertOnSubmit(m);
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void UpdateMeasurement(int id, string name, string description, MeasurementTypes type)
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    var original = from mes in db.Measurements where mes.mes_ID == id select mes;
                    foreach (Measurement m in original)
                    {
                        m.mes_Name = name;
                        m.mes_Description = description;
                        m.mes_Type = (int)type;
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void DeleteMeasurement(int id)
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    Measurement m = db.Measurements.Where(i => i.mes_ID == id).First();
                    db.Measurements.DeleteOnSubmit(m);
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Recipes
        public static void InsertRecipe(string name, string source, string description, string prep, string cook, List<IngredientEntry> ingredients)
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void UpdateRecipe(string name, string source, string description, string prep, string cook, List<IngredientEntry> ingredients, int rec_ID)
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void DeleteRecipe(string name, string source, string description, string prep, string cook, List<IngredientEntry> ingredients, int rec_ID)
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public static List<MeasurementTypes> getMeasurementTypes()
        {
            List<MeasurementTypes> t = new List<MeasurementTypes>();
            //t.Add(MeasurementTypes._default);
            t.Add(MeasurementTypes.weight);
            t.Add(MeasurementTypes.volume);
            t.Add(MeasurementTypes.count);
            return t;
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
                if (ultimateLetter == 's' || (ultimateLetter == 'h' && penultimateLetter == "s"))
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
                MesName = Utilities.Pluralize(MesName);
                if(Measurement.mes_Type == (int)MeasurementTypes.count)
                    IngName = Utilities.Pluralize(IngName);
            }
            if (Measurement.mes_Type == (int)MeasurementTypes.count)
                return Amount.ToString() + " " + IngName;
            else
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
