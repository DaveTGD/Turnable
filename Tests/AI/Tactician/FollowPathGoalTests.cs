﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TurnItUp.Pathfinding;
using TurnItUp.AI.Tactician;
using Entropy;
using Tests.Factories;
using TurnItUp.Locations;
using TurnItUp.Interfaces;

namespace Tests.AI.Tactician
{
    [TestClass]
    public class FollowPathGoalTests
    {
        private Entity _entity;
        private List<Node> _path;
        private ILevel _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _entity = EntropyFactory.BuildEntity();
            _path = new List<Node>();
        }

        [TestMethod]
        public void FollowPathGoal_Construction_IsSuccessful()
        {
            FollowPathGoal goal = new FollowPathGoal(_entity, _path);

            Assert.AreEqual(_entity, goal.Owner);
            Assert.AreEqual(_path, goal.Path);
        }

        [TestMethod]
        public void FollowPathGoal_WhenActivated_CreatesAMoveToAtomicGoal()
        {
            _path.Add(new Node(_level, 0, 0));
            _path.Add(new Node(_level, 0, 1));

            FollowPathGoal goal = new FollowPathGoal(_entity, _path);

            goal.Activate();

            Assert.AreEqual(1, goal.Subgoals.Count);
            Assert.IsInstanceOfType(goal.Subgoals[0], typeof(MoveToGoal));
            Assert.AreEqual(_entity, ((MoveToGoal)goal.Subgoals[0]).Owner);
            Assert.AreEqual(_path[1], ((MoveToGoal)goal.Subgoals[0]).Destination);
        }
    }
}
