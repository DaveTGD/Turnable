﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Factories;
using System.Collections.Generic;
using System.Linq;
using Turnable.Api;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Vision;
using Turnable.Utilities;

namespace Tests.Vision
{
    [TestClass]
    public class LineSegmentTests
    {
        [TestMethod]
        public void Constructor_GivenTwoAdjacentPositions_CreatesALineWithTwoPoints()
        {
            Position start = new Position(1, 1);

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Position end = start.NeighboringPosition(direction);

                LineSegment line = new LineSegment(start, end);

                Assert.AreEqual(2, line.Points.Count);
                Assert.AreEqual(start, line.Points[0]);
                Assert.AreEqual(end, line.Points[1]);
            }
        }

        [TestMethod]
        public void Constructor_GivenTwoDistantPoints_CreatesALine()
        {
            Position start = new Position(1, 1);
            Position end = new Position(2, 6);

            LineSegment line = new LineSegment(start, end);

            Assert.AreEqual(6, line.Points.Count);
            Assert.AreEqual(start, line.Points[0]);
            Assert.AreEqual(new Position(1, 2), line.Points[1]);
            Assert.AreEqual(new Position(1, 3), line.Points[2]);
            Assert.AreEqual(new Position(1, 4), line.Points[3]);
            Assert.AreEqual(new Position(1, 5), line.Points[4]);
            Assert.AreEqual(end, line.Points[5]);
        }

        [TestMethod]
        public void Start_ReturnsTheVeryFirstPoint()
        {
            Position start = new Position(1, 1);
            Position end = new Position(2, 6);
            LineSegment line = new LineSegment(start, end);

            Assert.AreEqual(line.Points[0], line.Start);
        }

        [TestMethod]
        public void End_ReturnsTheVeryLastPoint()
        {
            Position start = new Position(1, 1);
            Position end = new Position(2, 6);
            LineSegment line = new LineSegment(start, end);

            Assert.AreEqual(line.Points[5], line.End);
        }

        [TestMethod]
        public void IsVertical_IfLineSegmentIsVertical_ReturnsTrue()
        {
            LineSegment lineSegment = new LineSegment(new Position(0, 0), new Position(0, 4));

            Assert.IsTrue(lineSegment.IsVertical());

            lineSegment = new LineSegment(new Position(0, 0), new Position(4, 0));
            
            Assert.IsFalse(lineSegment.IsVertical());

            lineSegment = new LineSegment(new Position(0, 0), new Position(4, 4));

            Assert.IsFalse(lineSegment.IsVertical());
        }

        [TestMethod]
        public void IsHorizontal_IfLineSegmentIsHorizontal_ReturnsTrue()
        {
            LineSegment lineSegment = new LineSegment(new Position(0, 0), new Position(4, 0));

            Assert.IsTrue(lineSegment.IsHorizontal());

            lineSegment = new LineSegment(new Position(0, 0), new Position(0, 4));

            Assert.IsFalse(lineSegment.IsHorizontal());

            lineSegment = new LineSegment(new Position(0, 0), new Position(4, 4));

            Assert.IsFalse(lineSegment.IsHorizontal());
        }

        [TestMethod]
        public void GetRandomPoint_ReturnsARandomPointAlongTheLine()
        {
            Position start = new Position(1, 1);
            Position end = new Position(2, 6);
            LineSegment line = new LineSegment(start, end);

            Position randomPoint = line.GetRandomPoint();

            Assert.IsTrue(line.Points.Contains(randomPoint));
        }

        [TestMethod]
        public void GetMidpoint_ReturnsTheMidpointOfTheLine()
        {
            Position start = new Position(1, 1);
            Position end = new Position(2, 6);
            LineSegment line = new LineSegment(start, end);

            Position midpoint = line.GetMidpoint();

            // A line with an even number of points uses the (Number of Points)/2 point
            Assert.IsTrue(line.Points.Contains(midpoint));
            Assert.AreEqual(line.Points[2], midpoint);

            start = new Position(1, 1);
            end = new Position(2, 5);
            line = new LineSegment(start, end);

            midpoint = line.GetMidpoint();

            // A line with an odd number of points returns the exact midpoint
            Assert.IsTrue(line.Points.Contains(midpoint));
            Assert.AreEqual(line.Points[2], midpoint);
        }

        [TestMethod]
        public void Intersects_GivenARectangleThatTheLineSegmentDoesNotIntersectWith_ReturnsFalse()
        {
            Rectangle rectangle = new Rectangle(new Position(5, 5), new Position(10, 10));
            LineSegment lineSegment = new LineSegment(new Position(0, 1), new Position(4, 4));

            Assert.IsFalse(lineSegment.Intersects(rectangle));

            lineSegment = new LineSegment(new Position(4, 5), new Position(4, 10));

            Assert.IsFalse(lineSegment.Intersects(rectangle));
        }

