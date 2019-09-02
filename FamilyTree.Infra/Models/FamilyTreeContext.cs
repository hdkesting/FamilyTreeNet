using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTree.Infra.Models
{
    public class FamilyTreeContext : DbContext
    {
        public FamilyTreeContext(DbContextOptions<FamilyTreeContext> options)
            : base(options)
        {
        }

        public DbSet<Individual> Individuals { get; set; }

        public DbSet<Family> Families { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // setup n:m relation table for spouses
            modelBuilder.Entity<SpouseRelation>()
                .HasKey(sr => new { sr.SpouseId, sr.SpouseFamilyId });

            // relation i->f
            modelBuilder.Entity<SpouseRelation>()
                .HasOne(sr => sr.Spouse)
                .WithMany(i => i.SpouseFamilies)
                .HasForeignKey(sr => sr.SpouseId)
                ;
    
            // relation f->i
            modelBuilder.Entity<SpouseRelation>()
                .HasOne(sr => sr.SpouseFamily)
                .WithMany(f => f.Spouses)
                .HasForeignKey(sr => sr.SpouseFamilyId)
                ;

            // and for children
            modelBuilder.Entity<ChildRelation>()
                .HasKey(cr => new { cr.ChildId, cr.ChildFamilyId });

            modelBuilder.Entity<ChildRelation>()
                .HasOne(cr => cr.Child)
                .WithMany(i => i.ChildFamilies)
                //.HasForeignKey(cr => cr.ChildId)
                ;

            modelBuilder.Entity<ChildRelation>()
                .HasOne(cr => cr.ChildFamily)
                .WithMany(f => f.Children)
                //.HasForeignKey(cr => cr.ChildFamilyId)
                ;


            base.OnModelCreating(modelBuilder);
        }
    }
}
