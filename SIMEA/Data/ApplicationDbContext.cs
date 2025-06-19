using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SIMEA.Models;
using System.Collections.Generic;
namespace SIMEA.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityExtendUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<SettingGeneralInfo> SettingGeneralInfo { get; set; }

        // Business Architecture DbSets
        public DbSet<BusinessStrategyView> BusinessStrategyViews { get; set; }
        public DbSet<OperationModel> OperationModels { get; set; }
        public DbSet<OrganizationView> OrganizationViews { get; set; }
        public DbSet<CapabilityMap> CapabilityMaps { get; set; }
        public DbSet<ProcessModel> ProcessModels { get; set; }
        public DbSet<BusinessProductServiceView> BusinessProductServiceViews { get; set; }
        public DbSet<ObjectiveKeyResultModel> ObjectiveKeyResults { get; set; }
        public DbSet<BusinessOutcomeModel> BusinessOutcomes { get; set; }
        public DbSet<FinancialModelingView> FinancialModels { get; set; }
        public DbSet<EnhancedValueStreamModel> EnhancedValueStreams { get; set; }
        public DbSet<TechnologyTrendModel> TechnologyTrends { get; set; }
        public DbSet<InvestmentDecisionModel> InvestmentDecisions { get; set; }
        public DbSet<EnhancedBusinessOutcomeModel> EnhancedBusinessOutcomes { get; set; }

        // Application Architecture DbSets
        public DbSet<ApplicationArchitectureFramework> ApplicationArchitectureFrameworks { get; set; }
        public DbSet<ApplicationBusinessRequirementsView> ApplicationBusinessRequirementsViews { get; set; }
        public DbSet<ServiceCatalogue> ServiceCatalogues { get; set; }
        public DbSet<ApplicationToApplicationCrossReference> ApplicationToApplicationCrossReferences { get; set; }
        public DbSet<ApplicationSecurityModel> ApplicationSecurityModels { get; set; }
        public DbSet<ApplicationDataCrossReference> ApplicationDataCrossReferences { get; set; }

        // Data Architecture DbSets
        public DbSet<DataGovernanceModel> DataGovernanceModels { get; set; }
        public DbSet<InformationHierarchyView> InformationHierarchyViews { get; set; }
        public DbSet<InformationRequirementsView> InformationRequirementsViews { get; set; }
        public DbSet<DataFlowModel> DataFlowModels { get; set; }
        public DbSet<LogicalDataModel> LogicalDataModels { get; set; }
        public DbSet<DataSecurityModel> DataSecurityModels { get; set; }

        // Infrastructure Architecture DbSets
        public DbSet<InfrastructureBusinessRequirementsView> InfrastructureBusinessRequirementsViews { get; set; }
        public DbSet<SystemToApplicationCrossReference> SystemToApplicationCrossReferences { get; set; }
        public DbSet<ResourceNeedsModel> ResourceNeedsModels { get; set; }
        public DbSet<SystemToBusinessCrossReference> SystemToBusinessCrossReferences { get; set; }
        public DbSet<InfrastructureSecurityModel> InfrastructureSecurityModels { get; set; }
        public DbSet<SystemDataCrossReference> SystemDataCrossReferences { get; set; }

        // Artifact linking DbSet
        public DbSet<ArtifactLink> ArtifactLinks { get; set; }

        // Complex entity DbSets
        public DbSet<ProcessStage> ProcessStages { get; set; }
        public DbSet<StakeholderValueItem> StakeholderValues { get; set; }
        public DbSet<ApplicationRecommendation> ApplicationRecommendations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ArtifactLink primary key
            modelBuilder.Entity<ArtifactLink>().HasKey(a => a.Id);

            // Configure ProcessStage
            modelBuilder.Entity<ProcessStage>().HasKey(p => p.Id);
            
            // Configure StakeholderValueItem
            modelBuilder.Entity<StakeholderValueItem>().HasKey(s => s.Id);
            
            // Configure ApplicationRecommendation
            modelBuilder.Entity<ApplicationRecommendation>().HasKey(r => r.Id);
            
            // Configure ProcessModel relationships
            modelBuilder.Entity<ProcessModel>()
                .HasMany(p => p.Stages)
                .WithOne()
                .HasForeignKey("ProcessModelId"); 
                
            modelBuilder.Entity<ProcessModel>()
                .HasMany(p => p.StakeholderValues)
                .WithOne()
                .HasForeignKey("ProcessModelId");
                
            // Configure ApplicationArchitectureFramework relationships
            modelBuilder.Entity<ApplicationArchitectureFramework>()
                .HasMany(a => a.Recommendations)
                .WithOne()
                .HasForeignKey("ApplicationFrameworkId");

            // Configure complex property conversions for List<string> properties
            ConfigureListStringProperties(modelBuilder);
        }

        private void ConfigureListStringProperties(ModelBuilder modelBuilder)
        {
            var jsonOptions = new System.Text.Json.JsonSerializerOptions();

            // Existing configurations
            modelBuilder.Entity<SystemDataCrossReference>()
                .Property(e => e.Dependencies)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                );
                
            modelBuilder.Entity<SystemToApplicationCrossReference>()
                .Property(e => e.Dependencies)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                );
                
            modelBuilder.Entity<ResourceNeedsModel>()
                .Property(e => e.Dependencies)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                );
                
            modelBuilder.Entity<SystemToBusinessCrossReference>()
                .Property(e => e.Dependencies)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                );

            // New configurations for enhanced models
            ConfigureObjectiveKeyResultProperties(modelBuilder, jsonOptions);
            ConfigureBusinessOutcomeProperties(modelBuilder, jsonOptions);
            ConfigureFinancialModelingProperties(modelBuilder, jsonOptions);
            ConfigureTechnologyTrendProperties(modelBuilder, jsonOptions);
            
        }

        private void ConfigureObjectiveKeyResultProperties(ModelBuilder modelBuilder, System.Text.Json.JsonSerializerOptions jsonOptions)
        {
            modelBuilder.Entity<ObjectiveKeyResultModel>()
                .Property(e => e.RelatedCapabilities)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                );
        }

        private void ConfigureBusinessOutcomeProperties(ModelBuilder modelBuilder, System.Text.Json.JsonSerializerOptions jsonOptions)
        {
            modelBuilder.Entity<BusinessOutcomeModel>()
                .Property(e => e.ContributingOKRs)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                );
                
            modelBuilder.Entity<BusinessOutcomeModel>()
                .Property(e => e.RiskFactors)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                );
        }

        private void ConfigureFinancialModelingProperties(ModelBuilder modelBuilder, System.Text.Json.JsonSerializerOptions jsonOptions)
        {
            modelBuilder.Entity<FinancialModelingView>()
                .Property(e => e.Assumptions)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                );
                
            modelBuilder.Entity<FinancialModelingView>()
                .Property(e => e.RelatedOKRIds)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                );
        }

        private void ConfigureTechnologyTrendProperties(ModelBuilder modelBuilder, System.Text.Json.JsonSerializerOptions jsonOptions)
        {
            modelBuilder.Entity<TechnologyTrendModel>()
                .Property(e => e.IndustrySectors)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                );
                
            modelBuilder.Entity<TechnologyTrendModel>()
                .Property(e => e.KeyVendors)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, jsonOptions),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                );
        }
    }
}


