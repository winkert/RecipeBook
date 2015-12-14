using System.ComponentModel;

namespace RecipeBook
{
    public partial class Ingredient : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public override string ToString()
        {
            return ing_Name;
        }
    }
    public partial class Measurement : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public override string ToString()
        {
            return mes_Name;
        }
    }
    public partial class Recipe : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public override string ToString()
        {
            return rec_Name;
        }
    }
    public partial class RecipeCategory : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public override string ToString()
        {
            return cat_Name;
        }
    }
}