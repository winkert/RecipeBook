using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using RecipeBook.Components;
using System.Collections.Generic;
using System.Diagnostics;

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
            Paragraph headerText = section.AddParagraph();
            Paragraph DescParagraph = section.AddParagraph();
            Paragraph ingredientList = section.AddParagraph();
            Paragraph PrepInstr = section.AddParagraph();
            Paragraph CookInstr = section.AddParagraph();
            Font header = new Font("Times New Roman Bold", 20);
            Font body = new Font("Times New Roman", 14);
            headerText.Format = HeaderFormatter();
            headerText.AddFormattedText(recipe.Name, header);
            headerText.AddLineBreak();
            headerText.AddFormattedText("(" + recipe.Source + ")", new Font("Times New Roman", 16));
            DescParagraph.Format = BodyFormatter();
            DescParagraph.AddFormattedText(recipe.Description, body);
            ingredientList.Format = IngredientFormatter();
            int count = 1;
            foreach (IngredientEntry i in recipe.Ingredients)
            {
                string entry = count.ToString() + ") " + i.ToString();
                ingredientList.AddFormattedText(entry, body);
                ingredientList.AddLineBreak();
                count++;
            }
            PrepInstr.Format = BodyFormatter();
            PrepInstr.AddFormattedText(recipe.PrepInstructions, body);
            CookInstr.Format = BodyFormatter();
            CookInstr.AddFormattedText(recipe.CookInstructions, body);
        }
        private static ParagraphFormat HeaderFormatter()
        {
            ParagraphFormat headerformat = new ParagraphFormat();
            headerformat.SpaceAfter = new Unit(20);
            return headerformat;
        }
        private static ParagraphFormat IngredientFormatter()
        {
            ParagraphFormat ingredientformat = new ParagraphFormat();
            ingredientformat.LeftIndent = new Unit(15);
            ingredientformat.SpaceAfter = new Unit(15);
            return ingredientformat;
        }
        private static ParagraphFormat BodyFormatter()
        {
            ParagraphFormat bodyformat = new ParagraphFormat();
            bodyformat.FirstLineIndent = new Unit(20);
            bodyformat.SpaceAfter = new Unit(15);
            return bodyformat;
        }
    }
}
