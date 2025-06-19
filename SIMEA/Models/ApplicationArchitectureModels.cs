using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMEA.Models
{
    public class ApplicationArchitectureFramework
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Application name is required")]
        [StringLength(100, ErrorMessage = "Application name cannot exceed 100 characters")]
        [Display(Name = "Application Name")]
        public string ApplicationName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string ApplicationDescription { get; set; }

        [Required(ErrorMessage = "Business function is required")]
        [Display(Name = "Business Function")]
        public string BusinessFunctionSupported { get; set; }

        [Display(Name = "Technology Stack")]
        public List<string> TechnologyStack { get; set; } = new List<string>();

        [Display(Name = "Integration Points")]
        public List<string> IntegrationPoints { get; set; } = new List<string>();

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Version is required")]
        [RegularExpression(@"^\d+(\.\d+){1,3}$", ErrorMessage = "Invalid version format")]
        public string Version { get; set; }

        [Required(ErrorMessage = "Deployment environment is required")]
        [Display(Name = "Deployment Environment")]
        public string DeploymentEnvironment { get; set; }
        
        // Application Rationalization properties
        [Display(Name = "Rationalization Category")]
        public string RationalizationCategory { get; set; } // Tolerate, Invest, Migrate, Eliminate/Retire
        
        [Display(Name = "Rationalization Rationale")]
        public string RationalizationRationale { get; set; }
        
        [Display(Name = "Business Value")]
        [Range(1, 10, ErrorMessage = "Business value must be between 1 and 10")]
        public int? BusinessValue { get; set; } // 1-10
        
        [Display(Name = "Technical Fit")]
        [Range(1, 10, ErrorMessage = "Technical fit must be between 1 and 10")]
        public int? TechnicalFit { get; set; } // 1-10
        
        [Display(Name = "Annual Cost")]
        [DataType(DataType.Currency)]
        public decimal? AnnualCost { get; set; }
        
        [Display(Name = "Risk Score")]
        [Range(0, 10, ErrorMessage = "Risk score must be between 0 and 10")]
        public decimal? RiskScore { get; set; } // 1-10
        
        [Display(Name = "Current State")]
        public string CurrentState { get; set; }
        
        [Display(Name = "Future State")]
        public string FutureState { get; set; }
        
        [Display(Name = "Redundant With")]
        public List<string> RedundantWith { get; set; } = new List<string>();
        
        [Display(Name = "Redundant Applications")]
        public List<string> RedundantApplications { get; set; } = new List<string>();
        
        [Display(Name = "Replacement Options")]
        public List<string> ReplacementOptions { get; set; } = new List<string>();
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Assessment Date")]
        public DateTime? AssessmentDate { get; set; }
        
        [Display(Name = "Assessed By")]
        public string AssessedBy { get; set; }
        
        // Application Rationalization Recommendations
        [Display(Name = "Rationalization Recommendations")]
        public List<ApplicationRecommendation> Recommendations { get; set; } = new List<ApplicationRecommendation>();

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;

        public string Vendor { get; set; }
        
        public string ApplicationId => Id.ToString();
    }

    public class ApplicationRecommendation
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ApplicationId { get; set; }
        public string RecommendationType { get; set; } // Consolidate, Retire, Replace, Invest, etc.
        public string Description { get; set; }
        public string Business { get; set; }
        public decimal EstimatedCostSavings { get; set; }
        public decimal EstimatedImplementationCost { get; set; }
        public string Timeline { get; set; }
        public List<string> Dependencies { get; set; }
        public string Status { get; set; } // Pending, Approved, In Progress, Completed
    }

    public class ApplicationBusinessRequirementsView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Requirement ID is required")]
        [StringLength(50, ErrorMessage = "ID cannot exceed 50 characters")]
        [Display(Name = "Requirement ID")]
        public string RequirementId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string RequirementDescription { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        [Display(Name = "Priority")]
        public string Priority { get; set; }

        [Display(Name = "Stakeholders")]
        public List<string> Stakeholders { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Assigned To")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string AssignedTo { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public List<string> Dependencies { get; set; }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class ServiceCatalogue
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Service name is required")]
        [StringLength(100, ErrorMessage = "Service name cannot exceed 100 characters")]
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string ServiceDescription { get; set; }

        [Required(ErrorMessage = "Service owner is required")]
        [Display(Name = "Service Owner")]
        public string ServiceOwner { get; set; }

        [Required(ErrorMessage = "SLA is required")]
        [Display(Name = "SLA")]
        public string SLA { get; set; }

        [Required(ErrorMessage = "Cost is required")]
        [Range(0, 999999.99, ErrorMessage = "Cost must be between 0 and 999,999.99")]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        [Display(Name = "Supported Applications")]
        public List<string> SupportedApplications { get; set; }

        [Required(ErrorMessage = "Availability is required")]
        public string Availability { get; set; }

        public List<string> Dependencies { get; set; }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class ApplicationToApplicationCrossReference
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Source application is required")]
        [Display(Name = "Source Application")]
        public string SourceApplication { get; set; }

        [Required(ErrorMessage = "Target application is required")]
        [Display(Name = "Target Application")]
        public string TargetApplication { get; set; }

        [Required(ErrorMessage = "Integration type is required")]
        [Display(Name = "Integration Type")]
        public string IntegrationType { get; set; }

        [Required(ErrorMessage = "Data exchanged is required")]
        [Display(Name = "Data Exchanged")]
        public string DataExchanged { get; set; }

        [Required(ErrorMessage = "Frequency is required")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        public string Owner { get; set; }

        [Display(Name = "Security Requirements")]
        public string SecurityRequirements { get; set; }

        public List<string> Dependencies { get; set; }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class ApplicationSecurityModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Application name is required")]
        [Display(Name = "Application Name")]
        public string ApplicationName { get; set; }

        [Required(ErrorMessage = "Security requirement is required")]
        [Display(Name = "Security Requirement")]
        public string SecurityRequirement { get; set; }

        [Display(Name = "Compliance Standards")]
        public List<string> ComplianceStandards { get; set; }

        [Display(Name = "Vulnerabilities")]
        public List<string> Vulnerabilities { get; set; }

        [Display(Name = "Mitigation Strategies")]
        public List<string> MitigationStrategies { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        public string Owner { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Last Audit Date")]
        public string LastAuditDate { get; set; }

        public List<string> Dependencies { get; set; }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class ApplicationDataCrossReference
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Application name is required")]
        [Display(Name = "Application Name")]
        public string ApplicationName { get; set; }

        [Required(ErrorMessage = "Data entity is required")]
        [Display(Name = "Data Entity")]
        public string DataEntity { get; set; }

        [Required(ErrorMessage = "Data usage is required")]
        [Display(Name = "Data Usage")]
        public string DataUsage { get; set; }

        [Required(ErrorMessage = "Data sensitivity is required")]
        [Display(Name = "Data Sensitivity")]
        public string DataSensitivity { get; set; }

        [Required(ErrorMessage = "Access controls are required")]
        [Display(Name = "Access Controls")]
        public string AccessControls { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Retention policy is required")]
        [Display(Name = "Data Retention Policy")]
        public string DataRetentionPolicy { get; set; }

        public List<string> Dependencies { get; set; }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }
}