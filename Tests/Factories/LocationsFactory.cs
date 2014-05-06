﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TurnItUp.Tmx;
using TurnItUp.Locations;
using Entropy;
using TurnItUp.Interfaces;

namespace Tests.Factories
{
    public static class LocationsFactory
    {
        public static ILevel BuildLevel(string tmxPath = "../../Fixtures/FullExample.tmx")
        {
            World world = new World();

            // TODO: Perhaps use a LevelFactory here after one has been built?
            LevelFactory levelFactory = new LevelFactory();

            LevelSetUpParams setUpParams = new LevelSetUpParams();
            setUpParams.TmxPath = tmxPath;

            return levelFactory.BuildLevel(world, setUpParams);
        }
    }
}
