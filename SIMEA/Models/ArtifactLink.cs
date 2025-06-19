using System;
using System.ComponentModel.DataAnnotations;

namespace SIMEA.Models
{
    public enum ArtifactType
    {
        BusinessStrategy,
        OperationModel,
        OrganizationView,
        CapabilityMap,
        ProcessModel,
        BusinessProductService,
        ApplicationFramework,
        ApplicationBusinessRequirement,
        ServiceCatalogue,
        ApplicationCrossReference,
        ApplicationSecurity,
        ApplicationDataCrossReference,
        DataGovernance,
        InformationHierarchy,
        InformationRequirements,
        DataFlow,
        LogicalData,
        DataSecurity,
        InfrastructureBusinessRequirement,
        SystemToApplication,
        ResourceNeeds,
        SystemToBusiness,
        InfrastructureSecurity,
        SystemData
    }

    public class ArtifactLink
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Source artifact information
        public ArtifactType SourceArtifactType { get; set; }
        public string SourceArtifactId { get; set; } // Unique identifier for the source artifact

        // Target artifact information
        public ArtifactType TargetArtifactType { get; set; }
        public string TargetArtifactId { get; set; } // Unique identifier for the target artifact

        public string LinkDescription { get; set; }
        
        // Adding DateTime tracking properties
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime LinkCreatedDate { get; set; } = DateTime.Now;
        
        public string CreatedBy { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? LastModifiedDate { get; set; }
        
        public string LastModifiedBy { get; set; }
    }
}