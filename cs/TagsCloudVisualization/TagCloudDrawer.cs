﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class TagCloudDrawer
    {
        public static Bitmap Draw(
            Rectangle[] rectangles,
            int width, int height, Point center,
            Color bgColor, Color rectangleColor,
            bool drawCenter = false, bool drawCircle = false)
        {
            var myBitmap = new Bitmap(width, height);
            var g = Graphics.FromImage(myBitmap);
            g.Clear(bgColor);

            foreach (var rectangle in rectangles)
            {
                g.DrawRectangle(new Pen(rectangleColor, 2), rectangle);
            }

            if (drawCenter) DrawCenter(g, center);
            if (drawCircle) DrawMaxCircle(g, center, rectangles);

            return myBitmap;
        }

        private static void DrawCenter(Graphics g, Point center)
        {
            g.DrawEllipse(new Pen(Color.Blue, 2), center.X - 4, center.Y - 4, 8, 8);
        }

        private static void DrawMaxCircle(Graphics g, Point center, IEnumerable<Rectangle> rectangles)
        {
            var maxDistance = rectangles
                .Select(rect => GetMaxDistanceToNode(rect, center))
                .Max();
            g.DrawEllipse(
                new Pen(Color.Blue, 2),
                center.X - (int)maxDistance,
                center.Y - (int)maxDistance,
                (int)maxDistance * 2,
                (int)maxDistance * 2);
        }

        private static double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) +
                             Math.Pow(a.Y - b.Y, 2));
        }

        private static double GetMaxDistanceToNode(Rectangle rectangle, Point center)
        {
            var nodes = new[]
            {
                rectangle.Location,
                new Point(rectangle.Right, rectangle.Top),
                new Point(rectangle.Left, rectangle.Bottom),
                new Point(rectangle.Right, rectangle.Bottom)
            };
            return nodes
                .Select(node => GetDistance(node, center))
                .Max();
        }
    }
}