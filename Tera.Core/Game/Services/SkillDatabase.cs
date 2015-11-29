﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tera.Game
{
    // Contains information about skills
    // Currently this is limited to the name of the skill
    public class SkillDatabase
    {
        private readonly Dictionary<PlayerClass, List<UserSkill>> _userSkilldata =
            new Dictionary<PlayerClass, List<UserSkill>>();


        public SkillDatabase(string filename)
        {
            var mystic = new StreamReader(File.OpenRead(filename+"mystic.csv"));
            var common = new StreamReader(File.OpenRead(filename + "common.csv"));
            var warrior = new StreamReader(File.OpenRead(filename + "warrior.csv"));
            var gunner = new StreamReader(File.OpenRead(filename + "gunner.csv"));
            var reaper = new StreamReader(File.OpenRead(filename + "reaper.csv"));
            var archer = new StreamReader(File.OpenRead(filename + "archer.csv"));
            var slayer = new StreamReader(File.OpenRead(filename + "slayer.csv"));
            var berserker = new StreamReader(File.OpenRead(filename + "berserker.csv"));
            var sorcerer = new StreamReader(File.OpenRead(filename + "sorcerer.csv"));
            var lancer = new StreamReader(File.OpenRead(filename + "lancer.csv"));
            var priest = new StreamReader(File.OpenRead(filename + "priest.csv"));
            ParseFile(mystic, PlayerClass.Mystic);
            ParseFile(common, PlayerClass.Common);
            ParseFile(warrior, PlayerClass.Warrior);
            ParseFile(gunner, PlayerClass.Gunner);
            ParseFile(reaper, PlayerClass.Reaper);
            ParseFile(archer, PlayerClass.Archer);
            ParseFile(slayer, PlayerClass.Slayer);
            ParseFile(berserker, PlayerClass.Berserker);
            ParseFile(sorcerer, PlayerClass.Sorcerer);
            ParseFile(lancer, PlayerClass.Lancer);
            ParseFile(priest, PlayerClass.Priest);

        }

        private void ParseFile(StreamReader reader, PlayerClass playerClass)
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null) continue;
                var values = line.Split('\t');

                var skill = new UserSkill(int.Parse(values[0]),  playerClass, values[1]);
                if (!_userSkilldata.ContainsKey(skill.PlayerClass))
                {
                    _userSkilldata[skill.PlayerClass] = new List<UserSkill>();
                }
                _userSkilldata[skill.PlayerClass].Add(skill);
            }
        }

        // skillIds are reused across races and class, so we need a RaceGenderClass to disambiguate them
        public UserSkill Get(UserEntity user, int skillId)
        {
            List<UserSkill> skillsSpecific, skillsCommon;

            _userSkilldata.TryGetValue(user.RaceGenderClass.Class, out skillsSpecific);

            _userSkilldata.TryGetValue(PlayerClass.Common, out skillsCommon);
         

            var allSkills = new List<UserSkill>();
            if (skillsCommon != null)
            {
                allSkills = allSkills.Union(skillsCommon).ToList();
            }
            if (skillsSpecific != null)
            {
                allSkills = allSkills.Union(skillsSpecific).ToList();
            }

            return allSkills.FirstOrDefault(skill => skill.Id == skillId);
        }

        public UserSkill GetOrPlaceholder(UserEntity user, int skillId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existing = Get(user, skillId);
            if (existing != null)
                return existing;

            return new UserSkill(skillId, user.RaceGenderClass.Class, "Unknown " + skillId);
        }

        public string GetName(UserEntity user, int skillId)
        {
            return GetOrPlaceholder(user, skillId).Name;
        }
    }
}