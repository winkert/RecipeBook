using RecipeBook.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeBook
{
    /// <summary>
    /// Interaction logic for EditMeasurements.xaml
    /// </summary>
    public partial class EditMeasurements : Window
    {
        public EditMeasurements()
        {
            InitializeComponent();
            cmb_Type.ItemsSource = Recipes.getMeasurementTypes();
            RefreshList();
        }
        private int SelectedID = -1;
        #region Methods
        private void RefreshList()
        {
            using (RecipeBook_DataModelDataContext db = new RecipeBook_DataModelDataContext())
            {
                lst_Measurements.ItemsSource = db.Measurements.ToList();
            }
        }
        private void ClearFields()
        {
            txt_Name.Text = string.Empty;
            txt_Description.Text = string.Empty;
            cmb_Type.SelectedIndex = -1;
        }
        #endregion
        #region Events
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Recipes.InsertMeasurement(txt_Name.Text, txt_Description.Text, (MeasurementTypes)cmb_Type.SelectedItem);
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
            if (SelectedID > -1)
            {
                Recipes.DeleteMeasurement(SelectedID);
                RefreshList();
            }
            else
            {
                MessageBox.Show("Please select a measurement to delete.");
            }
        }
        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedID > -1)
            {
                Recipes.UpdateMeasurement(SelectedID, txt_Name.Text, txt_Description.Text, (MeasurementTypes)cmb_Type.SelectedItem);
                RefreshList();
                ClearFields(); 
            }
            else
            {
                MessageBox.Show("Please select a measurement to save.");
            }
        }
        private void lst_Measurements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_Measurements.SelectedIndex > -1)
            {
                Measurement sel = (Measurement)e.AddedItems[0];
                txt_Name.Text = sel.mes_Name;
                txt_Description.Text = sel.mes_Description;
                cmb_Type.SelectedItem = (MeasurementTypes)sel.mes_Type;
                SelectedID = sel.mes_ID;
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
