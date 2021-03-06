﻿using System;
using System.Windows;
using System.Windows.Input;
using DamageMeter.Skills.Skill.SkillDetail;
using Data;
using Tera.Game;

namespace DamageMeter.UI.SkillDetail
{
    /// <summary>
    ///     Logique d'interaction pour SkillContent.xaml
    /// </summary>
    public partial class SkillDetailDps
    {
        public SkillDetailDps(SkillDetailStats skill, Entity currentBoss, bool timedEncounter)
        {
            InitializeComponent();
            Update(skill, currentBoss, timedEncounter);
        }

        public void Update(SkillDetailStats skill, Entity currentBoss, bool timedEncounter)
        {
            var userskill = BasicTeraData.Instance.SkillDatabase.GetOrNull(skill.PlayerInfo.Player.User, skill.Id);
            bool? chained = userskill?.IsChained;
            string hit = userskill?.Detail;
           
            if (hit == null)
            {
                if (BasicTeraData.Instance.HotDotDatabase.Get(skill.Id) != null)
                {
                    hit = "DOT";
                }
            }
            if (hit != null)
            {
                LabelName.Content = hit;
            }
            if (chained == true)
            {
                LabelName.Content += " Chained";
            }

            LabelName.ToolTip = skill.Id;
            LabelCritRateDmg.Content = skill.CritRateDmg + "%";

            LabelDamagePercentage.Content = skill.GetDamagePercentage(currentBoss, timedEncounter) + "%";
            LabelTotalDamage.Content = FormatHelpers.Instance.FormatValue(skill.Damage);

            LabelNumberHitDmg.Content = skill.HitsDmg;

            LabelNumberCritDmg.Content = skill.CritsDmg;

            LabelAverageCrit.Content = FormatHelpers.Instance.FormatValue(skill.DmgAverageCrit);
            LabelBiggestCrit.Content = FormatHelpers.Instance.FormatValue(skill.DmgBiggestCrit);
            LabelAverageHit.Content = FormatHelpers.Instance.FormatValue(skill.DmgAverageHit);
            LabelAverageTotal.Content = FormatHelpers.Instance.FormatValue(skill.DmgAverageTotal);
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            var w = Window.GetWindow(this);
            try
            {
                w?.DragMove();
            }
            catch
            {
                Console.WriteLine(@"Exception move");
            }
        }
    }
}