        [TestMethod]
        public void Intersects_GivenARectangleThatTheLineSegmentIntersectsWith_ReturnsTrue()
        {
            Rectangle rectangle = new Rectangle(new Position(5, 5), new Position(10, 10));
            LineSegment lineSegment = new LineSegment(new Position(0, 1), new Position(5, 5));

            Assert.IsTrue(lineSegment.Intersects(rectangle));

            lineSegment = new LineSegment(new Position(15, 15), new Position(10, 10));

            Assert.IsTrue(lineSegment.Intersects(rectangle));

            lineSegment = new LineSegment(new Position(15, 15), new Position(5, 5));

            Assert.IsTrue(lineSegment.Intersects(rectangle));
        }

        [TestMethod]
        public void Intersects_AskedToExcludeTheStartingAndEndingPoints_ReturnsFalseIfOnlyTheStartOrEndPointsIntersectWithARectangle()
        {
            Rectangle rectangle = new Rectangle(new Position(5, 5), new Position(10, 10));
            LineSegment lineSegment = new LineSegment(new Position(5, 5), new Position(4, 4));

            Assert.IsFalse(lineSegment.Intersects(rectangle, true));

            lineSegment = new LineSegment(new Position(10, 10), new Position(11, 11));

            Assert.IsFalse(lineSegment.Intersects(rectangle, true));

            lineSegment = new LineSegment(new Position(5, 5), new Position(7, 7));

            Assert.IsTrue(lineSegment.Intersects(rectangle, true));
        }

        [TestMethod]
        public void IsParallelTo_GivenTwoSegmentsThatAreParallelToEachOther_ReturnsTrue()
        {
            LineSegment first = new LineSegment(new Position(0, 0), new Position(0, 4));
            LineSegment second = new LineSegment(new Position(1, 0), new Position(1, 4));

            Assert.IsTrue(first.IsParallelTo(second));

            first = new LineSegment(new Position(0, 4), new Position(0, 0));
            second = new LineSegment(new Position(1, 4), new Position(1, 0));

            Assert.IsTrue(first.IsParallelTo(second));

            first = new LineSegment(new Position(0, 0), new Position(4, 0));
            second = new LineSegment(new Position(0, 1), new Position(4, 1));

            Assert.IsTrue(first.IsParallelTo(second));

            first = new LineSegment(new Position(4, 0), new Position(0, 0));
            second = new LineSegment(new Position(4, 1), new Position(0, 1));

            Assert.IsTrue(first.IsParallelTo(second));
        }

        [TestMethod]
        public void IsParallelTo_GivenTwoSegmentsOfTheSameLine_ReturnsTrue()
        {
            // The exact same segment
            LineSegment first = new LineSegment(new Position(0, 0), new Position(0, 4));
            LineSegment second = new LineSegment(new Position(0, 0), new Position(0, 4));

            Assert.IsTrue(first.IsParallelTo(second));

            // Two segments on the same line
            first = new LineSegment(new Position(0, 4), new Position(0, 0));
            second = new LineSegment(new Position(0, 4), new Position(0, 8));

            Assert.IsTrue(first.IsParallelTo(second));
        }

