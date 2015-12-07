using RecipeBook.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            BuildComboBoxes();
        }
        #region Fields
        List<IngredientEntry> _Ingredients = new List<IngredientEntry>();
        //To be deleted
        List<Ingredient> allIngredients = new List<Ingredient>();
        List<Measurement> allMeasurements = new List<Measurement>();
        #endregion

        #region Methods
        private void BuildComboBoxes()
        {
            //using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
            //{
            //    cmb_IngredientNames.ItemsSource = db.Ingredients;
            //    cmb_Measurement.ItemsSource = db.Measurements;
            //}
            for (int n = 0; n < 3; n++)
            {
                Ingredient i = new Ingredient();
                i.ing_Name = "Ingredient" + n.ToString();
                Measurement m = new Measurement();
                m.mes_Name = "Measurement" + n.ToString();
                m.mes_Type = n;
                allIngredients.Add(i);
                allMeasurements.Add(m); 
            }
            cmb_IngredientNames.ItemsSource = allIngredients;
            cmb_Measurement.ItemsSource = allMeasurements;
        }
        private void RefreshIngredientList()
        {
            lst_Ingredients.ItemsSource = null;
            lst_Ingredients.ItemsSource = _Ingredients;
        }

        public void NewRecipe()
        {
            foreach(TextBox c in MainGrid.Children.OfType<TextBox>())
            {
                c.Text = string.Empty;
            }
            _Ingredients.Clear();
            RefreshIngredientList();
        }
        public void SaveRecipe()
        {
            Recipes.InsertRecipe(txt_RecipeName.Text, txt_RecipeSource.Text, txt_RecipeDescription.Text, txt_RecipePrepInstructions.Text, txt_RecipeCookInstructions.Text, _Ingredients);
        }

        #region Data Validation
        /// <summary>
        /// Checks if the current recipe has been saved based on the values of the text fields.
        /// </summary>
        /// <returns>new bool</returns>
        public bool HasSaved()
        {
            foreach (TextBox c in RecipeEntry.Children.OfType<TextBox>())
            {
                if (c.Text.Length > 0)
                    return false;
            }
            return true;
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

        }
        private void EditIngredients_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ViewRecipes_MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

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
                Ingredient i = (Ingredient)cmb_IngredientNames.SelectedItem;
                Measurement m = (Measurement)cmb_Measurement.SelectedItem;
                IngredientEntry iEntry = new IngredientEntry(i, m, txt_Amount.Text);
                _Ingredients.Add(iEntry);
            }
            RefreshIngredientList();
        }
        private void btn_DelIngredient_Click(object sender, RoutedEventArgs e)
        {
            _Ingredients.RemoveAt(lst_Ingredients.SelectedIndex);
            RefreshIngredientList();
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

    }
}
