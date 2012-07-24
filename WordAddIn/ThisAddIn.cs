using System.Diagnostics;

namespace VedicEditor
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        public void TransformText(params ITextTransform[] transforms)
        {
            TransformText(new CombiningTransform(transforms));
        }

        public void TransformText(ITextTransform transform)
        {
            Application.UndoRecord.StartCustomRecord("Преобразование текста");
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
