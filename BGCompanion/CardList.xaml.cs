using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BGCompanion
{
    /// <summary>
    /// Interaction logic for CardList.xaml
    /// </summary>
    public partial class CardList : Window
    {
        public CardList()
        {
            InitializeComponent();
            Cards.ItemsSource = Deck.Cards;
        }
        private void MenuImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
            {
                Deck.ImportDeck(openFileDialog.FileName);
            }
            Cards.ItemsSource = Deck.Cards;
        }

        private void MenuExport_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == true)
            {
                Deck.ExportDeck(saveFileDialog.FileName);
            }
        }

        private void MenuNewCard_Click(object sender, RoutedEventArgs e)
        {
            AddCardWindow w = new AddCardWindow();
            w.RaiseCustomEvent += new EventHandler<CustomEventArgs>(addcard_RaiseCustomEvent);
            w.Show();
        }

        void addcard_RaiseCustomEvent(object sender, CustomEventArgs e)
        {
            
            Cards.ItemsSource = null;
            Cards.ItemsSource = Deck.Cards;
            
        }
    }
}
