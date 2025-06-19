using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMEA.Models
{
    public class DataGovernanceModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Data entity is required")]
        [StringLength(100, ErrorMessage = "Data entity cannot exceed 100 characters")]
        [Display(Name = "Data Entity")]
        public string DataEntity { get; set; }

        [Required(ErrorMessage = "Data owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        [Display(Name = "Data Owner")]
        public string DataOwner { get; set; }

        [Required(ErrorMessage = "Data steward is required")]
        [StringLength(100, ErrorMessage = "Steward name cannot exceed 100 characters")]
        [Display(Name = "Data Steward")]
        public string DataSteward { get; set; }

        [Required(ErrorMessage = "At least one quality metric is required")]
        [Display(Name = "Quality Metrics")]
        public List<string> DataQualityMetrics { get; set; }

        [Required(ErrorMessage = "Data policies are required")]
        [Display(Name = "Policies")]
        public List<string> DataPolicies { get; set; }

        [Required(ErrorMessage = "Compliance requirements are required")]
        [Display(Name = "Compliance Requirements")]
        public List<string> ComplianceRequirements { get; set; }

        [Required(ErrorMessage = "Data lifecycle is required")]
        [StringLength(500, ErrorMessage = "Lifecycle description cannot exceed 500 characters")]
        [Display(Name = "Data Lifecycle")]
        public string DataLifecycle { get; set; }

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

    public class InformationHierarchyView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Information level is required")]
        [StringLength(100, ErrorMessage = "Information level cannot exceed 100 characters")]
        [Display(Name = "Information Level")]
        public string InformationLevel { get; set; }

        [Display(Name = "Parent Information")]
        public string ParentInformation { get; set; }

        [Display(Name = "Child Information")]
        public List<string> ChildInformation { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Usage description is required")]
        [StringLength(200, ErrorMessage = "Usage cannot exceed 200 characters")]
        public string Usage { get; set; }

        [Display(Name = "Dependencies")]
        public List<string> Dependencies { get; set; }

        [Required(ErrorMessage = "Security level is required")]
        [Display(Name = "Security Level")]
        public string SecurityLevel { get; set; }

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

    public class InformationRequirementsView
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

        [Required(ErrorMessage = "Data entity is required")]
        [StringLength(100, ErrorMessage = "Data entity cannot exceed 100 characters")]
        [Display(Name = "Data Entity")]
        public string DataEntity { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public string Priority { get; set; }

        [Required(ErrorMessage = "At least one stakeholder is required")]
        [Display(Name = "Stakeholders")]
        public List<string> Stakeholders { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Assigned to field is required")]
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

    public class DataFlowModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Data source is required")]
        [StringLength(100, ErrorMessage = "Source cannot exceed 100 characters")]
        [Display(Name = "Data Source")]
        public string DataSource { get; set; }

        [Required(ErrorMessage = "Data destination is required")]
        [StringLength(100, ErrorMessage = "Destination cannot exceed 100 characters")]
        [Display(Name = "Data Destination")]
        public string DataDestination { get; set; }

        [Required(ErrorMessage = "Flow description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Flow Description")]
        public string DataFlowDescription { get; set; }

        [Required(ErrorMessage = "Data type is required")]
        [Display(Name = "Data Type")]
        public string DataTypeName { get; set; }

        [Required(ErrorMessage = "Frequency is required")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Security requirements are required")]
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

    public class LogicalDataModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Entity name is required")]
        [StringLength(100, ErrorMessage = "Entity name cannot exceed 100 characters")]
        [Display(Name = "Entity Name")]
        public string EntityName { get; set; }

        [Required(ErrorMessage = "Attributes are required")]
        [Display(Name = "Attributes")]
        public List<string> Attributes { get; set; }

        [Required(ErrorMessage = "Relationships are required")]
        [Display(Name = "Relationships")]
        public List<string> Relationships { get; set; }

        [Required(ErrorMessage = "Primary key is required")]
        [Display(Name = "Primary Key")]
        public string PrimaryKey { get; set; }

        [Display(Name = "Foreign Key")]
        public string ForeignKey { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

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

    public class DataSecurityModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Data entity is required")]
        [StringLength(100, ErrorMessage = "Data entity cannot exceed 100 characters")]
        [Display(Name = "Data Entity")]
        public string DataEntity { get; set; }

        [Required(ErrorMessage = "Security requirement is required")]
        [StringLength(500, ErrorMessage = "Requirement cannot exceed 500 characters")]
        [Display(Name = "Security Requirement")]
        public string SecurityRequirement { get; set; }

        [Required(ErrorMessage = "Access controls are required")]
        [Display(Name = "Access Controls")]
        public string AccessControls { get; set; }

        [Required(ErrorMessage = "Encryption requirements are required")]
        [Display(Name = "Encryption Requirements")]
        public string EncryptionRequirements { get; set; }

        [Required(ErrorMessage = "Compliance standards are required")]
        [Display(Name = "Compliance Standards")]
        public List<string> ComplianceStandards { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Last audit date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Last Audit Date")]
        public string LastAuditDate { get; set; }

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