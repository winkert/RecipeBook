using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
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
            const bool unicode = false;
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
            Document file = new Document();
            file.Info.Title = recipe.Name;
            Section section = file.AddSection();
            DrawRecipe(recipe, ref section);
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);
            pdfRenderer.Document = file;
            pdfRenderer.RenderDocument();
            pdfRenderer.Save(filename);
            if (preview)
                Process.Start(filename);
        }
        public static void AllRecipesPDF(List<RecipeEntry> recipes, string filename, bool preview)
        {
            const bool unicode = false;
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
            Document file = new Document();
            file.Info.Title = "Recipe Book";
            foreach (RecipeEntry recipe in recipes)
            {
                Section section = file.AddSection();
                DrawRecipe(recipe, ref section);
            }
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);
            pdfRenderer.Document = file;
            pdfRenderer.RenderDocument();
            pdfRenderer.Save(filename);
            if (preview)
                Process.Start(filename);
        }
        private static void DrawRecipe(RecipeEntry recipe, ref Section section)
        {
            ///<example>
            ///     [Recipe Name]                   [Recipe Source]
            ///  [Recipe Description]
            ///  [# Recipe Ingredients]
            ///  [Recipe Preparation Instructions]
            ///  [Recipe Cooking Instructions]
            /// </example>
            Paragraph paragraph = section.AddParagraph();
            //tfx.Alignment = XParagraphAlignment.Left;
            double width = 1024;
            Font header = new Font("Times New Roman Bold", 20);
            Font body = new Font("Times New Roman", 14);
            XBrush black = XBrushes.Black;
            double lineheight = 15;
            double cpl = 100; //Characters per Line
            double DescHeight = (recipe.Description.Length / cpl) * lineheight;
            double IngredientHeight = (recipe.Ingredients.Count) * (lineheight + 5);
            double PrepHeight = (recipe.PrepInstructions.Length / cpl) * lineheight;
            double CookHeight = (recipe.CookInstructions.Length / cpl) * lineheight;
            XRect bounding = new XRect(30, 30, width, lineheight);
            paragraph.AddFormattedText(recipe.Name, header);

            //tfx.DrawString(recipe.Name, header, black, bounding, XStringFormats.TopLeft);
            //bounding = new XRect(300, 30, width, lineheight);
            //tfx.DrawString(recipe.Source, header, black, bounding, XStringFormats.TopLeft);
            //bounding = new XRect(10, 60, width, DescHeight);
            //tfx.DrawString(recipe.Description, body, black, bounding, XStringFormats.TopLeft);
            double spacing = 0;
            int count = 1;
            foreach(IngredientEntry i in recipe.Ingredients)
            {
                bounding = new XRect(20, 70 + DescHeight + spacing, width, lineheight);
                
                //tfx.DrawString(count.ToString() + ") " + i.ToString(), body, black, bounding, XStringFormats.TopLeft);
                spacing += lineheight;
                count++;
            }
            bounding = new XRect(10, 70 + DescHeight + IngredientHeight, width, PrepHeight);
            tfx.DrawString(recipe.PrepInstructions, body, black, bounding, XStringFormats.TopLeft);
            bounding = new XRect(10, 80 + DescHeight + IngredientHeight + PrepHeight, width, CookHeight);
            tfx.DrawString(recipe.CookInstructions, body, black, bounding, XStringFormats.TopLeft);
        }
    }
}
