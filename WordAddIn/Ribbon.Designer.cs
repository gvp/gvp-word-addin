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
            this.group1 = this.Factory.CreateRibbonGroup();
            this.dropDownFont = this.Factory.CreateRibbonDropDown();
            this.buttonLaunch = this.Factory.CreateRibbonButton();
            this.tabVedicEditor.SuspendLayout();
            this.group1.SuspendLayout();
            // 
            // tabVedicEditor
            // 
            this.tabVedicEditor.Groups.Add(this.group1);
            this.tabVedicEditor.Label = "Ведический редактор";
            this.tabVedicEditor.Name = "tabVedicEditor";
            // 
            // group1
            // 
            this.group1.Items.Add(this.dropDownFont);
            this.group1.Items.Add(this.buttonLaunch);
            this.group1.Label = "Преобразование шрифтов";
            this.group1.Name = "group1";
            // 
            // dropDownFont
            // 
            this.dropDownFont.Label = "Желаемый шрифт";
            this.dropDownFont.Name = "dropDownFont";
            this.dropDownFont.ShowImage = true;
            this.dropDownFont.ShowItemImage = false;
            this.dropDownFont.ShowLabel = false;
            this.dropDownFont.SizeString = "Желаемый шрифт";
            // 
            // buttonLaunch
            // 
            this.buttonLaunch.Label = "Преобразовать";
            this.buttonLaunch.Name = "buttonLaunch";
            this.buttonLaunch.OfficeImageId = "ReplaceDialog";
            this.buttonLaunch.ShowImage = true;
            this.buttonLaunch.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonLaunch_Click);
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tabVedicEditor);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon_Load);
            this.tabVedicEditor.ResumeLayout(false);
            this.tabVedicEditor.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabVedicEditor;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDownFont;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonLaunch;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
