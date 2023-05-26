using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Pairs
{
    public partial class PlayScreen : Window
    {
        private string saveFile;
        private User user;
        private int level = 0;
        private int width;
        private int height;
        private List<Card> cards = new List<Card>();
        private List<BitmapImage> images = new List<BitmapImage>();
        private BitmapImage basicImage = new BitmapImage();
        private int singlePhotoIndex = -1;
        private int lastPhotoClickedIndex = -1;
        private int matchedPhotoIndex = -2;
        private int cardsMached;

        public PlayScreen(User currentUser)
        {
            InitializeComponent();

            user = currentUser;
            saveFile = @"..\..\Resurse\UsersSavedGamesData\" + user.username + ".xml";
            userAvatar.Source = new BitmapImage(new Uri(@"Resurse\Avatars\" + currentUser.avatarName, UriKind.Relative));
            usernameLabel.Content = user.username;
            saveButton.IsEnabled = false;

            Uri imgUri1 = new Uri(@"..\..\Resurse\Images\basicImg.png", UriKind.Relative);
            basicImage = new BitmapImage(imgUri1);

            string[] imagePaths = Directory.GetFiles(@"..\..\Resurse\Images", "*.jpg");
            foreach (string imagePath in imagePaths)
            {
                Uri imgUri = new Uri(imagePath, UriKind.Relative);
                BitmapImage image = new BitmapImage(imgUri);
                images.Add(image);
            }
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameType gameType = new GameType();
            this.Hide();
            gameType.ShowDialog();
            this.Show();
            
            newGameButton.IsEnabled = false;
            loadButton.IsEnabled = false;
            saveButton.IsEnabled = true;

            width = gameType.width;
            height = gameType.height;
            CreateGrid();
            user.gamesPlayed++;
            startGame();

        }

        private void startGame()
        {
            level++;
            gameInfoLabel.Content = "Level: " + level;
            cardsMached = 0;
            cards.Clear();
            AssignImages();
        }

        private void CreateGrid()
        {
            for (int rowNumber = 0; rowNumber < height; rowNumber++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                imagesGrid.RowDefinitions.Add(row);
            }

            for (int columnNumber = 0; columnNumber < width; columnNumber++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(1, GridUnitType.Star);
                imagesGrid.ColumnDefinitions.Add(column);
            }

        }

        private void AssignImages()
        {
            Random random = new Random();
            Dictionary<int, int> frequency = new Dictionary<int, int>();
            int maxIndex;
            if (width * height % 2 == 1)
            {
                maxIndex = width * height / 2 + 1;
            }
            else
            {
                maxIndex = width * height / 2;
            }

            int indexRow = 0, indexColumn = 0;

            while (cards.Count < (width * height))
            {
                Card card = new Card();
                card.button.Content = new Image
                {
                    Source = basicImage,
                    Stretch = Stretch.Uniform
                };

                card.button.Name = 'B' + indexRow.ToString() + indexColumn.ToString();
                card.button.AddHandler(Button.ClickEvent, new RoutedEventHandler(card_Click));

                int randomNr = random.Next(0, maxIndex);
                while (frequency.ContainsKey(randomNr) && frequency[randomNr] == 2)
                {
                    randomNr = random.Next(0, maxIndex);
                }
                card.indexOfPhoto = randomNr;
                if (!frequency.ContainsKey(randomNr))
                {
                    frequency.Add(randomNr, 1);
                }
                else
                {
                    frequency[randomNr]++;
                }

                imagesGrid.Children.Add(card.button);
                Grid.SetRow(card.button, indexRow);
                Grid.SetColumn(card.button, indexColumn);
                if (indexColumn == width - 1)
                {
                    indexColumn = 0;
                    indexRow++;
                }
                else
                {
                    indexColumn++;
                }

                cards.Add(card);
            }

            if (width * height % 2 == 1)
            {
                foreach (var item in frequency)
                {
                    if (item.Value == 1)
                    {
                        singlePhotoIndex = item.Key;
                        break;
                    }
                }
            }
        }

        private void destroyOrFlipDownCards()
        {

            if (cards[matchedPhotoIndex].indexOfPhoto == cards[lastPhotoClickedIndex].indexOfPhoto)
            {
                cards[matchedPhotoIndex].button.Visibility = Visibility.Collapsed;
                cards[lastPhotoClickedIndex].button.Visibility = Visibility.Collapsed;
                cardsMached += 2;
            }
            else
            {
                cards[matchedPhotoIndex].button.Content = new Image
                {
                    Source = basicImage,
                    Stretch = Stretch.Uniform
                };
                cards[lastPhotoClickedIndex].button.Content = new Image
                {
                    Source = basicImage,
                    Stretch = Stretch.Uniform
                };

                cards[matchedPhotoIndex].button.IsEnabled = true;
                cards[lastPhotoClickedIndex].button.IsEnabled = true;
            }
            matchedPhotoIndex = -2;
            lastPhotoClickedIndex = -1;
        }

        public void card_Click(object sender, RoutedEventArgs e)
        {
            if (matchedPhotoIndex != -2)
            {
                destroyOrFlipDownCards();
            }

            int indexCard = ((sender as Button).Name[1] - '0') * width + (sender as Button).Name[2] - '0';
            cards[indexCard].button.Content = new Image
            {
                Source = images[cards[indexCard].indexOfPhoto],
                Stretch = Stretch.Uniform
            };
            cards[indexCard].button.IsEnabled = false;

            if (singlePhotoIndex != -1 && cards[indexCard].indexOfPhoto == singlePhotoIndex)
            {
                cards[indexCard].button.Visibility = Visibility.Collapsed;
                singlePhotoIndex = -1;
                cardsMached++;

                if (width * height - cardsMached < 2)
                {
                    startGame();
                }
            }
            else if (lastPhotoClickedIndex == -1)
            {
                lastPhotoClickedIndex = indexCard;
            }
            else
            {
                matchedPhotoIndex = indexCard;
                if (width * height - cardsMached <= 2)
                {
                    destroyOrFlipDownCards();
                    if (level != 3)
                    {
                        startGame();
                    }
                    else
                    {
                        level = 0;
                        gameInfoLabel.Content = "You win!!";
                        newGameButton.IsEnabled = true;
                        if (File.Exists(saveFile))
                        {
                            File.WriteAllText(saveFile, string.Empty);
                        }
                        user.wins++;
                    }
                }
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Owner.Show();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer ser = new XmlSerializer(typeof(XmlData));
            TextWriter writer;

            if (File.Exists(@"..\..\Resurse\UsersSavedGamesData\" + user.username))
            {
                writer = new StreamWriter(File.OpenWrite(saveFile));
            }
            else
            {
                writer = new StreamWriter(File.Create(saveFile));
            }

            XmlData data = new XmlData(level, cards, cardsMached, width, height, singlePhotoIndex);

            ser.Serialize(writer, data);
            writer.Dispose();
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(saveFile))
            {
                MessageBox.Show("You didn't save any game!");
                return;
            }
            else if (new FileInfo(saveFile).Length == 0)
            {
                MessageBox.Show("You saved a game which you already win it so you do not have saved games!");
                return;
            }

            newGameButton.IsEnabled = false;
            loadButton.IsEnabled = false;
            saveButton.IsEnabled = true;

            XmlSerializer ser = new XmlSerializer(typeof(XmlData));
            XmlData savedGame;
            using (Stream reader = new FileStream(saveFile, FileMode.Open))
            {
                savedGame = (XmlData)ser.Deserialize(reader);
            }

            height = savedGame.height;
            width = savedGame.width;
            level = savedGame.level;
            singlePhotoIndex = savedGame.singlePhotoIndex;
            cardsMached = savedGame.cardsMached;
            gameInfoLabel.Content = "Level: " + level;
            CreateGrid();

            int indexRow = 0, indexColumn = 0;
            foreach (var cardData in savedGame.cards)
            {
                Card card = new Card();
                card.button.Content = new Image
                {
                    Source = basicImage,
                    Stretch = Stretch.Uniform
                };

                card.button.Name = 'B' + indexRow.ToString() + indexColumn.ToString();
                card.button.AddHandler(Button.ClickEvent, new RoutedEventHandler(card_Click));

                imagesGrid.Children.Add(card.button);
                Grid.SetRow(card.button, indexRow);
                Grid.SetColumn(card.button, indexColumn);

                if (indexColumn == width - 1)
                {
                    indexColumn = 0;
                    indexRow++;
                }
                else
                {
                    indexColumn++;
                }

                if (!cardData.buttonIsVisible)
                {
                    card.button.Visibility = Visibility.Collapsed;
                }
                card.indexOfPhoto = cardData.indexOfPhoto;

                cards.Add(card);
            }

        }
    }
}
