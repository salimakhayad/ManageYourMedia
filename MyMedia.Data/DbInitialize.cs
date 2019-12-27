using System;
using System.Collections.Generic;
using System.Text;

namespace MyMedia.Data
{
    public class DbInitialize
    {
        public static void Initialize(MediaDbContext context)
        {
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}
