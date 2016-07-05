﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioWeb.Models
{
    public class AudioEntity : TableEntity
    {
        public string Title { get; set; }
        public int Plays { get; set; }
        public int Skips { get; set; }
    }
}