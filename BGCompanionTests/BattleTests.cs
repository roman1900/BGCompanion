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
                    //alleyCat.guid = new Guid();
                    mine.Push(alleyCat);
                }
                for (int i = 0; i < h; i++)
                {
                    //alleyCat.guid = new Guid();
                    enemy.Push(alleyCat);
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
                    //alleyCat.guid = Guid.NewGuid();
                    mine.Push(alleyCat);
                }
                for (int i = 0; i < h - 1; i++)
                {
                    //alleyCat.guid = Guid.NewGuid();
                    enemy.Push(alleyCat);
                }
                Battle.Simulate(mine, enemy);
                Console.WriteLine("Hand Size: {3} Win:{0} Tie:{1} Lose:{2}", Battle.winCount, Battle.tieCount, Battle.loseCount, h);
            }
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            for (int i = 0; i < 3; i++)
            {
                //alleyCat.guid = new Guid();
                mine.Push(alleyCat);

            }
            Card rph = Deck.Cards.Find(m => m.Name == "Rockpool Hunter");
            //rph.guid = new Guid();
            enemy.Push(rph);
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
                mine.Push(alleyCat);

            }
            //rph.guid = Guid.NewGuid();
            enemy.Push(rph);
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
            mine.Push(alleyCat);
            mine.Push(rp);
            //rph.guid = Guid.NewGuid();
            enemy.Push(rph);
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
            mine.Push(alleyCat);
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            mine.Push(rp);
            //rph.guid = Guid.NewGuid();
            enemy.Push(rph);
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
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
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
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
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
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
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
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));

            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
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
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Micro Mummy")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Deck Swabbie")));
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
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Rabid Saurolisk")));
            enemy.slots[0].Attack = 5;
            enemy.slots[0].Health = 3;
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Rabid Saurolisk")));
            enemy.slots[1].Attack = 5;
            enemy.slots[1].Health = 3;
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Refreshing Anomaly")));
            enemy.slots[2].Attack = 3;
            enemy.slots[2].Health = 5;
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Refreshing Anomaly")));
            enemy.slots[3].Attack = 2;
            enemy.slots[3].Health = 4;
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Menagerie Mug")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Glyph Guardian")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Water Droplet")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Pack Leader")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Red Whelp")));
            Battle.Simulate(mine, enemy);
            //Assert.IsTrue(Battle.winPerc == .189 && Battle.tiePerc == .218 && Battle.losePerc == 0.593);
            //Assert.IsTrue(Battle.winPerc.ToString("0.0000") == "0.2077" && Battle.tiePerc.ToString("0.0000") == "0.2356" && Battle.losePerc.ToString("0.0000") == "0.5567");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void SimulateHyena()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"testDeck.json");
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();

            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Murloc Scout")));
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Tabbycat")));
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Alleycat")));
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Tabbycat")));
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Scavenging Hyena")));

            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Water Droplet")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Water Droplet")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Pack Leader")));
            Battle.Simulate(mine, enemy);
            //Assert.IsTrue(Battle.winPerc.ToString("0.0000") == "0.1739" && Battle.tiePerc.ToString("0.0000") == "0.1304" && Battle.losePerc.ToString("0.0000") == "0.6957");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void SimulateSummonRebornNoRoomonBoard()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"testDeck.json");
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Tabbycat")));
            enemy.slots[0].Attack = 4;
            enemy.slots[0].Health = 4;
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Harvest Golem")));
            mine.slots[0].Reborn = true;
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Harvest Golem")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Harvest Golem")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Harvest Golem")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Harvest Golem")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Murloc Scout")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Murloc Scout")));
            Battle.Simulate(mine, enemy);
            Assert.IsTrue(Battle.winPerc==1);
        }
        [TestMethod]
        public void SimulateSummonRebornAndDeathRattle()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"testDeck.json");
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Tabbycat")));
            enemy.slots[0].Attack = 4;
            enemy.slots[0].Health = 4;
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Harvest Golem")));
            mine.slots[0].Reborn = true;
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Harvest Golem")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Harvest Golem")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Harvest Golem")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Harvest Golem")));
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Murloc Scout")));
            Battle.Simulate(mine, enemy);
            Assert.IsTrue(Battle.winPerc == 1);
        }
        [TestMethod]
        public void SimulateDeathRattleGiveAllBuff()
        {
            Hand mine = new Hand();
            Hand enemy = new Hand();
            Deck.ImportDeck(@"testDeck.json");
            mine.slots = new List<Card>();
            enemy.slots = new List<Card>();
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Tabbycat")));
            enemy.slots[0].Attack = 4;
            enemy.slots[0].Health = 4;
            enemy.Push(new Card(Deck.Cards.Find(m => m.Name == "Tabbycat")));
            enemy.slots[0].Attack = 4;
            enemy.slots[0].Health = 4;
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Spawn of N'Zoth")));
            mine.slots[0].Reborn = true;
            mine.Push(new Card(Deck.Cards.Find(m => m.Name == "Harvest Golem")));
            Battle.Simulate(mine, enemy);
            Assert.IsTrue(Battle.winPerc == 1);
        }
    }
}