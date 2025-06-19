using System.Collections.Generic;
using SIMEA.Models; // Ensure ArtifactLink and ArtifactType are visible

namespace SIMEA.Models
{
    // Aggregate for Business Architecture artifacts
    public class BusinessArchitectureArtifacts
    {
        public int Id { get; set; }
        public List<BusinessStrategyView> BusinessStrategies { get; set; }
        public List<OperationModel> OperationModels { get; set; }
        public List<OrganizationView> OrganizationViews { get; set; }
        public List<CapabilityMap> CapabilityMaps { get; set; }
        public List<ProcessModel> ProcessModels { get; set; }
        public List<BusinessProductServiceView> BusinessProductServices { get; set; }
    }

    // Aggregate for Application Architecture artifacts
    public class ApplicationArchitectureArtifacts
    {
        public int Id { get; set; }
        public List<ApplicationArchitectureFramework> ApplicationFrameworks { get; set; }
        public List<ApplicationBusinessRequirementsView> ApplicationBusinessRequirements { get; set; }
        public List<ServiceCatalogue> ServiceCatalogues { get; set; }
        public List<ApplicationToApplicationCrossReference> ApplicationCrossReferences { get; set; }
        public List<ApplicationSecurityModel> ApplicationSecurityModels { get; set; }
        public List<ApplicationDataCrossReference> ApplicationDataCrossReferences { get; set; }
    }

    // Aggregate for Data Architecture artifacts
    public class DataArchitectureArtifacts
    {
        public int Id { get; set; }
        public List<DataGovernanceModel> DataGovernanceModels { get; set; }
        public List<InformationHierarchyView> InformationHierarchies { get; set; }
        public List<InformationRequirementsView> InformationRequirements { get; set; }
        public List<DataFlowModel> DataFlowModels { get; set; }
        public List<LogicalDataModel> LogicalDataModels { get; set; }
        public List<DataSecurityModel> DataSecurityModels { get; set; }
    }

    // Aggregate for Infrastructure Architecture artifacts
    public class InfrastructureArchitectureArtifacts
    {
        public int Id { get; set; }
        public List<InfrastructureBusinessRequirementsView> InfrastructureBusinessRequirements { get; set; }
        public List<SystemToApplicationCrossReference> SystemToApplicationCrossReferences { get; set; }
        public List<ResourceNeedsModel> ResourceNeeds { get; set; }
        public List<SystemToBusinessCrossReference> SystemToBusinessCrossReferences { get; set; }
        public List<InfrastructureSecurityModel> InfrastructureSecurityModels { get; set; }
        public List<SystemDataCrossReference> SystemDataCrossReferences { get; set; }
    }

    // Central Enterprise Architecture that ties all domains together
    public class EnterpriseArchitecture
    {
        public int Id { get; set; }
        public BusinessArchitectureArtifacts BusinessArtifacts { get; set; }
        public ApplicationArchitectureArtifacts ApplicationArtifacts { get; set; }
        public DataArchitectureArtifacts DataArtifacts { get; set; }
        public InfrastructureArchitectureArtifacts InfrastructureArtifacts { get; set; }
        // New linking property to connect multiple artifacts across domains
        public List<ArtifactLink> ArtifactLinks { get; set; }
    }
}
