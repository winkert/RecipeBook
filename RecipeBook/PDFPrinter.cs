using PdfSharp.Drawing;
using PdfSharp.Pdf;
using RecipeBook.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Utilities
{
    public static class PDFPrinter
    {
        public static void SingleRecipePDF(RecipeEntry recipe, string filename, bool preview)
        {
            PdfDocument file = new PdfDocument();
            file.Info.Title = recipe.Name;
            PdfPage page = file.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawRecipe(recipe, ref gfx, page.Width);
            file.Save(filename);
            if (preview)
                Process.Start(filename);
        }
        public static void AllRecipesPDF(List<RecipeEntry> recipes, string filename, bool preview)
        {
            PdfDocument file = new PdfDocument();
            file.Info.Title = "Recipe Book";
            foreach (RecipeEntry recipe in recipes)
            {
                PdfPage page = file.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                DrawRecipe(recipe, ref gfx, page.Width); 
            }
            file.Save(filename);
            if (preview)
                Process.Start(filename);
        }
        private static void DrawRecipe(RecipeEntry recipe, ref XGraphics gfx, double width)
        {
            ///<example>
            ///     [Recipe Name]                   [Recipe Source]
            ///  [Recipe Description]
            ///  [# Recipe Ingredients]
            ///  [Recipe Preparation Instructions]
            ///  [Recipe Cooking Instructions]
            /// </example>
            XFont header = new XFont("Times New Roman", 20, XFontStyle.Bold);
            XFont body = new XFont("Times New Roman", 14);
            XBrush black = XBrushes.Black;
            double lineheight = 15;
            double cpl = 300; //Characters per Line
            double DescHeight = (recipe.Description.Count() / cpl) * lineheight;
            double PrepHeight = (recipe.PrepInstructions.Count() / cpl) * lineheight;
            double CookHeight = (recipe.CookInstructions.Count() / cpl) * lineheight;
            gfx.DrawString(recipe.Name, header, black, new XRect(30, 30, 100, 25));
            gfx.DrawString(recipe.Source, header, black, new XRect(300, 30, 100, 25));
            gfx.DrawString(recipe.Description, body, black, new XRect(10, 60, width, DescHeight));
            gfx.DrawString(recipe.PrepInstructions, body, black, new XRect(10, 65 + DescHeight, width, PrepHeight));
            gfx.DrawString(recipe.CookInstructions, body, black, new XRect(10, 70 + DescHeight + PrepHeight, width, CookHeight));
        }
    }
}
