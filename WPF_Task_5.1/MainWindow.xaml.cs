using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Task_5._1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fontWeight = "Light "; // Установка значения по умолчанию для отображения в StatusBar
        string fontStyle = "Normal "; // Установка значения по умолчанию для отображения в StatusBar
        string fontDecotations = ""; // Установка значения по умолчанию для отображения в StatusBar

        public MainWindow()
        {
            InitializeComponent();
            statusBarItemName1.Text = "Arial"; // Установка значения по умолчанию для отображения в StatusBar
            statusBarItemName2.Text = "16"; // Установка значения по умолчанию для отображения в StatusBar
            statusBarItemName3.Text = fontWeight + fontStyle + fontDecotations; // Установка значения по умолчанию для отображения в StatusBar
            statusBarItemName4.Text = "0"; // Установка значения по умолчанию для отображения в StatusBar
            statusBarItemName5.Text = "1"; // Установка значения по умолчанию для отображения в StatusBar
            fontColor.Content = "Черный шрифт"; // Установка значения цвета шрифта для светлого стиля оформления (отображается у RadioButton и динамически меняется при смене стиля оформления) 

            Uri uri = new Uri("LightStyle.xaml", UriKind.Relative); // Установка стиля оформления по умолчанию (светлая)
            ResourceDictionary resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // Выбор стиля шрифта
        {
            string fontName = (sender as ComboBox).SelectedItem.ToString();
            if (textbox != null)
                textbox.FontFamily = new FontFamily(fontName);
            statusBarItemName1.Text = fontName;
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e) // Выбор размера шрифта
        {
            int fontSize = (int)(sender as ComboBox).SelectedItem;
            if (textbox != null)
                textbox.FontSize = fontSize;
            statusBarItemName2.Text = fontSize.ToString();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e) // Выбор цвета текста - черный (для светлой темы оформления) или белый (для темной темы оформления)
        {
            if (fontColor.Content == "Черный шрифт")
            {
                if (textbox != null)
                    textbox.Foreground = Brushes.Black;
            }
            else
            {
                if (textbox != null)
                {
                    textbox.Foreground = Brushes.White;
                    fontColor.Content = "Белый шрифт";
                }
            }
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e) // Выбор цвета текста - красный
        {
            if (textbox != null)
                textbox.Foreground = Brushes.Red;
        }

        private void FontPropertiesExecuted(object sender, ExecutedRoutedEventArgs e) // Выбор пункта меню "Формат -> Шрифт"
        {
            WindowFonts windowFonts = new WindowFonts();
            windowFonts.ShowDialog();
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e) // Выбор пункта меню "Файл -> Открыть"
        {
            if (textbox.Text.Length != 0)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить текущие изменения?", "Открытие нового файла", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveToFile();
                    OpenFile();
                }
                else if (result == MessageBoxResult.No)
                {
                    OpenFile();
                }
            }
            else
            {
                OpenFile();
            }
        }

        private void OpenFile() // Метод для открытия файла
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                textbox.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e) // Выбор пункта меню "Файл -> Сохранить"
        {
            SaveToFile();
        }

        private void SaveToFile() // Метод для сохранения в файл
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, textbox.Text);
        }

        private void ExitExecuted(object sender, ExecutedRoutedEventArgs e) // Выбор пункта меню "Файл -> Закрыть"
        {
            Application.Current.MainWindow.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) //Закрытие MainWindow
        {
            if (textbox.Text.Length != 0)
            {
                MessageBoxResult result = MessageBox.Show("Сохранить изменения?", "Выход", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveToFile();
                    e.Cancel = false;
                }
                else if (result == MessageBoxResult.No)
                {
                    e.Cancel = false;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void SetBoldExecuted(object sender, ExecutedRoutedEventArgs e) // Выбор пункта меню "Формат -> Начертание -> Полужирный" или нажатие кнопки Bold
        {
            if (textbox.FontWeight == FontWeights.Bold)
            {
                textbox.FontWeight = FontWeights.Light;
                fontWeight = "Light ";
            }
            else
            {
                textbox.FontWeight = FontWeights.Bold;
                fontWeight = "Bold ";
            }
            statusBarItemName3.Text = fontWeight + fontStyle + fontDecotations; // отслеживание данных для StatusBar

        }

        private void SetItalicExecuted(object sender, ExecutedRoutedEventArgs e) // Выбор пункта меню "Формат -> Начертание -> Курсив" или нажатие кнопки Italic
        {
            if (textbox.FontStyle == FontStyles.Italic)
            {
                textbox.FontStyle = FontStyles.Normal;
                fontStyle = "Normal ";
            }
            else
            {
                textbox.FontStyle = FontStyles.Italic;
                fontStyle = "Italic ";
            }
            statusBarItemName3.Text = fontWeight + fontStyle + fontDecotations; // отслеживание данных для StatusBar
        }

        private void SetUnderlineExecuted(object sender, ExecutedRoutedEventArgs e) // Выбор пункта меню "Формат -> Начертание -> Подчеркнутый" или нажатие кнопки Underline
        {
            if (textbox.TextDecorations == TextDecorations.Underline)
            {
                textbox.TextDecorations = null;
                fontDecotations = "";
            }
            else
            {
                textbox.TextDecorations = TextDecorations.Underline;
                fontDecotations = "Underlined";
            }
            statusBarItemName3.Text = fontWeight + fontStyle + fontDecotations; // отслеживание данных для StatusBar
        }

        private void AboutExecuted(object sender, ExecutedRoutedEventArgs e) // Выбор пункта меню "О программе"
        {
            MessageBox.Show("Текстовый редактор. Версия 1.03");
        }

        private void textbox_TextChanged(object sender, TextChangedEventArgs e) // отслеживание данных для StatusBar
        {
            statusBarItemName4.Text = textbox.Text.Length.ToString();
            statusBarItemName5.Text = textbox.LineCount.ToString();
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e) // Скрытие кнопок пререполнения в ToolBox
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            //var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            //if (mainPanelBorder != null)
            //{
            //    mainPanelBorder.Margin = new Thickness();
            //}
        }

        private void SetBlackThemeExecuted(object sender, ExecutedRoutedEventArgs e) // Установка темного стиля оформления
        {
            Uri uri = new Uri("DarkStyle.xaml", UriKind.Relative);
            ResourceDictionary resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            if (textbox.Foreground != Brushes.Red)
                textbox.Foreground = Brushes.White;
            fontColor.Content = "Белый шрифт";
            ButtonDarkStyle.IsChecked = true; // изменение нажатой RadioButton, если выбор стиля оформления сделан из меню
        }

        private void SetLightThemeExecuted(object sender, ExecutedRoutedEventArgs e) // Установка светлого стиля оформления
        {
            Uri uri = new Uri("LightStyle.xaml", UriKind.Relative);
            ResourceDictionary resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            if (textbox.Foreground != Brushes.Red)
                textbox.Foreground = Brushes.Black;
            fontColor.Content = "Черный шрифт";
            ButtonLightStyle.IsChecked = true; // изменение нажатой RadioButton, если выбор стиля оформления сделан из меню
        }
    }
}
