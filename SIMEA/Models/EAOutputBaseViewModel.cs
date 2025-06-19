using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SIMEA.Models;

namespace SIMEA.Models.ViewModels
{
    // Base view model for all EA output views
    public class EAOutputBaseViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        
        // Adding datetime tracking properties
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
        
        public string GeneratedBy { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
        
        public string LastModifiedBy { get; set; }
    }

    // Strategic Alignment View
    public class StrategicAlignmentViewModel : EAOutputBaseViewModel
    {
        public List<BusinessStrategyView> BusinessStrategies { get; set; }
        public List<CapabilityMap> Capabilities { get; set; }
        public List<ArtifactLink> StrategyCapabilityLinks { get; set; }
        public List<ApplicationArchitectureFramework> Applications { get; set; }
        public List<ArtifactLink> CapabilityApplicationLinks { get; set; }
    }

    // Application Portfolio Dashboard
    public class ApplicationPortfolioViewModel : EAOutputBaseViewModel
    {
        public List<ApplicationArchitectureFramework> Applications { get; set; }
        public List<ServiceCatalogue> Services { get; set; }
        public List<ApplicationToApplicationCrossReference> AppDependencies { get; set; }
        public List<ApplicationBusinessRequirementsView> BusinessRequirements { get; set; }
    }

    // Architecture Roadmap
    public class ArchitectureRoadmapViewModel : EAOutputBaseViewModel
    {
        // Timeline-based roadmap elements
        public List<RoadmapItem> RoadmapItems { get; set; }
        public List<string> TimelinePhases { get; set; }
    }

    public class RoadmapItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Phase { get; set; }
        public string Domain { get; set; } // Business, Application, Data, Infrastructure
        public string Description { get; set; }
        public List<string> Dependencies { get; set; }
        
        // Adding datetime properties for roadmap planning
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;
    }

    // Cross-Domain Impact Analysis
    public class CrossDomainImpactViewModel : EAOutputBaseViewModel
    {
        public List<ArtifactNodeViewModel> AllArtifacts { get; set; }
        public List<ArtifactConnectionViewModel> CrossDomainConnections { get; set; }
        public List<ImpactAnalysisItem> ImpactItems { get; set; }
    }

    public class ImpactAnalysisItem
    {
        public string SourceId { get; set; }
        public string SourceName { get; set; }
        public ArtifactDomain SourceDomain { get; set; }
        public List<ArtifactNodeViewModel> ImpactedArtifacts { get; set; }
        public int TotalImpactScore { get; set; }
    }

    // Technology Standards Compliance
    public class TechnologyStandardsViewModel : EAOutputBaseViewModel
    {
        public List<TechnologyStandardItem> StandardsItems { get; set; }
        public Dictionary<string, int> ComplianceByDomain { get; set; }
        public List<ApplicationArchitectureFramework> NonCompliantApplications { get; set; }
    }

    public class TechnologyStandardItem
    {
        public string Category { get; set; }
        public string StandardName { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public int CompliantCount { get; set; }
        public int NonCompliantCount { get; set; }
        public List<string> CompliantApplications { get; set; }
        public List<string> NonCompliantApplications { get; set; }
    }

    // Data Flow Diagram
    public class DataFlowDiagramViewModel : EAOutputBaseViewModel
    {
        public List<DataFlowModel> DataFlows { get; set; }
        public List<LogicalDataModel> LogicalDataModels { get; set; }
        public List<ApplicationArchitectureFramework> Applications { get; set; }
        public List<ApplicationDataCrossReference> AppDataReferences { get; set; }
        public List<SystemDataCrossReference> SystemDataReferences { get; set; }
    }

    // Capability to Application Mapping
    public class CapabilityApplicationMappingViewModel : EAOutputBaseViewModel
    {
        public List<CapabilityMap> Capabilities { get; set; }
        public List<ApplicationArchitectureFramework> Applications { get; set; }
        public List<CapabilityApplicationMapping> Mappings { get; set; }
    }

    public class CapabilityApplicationMapping
    {
        public string CapabilityId { get; set; }
        public string CapabilityName { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string SupportLevel { get; set; } // Full, Partial, None
        public string Description { get; set; }
        
        // Adding datetime tracking
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime MappingDate { get; set; } = DateTime.Now;
        
        public string MappedBy { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ReviewDate { get; set; }
    }

    // Business Value Heat Map
    public class BusinessValueHeatMapViewModel : EAOutputBaseViewModel
    {
        public List<BusinessProductServiceView> Products { get; set; }
        public List<CapabilityMap> Capabilities { get; set; }
        public List<HeatMapItem> HeatMapItems { get; set; }
    }

    public class HeatMapItem
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public int BusinessValue { get; set; } // 1-10
        public int StrategicImportance { get; set; } // 1-10
        public int RiskLevel { get; set; } // 1-10
        public string Domain { get; set; }
        
        // Adding assessment date
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AssessmentDate { get; set; } = DateTime.Now;
    }

    // Integration Landscape
    public class IntegrationLandscapeViewModel : EAOutputBaseViewModel
    {
        public List<ApplicationToApplicationCrossReference> AppIntegrations { get; set; }
        public List<SystemToApplicationCrossReference> SystemAppIntegrations { get; set; }
        public List<IntegrationPattern> IntegrationPatterns { get; set; }
    }

    public class IntegrationPattern
    {
        public string PatternType { get; set; } // API, File Transfer, Messaging, etc.
        public int Count { get; set; }
        public List<string> Examples { get; set; }
    }

    // Business Process to Tech Stack Alignment
    public class BusinessProcessTechAlignmentViewModel : EAOutputBaseViewModel
    {
        public List<ProcessModel> BusinessProcesses { get; set; }
        public List<ApplicationArchitectureFramework> Applications { get; set; }
        public List<ProcessTechMapping> Mappings { get; set; }
    }

    public class ProcessTechMapping
    {
        public string ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string TechnologyStack { get; set; }
        public int AlignmentScore { get; set; } // 1-5
        
        // Added for JavaScript compatibility
        public int Score
        {
            get { return AlignmentScore; }
            set { AlignmentScore = value; }
        }
        
        // Adding mapping date tracking
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime MappingDate { get; set; } = DateTime.Now;
    }

    // Service Dependency Map
    public class ServiceDependencyMapViewModel : EAOutputBaseViewModel
    {
        public List<ServiceCatalogue> Services { get; set; }
        public List<ApplicationArchitectureFramework> Applications { get; set; }
        public List<ServiceDependency> Dependencies { get; set; }
    }

    public class ServiceDependency
    {
        public string SourceServiceId { get; set; }
        public string SourceServiceName { get; set; }
        public string TargetServiceId { get; set; }
        public string TargetServiceName { get; set; }
        public string DependencyType { get; set; }
        public string CriticalityLevel { get; set; }
        
        // Adding dependency identification date
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime IdentifiedDate { get; set; } = DateTime.Now;
    }
    
    // Value Stream Modeling
    public class ValueStreamModelingViewModel : EAOutputBaseViewModel
    {
        public List<ProcessModel> BusinessProcesses { get; set; }
    }
    
    // Application Rationalization
    public class ApplicationRationalizationViewModel : EAOutputBaseViewModel
    {
        public List<ApplicationArchitectureFramework> Applications { get; set; }
        public Dictionary<string, int> CategoryCounts { get; set; } // Counts by category
    }
}