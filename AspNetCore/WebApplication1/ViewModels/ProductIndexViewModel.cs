﻿using AspNetCoreDataSource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreTest.ViewModels
{
    public class ProductIndexViewModel
    {
        public IList<Contact> Contacts { get; set; }
        public int Count { get; set; }
    }
}
