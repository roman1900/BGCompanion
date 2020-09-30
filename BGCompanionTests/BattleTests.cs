﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            Assert.IsTrue(Battle.winCount == 0 && Battle.loseCount == 0);
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

            Assert.IsTrue(Battle.loseCount == 0 && Battle.tieCount == 2 && Battle.winCount == 0);
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

            Assert.IsTrue(Battle.loseCount == 0 && Battle.tieCount == 1 && Battle.winCount == 0);
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

            Assert.IsTrue(Battle.loseCount == 0 && Battle.tieCount == 0 && Battle.winCount == 1);
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
            Assert.IsTrue(Battle.loseCount == 0 && Battle.tieCount == 0 && Battle.winCount == 2);
        }

    }
}