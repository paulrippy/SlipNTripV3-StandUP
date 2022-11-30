using SlipNTrip.Pages;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace SlipNTrip.Pages
{
    public class SSGraphicPage : ContentPage
    {
        SKImageInfo info;
        SKSurface surface;
        SKCanvas canvas;
        private double[,] startingLocationArray;
        private double[,] endingLocationArray;
        private double[,] paintArray;
        private string strStartLocation;
        private string strEndLocation;
        private string paintTitle;
        private double maxDarkRed = 1000;
        private double minDarkRed = 800;
        private double maxFirebrick = 800;
        private double minFirebrick = 600;
        private double maxCrimson = 600;
        private double minCrimson = 400;
        private double maxTomato = 400;
        private double minTomato = 200;

        private SKCanvasView startingLocationCanvasView;
        private SKCanvasView endingLocationCanvasView;

        public SSGraphicPage(double[,] startingLocationArray, double[,] endingLocationArray)
        {
            //Title = "Patient's Location on Device";
            this.startingLocationArray = startingLocationArray;
            this.endingLocationArray = endingLocationArray;
            strStartLocation = "Starting Location";
            strEndLocation = "Ending Location";
            paintArray = startingLocationArray;
            paintTitle = strStartLocation;

            ToolbarItem locationToolbarItem = new ToolbarItem
            {
                Text = "Patient's Location on Device\t",
                Order = ToolbarItemOrder.Primary,
                
                Priority = 0,
            };
            locationToolbarItem.Clicked += locationToolbarItemClicked;
            this.ToolbarItems.Add(locationToolbarItem);

            ToolbarItem keyToolbarItem = new ToolbarItem
            {
                Text = "Color Key\t",
                Order = ToolbarItemOrder.Primary,
                Priority = 1,
            };
            keyToolbarItem.Clicked += keyToolbarItemClicked;
            this.ToolbarItems.Add(keyToolbarItem);

            ToolbarItem helpToolbarItem = new ToolbarItem
            {
                Text = "?",
                Order = ToolbarItemOrder.Primary,
                Priority = 2
            };
            helpToolbarItem.Clicked += helpToolbarItemClicked;
            this.ToolbarItems.Add(helpToolbarItem);

            startingLocationCanvasView = new SKCanvasView();
            endingLocationCanvasView = new SKCanvasView();

            startingLocationCanvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = startingLocationCanvasView;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            info = args.Info;
            surface = args.Surface;
            canvas = surface.Canvas;

            canvas.Clear();

            SKPaint paint = new SKPaint();

            // Create an SKPaint object to display the text
            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.DarkGray
            };
            // Adjust TextSize property so text is 90% of screen width
            float textWidth = textPaint.MeasureText(paintTitle);
            textPaint.TextSize = 0.6f * info.Width * textPaint.TextSize / textWidth;

            // Find the text bounds
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(paintTitle, ref textBounds);

            // Calculate offsets to center the text on the screen
            float xText = info.Width / 2 - textBounds.MidX;

            // And draw the text
            canvas.DrawText(paintTitle, xText, 150, textPaint);
          
            // Paint Color for Grid Lines
            paint.Style = SKPaintStyle.Stroke;
            paint.Color = SKColors.Gray;
            paint.StrokeWidth = 10;

            //Coordinate Values
            int numLines = 7;
            int yStartingCordinate = 300;
            int yEndingCorrdinate = yStartingCordinate;
            int xStartingCorrdinate = 25;
            int xEndingCorrdinate = xStartingCorrdinate + (numLines - 1) * 100;

            //Horizontal Lines
            for (int i = 0; i < 13; i++)
            {
                canvas.DrawLine(xStartingCorrdinate, yEndingCorrdinate, 1225, yEndingCorrdinate, paint);
                yEndingCorrdinate += 100;
            }

            //Vertical Lines
            xEndingCorrdinate = xStartingCorrdinate;
            for (int i = 0; i < 13; i++)
            {
                canvas.DrawLine(xEndingCorrdinate, yStartingCordinate, xEndingCorrdinate, yEndingCorrdinate - 100, paint);
                xEndingCorrdinate += 100;
            }

            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.Red;

            int xArrayLocation = 0;
            for (int x = xStartingCorrdinate + 100; x < xEndingCorrdinate - 100; x += 100) {
                int yArrayLocation = 0;
                for (int y = yStartingCordinate + 100; y < yEndingCorrdinate - 100; y += 100)
                {
                    if (paintArray[xArrayLocation,yArrayLocation] > minDarkRed)
                    {
                        paint.Color = SKColors.DarkRed;
                    }
                    else if(paintArray[xArrayLocation,yArrayLocation] > minFirebrick)
                    {
                        paint.Color = SKColors.Firebrick;
                    }
                    else if(paintArray[xArrayLocation,yArrayLocation] > minCrimson)
                    {
                        paint.Color = SKColors.Crimson;
                    }
                    else if(paintArray[xArrayLocation,yArrayLocation] > minTomato)
                    {
                        paint.Color = SKColors.Tomato;
                    }
                    else
                    {
                        paint.Color = SKColors.Green;
                    }
                    canvas.DrawCircle(x, y, 25, paint);
                    yArrayLocation += 1; 
                }
                xArrayLocation += 1;
            }
        }

        async void locationToolbarItemClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Which location would you like to view?", "Cancel", null, "Starting Location", "Ending Location");
            if (action.Equals("Ending Location"))
            {
                paintArray = endingLocationArray;
                paintTitle = strEndLocation;
                endingLocationCanvasView.PaintSurface += OnCanvasViewPaintSurface;
                Content = endingLocationCanvasView;
            }
            else if (action.Equals("Starting Location"))
            {
                paintArray = startingLocationArray;
                paintTitle = strStartLocation;
                startingLocationCanvasView.PaintSurface += OnCanvasViewPaintSurface;
                Content = startingLocationCanvasView;
            }
        }

        async void keyToolbarItemClicked(object sender, EventArgs e)
        {
            string keyMessage = "This will display an image of the key explaining the colors";
            await DisplayAlert("Patient's Location on Device: Color Key", keyMessage, "Done");
        }

        async void helpToolbarItemClicked(object sender, EventArgs e)
        {
            string helpMessage = "Purpose: To display patient's location on device\n" +
                "Meaning of Colors: \n" +
                "\tDifferent shades of Red: Where pressure \n \t \tis applied; Darker the shade, the more \n \t \tpressure is applied\n" +
                "\tGreen: No pressure is applied";
            await DisplayAlert("Help - Patient's Location on Device", helpMessage, "Done");
        }
    }
}