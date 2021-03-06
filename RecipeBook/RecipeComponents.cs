﻿using RecipeBook.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

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
                    var original = (from ing in db.Ingredients where ing.ing_ID == id select ing).First();
                    original.ing_Name = name;
                    original.ing_Description = description;
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
        public static void InsertRecipe(string name, string source, RecipeCategory category, string description, string prep, string cook, List<IngredientEntry> ingredients, out int recid)
        {
            int id;
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    Recipe rec = new Recipe();
                    rec.rec_Name = name;
                    rec.rec_Source = source;
                    rec.rec_Description = description;
                    rec.rec_PreparationInstructions = prep;
                    rec.rec_CookingInstructions = cook;
                    rec.rec_EntryDate = DateTime.Today;
                    rec.cat_ID = category.cat_ID;
                    db.Recipes.InsertOnSubmit(rec);
                    db.SubmitChanges();
                    id = rec.rec_ID;
                }
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    var recipe = (from rec in db.Recipes where rec.rec_ID == id select rec).First();
                    foreach (IngredientEntry i in ingredients)
                    {
                        RecipeIngredient recing = new RecipeIngredient();
                        recing.rec_ID = id;
                        recing.ing_ID = i.Ingredient.ing_ID;
                        recing.mes_ID = i.Measurement.mes_ID;
                        recing.ri_Amount = i.Amount;
                        recipe.RecipeIngredients.Add(recing);
                    }
                    db.SubmitChanges();
                }
                recid = id;
            }
            catch (Exception)
            {
                recid = -1;
                throw;
            }
        }
        public static void UpdateRecipe(string name, string source, RecipeCategory category, string description, string prep, string cook, List<IngredientEntry> ingredients, int rec_ID)
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    var original = (from rec in db.Recipes where rec.rec_ID == rec_ID select rec).First();
                    original.rec_Name = name;
                    original.rec_Source = source;
                    original.rec_Description = description;
                    original.rec_PreparationInstructions = prep;
                    original.rec_CookingInstructions = cook;
                    original.cat_ID = category.cat_ID;
                    db.SubmitChanges();
                }
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    var original = (from rec in db.Recipes where rec.rec_ID == rec_ID select rec.RecipeIngredients).First();
                    original.Clear();
                    foreach (IngredientEntry i in ingredients)
                    {
                        RecipeIngredient recTemp = new RecipeIngredient();
                        recTemp.ing_ID = i.Ingredient.ing_ID;
                        recTemp.mes_ID = i.Measurement.mes_ID;
                        recTemp.ri_Amount = i.Amount;
                        recTemp.rec_ID = rec_ID;
                        original.Add(recTemp);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void DeleteRecipe(int rec_ID)
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    var recings = from ri in db.RecipeIngredients where ri.rec_ID == rec_ID select ri;
                    db.RecipeIngredients.DeleteAllOnSubmit(recings);
                    db.SubmitChanges();
                }
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    var rec = from r in db.Recipes where r.rec_ID == rec_ID select r;
                    db.Recipes.DeleteOnSubmit(rec.First());
                    db.SubmitChanges();
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
    /// <summary>
    /// Object class to hold information about recipes from the database and display the information in a readable way.
    /// </summary>
    public class RecipeEntry
    {
        public RecipeEntry() { }
        public RecipeEntry(string name, string source, RecipeCategory category, string description, string prep, string cook, List<IngredientEntry> ingredients, int id)
        {
            _ingredients = ingredients;
            _name = name;
            _category = category;
            _description = description;
            _source = source;
            _prepinstructions = prep;
            _cookinstructions = cook;
            recID = id;
    }
        private List<IngredientEntry> _ingredients;
        private string _name;
        private RecipeCategory _category;
        private string _description;
        private string _source;
        private string _prepinstructions;
        private string _cookinstructions;
        public int recID;
        public List<IngredientEntry> Ingredients { get { return _ingredients; } }
        public string Name { get { return _name; } }
        public RecipeCategory Category { get { return _category; } }
        public string Description { get { return _description; } }
        public string Source { get { return _source; } }
        public string PrepInstructions { get { return _prepinstructions; } }
        public string CookInstructions { get { return _cookinstructions; } }
        public void UpdateRecipe(string name, string description, string source, string prep, string cook, List<IngredientEntry> ingredients)
        {
            _ingredients = ingredients;
            _name = name;
            _description = description;
            _source = source;
            _prepinstructions = prep;
            _cookinstructions = cook;
        }
        public override string ToString()
        {
            return Name + "(" + ConcatenateIngredients() + ")";
        }
        private string ConcatenateIngredients()
        {
            string ingredients = "";
            int count = 0;
            foreach(IngredientEntry i in _ingredients)
            {
                count++;
                ingredients += i.Ingredient.ing_Name + "(" + i.Amount + " " + i.Measurement.mes_Name + ")";
                if (count > 3)
                    break;
                else
                    ingredients += ", ";
            }
            char last = ingredients.LastOrDefault();
            if (last == ' ')
                ingredients = ingredients.Substring(0, ingredients.Length - 2);
            return ingredients;
        }
    }
    /// <summary>
    /// Object class to hold information about ingredients from the database and display the information in a readable way.
    /// </summary>
    public class IngredientEntry
    {
        public IngredientEntry() { }
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
                MesName = Common.Pluralize(MesName);
                if(Measurement.mes_Type == (int)MeasurementTypes.count)
                    IngName = Common.Pluralize(IngName);
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
