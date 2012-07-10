namespace VedicEditor
{
    partial class Ribbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabVedicEditor = this.Factory.CreateRibbonTab();
            this.groupDiacrytics = this.Factory.CreateRibbonGroup();
            this.buttonConvertToThamesM = this.Factory.CreateRibbonButton();
            this.tabVedicEditor.SuspendLayout();
            this.groupDiacrytics.SuspendLayout();
            // 
            // tabVedicEditor
            // 
            this.tabVedicEditor.Groups.Add(this.groupDiacrytics);
            this.tabVedicEditor.Label = "Ведический редактор";
            this.tabVedicEditor.Name = "tabVedicEditor";
            // 
            // groupDiacrytics
            // 
            this.groupDiacrytics.Items.Add(this.buttonConvertToThamesM);
            this.groupDiacrytics.Label = "Диакритические знаки";
            this.groupDiacrytics.Name = "groupDiacrytics";
            // 
            // buttonConvertToThamesM
            // 
            this.buttonConvertToThamesM.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.buttonConvertToThamesM.Label = "Привести к ThamesM";
            this.buttonConvertToThamesM.Name = "buttonConvertToThamesM";
            this.buttonConvertToThamesM.OfficeImageId = "ReplaceDialog";
            this.buttonConvertToThamesM.ScreenTip = "Привести к шрифту ThamesM";
            this.buttonConvertToThamesM.ShowImage = true;
            this.buttonConvertToThamesM.SuperTip = "Преобразование выделенного текста так, чтобы все символы с диакритическими знакам" +
    "и корректно отображались в шрифте ThamesM";
            this.buttonConvertToThamesM.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonConvertToThamesM_Click);
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tabVedicEditor);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon_Load);
            this.tabVedicEditor.ResumeLayout(false);
            this.tabVedicEditor.PerformLayout();
            this.groupDiacrytics.ResumeLayout(false);
            this.groupDiacrytics.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabVedicEditor;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupDiacrytics;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonConvertToThamesM;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
