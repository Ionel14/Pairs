using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pairs
{
    [Serializable]
    public class XmlData
    {
        public class CardData
        {
            public bool buttonIsVisible { get; set; }
            public int indexOfPhoto { get; set; }

            public CardData() { }
            public CardData(bool buttonIsVisible, int indexOfPhoto)
            {
                this.buttonIsVisible = buttonIsVisible;
                this.indexOfPhoto = indexOfPhoto;
            }
        }
        public int level { get; set; }
        public List<CardData> cards { get; set; }
        public int cardsMached { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int singlePhotoIndex { get; set; }

        public XmlData() { }
        public XmlData(int level, List<Card> cards, int cardsMached, int width, int height, int singlePhotoIndex)
        {
            this.level = level;
            this.cardsMached = cardsMached;
            this.width = width;
            this.height = height;

            this.cards = new List<CardData>();
            foreach (Card card in cards)
            {
                this.cards.Add(new CardData(card.button.IsVisible, card.indexOfPhoto));
            }

            this.singlePhotoIndex = singlePhotoIndex;
        }
    }
}
