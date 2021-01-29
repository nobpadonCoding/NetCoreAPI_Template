using Microsoft.EntityFrameworkCore;
using NetCoreAPI_Template_v2.Models;
using NetCoreAPI_Template_v2.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAPI_Template_v2.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<CharacterSkill> CharacterSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterSkill>().HasKey(x=>new{x.CharacterId, x.SkillId});
        }

    }
}