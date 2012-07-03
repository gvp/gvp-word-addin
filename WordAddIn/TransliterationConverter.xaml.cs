using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace VedicEditor
{
    /// <summary>
    /// Логика взаимодействия для TransliterationConverter.xaml
    /// </summary>
    public partial class TransliterationConverter : UserControl
    {
        public TransliterationConverter()
        {
            InitializeComponent();
            initialFontCombo.ItemsSource = FontTransformer.LatinFonts.Union(FontTransformer.CyrillicFonts);
            endFontCombo.ItemsSource = FontTransformer.CyrillicFonts;
        }

        private void convertButton_Click(object sender, RoutedEventArgs e)
        {
            var transformer = new FontTransformer(endFontCombo.Text);
            transformer.Transform();
        }
    }
}
