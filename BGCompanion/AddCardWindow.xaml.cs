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
using System.Text.RegularExpressions;

namespace BGCompanion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AddCardWindow : Window
    {

        public event EventHandler<CustomEventArgs> RaiseCustomEvent;
        public AddCardWindow()
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
            Buff.ItemsSource = Enum.GetValues(typeof(Buffs));
            //TODO(#4): Make this disabled until whenever is selected in Buff
            Who.ItemsSource = Enum.GetValues(typeof(Tribe));
            cardTribe.ItemsSource = Enum.GetValues(typeof(Race));
            Trigger.ItemsSource = Enum.GetValues(typeof(WheneverTrigger));
            Target.ItemsSource = Enum.GetValues(typeof(Tribe));
            Give.ItemsSource = Enum.GetValues(typeof(Attribute));


            //d.AddCard(card);
            //d.ExportDeck(@"c:\temp\deck.json");
        }

        private void AddCard_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Enforce Tribe Selection on Add Card Window
            Card card = new Card(cardName.Text, Int32.Parse(Attack.Text), Int32.Parse(Health.Text), Int32.Parse(Mana.Text), Int32.Parse(Tier.Text),Taunt.IsChecked == true ,DivineShield.IsChecked == true,Reborn.IsChecked == true,Cleave.IsChecked == true,Poisonous.IsChecked == true, (Race)cardTribe.SelectedItem);
            if (AddEffect.IsChecked == true)
            {
                Effect effect = new Effect();
                 
                effect.Give = Give.SelectedIndex != -1 ? (Attribute)Give.SelectedItem : effect.Give;
                effect.Attack = effectAttack.Text.Length > 0 ? Int32.Parse(effectAttack.Text) : 0;
                effect.Health = effectHealth.Text.Length > 0 ? Int32.Parse(effectHealth.Text) : 0;
                effect.DamagePer = DamagePer.IsChecked == true;
                int val;
                effect.Damage = Int32.TryParse(effectDamage.Text, out val) ? val : 0;
                val = 0;
                Card SummonThis = Deck.Cards.Find(x => x.Name == effectSummons.Text);
                List<Card> SummonList = new List<Card>();
                if (SummonThis!=null) // We found a matching card to ad to the summons list
                {
                    if (Int32.TryParse(SummonsCount.Text, out val)) //The count of Summons we are to add
                    {
                        for (int i = 0; i < val; i++)
                        {
                            SummonList.Add(SummonThis);
                        }
                        effect.Summons = SummonList;
                    }
                }
                foreach (Tribe who in Who.SelectedItems) {
                    effect.Who = effect.Who | who;
                }
                foreach (Tribe target in Target.SelectedItems)
                {
                    effect.Target = effect.Target | target;
                }
                foreach (Buffs what in Buff.SelectedItems)
                {
                    effect.What = effect.What | what;
                }
                foreach (WheneverTrigger we in Trigger.SelectedItems)
                {
                    effect.Trigger = effect.Trigger | we;
                }
                card.buffs.Add(effect);
            }


            //string a = JsonConvert.SerializeObject(card);
            Deck.AddCard(card);

            RaiseCustomEvent(this, new CustomEventArgs("Update"));
            this.Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(string s)
        {
            msg = s;
        }
        private string msg;
        public string Message
        {
            get { return msg; }
        }
    }
}
