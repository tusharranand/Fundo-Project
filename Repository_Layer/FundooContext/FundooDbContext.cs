﻿using Microsoft.EntityFrameworkCore;
using Repository_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.FundooContext
{
    public class FundooDbContext : DbContext
    {
        public FundooDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<Label> Label { get; set; }
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }
    }
}
