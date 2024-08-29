namespace RMSApiServer
{
    using Microsoft.EntityFrameworkCore;
    using RMSApiServer.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }

		public DbSet<Equipment> Equipment { get; set; }
		public DbSet<EquipmentRecipeMap> EquipmentRecipeMap { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
		public DbSet<RecipeParameter> RecipeParameter { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary Key
			modelBuilder.Entity<Equipment>()
				.HasKey(e => new { e.EquipmentId, e.SiteId });

			modelBuilder.Entity<Recipe>()
				.HasKey(r => new { r.RecipeId, r.SiteId });

			modelBuilder.Entity<RecipeParameter>()
				.HasKey(rp => new { rp.RecipeId, rp.RecipeParamId, rp.SiteId });

			modelBuilder.Entity<EquipmentRecipeMap>()
				.HasKey(M => new { M.EquipmentId, M.RecipeId, M.SiteId });

            // Foreign Key
			modelBuilder.Entity<RecipeParameter>()
				.HasOne(rp => rp.Recipe)
				.WithMany(r => r.RecipeParameters)
				.HasForeignKey(rp => new { rp.RecipeId, rp.SiteId });

			modelBuilder.Entity<EquipmentRecipeMap>()
				.HasOne(M => M.Recipe)
				.WithMany(r => r.EquipmentRecipeMap)
				.HasForeignKey(M => new { M.RecipeId, M.SiteId });

			modelBuilder.Entity<EquipmentRecipeMap>()
				.HasOne(M => M.Equipment)
				.WithMany(e => e.EquipmentRecipeMap)
				.HasForeignKey(M => new { M.EquipmentId, M.SiteId });

			base.OnModelCreating(modelBuilder);
        }
    }
}
