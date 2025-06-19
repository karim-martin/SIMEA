using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMEA.Models
{
    public class InfrastructureBusinessRequirementsView
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

        [Required(ErrorMessage = "Business unit is required")]
        [StringLength(100, ErrorMessage = "Business unit cannot exceed 100 characters")]
        [Display(Name = "Business Unit")]
        public string BusinessUnit { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        [RegularExpression("^(High|Medium|Low)$", ErrorMessage = "Invalid priority value")]
        public string Priority { get; set; }

        [Required(ErrorMessage = "At least one stakeholder is required")]
        [Display(Name = "Stakeholders")]
        public List<string> Stakeholders { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Draft|Approved|Implemented)$", ErrorMessage = "Invalid status")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Assigned to is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Assigned To")]
        public string AssignedTo { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        
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

    public class SystemToApplicationCrossReference
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "System name is required")]
        [StringLength(100, ErrorMessage = "System name cannot exceed 100 characters")]
        [Display(Name = "System Name")]
        public string SystemName { get; set; }

        [Required(ErrorMessage = "Application name is required")]
        [StringLength(100, ErrorMessage = "Application name cannot exceed 100 characters")]
        [Display(Name = "Application Name")]
        public string ApplicationName { get; set; }

        [Required(ErrorMessage = "Integration type is required")]
        [StringLength(50, ErrorMessage = "Integration type cannot exceed 50 characters")]
        [Display(Name = "Integration Type")]
        public string IntegrationType { get; set; }

        [Required(ErrorMessage = "Data exchanged description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Data Exchanged")]
        public string DataExchanged { get; set; }

        [Required(ErrorMessage = "Frequency is required")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Security requirements are required")]
        [StringLength(500, ErrorMessage = "Requirements cannot exceed 500 characters")]
        [Display(Name = "Security Requirements")]
        public string SecurityRequirements { get; set; }

        [Display(Name = "Dependencies")]
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

    public class ResourceNeedsModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Resource type is required")]
        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters")]
        [Display(Name = "Resource Type")]
        public string ResourceType { get; set; }

        [Required(ErrorMessage = "Resource name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Resource Name")]
        public string ResourceName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 9999, ErrorMessage = "Quantity must be between 1 and 9999")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Cost is required")]
        [Range(0, 999999.99, ErrorMessage = "Cost must be between 0 and 999,999.99")]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        [Display(Name = "Dependencies")]
        public List<string> Dependencies { get; set; }

        [Required(ErrorMessage = "Allocation status is required")]
        [RegularExpression("^(Available|Allocated|In Use)$", ErrorMessage = "Invalid status")]
        [Display(Name = "Allocation Status")]
        public string AllocationStatus { get; set; }

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

    public class SystemToBusinessCrossReference
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "System name is required")]
        [StringLength(100, ErrorMessage = "System name cannot exceed 100 characters")]
        [Display(Name = "System Name")]
        public string SystemName { get; set; }

        [Required(ErrorMessage = "Business unit is required")]
        [StringLength(100, ErrorMessage = "Business unit cannot exceed 100 characters")]
        [Display(Name = "Business Unit")]
        public string BusinessUnit { get; set; }

        [Required(ErrorMessage = "Business process is required")]
        [StringLength(200, ErrorMessage = "Process name cannot exceed 200 characters")]
        [Display(Name = "Business Process Supported")]
        public string BusinessProcessSupported { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Criticality is required")]
        [RegularExpression("^(Critical|High|Medium|Low)$", ErrorMessage = "Invalid criticality")]
        public string Criticality { get; set; }

        [Required(ErrorMessage = "Dependencies are required")]
        [Display(Name = "Dependencies")]
        public List<string> Dependencies { get; set; }

        [Required(ErrorMessage = "Performance metrics are required")]
        [Display(Name = "Performance Metrics")]
        public List<string> PerformanceMetrics { get; set; }

        [Required(ErrorMessage = "Security requirements are required")]
        [StringLength(500, ErrorMessage = "Requirements cannot exceed 500 characters")]
        [Display(Name = "Security Requirements")]
        public string SecurityRequirements { get; set; }

        
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

    public class InfrastructureSecurityModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "System name is required")]
        [StringLength(100, ErrorMessage = "System name cannot exceed 100 characters")]
        [Display(Name = "System Name")]
        public string SystemName { get; set; }

        [Required(ErrorMessage = "Security requirement is required")]
        [StringLength(500, ErrorMessage = "Requirement cannot exceed 500 characters")]
        [Display(Name = "Security Requirement")]
        public string SecurityRequirement { get; set; }

        [Required(ErrorMessage = "Compliance standards are required")]
        [Display(Name = "Compliance Standards")]
        public List<string> ComplianceStandards { get; set; }

        [Required(ErrorMessage = "Vulnerabilities list is required")]
        [Display(Name = "Vulnerabilities")]
        public List<string> Vulnerabilities { get; set; }

        [Required(ErrorMessage = "Mitigation strategies are required")]
        [Display(Name = "Mitigation Strategies")]
        public List<string> MitigationStrategies { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Last audit date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Last Audit Date")]
        public DateTime LastAuditDate { get; set; }

        [Display(Name = "Dependencies")]
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

    public class SystemDataCrossReference
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "System name is required")]
        [StringLength(100, ErrorMessage = "System name cannot exceed 100 characters")]
        [Display(Name = "System Name")]
        public string SystemName { get; set; }

        [Required(ErrorMessage = "Data entity is required")]
        [StringLength(100, ErrorMessage = "Data entity cannot exceed 100 characters")]
        [Display(Name = "Data Entity")]
        public string DataEntity { get; set; }

        [Required(ErrorMessage = "Data usage is required")]
        [StringLength(200, ErrorMessage = "Usage description cannot exceed 200 characters")]
        [Display(Name = "Data Usage")]
        public string DataUsage { get; set; }

        [Required(ErrorMessage = "Data sensitivity is required")]
        [Display(Name = "Data Sensitivity")]
        public string DataSensitivity { get; set; }

        [Required(ErrorMessage = "Access controls are required")]
        [StringLength(500, ErrorMessage = "Access controls cannot exceed 500 characters")]
        [Display(Name = "Access Controls")]
        public string AccessControls { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Retention policy is required")]
        [StringLength(200, ErrorMessage = "Policy cannot exceed 200 characters")]
        [Display(Name = "Data Retention Policy")]
        public string DataRetentionPolicy { get; set; }

        [Required(ErrorMessage = "Dependencies are required")]
        [Display(Name = "Dependencies")]
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