        [TestMethod]
        public void IsParallelTo_GivenTwoSegmentsThatAreNotParallelToEachOther_ReturnsFalse()
        {
            LineSegment first = new LineSegment(new Position(0, 0), new Position(0, 4));
            LineSegment second = new LineSegment(new Position(0, 0), new Position(4, 0));

            Assert.IsFalse(first.IsParallelTo(second));

            first = new LineSegment(new Position(0, 0), new Position(0, 4));
            second = new LineSegment(new Position(0, 4), new Position(4, 4));

            Assert.IsFalse(first.IsParallelTo(second));

            first = new LineSegment(new Position(5, 5), new Position(3, 3));
            second = new LineSegment(new Position(17, 13), new Position(4, 4));

            Assert.IsFalse(first.IsParallelTo(second));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DistanceBetween_GivenTwoSegmentsThatAreNotParallel_ThrowsAnException()
        {
            LineSegment first = new LineSegment(new Position(0, 0), new Position(0, 4));
            LineSegment second = new LineSegment(new Position(0, 0), new Position(4, 0));

            first.DistanceBetween(second);
        }

        [TestMethod]
        public void DistanceBetween_GivenTwoParallelSegments_ReturnsTheDistanceBetweenThem()
        {
            LineSegment first = new LineSegment(new Position(0, 0), new Position(0, 4));
            LineSegment second = new LineSegment(new Position(1, 0), new Position(1, 4));

            Assert.AreEqual(1, first.DistanceBetween(second));

            first = new LineSegment(new Position(0, 0), new Position(0, 4));
            second = new LineSegment(new Position(0, 0), new Position(0, 4));

            Assert.AreEqual(0, first.DistanceBetween(second));

            first = new LineSegment(new Position(0, 2), new Position(4, 2));
            second = new LineSegment(new Position(8, 1), new Position(5, 1));

            Assert.AreEqual(1, first.DistanceBetween(second));

            first = new LineSegment(new Position(2, 0), new Position(2, 4));
            second = new LineSegment(new Position(5, 0), new Position(5, 2));

            Assert.AreEqual(3, first.DistanceBetween(second));
        }

        [TestMethod]
        public void IsTouching_GivenTwoNonParallelSegments_ReturnsFalse()
        {
            LineSegment first = new LineSegment(new Position(0, 1), new Position(5, 7));
            LineSegment second = new LineSegment(new Position(0, 1), new Position(4, 6));

            Assert.IsFalse(first.IsTouching(second));
        }

        [TestMethod]
        public void IsTouching_GivenTwoParallelSegmentsThatTouchAlongTheirEntireLength_ReturnsTrue()
        {
            // * First line segment; - Second Line Segment
            // ****
            // ----
            LineSegment first = new LineSegment(new Position(0, 1), new Position(3, 1));
            LineSegment second = new LineSegment(new Position(0, 0), new Position(3, 0));

            Assert.IsTrue(first.IsTouching(second));

            // ----
            // ****
            Assert.IsTrue(second.IsTouching(first));

            // *-
            // *-
            // *-
            // *-
            first = new LineSegment(new Position(0, 0), new Position(0, 3));
            second = new LineSegment(new Position(1, 0), new Position(1, 3));

            Assert.IsTrue(first.IsTouching(second));

            // -*
            // -*
            // -*
            // -*
            Assert.IsTrue(second.IsTouching(first));
        }

        [TestMethod]
        public void IsTouching_GivenTwoParallelSegmentsThatTouchAlongSomePartOfTheirLengtth_ReturnsTrue()
        {
            // * First line segment; - Second Line Segment
            // ****
            //    ----
            LineSegment first = new LineSegment(new Position(0, 1), new Position(3, 1));
            LineSegment second = new LineSegment(new Position(3, 0), new Position(6, 0));

            Assert.IsTrue(first.IsTouching(second));

            // ----
            //    ****
            Assert.IsTrue(second.IsTouching(first));

            // *
            // *
            // *
            // *-
            //  -
            //  -
            //  -
            first = new LineSegment(new Position(0, 3), new Position(0, 6));
            second = new LineSegment(new Position(1, 0), new Position(1, 3));

            Assert.IsTrue(first.IsTouching(second));

            // -
            // -
            // -
            // -*
            //  *
            //  *
            //  *
            Assert.IsTrue(second.IsTouching(first));
        }

        [TestMethod]
        public void IsTouching_GivenTwoCloseParallelSegmentsThatDontTouch_ReturnsFalse()
        {
            // * First line segment; - Second Line Segment
            // ****
            //     ----
            LineSegment first = new LineSegment(new Position(0, 1), new Position(3, 1));
            LineSegment second = new LineSegment(new Position(4, 0), new Position(7, 0));

            Assert.IsFalse(first.IsTouching(second));

            // ----
            //     ****
            Assert.IsFalse(second.IsTouching(first));

            // *
            // *
            // *
            // *
            //  -
            //  -
            //  -
            //  -
            first = new LineSegment(new Position(0, 4), new Position(0, 7));
            second = new LineSegment(new Position(1, 0), new Position(1, 3));

            Assert.IsFalse(first.IsTouching(second));

            // -
            // -
            // -
            // -
            //  *
            //  *
            //  *
            //  *
            Assert.IsFalse(second.IsTouching(first));
        }

        [TestMethod]
        public void IsTouching_GivenTwoParallelSegmentsThatDontTouch_ReturnsFalse()
        {
            // * First line segment; - Second Line Segment
            // ****
            //     
            //     
            //     ----
            LineSegment first = new LineSegment(new Position(0, 3), new Position(3, 3));
            LineSegment second = new LineSegment(new Position(4, 0), new Position(7, 0));

            Assert.IsFalse(first.IsTouching(second));

            // ----
            //     
            //     
            //     ****
            Assert.IsFalse(second.IsTouching(first));

            // *
            // *
            // *
            // *
            //    -
            //    -
            //    -
            //    -
            first = new LineSegment(new Position(0, 4), new Position(0, 7));
            second = new LineSegment(new Position(3, 0), new Position(3, 3));

            Assert.IsFalse(first.IsTouching(second));

            // -
            // -
            // -
            // -
            //    *
            //    *
            //    *
            //    *
            Assert.IsFalse(second.IsTouching(first));
        }
    }
}
