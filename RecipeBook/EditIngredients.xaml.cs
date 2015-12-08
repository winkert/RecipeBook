using RecipeBook.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeBook
{
    /// <summary>
    /// Interaction logic for EditIngredients.xaml
    /// </summary>
    public partial class EditIngredients : Window
    {
        public EditIngredients()
        {
            InitializeComponent();
            RefreshList();
        }
        private int SelectedID = -1;
        #region Methods
        private void RefreshList()
        {
            using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
            {
                lst_Ingredients.ItemsSource = db.Ingredients.ToList();
            }
        }
        private void ClearFields()
        {
            txt_Name.Text = string.Empty;
            txt_Description.Text = string.Empty;
        }
        #endregion
        #region Events
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Recipes.InsertIngredient(txt_Name.Text, txt_Description.Text);
                RefreshList();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Recipes.DeleteIngredient(SelectedID);
            RefreshList();
        }
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Recipes.UpdateIngredient(SelectedID, txt_Name.Text, txt_Description.Text);
            RefreshList();
            ClearFields();
        }
        private void lst_Measurements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_Ingredients.SelectedIndex > -1)
            {
                Ingredient sel = (Ingredient)e.AddedItems[0];
                txt_Name.Text = sel.ing_Name;
                txt_Description.Text = sel.ing_Description;
                SelectedID = sel.ing_ID;
            }
            else
            {
                txt_Name.Text = string.Empty;
                txt_Description.Text = string.Empty;
                SelectedID = -1;
            }
        }
        #endregion
    }
}
