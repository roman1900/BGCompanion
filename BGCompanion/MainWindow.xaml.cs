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

namespace BGCompanion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Hand hand = new Hand();
            Card card = new Card("Selfless Hero", 2, 1, 1, Tribe.Neutral);
            Effect buff = new Effect();
            buff.What = Attribute.deathRattle;
            buff.Give = Attribute.divineShield;
            buff.Who = Tribe.self;
            buff.Target = Tribe.friendly | Tribe.random;
            card.AddBuff(buff);
            string a = JsonConvert.SerializeObject(card);
            var deserializedObject = JsonConvert.DeserializeObject<Card>(a);
            What.ItemsSource = Enum.GetValues(typeof(Attribute));
            Who.ItemsSource = Enum.GetValues(typeof(Tribe));
            Deck d = new Deck();
            d.AddCard(card);
            d.ExportDeck(@"c:\temp\deck.json");
        }
    }
}
