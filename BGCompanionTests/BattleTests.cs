using Microsoft.VisualStudio.TestTools.UnitTesting;
using BGCompanion;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BGCompanion.Tests
{
    [TestClass()]
    public class BattleTests
    {
        [TestMethod()]
        public void SimulateTest()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            Deck.ImportDeck(@"tier1.json");
            Card alleyCat = Deck.Cards.Find(m => m.Name == "Alleycat");
            for (int h = 1; h <= Hand.maxHandSize; h++)
            {
                mine.slots = new List<Card>();
                enemy.slots = new List<Card>();
                for (int i = 0; i < h; i++)
                {
                    alleyCat.guid = new Guid();
                    mine.slots.Add(alleyCat);
                }
                for (int i = 0; i < h; i++)
                {
                    alleyCat.guid = new Guid();
                    enemy.slots.Add(alleyCat);
                }
                Battle.Simulate(mine, enemy);
                Console.WriteLine("Hand Size: {3} Win:{0} Tie:{1} Lose:{2}", Battle.winCount, Battle.tieCount, Battle.loseCount, h);
            }
            for (int h = 1; h <= Hand.maxHandSize; h++)
            {
                mine.slots = new List<Card>();
                enemy.slots = new List<Card>();
                for (int i = 0; i < h; i++)
                {
                    alleyCat.guid = Guid.NewGuid();
                    mine.slots.Add(alleyCat);
                }
                for (int i = 0; i < h - 1; i++)
                {
                    alleyCat.guid = Guid.NewGuid();
                    enemy.slots.Add(alleyCat);
                }
                Battle.Simulate(mine, enemy);
                Console.WriteLine("Hand Size: {3} Win:{0} Tie:{1} Lose:{2}", Battle.winCount, Battle.tieCount, Battle.loseCount, h);
            }
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            for (int i = 0; i < 3; i++)
            {
                alleyCat.guid = new Guid();
                mine.slots.Add(alleyCat);

            }
            Card rph = Deck.Cards.Find(m => m.Name == "Rockpool Hunter");
            rph.guid = new Guid();
            enemy.slots.Add(rph);
            Battle.Simulate(mine, enemy);
            Console.WriteLine("Hand: 3 Alleycats vs Rockpool Hunter Win:{0} Tie:{1} Lose:{2}", Battle.winCount, Battle.tieCount, Battle.loseCount);

            Assert.IsTrue(Battle.winPerc == 0 && Battle.losePerc == 0 && Battle.tiePerc == 1);
            //Assert.Fail();

        }
        [TestMethod]
        public void SimulateTestNo2()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"tier1.json");
            //Card alleyCat = Deck.Cards.Find(m => m.Name == "Alleycat");
            Card rph = new Card(Deck.Cards.Find(m => m.Name == "Rockpool Hunter"));
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            for (int i = 0; i < 3; i++)
            {
                Card alleyCat = new Card(Deck.Cards.Find(m => m.Name == "Alleycat"));
                //alleyCat.guid = Guid.NewGuid();
                mine.slots.Add(alleyCat);

            }
            rph.guid = Guid.NewGuid();
            enemy.slots.Add(rph);
            Battle.Simulate(mine, enemy);
            Console.WriteLine("Hand: 3 Alleycats vs Rockpool Hunter Win:{0} Tie:{1} Lose:{2}", Battle.winCount, Battle.tieCount, Battle.loseCount);

            Assert.IsTrue(Battle.winPerc == 0 && Battle.losePerc == 0 && Battle.tiePerc == 1);
        }
        [TestMethod]
        public void SimulateTestDivineShieldTaunt()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"tier1.json");
            //Card alleyCat = Deck.Cards.Find(m => m.Name == "Alleycat");
            Card rph = new Card(Deck.Cards.Find(m => m.Name == "Rockpool Hunter"));
            Card rp = new Card(Deck.Cards.Find(m => m.Name == "Righteous Protector"));
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            Card alleyCat = new Card(Deck.Cards.Find(m => m.Name == "Alleycat"));
            mine.slots.Add(alleyCat);
            mine.slots.Add(rp);
            rph.guid = Guid.NewGuid();
            enemy.slots.Add(rph);
            Battle.Simulate(mine, enemy);
            Console.WriteLine("Hand: 3 Alleycats vs Rockpool Hunter Win:{0} Tie:{1} Lose:{2}", Battle.winCount, Battle.tieCount, Battle.loseCount);

           
            Assert.IsTrue(Battle.winPerc == 0 && Battle.losePerc == 0 && Battle.tiePerc == 1);
        }
        [TestMethod]
        public void SimulateTestTauntOutOfPosition()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"tier1.json");
            //Card alleyCat = Deck.Cards.Find(m => m.Name == "Alleycat");
            Card rph = new Card(Deck.Cards.Find(m => m.Name == "Rockpool Hunter"));
            Card rp = new Card(Deck.Cards.Find(m => m.Name == "Righteous Protector"));
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            Card alleyCat = new Card(Deck.Cards.Find(m => m.Name == "Alleycat"));
            mine.slots.Add(alleyCat);
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            mine.slots.Add(rp);
            rph.guid = Guid.NewGuid();
            enemy.slots.Add(rph);
            Battle.Simulate(mine, enemy);
            Console.WriteLine("Hand: 3 Alleycats vs Rockpool Hunter Win:{0} Tie:{1} Lose:{2}", Battle.winCount, Battle.tieCount, Battle.loseCount);

            
            Assert.IsTrue(Battle.winPerc == 1 && Battle.losePerc == 0 && Battle.tiePerc == 0);
        }
        [TestMethod]
        public void SimulateTestSingleRedWhelp()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"tier1.json");
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            Battle.Simulate(mine, enemy);
            Console.WriteLine("Hand: 1 Red Whelp vs 2 Alleycat's Win:{0} Tie:{1} Lose:{2}", Battle.winCount, Battle.tieCount, Battle.loseCount);
            
            Assert.IsTrue(Battle.winPerc == 1 && Battle.losePerc == 0 && Battle.tiePerc == 0);
        }
        [TestMethod]
        public void SimulateTestTwoRedWhelps()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"tier1.json");
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            Battle.Simulate(mine, enemy);
            Console.WriteLine("Hand: 1 Red Whelp vs 2 Alleycat's Win:{0} Tie:{1} Lose:{2}", Battle.winCount, Battle.tieCount, Battle.loseCount);
            
            Assert.IsTrue(Battle.winPerc == 1 && Battle.losePerc == 0 && Battle.tiePerc == 0);
        }
        [TestMethod]
        public void SimulateTestTwoRedWhelpsVTwoRedWhelps()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"tier1.json");
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            Battle.Simulate(mine, enemy);
            Console.WriteLine("Hand: 1 Red Whelp vs 2 Alleycat's Win:{0} Tie:{1} Lose:{2}", Battle.winCount, Battle.tieCount, Battle.loseCount);
            
            Assert.IsTrue(Battle.winPerc == .5 && Battle.losePerc == .5 && Battle.tiePerc == 0);
        }
        [TestMethod]
        public void SimulateTest2RWv2RWand1AC()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"tier1.json");
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));

            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            Battle.Simulate(mine, enemy);
            //Console.WriteLine("Results Win:{0} Tie:{1} Lose:{2}", Battle.winCount, Battle.tieCount, Battle.loseCount);
            Assert.IsTrue(Battle.winPerc == .50 && Battle.tiePerc == 0 && Battle.losePerc == .50);
        }
        [TestMethod]
        public void SimulateTest1MicroMv1DeckS()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"tier1.json");
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Micro Mummy")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Deck Swabbie")));
            Battle.Simulate(mine, enemy);
            Assert.IsTrue(Battle.winPerc == 0 && Battle.tiePerc == 1 && Battle.losePerc == 0);
        }
        [TestMethod]
        public void SimulateTestAsmo()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"testDeck.json");
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Rabid Saurolisk")));
            enemy.slots[0].Attack = 5;
            enemy.slots[0].Health = 3;
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Rabid Saurolisk")));
            enemy.slots[1].Attack = 5;
            enemy.slots[1].Health = 3;
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Refreshing Anomaly")));
            enemy.slots[2].Attack = 3;
            enemy.slots[2].Health = 5;
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Refreshing Anomaly")));
            enemy.slots[3].Attack = 2;
            enemy.slots[3].Health = 4;
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Menagerie Mug")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Glyph Guardian")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Water Droplet")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Pack Leader")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            Battle.Simulate(mine, enemy);
            Assert.IsTrue(Battle.winPerc == .189 && Battle.tiePerc == .218 && Battle.losePerc == 0.593);
        }
        [TestMethod]
        public void SimulateHyena()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"testDeck.json");
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();

            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Murloc Scout")));
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Tabbycat")));
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Tabbycat")));
            enemy.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Scavenging Hyena")));

            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Water Droplet")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Water Droplet")));
            mine.slots.Add(new Card(Deck.Cards.Find(m => m.Name == "Pack Leader")));
            Battle.Simulate(mine, enemy);
            Assert.IsTrue(Battle.winPerc.ToString("0.0000") == "0.1739" && Battle.tiePerc.ToString("0.0000") == "0.1304" && Battle.losePerc.ToString("0.0000") == "0.6957");
        }
    }
}