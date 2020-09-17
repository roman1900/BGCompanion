using System;
using System.Collections.Generic;
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
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace BGCompanion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Deck d = new Deck();
        public MainWindow()
        {
            InitializeComponent();
            //Hand hand = new Hand();
            //Card card = new Card("Selfless Hero", 2, 1, 1, 1, Race.Neutral);
            //Effect buff = new Effect();
            //buff.What = Attribute.deathRattle;
            //buff.Give = Attribute.divineShield;
            //buff.Who = Tribe.self;
            //buff.Target = Tribe.friendly | Tribe.random;
            //card.AddBuff(buff);
            //string a = JsonConvert.SerializeObject(card);
            //var deserializedObject = JsonConvert.DeserializeObject<Card>(a);
            What.ItemsSource = Enum.GetValues(typeof(Attribute));
            Who.ItemsSource = Enum.GetValues(typeof(Tribe));
            cardTribe.ItemsSource = Enum.GetValues(typeof(Race));
            Target.ItemsSource = Enum.GetValues(typeof(Tribe));
            Give.ItemsSource = Enum.GetValues(typeof(Attribute));


            //d.AddCard(card);
            //d.ExportDeck(@"c:\temp\deck.json");
        }

        private void AddCard_Click(object sender, RoutedEventArgs e)
        {
           
            //TODO: Check for int parsing errors
            Card card = new Card(cardName.Text, Int32.Parse(Attack.Text), Int32.Parse(Health.Text), Int32.Parse(Mana.Text), Int32.Parse(Tier.Text), (Race)cardTribe.SelectedItem);
            if (AddEffect.IsChecked == true)
            {
                Effect effect = new Effect();
                 
                effect.Give = Give.SelectedIndex != -1 ? (Attribute)Give.SelectedItem : effect.Give;
                effect.Attack = effectAttack.Text.Length > 0 ? Int32.Parse(effectAttack.Text) : 0;
                effect.Health = effectHealth.Text.Length > 0 ? Int32.Parse(effectHealth.Text) : 0;
                int val;
                effect.Damage = Int32.TryParse(effectDamage.Text, out val) ? val : 0;
                foreach (Tribe who in Who.SelectedItems) {
                    effect.Who = effect.Who | who;
                }
                foreach (Tribe target in Target.SelectedItems)
                {
                    effect.Target = effect.Target | target;
                }
                foreach (Attribute what in What.SelectedItems)
                {
                    effect.What = effect.What | what;
                }
                card.buffs.Add(effect);
            }


            //string a = JsonConvert.SerializeObject(card);
            d.AddCard(card);

        }

        private void MenuImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuExport_Click(object sender, RoutedEventArgs e)
        {
            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            if(saveFileDialog.ShowDialog() == true)
            {
                d.ExportDeck(saveFileDialog.FileName);
            }
        }
    }
}
