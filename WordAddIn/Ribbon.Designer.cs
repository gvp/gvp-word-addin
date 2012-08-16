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
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl1 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl2 = this.Factory.CreateRibbonDropDownItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ribbon));
            this.tabVedicEditor = this.Factory.CreateRibbonTab();
            this.groupTransliteration = this.Factory.CreateRibbonGroup();
            this.dropDownFont = this.Factory.CreateRibbonDropDown();
            this.checkBoxDevanagari = this.Factory.CreateRibbonCheckBox();
            this.checkBoxRoman = this.Factory.CreateRibbonCheckBox();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.buttonProcess = this.Factory.CreateRibbonButton();
            this.tabVedicEditor.SuspendLayout();
            this.groupTransliteration.SuspendLayout();
            // 
            // tabVedicEditor
            // 
            this.tabVedicEditor.Groups.Add(this.groupTransliteration);
            this.tabVedicEditor.Label = "Ведический редактор";
            this.tabVedicEditor.Name = "tabVedicEditor";
            // 
            // groupTransliteration
            // 
            this.groupTransliteration.Items.Add(this.dropDownFont);
            this.groupTransliteration.Items.Add(this.checkBoxDevanagari);
            this.groupTransliteration.Items.Add(this.checkBoxRoman);
            this.groupTransliteration.Items.Add(this.separator1);
            this.groupTransliteration.Items.Add(this.buttonProcess);
            this.groupTransliteration.Label = "Транслитерация";
            this.groupTransliteration.Name = "groupTransliteration";
            // 
            // dropDownFont
            // 
            ribbonDropDownItemImpl1.Label = "Обычный";
            ribbonDropDownItemImpl2.Label = "ThamesM";
            this.dropDownFont.Items.Add(ribbonDropDownItemImpl1);
            this.dropDownFont.Items.Add(ribbonDropDownItemImpl2);
            this.dropDownFont.Label = "Шрифт";
            this.dropDownFont.Name = "dropDownFont";
            // 
            // checkBoxDevanagari
            // 
            this.checkBoxDevanagari.Label = "Из Деванагари в Латиницу";
            this.checkBoxDevanagari.Name = "checkBoxDevanagari";
            // 
            // checkBoxRoman
            // 
            this.checkBoxRoman.Label = "Из Латиницы в Кириллицу";
            this.checkBoxRoman.Name = "checkBoxRoman";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // buttonProcess
            // 
            this.buttonProcess.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.buttonProcess.Label = "Преобразовать";
            this.buttonProcess.Name = "buttonProcess";
            this.buttonProcess.OfficeImageId = "ReplaceDialog";
            this.buttonProcess.ScreenTip = "Выполнить отмеченные преобразования";
            this.buttonProcess.ShowImage = true;
            this.buttonProcess.SuperTip = resources.GetString("buttonProcess.SuperTip");
            this.buttonProcess.Tag = "";
            this.buttonProcess.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Process);
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tabVedicEditor);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon_Load);
            this.tabVedicEditor.ResumeLayout(false);
            this.tabVedicEditor.PerformLayout();
            this.groupTransliteration.ResumeLayout(false);
            this.groupTransliteration.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabVedicEditor;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupTransliteration;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonProcess;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox checkBoxDevanagari;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox checkBoxRoman;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDownFont;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
