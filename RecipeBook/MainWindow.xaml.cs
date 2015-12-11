using Microsoft.Win32;
using RecipeBook.Components;
using RecipeBook.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BuildIngredientComboBox();
            BuildMeasurementComboBox();
            BuildRecipeListBox();
        }
        #region Fields
        List<IngredientEntry> _Ingredients = new List<IngredientEntry>();
        List<RecipeEntry> _Recipes = new List<RecipeEntry>();
        int SelectedRecipe = -1;
        #endregion
        #region Methods
        #region Dynamic Fields
        private void BuildIngredientComboBox()
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    cmb_IngredientNames.ItemsSource = db.Ingredients;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Building Ingredient Combobox failed: " + e.Message);
            }
        }
        private void BuildMeasurementComboBox()
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    cmb_Measurement.ItemsSource = db.Measurements;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Building Measurement Combobox failed: " + e.Message);
            }
        }
        private void BuildRecipeListBox()
        {
            try
            {
                using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
                {
                    var r = from re in db.Recipes select re;
                    foreach(Recipe re in r)
                    {
                        var i = from ing in db.Ingredients join ri in db.RecipeIngredients on ing.ing_ID equals ri.ing_ID where ri.rec_ID == re.rec_ID select ing;
                        List<IngredientEntry> ingredients = new List<IngredientEntry>();
                        foreach(Ingredient ing in i)
                        {
                            var m = from mes in db.RecipeIngredients where mes.ing_ID == ing.ing_ID && mes.rec_ID == re.rec_ID select mes.Measurement;
                            var a = from mes in db.RecipeIngredients where mes.ing_ID == ing.ing_ID && mes.rec_ID == re.rec_ID select mes.ri_Amount;
                            IngredientEntry ingred = new IngredientEntry(ing, m.First(), (double)a.First());
                            ingredients.Add(ingred);
                        }
                        RecipeEntry rec = new RecipeEntry(re.rec_Name,re.rec_Description,re.rec_Source,re.rec_PreparationInstructions,re.rec_CookingInstructions, ingredients, re.rec_ID);
                        _Recipes.Add(rec);
                    }
                }
                lst_Recipes.ItemsSource = _Recipes;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void RefreshIngredientList()
        {
            lst_Ingredients.ItemsSource = null;
            lst_Ingredients.ItemsSource = _Ingredients;
        }
        private void RefreshRecipeList()
        {
            lst_Recipes.ItemsSource = null;
            lst_Recipes.ItemsSource = _Recipes;
        }
        #endregion
        public void NewRecipe()
        {
            foreach(TextBox c in MainGrid.Children.OfType<TextBox>())
            {
                c.Text = string.Empty;
            }
            lst_Recipes.SelectedIndex = -1;
            _Ingredients.Clear();
            RefreshIngredientList();
            txt_RecipeName.Focus();
        }
        public void SaveRecipe()
        {
            int id;
            if (SelectedRecipe == -1)
            {
                try
                {
                    Recipes.InsertRecipe(txt_RecipeName.Text, txt_RecipeSource.Text, txt_RecipeDescription.Text, txt_RecipePrepInstructions.Text, txt_RecipeCookInstructions.Text, _Ingredients, out id);
                }
                catch (Exception e)
                {
                    id = -1;
                    MessageBox.Show(e.Message);
                }
                RecipeEntry _rec = new RecipeEntry(txt_RecipeName.Text, txt_RecipeSource.Text, txt_RecipeDescription.Text, txt_RecipePrepInstructions.Text, txt_RecipeCookInstructions.Text, _Ingredients, id);
                _Recipes.Add(_rec); 
            }
            else
            {
                id = SelectedRecipe;
                try
                {
                    Recipes.UpdateRecipe(txt_RecipeName.Text, txt_RecipeSource.Text, txt_RecipeDescription.Text, txt_RecipePrepInstructions.Text, txt_RecipeCookInstructions.Text, _Ingredients, id);
                }
                catch(Exception e)
                {
                    id = -1;
                    MessageBox.Show(e.Message);
                }
                _Recipes.Where(r => r.recID == SelectedRecipe).First().UpdateRecipe(txt_RecipeName.Text, txt_RecipeSource.Text, txt_RecipeDescription.Text, txt_RecipePrepInstructions.Text, txt_RecipeCookInstructions.Text, _Ingredients);
            }
            RefreshRecipeList();
        }
        public void NewIngredient()
        {
            cmb_IngredientNames.SelectedIndex = -1;
            cmb_Measurement.SelectedIndex = -1;
            txt_Amount.Text = string.Empty;
            cmb_IngredientNames.Focus();
        }
        public void SaveIngredient()
        {
            Ingredient i = (Ingredient)cmb_IngredientNames.SelectedItem;
            Measurement m = (Measurement)cmb_Measurement.SelectedItem;
            IngredientEntry iEntry = new IngredientEntry(i, m, txt_Amount.Text);
            _Ingredients.Add(iEntry);
        }
        #region Data Validation
        /// <summary>
        /// Checks if the current recipe has been saved based on the values of the text fields.
        /// </summary>
        /// <returns>new bool</returns>
        public bool HasSaved()
        {
            bool hasSaved = true;
            foreach (TextBox c in grd_RecipeEntry.Children.OfType<TextBox>())
            {
                if (c.Text.Length > 0)
                    hasSaved = false;
            }
            RecipeEntry recTemp = new RecipeEntry(txt_RecipeName.Text, txt_RecipeSource.Text, txt_RecipeDescription.Text, txt_RecipePrepInstructions.Text, txt_RecipeCookInstructions.Text, _Ingredients, SelectedRecipe);
            if (_Recipes.Contains(recTemp))
            {
                hasSaved = true;
            }
            return hasSaved;
        }
        /// <summary>
        /// Checks if the data for the ingredient to be added has been entered completely and correctly.
        /// </summary>
        /// <returns>new bool</returns>
        public bool ValidateIngredient(out string result)
        {
            result = "";
            double amount;
            if(cmb_IngredientNames.SelectedIndex == -1)
            {
                result = "Please select an ingredient.";
                return false;
            } else
            if(cmb_Measurement.SelectedIndex == -1)
            {
                result = "Please select a measurement.";
                return false;
            } else
            if(txt_Amount.Text.Length < 1)
            {
                result = "Please enter an amount.";
                return false;
            } else
            if(!double.TryParse(txt_Amount.Text, out amount))
            {
                result = "Please enter a numeric amount.";
                return false;
            } else
                return true;
        }
        #endregion
        #endregion
        #region Event Handlers
        #region Menu Events
        private void NewRecipe_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!HasSaved())
            {
                MessageBoxResult result = MessageBox.Show("You have not saved your current recipe, are you sure you want to start a new one?", "Do you want to save first?", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    NewRecipe();
                } 
            }
        }
        private void SaveRecipe_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!HasSaved())
            {
                SaveRecipe();
            }
            else
            {
                MessageBox.Show("There is no recipe to save.");
            }
        }
        private void DeleteRecipe_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Recipes.DeleteRecipe(SelectedRecipe);
            _Recipes.RemoveAt(lst_Recipes.SelectedIndex);
            RefreshRecipeList();
        }
        private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!HasSaved())
            {
                MessageBoxResult result = MessageBox.Show("You have not saved your current recipe, are you sure you want to quit?", "Do you want to save first?", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    Close();
                } 
            }
            else
            {
                Close();
            }
        }
        private void EditMeasurements_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditMeasurements window;
            if (OwnedWindows.OfType<EditMeasurements>().Count() == 0)
            {
                window = new EditMeasurements(); 
            }
            else
            {
                window = OwnedWindows.OfType<EditMeasurements>().First();
            }
            window.Show();
        }
        private void EditIngredients_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditIngredients window;
            if (OwnedWindows.OfType<EditIngredients>().Count() == 0)
            {
                window = new EditIngredients();
            }
            else
            {
                window = OwnedWindows.OfType<EditIngredients>().First();
            }
            window.Show();
        }
        private void ViewRecipes_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewRecipes window;
            if (OwnedWindows.OfType<ViewRecipes>().Count() == 0)
            {
                window = new ViewRecipes();
            }
            else
            {
                window = OwnedWindows.OfType<ViewRecipes>().First();
            }
            window.Show();
        }
        private void PrintRecipe_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".pdf";
            save.Filter = "PDF (.pdf)|*.pdf";
            bool? result = save.ShowDialog();
            if (result == true)
            {
                string filename = save.FileName;
                PDFPrinter.SingleRecipePDF(_Recipes[lst_Recipes.SelectedIndex], filename, true);
            }
        }
        private void PrintAllRecipes_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".pdf";
            save.Filter = "PDF (.pdf)|*.pdf";
            bool? result = save.ShowDialog();
            if (result == true)
            {
                string filename = save.FileName;
                PDFPrinter.AllRecipesPDF(_Recipes, filename, true);
            }
        }
        #endregion
        #region Ingredient Events
        private void btn_AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            string Message = "";
            if(!ValidateIngredient(out Message))
            {
                MessageBox.Show(Message);
            }
            else
            {
                SaveIngredient();
            }
            RefreshIngredientList();
            NewIngredient();
        }
        private void btn_DelIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (lst_Ingredients.SelectedIndex > -1)
            {
                _Ingredients.RemoveAt(lst_Ingredients.SelectedIndex);
                RefreshIngredientList();
                NewIngredient(); 
            }
        }
        private void lst_Ingredients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_Ingredients.SelectedIndex > -1)
            {
                IngredientEntry sel = _Ingredients[lst_Ingredients.SelectedIndex];
                cmb_IngredientNames.SelectedItem = sel.Ingredient;
                cmb_Measurement.SelectedItem = sel.Measurement;
                txt_Amount.Text = sel.Amount.ToString();
            }
            else
            {
                cmb_IngredientNames.SelectedIndex = -1;
                cmb_Measurement.SelectedIndex = -1;
                txt_Amount.Text = string.Empty;
            }
        }
        #endregion
        private void MainWindow_Activated(object sender, EventArgs e)
        {
            BuildIngredientComboBox();
            BuildMeasurementComboBox();
            RefreshIngredientList();
            //RefreshRecipeList();
        }
        private void lst_Recipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_Recipes.SelectedIndex > -1)
            {
                RecipeEntry sel = _Recipes[lst_Recipes.SelectedIndex];
                txt_RecipeName.Text = sel.Name;
                txt_RecipeDescription.Text = sel.Description;
                txt_RecipePrepInstructions.Text = sel.PrepInstructions;
                txt_RecipeCookInstructions.Text = sel.CookInstructions;
                txt_RecipeSource.Text = sel.Source;
                _Ingredients = sel.Ingredients;
                RefreshIngredientList();
                SelectedRecipe = sel.recID;
            }
            else
            {
                txt_RecipeName.Text = string.Empty;
                txt_RecipeDescription.Text = string.Empty;
                txt_RecipePrepInstructions.Text = string.Empty;
                txt_RecipeCookInstructions.Text = string.Empty;
                txt_RecipeSource.Text = string.Empty;
                _Ingredients = new List<IngredientEntry>();
                RefreshIngredientList();
                SelectedRecipe = -1;
            }
        }
        #endregion
    }
}
