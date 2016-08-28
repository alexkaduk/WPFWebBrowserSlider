using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WebViewSlider
{
    class Slider
    {
        private Grid wrap;
        string[][] urls;

        private Grid labelWrap = new Grid();
        private Label urlLabel = new Label();

        private Grid buttonsWrap = new Grid();
        Button leftButton = new Button();
        Button upButton = new Button();
        Button rightButton = new Button();
        Button downButton = new Button();

        private Grid slidesWrap = new Grid();
        WebBrowser webView = new WebBrowser();

        int urlLabelHeight = 40;

        private int maxX = 1,
            maxY = 1,
            currentX = 0,
            currentY = 0;

        public Slider(Grid currentWrap, string[][] urls)
        {
            this.wrap = currentWrap;
            this.urls = urls;

            maxX = urls.Length;
            maxY = urls[0].Length;

            CreateSlider();
        }

        private void CreateSlider()
        {
            //main grid which contains 3 rows:
            // - row 0 - lable with url 
            // - row 1 - slides
            // - row 2 - button to slide slides

            int wrapRowsCount = 3;
            RowDefinition[] sliderGridRows = new RowDefinition[wrapRowsCount];

            for (int i = 0; i < wrapRowsCount; i++)
            {
                sliderGridRows[i] = new RowDefinition();
                sliderGridRows[i].Height = (i % 2 == 0) ? new GridLength(1, GridUnitType.Auto) : new GridLength(1, GridUnitType.Star);
                wrap.RowDefinitions.Add(sliderGridRows[i]);
            }

            //----- row 0 - lable with url -----
            labelWrap.Background = new SolidColorBrush(Colors.Beige);
            urlLabel = new Label();
            urlLabel.Content = urls[0][0];
            urlLabel.Height = urlLabelHeight;
            urlLabel.HorizontalAlignment = HorizontalAlignment.Left;
            urlLabel.VerticalAlignment = VerticalAlignment.Top;
            urlLabel.FontSize = 20;
            Grid.SetRow(urlLabel, 0);

            //----- row 1 - slides -----
            slidesWrap.ClipToBounds = true;
            slidesWrap.Background = new SolidColorBrush(Colors.Red);
            Grid.SetRow(slidesWrap, 1);

            SetWidthHeight();

            webView.Navigate(urls[0][0]);
            webView.VerticalAlignment = VerticalAlignment.Top;
            webView.HorizontalAlignment = HorizontalAlignment.Left;

            //----- row 2 - button to slide slides -----
            buttonsWrap.Background = new SolidColorBrush(Colors.Beige);
            Grid.SetRow(buttonsWrap, 2);

            ColumnDefinition[] buttonsWrapColumns = new ColumnDefinition[3];
            for (int i = 0; i < 3; i++)
            {
                buttonsWrapColumns[i] = new ColumnDefinition();
                buttonsWrap.ColumnDefinitions.Add(buttonsWrapColumns[i]);
            }

            Grid buttonsContainer = new Grid();
            Grid.SetColumn(buttonsContainer, 1);

            RowDefinition[] buttonsContainerRows = new RowDefinition[3];
            for (int i = 0; i < 3; i++)
            {
                buttonsContainerRows[i] = new RowDefinition();
                buttonsContainer.RowDefinitions.Add(buttonsContainerRows[i]);
            }

            ColumnDefinition[] buttonsContainerColumns = new ColumnDefinition[3];
            for (int i = 0; i < 3; i++)
            {
                buttonsContainerColumns[i] = new ColumnDefinition();
                buttonsContainer.ColumnDefinitions.Add(buttonsContainerColumns[i]);
            }

            int buttonFontSize = 30;

            leftButton.Content = "\u2190";
            leftButton.FontSize = buttonFontSize;
            leftButton.Click += LeftButton_Click;
            Grid.SetRow(leftButton, 1);
            Grid.SetColumn(leftButton, 0);

            upButton.Content = "\u2191";
            upButton.FontSize = buttonFontSize;
            upButton.Click += UpButton_Click;
            Grid.SetRow(upButton, 0);
            Grid.SetColumn(upButton, 1);

            rightButton.Content = "\u2192";
            rightButton.FontSize = buttonFontSize;
            rightButton.IsEnabled = false;
            rightButton.Click += RightButton_Click1;
            Grid.SetRow(rightButton, 1);
            Grid.SetColumn(rightButton, 2);

            downButton.Content = "\u2193";
            downButton.FontSize = buttonFontSize;
            downButton.IsEnabled = false;
            downButton.Click += DownButton_Click;
            Grid.SetRow(downButton, 2);
            Grid.SetColumn(downButton, 1);

            //----- create slider controls structure -----
            labelWrap.Children.Add(urlLabel);
            wrap.Children.Add(labelWrap);

            slidesWrap.Children.Add(webView);
            wrap.Children.Add(slidesWrap);

            buttonsContainer.Children.Add(leftButton);
            buttonsContainer.Children.Add(upButton);
            buttonsContainer.Children.Add(rightButton);
            buttonsContainer.Children.Add(downButton);
            buttonsWrap.Children.Add(buttonsContainer);
            wrap.Children.Add(buttonsWrap);
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollWebView(0);
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollWebView(1);
        }

        private void RightButton_Click1(object sender, RoutedEventArgs e)
        {
            ScrollWebView(2);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            ScrollWebView(3);
        }
                
        public void SetWidthHeight()
        {
            webView.Width = wrap.ActualWidth;

            slidesWrap.Height = GetSlidesWrapHeight();
            webView.Height = slidesWrap.Height;
        }

        private double GetSlidesWrapHeight()
        {
            return wrap.ActualHeight - labelWrap.ActualHeight - buttonsWrap.ActualHeight;
        }

        public void ScrollWebView(int key)
        {
            int scrollsCountX = maxX - 1,
                scrollsCountY = maxY - 1;


            switch (key)
            {
                //move left
                case 0:
                    currentX--;
                    rightButton.IsEnabled = true;

                    if (currentX <= ((-1) * scrollsCountX))
                    {
                        leftButton.IsEnabled = false;
                        currentX = (-1) * scrollsCountX;

                    }

                    ChangeUrl(currentX, currentY); break;

                //move up
                case 1:
                    currentY--;
                    downButton.IsEnabled = true;

                    if (currentY <= ((-1) * scrollsCountY))
                    {
                        upButton.IsEnabled = false;
                        currentY = (-1) * scrollsCountY;
                    }

                    ChangeUrl(currentX, currentY); break;

                // move right
                case 2:
                    currentX++;
                    leftButton.IsEnabled = true;

                    if (currentX >= 0)
                    {
                        rightButton.IsEnabled = false;
                        currentX = 0;
                    }

                    ChangeUrl(currentX, currentY); break;

                //move down
                case 3:
                    currentY++;
                    upButton.IsEnabled = true;

                    if (currentY >= 0)
                    {
                        downButton.IsEnabled = false;
                        currentY = 0;
                    }

                    ChangeUrl(currentX, currentY); break;

                default: break;
            }
        }

        private void ChangeUrl(int x, int y)
        {
            var url = urls[(-1) * x][(-1) * y];

            urlLabel.Content = url;
            webView.Navigate(url);
        }

    }
}
