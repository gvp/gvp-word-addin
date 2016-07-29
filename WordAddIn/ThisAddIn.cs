using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            SetUICulture(Application);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        protected override IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            SetUICulture(GetHostItem<Word.Application>(typeof(Word.Application), "Application"));
            return new Ribbon();
        }

        private static void SetUICulture(Word.Application app)
        {
            int lcid = app.LanguageSettings.get_LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lcid);
        }

        public void TransformText(params ITextTransform[] transforms)
        {
            TransformText(new CombiningTransform(transforms));
        }

        public void TransformText(ITextTransform transform)
        {
            Application.UndoRecord.StartCustomRecord(Properties.Resources.FontTransformationUndoRecord);
            Application.ScreenUpdating = false;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var range = Application.Selection.Range;
                if (range.Characters.Count == 0)
                    return;
                transform.Apply(range);
                range.Select();
            }
            finally
            {
                Trace.WriteLine(stopwatch.Elapsed);
                Application.ScreenUpdating = true;
                Application.UndoRecord.EndCustomRecord();
            }
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
