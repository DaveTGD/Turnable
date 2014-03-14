﻿using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Locations;
using TurnItUp.Skills;

namespace TurnItUp.Interfaces
{
    public interface ISkill
    {
        string Name { get; set; }
        RangeType RangeType { get; set; }
        TargetType TargetType { get; set; }
        int Range { get; set; }
        ISkillOriginMapCalculator OriginMapCalculator { get; set; }

        TargetMap CalculateTargetMap(ILevel level, Position skillUserPosition);
        void Apply(Entity user, Entity target);
    }
}