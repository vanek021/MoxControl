﻿using MoxControl.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Models.Entities.Settings
{
    public class GeneralSetting : BaseRecord
    {
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public bool IsHide { get; set; } = false;
    }
}
