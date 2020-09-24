using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace BGCompanion
{
    public static class Deck
    {
        public static List<Card> Cards = new List<Card>();

        public static void ImportDeck(string filePath)
        {
            if (File.Exists(filePath))
            {
                Cards = JsonConvert.DeserializeObject<List<Card>>(File.ReadAllText(filePath));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public static void AddCard(Card card)
        {
            if (Cards.Contains(card))
            {
                throw new NotImplementedException();
            }
            else
            {
                Cards.Add(card);
            }
        }
        public static void RemoveCard(Card card)
        {
            if (Cards.Contains(card))
            {
                Cards.Remove(card);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public static void ExportDeck(string filePath)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryName))
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(Cards));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

    }
}
