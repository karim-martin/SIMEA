using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMEA.Models
{
    public class BusinessStrategyView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Business goal is required")]
        [StringLength(200, ErrorMessage = "Business goal cannot exceed 200 characters")]
        [Display(Name = "Business Goal")]
        public string BusinessGoal { get; set; }

        [Required(ErrorMessage = "Strategic objective is required")]
        [StringLength(500, ErrorMessage = "Strategic objective cannot exceed 500 characters")]
        [Display(Name = "Strategic Objective")]
        public string StrategicObjective { get; set; }

        [Required(ErrorMessage = "At least one KPI is required")]
        [Display(Name = "Key Performance Indicators")]
        public List<string> KeyPerformanceIndicators { get; set; }

        [Required(ErrorMessage = "Stakeholders list is required")]
        [Display(Name = "Stakeholders")]
        public List<string> Stakeholders { get; set; }

        [Required(ErrorMessage = "Timeframe is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Timeframe")]
        public string Timeframe { get; set; }

        [Required(ErrorMessage = "Vision alignment description is required")]
        [StringLength(500, ErrorMessage = "Vision alignment cannot exceed 500 characters")]
        [Display(Name = "Vision Alignment")]
        public string OrganizationalVisionAlignment { get; set; }

        [Display(Name = "Risks")]
        public List<string> Risks { get; set; }

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

    public class OperationModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Process name is required")]
        [StringLength(100, ErrorMessage = "Process name cannot exceed 100 characters")]
        [Display(Name = "Process Name")]
        public string OperationalProcess { get; set; }

        [Required(ErrorMessage = "Process owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        [Display(Name = "Process Owner")]
        public string ProcessOwner { get; set; }

        [Required(ErrorMessage = "Inputs list is required")]
        [Display(Name = "Inputs")]
        public List<string> Inputs { get; set; }

        [Required(ErrorMessage = "Outputs list is required")]
        [Display(Name = "Outputs")]
        public List<string> Outputs { get; set; }

        [Required(ErrorMessage = "Key activities are required")]
        [Display(Name = "Key Activities")]
        public List<string> KeyActivities { get; set; }

        [Required(ErrorMessage = "Required resources list is needed")]
        [Display(Name = "Required Resources")]
        public List<string> ResourcesRequired { get; set; }

        [Required(ErrorMessage = "Performance metrics are required")]
        [Display(Name = "Performance Metrics")]
        public List<string> PerformanceMetrics { get; set; }

        [Display(Name = "Challenges")]
        public List<string> Challenges { get; set; }

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

    public class OrganizationView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department/Unit name is required")]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters")]
        [Display(Name = "Department/Unit")]
        public string DepartmentOrUnit { get; set; }

        [Required(ErrorMessage = "Roles & responsibilities are required")]
        [StringLength(500, ErrorMessage = "Roles description cannot exceed 500 characters")]
        [Display(Name = "Roles & Responsibilities")]
        public string RolesAndResponsibilities { get; set; }

        [Required(ErrorMessage = "Reporting structure is required")]
        [StringLength(500, ErrorMessage = "Reporting structure cannot exceed 500 characters")]
        [Display(Name = "Reporting Structure")]
        public string ReportingStructure { get; set; }

        [Required(ErrorMessage = "Key personnel list is required")]
        [Display(Name = "Key Personnel")]
        public List<string> KeyPersonnel { get; set; }

        [Required(ErrorMessage = "Collaboration points are required")]
        [Display(Name = "Collaboration Points")]
        public List<string> CollaborationPoints { get; set; }

        [Required(ErrorMessage = "Organizational goals are required")]
        [Display(Name = "Organizational Goals")]
        public List<string> OrganizationalGoals { get; set; }

        [Display(Name = "Challenges")]
        public List<string> Challenges { get; set; }

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

    public class CapabilityMap
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Capability name is required")]
        [StringLength(100, ErrorMessage = "Capability name cannot exceed 100 characters")]
        [Display(Name = "Capability Name")]
        public string CapabilityName { get; set; }

        [Required(ErrorMessage = "Capability description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string CapabilityDescription { get; set; }

        [Required(ErrorMessage = "Maturity level is required")]
        [Display(Name = "Maturity Level")]
        public string MaturityLevel { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Display(Name = "Dependencies")]
        public List<string> Dependencies { get; set; }

        [Required(ErrorMessage = "Key processes are required")]
        [Display(Name = "Key Processes")]
        public List<string> KeyProcesses { get; set; }

        [Display(Name = "Gaps")]
        public List<string> Gaps { get; set; }

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

    public class ProcessModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Process name is required")]
        [StringLength(100, ErrorMessage = "Process name cannot exceed 100 characters")]
        [Display(Name = "Process Name")]
        public string ProcessName { get; set; }

        [Required(ErrorMessage = "Process description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string ProcessDescription { get; set; }

        [Required(ErrorMessage = "Inputs list is required")]
        [Display(Name = "Inputs")]
        public List<string> Inputs { get; set; }

        [Required(ErrorMessage = "Outputs list is required")]
        [Display(Name = "Outputs")]
        public List<string> Outputs { get; set; }

        [Required(ErrorMessage = "Workflow steps are required")]
        [Display(Name = "Steps/Workflow")]
        public List<string> StepsWorkflow { get; set; }

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Tools/Systems list is required")]
        [Display(Name = "Tools/Systems Used")]
        public List<string> ToolsSystemsUsed { get; set; }

        [Required(ErrorMessage = "Performance metrics are required")]
        [Display(Name = "Performance Metrics")]
        public List<string> PerformanceMetrics { get; set; }

        // Value Stream properties (added from ValueStream model)
        [Display(Name = "Customer Type")]
        public string CustomerType { get; set; }

        [Display(Name = "Value Stream Stages")]
        public List<ProcessStage> Stages { get; set; }

        [Display(Name = "Key Metrics")]
        public List<string> KeyMetrics { get; set; }

        [Display(Name = "Cycle Efficiency")]
        public decimal? CycleEfficiency { get; set; } // Value-add time / total lead time

        [Display(Name = "Lead Time")]
        public TimeSpan? LeadTime { get; set; }

        [Display(Name = "Bottlenecks")]
        public List<string> Bottlenecks { get; set; }

        [Display(Name = "Improvement Opportunities")]
        public List<string> ImprovementOpportunities { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Mapping Date")]
        public DateTime? MappingDate { get; set; }

        // Stakeholder value properties
        [Display(Name = "Stakeholder Values")]
        public List<StakeholderValueItem> StakeholderValues { get; set; }

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

    public class ProcessStage
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan ProcessTime { get; set; }
        public TimeSpan WaitTime { get; set; }
        public List<string> ProcessIds { get; set; } // IDs of related processes
        public List<string> ApplicationIds { get; set; } // IDs of supporting applications
        public bool IsValueAdd { get; set; }
        public decimal PercentComplete { get; set; }
        public decimal DefectRate { get; set; }
        public List<string> Issues { get; set; }
    }

    public class StakeholderValueItem
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StakeholderType { get; set; }
        public string ValueDescription { get; set; }
        public int Priority { get; set; } // 1-10
        public string ProcessId { get; set; }
        public string MetricOfSuccess { get; set; }
    }

    public class BusinessProductServiceView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product/Service name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Product/Service Name")]
        public string ProductServiceName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Target market is required")]
        [StringLength(200, ErrorMessage = "Target market cannot exceed 200 characters")]
        [Display(Name = "Target Market/Customer")]
        public string TargetMarketCustomer { get; set; }

        [Required(ErrorMessage = "Value proposition is required")]
        [StringLength(500, ErrorMessage = "Value proposition cannot exceed 500 characters")]
        [Display(Name = "Value Proposition")]
        public string ValueProposition { get; set; }

        [Required(ErrorMessage = "Revenue model is required")]
        [StringLength(200, ErrorMessage = "Revenue model cannot exceed 200 characters")]
        [Display(Name = "Revenue Model")]
        public string RevenueModel { get; set; }

        [Required(ErrorMessage = "Key features are required")]
        [Display(Name = "Key Features")]
        public List<string> KeyFeatures { get; set; }

        [Display(Name = "Dependencies")]
        public List<string> Dependencies { get; set; }

        [Required(ErrorMessage = "Competitors list is required")]
        [Display(Name = "Competitors")]
        public List<string> Competitors { get; set; }

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

    public class ObjectiveKeyResultModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Objective is required")]
        [StringLength(200, ErrorMessage = "Objective cannot exceed 200 characters")]
        [Display(Name = "Objective")]
        public string Objective { get; set; }

        [Display(Name = "Key Results")]
        public List<KeyResult> KeyResults { get; set; } = new List<KeyResult>();

        [Required(ErrorMessage = "Owner is required")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Quarter is required")]
        [Display(Name = "Quarter")]
        public string Quarter { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Overall Progress")]
        [Range(0, 100, ErrorMessage = "Progress must be between 0 and 100")]
        public decimal OverallProgress { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } // On Track, At Risk, Behind, Completed

        [Display(Name = "Business Strategy Link")]
        public int? BusinessStrategyId { get; set; }

        [Display(Name = "Related Capabilities")]
        public List<string> RelatedCapabilities { get; set; } = new List<string>();

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class KeyResult
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal Target { get; set; }
        
        public decimal Current { get; set; }
        
        public string Unit { get; set; }
        
        public decimal Progress => Target != 0 ? (Current / Target) * 100 : 0;
        
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        
        public string Status { get; set; } // On Track, At Risk, Behind, Completed
    }

    public class BusinessOutcomeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Outcome name is required")]
        [StringLength(200, ErrorMessage = "Outcome name cannot exceed 200 characters")]
        [Display(Name = "Business Outcome")]
        public string OutcomeName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [Display(Name = "Success Metrics")]
        public List<SuccessMetric> SuccessMetrics { get; set; } = new List<SuccessMetric>();

        [Display(Name = "Contributing OKRs")]
        public List<string> ContributingOKRs { get; set; } = new List<string>();

        [Display(Name = "Expected Value")]
        [DataType(DataType.Currency)]
        public decimal? ExpectedValue { get; set; }

        [Display(Name = "Realization Timeline")]
        public string RealizationTimeline { get; set; }

        [Display(Name = "Risk Factors")]
        public List<string> RiskFactors { get; set; } = new List<string>();

        [Display(Name = "Stakeholder Benefits")]
        public List<StakeholderBenefit> StakeholderBenefits { get; set; } = new List<StakeholderBenefit>();

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class SuccessMetric
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MetricName { get; set; }
        public string Description { get; set; }
        public decimal BaselineValue { get; set; }
        public decimal TargetValue { get; set; }
        public decimal CurrentValue { get; set; }
        public string Unit { get; set; }
        public string MeasurementFrequency { get; set; }
        public DateTime LastMeasured { get; set; }
    }

    public class StakeholderBenefit
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StakeholderGroup { get; set; }
        public string BenefitDescription { get; set; }
        public string BenefitType { get; set; } // Financial, Operational, Strategic, etc.
        public decimal? QuantifiedValue { get; set; }
        public string ValueUnit { get; set; }
    }

    public class FinancialModelingView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Model name is required")]
        [StringLength(200, ErrorMessage = "Model name cannot exceed 200 characters")]
        [Display(Name = "Financial Model Name")]
        public string ModelName { get; set; }

        [Required(ErrorMessage = "Model type is required")]
        [Display(Name = "Model Type")]
        public string ModelType { get; set; } // ROI, NPV, TCO, Business Case, etc.

        [Display(Name = "Investment Category")]
        public string InvestmentCategory { get; set; } // Capability Enhancement, New Technology, Process Improvement, etc.

        [Required(ErrorMessage = "Total investment is required")]
        [Display(Name = "Total Investment")]
        [DataType(DataType.Currency)]
        public decimal TotalInvestment { get; set; }

        [Display(Name = "Annual Operating Cost")]
        [DataType(DataType.Currency)]
        public decimal AnnualOperatingCost { get; set; }

        [Display(Name = "Expected Annual Benefits")]
        [DataType(DataType.Currency)]
        public decimal ExpectedAnnualBenefits { get; set; }

        [Display(Name = "Payback Period (Months)")]
        [Range(0, 240, ErrorMessage = "Payback period must be between 0 and 240 months")]
        public int PaybackPeriodMonths { get; set; }

        [Display(Name = "ROI Percentage")]
        [Range(-100, 1000, ErrorMessage = "ROI must be between -100% and 1000%")]
        public decimal ROIPercentage { get; set; }

        [Display(Name = "NPV")]
        [DataType(DataType.Currency)]
        public decimal NetPresentValue { get; set; }

        [Display(Name = "Discount Rate")]
        [Range(0, 50, ErrorMessage = "Discount rate must be between 0% and 50%")]
        public decimal DiscountRate { get; set; }

        [Display(Name = "Analysis Period (Years)")]
        [Range(1, 10, ErrorMessage = "Analysis period must be between 1 and 10 years")]
        public int AnalysisPeriodYears { get; set; }

        [Display(Name = "Cost Breakdown")]
        public List<CostItem> CostBreakdown { get; set; } = new List<CostItem>();

        [Display(Name = "Benefit Breakdown")]
        public List<BenefitItem> BenefitBreakdown { get; set; } = new List<BenefitItem>();

        [Display(Name = "Risk Factors")]
        public List<FinancialRiskFactor> RiskFactors { get; set; } = new List<FinancialRiskFactor>();

        [Display(Name = "Assumptions")]
        public List<string> Assumptions { get; set; } = new List<string>();

        [Display(Name = "Sensitivity Analysis")]
        public string SensitivityAnalysis { get; set; }

        [Display(Name = "Recommendation")]
        public string Recommendation { get; set; } // Approve, Reject, Modify, Further Analysis

        [Display(Name = "Decision Status")]
        public string DecisionStatus { get; set; } // Pending, Approved, Rejected, Deferred

        [Display(Name = "Decision Date")]
        [DataType(DataType.Date)]
        public DateTime? DecisionDate { get; set; }

        [Display(Name = "Decision Maker")]
        public string DecisionMaker { get; set; }

        [Display(Name = "Related Business Strategy")]
        public int? BusinessStrategyId { get; set; }

        [Display(Name = "Related OKRs")]
        public List<string> RelatedOKRIds { get; set; } = new List<string>();

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class CostItem
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Frequency { get; set; } // One-time, Monthly, Annual
        public int OccursInYear { get; set; }
    }

    public class BenefitItem
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Frequency { get; set; } // One-time, Monthly, Annual
        public int StartsInYear { get; set; }
        public decimal Confidence { get; set; } // 0-100%
    }

    public class FinancialRiskFactor
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string RiskDescription { get; set; }
        public string Impact { get; set; } // High, Medium, Low
        public string Likelihood { get; set; } // High, Medium, Low
        public string MitigationStrategy { get; set; }
        public decimal PotentialImpactAmount { get; set; }
    }

    public class EnhancedValueStreamModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Value stream name is required")]
        [StringLength(200, ErrorMessage = "Value stream name cannot exceed 200 characters")]
        [Display(Name = "Value Stream Name")]
        public string ValueStreamName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [Display(Name = "Customer Segments")]
        public List<string> CustomerSegments { get; set; } = new List<string>();

        [Display(Name = "Value Proposition")]
        public string ValueProposition { get; set; }

        [Display(Name = "Value Stream Stages")]
        public List<ValueStreamStage> Stages { get; set; } = new List<ValueStreamStage>();

        [Display(Name = "End-to-End Lead Time (Days)")]
        public decimal EndToEndLeadTime { get; set; }

        [Display(Name = "Total Process Time (Days)")]
        public decimal TotalProcessTime { get; set; }

        [Display(Name = "Value-Added Time (Days)")]
        public decimal ValueAddedTime { get; set; }

        [Display(Name = "Flow Efficiency")]
        public decimal FlowEfficiency => TotalProcessTime > 0 ? (ValueAddedTime / TotalProcessTime) * 100 : 0;

        [Display(Name = "Customer Value Metrics")]
        public List<ValueMetric> CustomerValueMetrics { get; set; } = new List<ValueMetric>();

        [Display(Name = "Business Value Metrics")]
        public List<ValueMetric> BusinessValueMetrics { get; set; } = new List<ValueMetric>();

        [Display(Name = "Supporting Capabilities")]
        public List<string> SupportingCapabilities { get; set; } = new List<string>();

        [Display(Name = "Supporting Applications")]
        public List<string> SupportingApplications { get; set; } = new List<string>();

        [Display(Name = "Data Requirements")]
        public List<string> DataRequirements { get; set; } = new List<string>();

        [Display(Name = "Pain Points")]
        public List<PainPoint> PainPoints { get; set; } = new List<PainPoint>();

        [Display(Name = "Improvement Opportunities")]
        public List<ImprovementOpportunity> ImprovementOpportunities { get; set; } = new List<ImprovementOpportunity>();

        [Display(Name = "Related OKRs")]
        public List<string> RelatedOKRIds { get; set; } = new List<string>();

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class ValueStreamStage
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StageName { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
        public decimal ProcessTime { get; set; }
        public decimal WaitTime { get; set; }
        public bool IsValueAdding { get; set; }
        public List<string> Activities { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> SupportingApplications { get; set; } = new List<string>();
        public List<string> DataInputs { get; set; } = new List<string>();
        public List<string> DataOutputs { get; set; } = new List<string>();
        public decimal QualityGate { get; set; } // % of items that pass without rework
    }

    public class ValueMetric
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MetricName { get; set; }
        public string Description { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal TargetValue { get; set; }
        public string Unit { get; set; }
        public string MeasurementFrequency { get; set; }
        public DateTime LastMeasured { get; set; }
        public string Trend { get; set; } // Improving, Stable, Declining
    }

    public class PainPoint
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; }
        public string Impact { get; set; } // High, Medium, Low
        public string Frequency { get; set; } // Daily, Weekly, Monthly, Occasionally
        public string AffectedStage { get; set; }
        public List<string> RootCauses { get; set; } = new List<string>();
    }

    public class ImprovementOpportunity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string OpportunityName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; } // Process, Technology, Organization, etc.
        public decimal EstimatedImpact { get; set; }
        public string ImpactUnit { get; set; }
        public decimal EstimatedEffort { get; set; }
        public string EffortUnit { get; set; }
        public string Priority { get; set; } // High, Medium, Low
        public string Status { get; set; } // Identified, Planned, In Progress, Completed
        public DateTime? TargetDate { get; set; }
        public string Owner { get; set; }
    }

    // Technology Trend Models
    public class TechnologyTrendModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Trend name is required")]
        [StringLength(200, ErrorMessage = "Trend name cannot exceed 200 characters")]
        [Display(Name = "Technology Trend")]
        public string TrendName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [Display(Name = "Technology Category")]
        public string TechnologyCategory { get; set; } // AI/ML, Cloud, IoT, Blockchain, etc.

        // Alias for view compatibility
        [Display(Name = "Trend Category")]
        public string TrendCategory => TechnologyCategory;

        [Display(Name = "Industry Sectors")]
        public List<string> IndustrySectors { get; set; } = new List<string>();

        [Display(Name = "Maturity Level")]
        public string MaturityLevel { get; set; } // Emerging, Growth, Mature, Declining

        [Display(Name = "Adoption Timeline")]
        public string AdoptionTimeline { get; set; } // 0-1 years, 1-3 years, 3-5 years, 5+ years

        [Display(Name = "Strategic Relevance")]
        [Range(1, 10, ErrorMessage = "Strategic relevance must be between 1 and 10")]
        public int StrategicRelevance { get; set; } // 1-10

        [Display(Name = "Disruption Potential")]
        [Range(1, 10, ErrorMessage = "Disruption potential must be between 1 and 10")]
        public int DisruptionPotential { get; set; } // 1-10

        [Display(Name = "Investment Required")]
        [Range(1, 10, ErrorMessage = "Investment required must be between 1 and 10")]
        public int InvestmentRequired { get; set; } // 1-10

        [Display(Name = "Risk Level")]
        [Range(1, 10, ErrorMessage = "Risk level must be between 1 and 10")]
        public int RiskLevel { get; set; } // 1-10

        // Additional properties for view compatibility
        [Display(Name = "Technical Requirements")]
        public string TechnicalRequirements { get; set; }

        [Display(Name = "Risk Assessment")]
        public string RiskAssessment { get; set; }

        [Display(Name = "Recommended Actions")]
        public string RecommendedActions { get; set; }

        [Display(Name = "Opportunities")]
        public List<TrendOpportunity> Opportunities { get; set; } = new List<TrendOpportunity>();

        [Display(Name = "Threats/Risks")]
        public List<TrendRisk> Threats { get; set; } = new List<TrendRisk>();

        [Display(Name = "Key Vendors/Players")]
        public List<string> KeyVendors { get; set; } = new List<string>();

        [Display(Name = "Use Cases")]
        public List<TechnologyUseCase> UseCases { get; set; } = new List<TechnologyUseCase>();

        [Display(Name = "Prerequisites")]
        public List<string> Prerequisites { get; set; } = new List<string>();

        [Display(Name = "Assessment Status")]
        public string AssessmentStatus { get; set; } // Identified, Under Review, Assessed, Approved for Exploration

        [Display(Name = "Next Review Date")]
        [DataType(DataType.Date)]
        public DateTime? NextReviewDate { get; set; }

        [Display(Name = "Related Business Capabilities")]
        public List<string> RelatedCapabilities { get; set; } = new List<string>();

        [Display(Name = "Business Impact")]
        public string BusinessImpact { get; set; } // High, Medium, Low

        [Display(Name = "Expected Business Value")]
        [DataType(DataType.Currency)]
        public decimal? ExpectedBusinessValue { get; set; }

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class TrendOpportunity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; }
        public string Impact { get; set; } // High, Medium, Low
        public decimal PotentialValue { get; set; }
        public string ValueUnit { get; set; }
        public string Timeframe { get; set; }
    }

    public class TrendRisk
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; }
        public string Impact { get; set; } // High, Medium, Low
        public string Likelihood { get; set; } // High, Medium, Low
        public string MitigationStrategy { get; set; }
    }

    public class TechnologyUseCase
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Description { get; set; }
        public string BusinessValue { get; set; }
        public string FeasibilityLevel { get; set; } // High, Medium, Low
        public string ImplementationComplexity { get; set; } // High, Medium, Low
    }

    // Enhanced Investment Decision Models
    public class InvestmentDecisionModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Investment name is required")]
        [StringLength(200, ErrorMessage = "Investment name cannot exceed 200 characters")]
        [Display(Name = "Investment Name")]
        public string InvestmentName { get; set; }

        [Display(Name = "Description")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [Display(Name = "Investment Type")]
        public string InvestmentType { get; set; } // Technology, Process, People, Infrastructure, Innovation

        [Display(Name = "Investment Category")]
        public string InvestmentCategory { get; set; } // Strategic, Operational, Compliance, Innovation

        [Display(Name = "Business Driver")]
        public string BusinessDriver { get; set; }

        [Display(Name = "Strategic Alignment")]
        public List<string> StrategicAlignment { get; set; } = new List<string>();

        [Display(Name = "Total Investment")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Total investment is required")]
        public decimal TotalInvestment { get; set; }

        [Display(Name = "Investment Breakdown")]
        public List<InvestmentBreakdownItem> InvestmentBreakdown { get; set; } = new List<InvestmentBreakdownItem>();

        [Display(Name = "Expected Benefits")]
        [DataType(DataType.Currency)]
        public decimal ExpectedBenefits { get; set; }

        [Display(Name = "Benefit Categories")]
        public List<BenefitCategory> BenefitCategories { get; set; } = new List<BenefitCategory>();

        [Display(Name = "Payback Period (Months)")]
        [Range(0, 120, ErrorMessage = "Payback period must be between 0 and 120 months")]
        public int PaybackPeriodMonths { get; set; }

        [Display(Name = "ROI Percentage")]
        [Range(0, 1000, ErrorMessage = "ROI must be between 0 and 1000 percent")]
        public decimal ROIPercentage { get; set; }

        [Display(Name = "NPV")]
        [DataType(DataType.Currency)]
        public decimal NPV { get; set; }

        [Display(Name = "IRR Percentage")]
        public decimal IRRPercentage { get; set; }

        [Display(Name = "Risk Assessment")]
        public InvestmentRiskAssessment RiskAssessment { get; set; } = new InvestmentRiskAssessment();

        [Display(Name = "Decision Criteria")]
        public List<string> DecisionCriteria { get; set; } = new List<string>();

        [Display(Name = "Alternative Options")]
        public List<string> AlternativeOptions { get; set; } = new List<string>();

        [Display(Name = "Implementation Timeline")]
        public string ImplementationTimeline { get; set; }

        [Display(Name = "Resource Requirements")]
        public InvestmentResourceRequirements ResourceRequirements { get; set; } = new InvestmentResourceRequirements();

        [Display(Name = "Success Metrics")]
        public List<string> SuccessMetrics { get; set; } = new List<string>();

        [Display(Name = "Decision Status")]
        public string DecisionStatus { get; set; } // Under Review, Approved, Rejected, Deferred, Implemented

        [Display(Name = "Decision Date")]
        [DataType(DataType.Date)]
        public DateTime? DecisionDate { get; set; }

        [Display(Name = "Decision Maker")]
        public string DecisionMaker { get; set; }

        [Display(Name = "Decision Rationale")]
        public string DecisionRationale { get; set; }

        [Display(Name = "Priority Score")]
        [Range(1, 10, ErrorMessage = "Priority score must be between 1 and 10")]
        public int PriorityScore { get; set; }

        [Display(Name = "Related Business Outcomes")]
        public List<string> RelatedBusinessOutcomes { get; set; } = new List<string>();

        [Display(Name = "Related Technology Trends")]
        public List<string> RelatedTechnologyTrends { get; set; } = new List<string>();

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class InvestmentBreakdownItem
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Category { get; set; } // Technology, People, Process, Infrastructure
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public string Timeline { get; set; }
        public string Justification { get; set; }
    }

    public class BenefitCategory
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Category { get; set; } // Cost Savings, Revenue Increase, Risk Mitigation, Efficiency Gains
        public string Description { get; set; }
        public decimal QuantifiedValue { get; set; }
        public string ValueUnit { get; set; }
        public string RealizationTimeframe { get; set; }
        public decimal Confidence { get; set; } // 0-100%
    }

    public class InvestmentRiskAssessment
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Display(Name = "Overall Risk Level")]
        public string OverallRiskLevel { get; set; } // Low, Medium, High

        [Display(Name = "Technical Risk")]
        [Range(1, 10, ErrorMessage = "Technical risk must be between 1 and 10")]
        public int TechnicalRisk { get; set; }

        [Display(Name = "Financial Risk")]
        [Range(1, 10, ErrorMessage = "Financial risk must be between 1 and 10")]
        public int FinancialRisk { get; set; }

        [Display(Name = "Market Risk")]
        [Range(1, 10, ErrorMessage = "Market risk must be between 1 and 10")]
        public int MarketRisk { get; set; }

        [Display(Name = "Operational Risk")]
        [Range(1, 10, ErrorMessage = "Operational risk must be between 1 and 10")]
        public int OperationalRisk { get; set; }

        [Display(Name = "Risk Factors")]
        public List<InvestmentRiskFactor> RiskFactors { get; set; } = new List<InvestmentRiskFactor>();

        [Display(Name = "Mitigation Strategies")]
        public List<string> MitigationStrategies { get; set; } = new List<string>();
    }

    public class InvestmentRiskFactor
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string RiskDescription { get; set; }
        public string RiskCategory { get; set; } // Technical, Financial, Market, Operational
        public string Impact { get; set; } // High, Medium, Low
        public string Probability { get; set; } // High, Medium, Low
        public string MitigationStrategy { get; set; }
        public string Owner { get; set; }
    }

    public class InvestmentResourceRequirements
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Display(Name = "Budget Required")]
        [DataType(DataType.Currency)]
        public decimal BudgetRequired { get; set; }

        [Display(Name = "FTE Required")]
        public int FTERequired { get; set; }

        [Display(Name = "Skills Required")]
        public List<string> SkillsRequired { get; set; } = new List<string>();

        [Display(Name = "Technology Requirements")]
        public List<string> TechnologyRequirements { get; set; } = new List<string>();

        [Display(Name = "Infrastructure Requirements")]
        public List<string> InfrastructureRequirements { get; set; } = new List<string>();

        [Display(Name = "External Dependencies")]
        public List<string> ExternalDependencies { get; set; } = new List<string>();
    }

    // Enhanced Business Outcome Models (extending existing BusinessOutcomeModel)
    public class EnhancedBusinessOutcomeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Outcome name is required")]
        [StringLength(200, ErrorMessage = "Outcome name cannot exceed 200 characters")]
        [Display(Name = "Business Outcome")]
        public string OutcomeName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [Display(Name = "Outcome Type")]
        public string OutcomeType { get; set; } // Financial, Operational, Strategic, Customer, Innovation

        [Display(Name = "Outcome Category")]
        public string OutcomeCategory { get; set; } // Revenue Growth, Cost Reduction, Risk Mitigation, Customer Satisfaction, etc.

        [Display(Name = "Business Priority")]
        public string BusinessPriority { get; set; } // Critical, High, Medium, Low

        // Additional properties for view compatibility
        [Display(Name = "Priority Level")]
        public string PriorityLevel => BusinessPriority; // Alias for BusinessPriority

        [Display(Name = "Strategic Alignment")]
        public string StrategicAlignment { get; set; }

        [Display(Name = "Target Value")]
        public decimal TargetValue { get; set; }

        [Display(Name = "Current Value")]
        public decimal CurrentValue { get; set; }

        [Display(Name = "Measurement Unit")]
        public string MeasurementUnit { get; set; }

        [Display(Name = "Completion Percentage")]
        [Range(0, 100, ErrorMessage = "Completion percentage must be between 0 and 100")]
        public decimal CompletionPercentage { get; set; }

        [Display(Name = "Status")]
        public string Status => CurrentStatus; // Alias for CurrentStatus

        [Display(Name = "Target Date")]
        [DataType(DataType.Date)]
        public DateTime? TargetDate { get; set; }

        [Display(Name = "Actual Date")]
        [DataType(DataType.Date)]
        public DateTime? ActualDate { get; set; }

        [Display(Name = "Business Impact")]
        public string BusinessImpact { get; set; }

        [Display(Name = "Owner Name")]
        public string OwnerName => Owner; // Alias for Owner

        [Display(Name = "Sponsor Name")]
        public string SponsorName => Sponsor; // Alias for Sponsor

        [Display(Name = "Review Notes")]
        public string ReviewNotes { get; set; }

        [Display(Name = "Strategic Objectives")]
        public List<string> StrategicObjectives { get; set; } = new List<string>();

        [Display(Name = "Success Metrics")]
        public List<EnhancedSuccessMetric> SuccessMetrics { get; set; } = new List<EnhancedSuccessMetric>();

        [Display(Name = "Contributing Investments")]
        public List<string> ContributingInvestments { get; set; } = new List<string>();

        [Display(Name = "Related Technology Trends")]
        public List<string> RelatedTechnologyTrends { get; set; } = new List<string>();

        [Display(Name = "Contributing OKRs")]
        public List<string> ContributingOKRs { get; set; } = new List<string>();

        [Display(Name = "Expected Value")]
        [DataType(DataType.Currency)]
        public decimal ExpectedValue { get; set; }

        [Display(Name = "Value Realization")]
        public BusinessValueRealization ValueRealization { get; set; } = new BusinessValueRealization();

        [Display(Name = "Impact Assessment")]
        public BusinessImpactDetails ImpactAssessment { get; set; } = new BusinessImpactDetails();

        [Display(Name = "Realization Timeline")]
        public string RealizationTimeline { get; set; }

        [Display(Name = "Milestone Timeline")]
        public List<OutcomeMilestone> Milestones { get; set; } = new List<OutcomeMilestone>();

        [Display(Name = "Risk Factors")]
        public List<BusinessOutcomeRisk> RiskFactors { get; set; } = new List<BusinessOutcomeRisk>();

        [Display(Name = "Stakeholder Benefits")]
        public List<EnhancedStakeholderBenefit> StakeholderBenefits { get; set; } = new List<EnhancedStakeholderBenefit>();

        [Display(Name = "Dependencies")]
        public List<string> Dependencies { get; set; } = new List<string>();

        [Display(Name = "Enablers")]
        public List<string> Enablers { get; set; } = new List<string>();

        [Display(Name = "Current Status")]
        public string CurrentStatus { get; set; } // Planned, In Progress, Achieved, At Risk, Delayed

        [Display(Name = "Progress Percentage")]
        [Range(0, 100, ErrorMessage = "Progress must be between 0 and 100")]
        public decimal ProgressPercentage { get; set; }

        [Display(Name = "Owner")]
        public string Owner { get; set; }

        [Display(Name = "Sponsor")]
        public string Sponsor { get; set; }

        [Display(Name = "Review Date")]
        [DataType(DataType.Date)]
        public DateTime? ReviewDate { get; set; }

        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class EnhancedSuccessMetric
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MetricName { get; set; }
        public string Description { get; set; }
        public string MetricType { get; set; } // Leading, Lagging
        public decimal BaselineValue { get; set; }
        public decimal TargetValue { get; set; }
        public decimal CurrentValue { get; set; }
        public string Unit { get; set; }
        public string MeasurementFrequency { get; set; }
        public DateTime LastMeasured { get; set; }
        public string DataSource { get; set; }
        public string Status { get; set; } // On Track, At Risk, Behind, Achieved
        public decimal ProgressPercentage { get; set; }
    }

    public class BusinessValueRealization
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Display(Name = "Planned Value")]
        [DataType(DataType.Currency)]
        public decimal PlannedValue { get; set; }

        [Display(Name = "Realized Value")]
        [DataType(DataType.Currency)]
        public decimal RealizedValue { get; set; }

        [Display(Name = "Value Realization %")]
        public decimal ValueRealizationPercentage => PlannedValue != 0 ? (RealizedValue / PlannedValue) * 100 : 0;

        [Display(Name = "Time to Value (Months)")]
        public int TimeToValueMonths { get; set; }

        [Display(Name = "Value Streams")]
        public List<string> ValueStreams { get; set; } = new List<string>();

        [Display(Name = "Value Drivers")]
        public List<string> ValueDrivers { get; set; } = new List<string>();
    }

    public class BusinessImpactDetails
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Display(Name = "Revenue Impact")]
        [DataType(DataType.Currency)]
        public decimal RevenueImpact { get; set; }

        [Display(Name = "Cost Impact")]
        [DataType(DataType.Currency)]
        public decimal CostImpact { get; set; }

        [Display(Name = "Efficiency Gains %")]
        public decimal EfficiencyGains { get; set; }

        [Display(Name = "Market Share Impact %")]
        public decimal MarketShareImpact { get; set; }

        [Display(Name = "Customer Satisfaction Impact")]
        [Range(-10, 10, ErrorMessage = "Impact must be between -10 and 10")]
        public decimal CustomerSatisfactionImpact { get; set; }

        [Display(Name = "Employee Satisfaction Impact")]
        [Range(-10, 10, ErrorMessage = "Impact must be between -10 and 10")]
        public decimal EmployeeSatisfactionImpact { get; set; }

        [Display(Name = "Risk Mitigation Value")]
        [DataType(DataType.Currency)]
        public decimal RiskMitigationValue { get; set; }

        [Display(Name = "Innovation Impact")]
        public string InnovationImpact { get; set; } // High, Medium, Low

        [Display(Name = "Competitive Advantage")]
        public string CompetitiveAdvantage { get; set; }
    }

    public class OutcomeMilestone
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MilestoneName { get; set; }
        public string Description { get; set; }
        public DateTime PlannedDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public string Status { get; set; } // Not Started, In Progress, Completed, Delayed
        public List<string> Deliverables { get; set; } = new List<string>();
        public decimal ValueContribution { get; set; }
    }

    public class BusinessOutcomeRisk
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string RiskDescription { get; set; }
        public string RiskCategory { get; set; } // Market, Technology, Operational, Financial
        public string Impact { get; set; } // High, Medium, Low
        public string Probability { get; set; } // High, Medium, Low
        public string MitigationStrategy { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; } // Open, Mitigated, Closed
        public decimal PotentialImpactValue { get; set; }
    }

    public class EnhancedStakeholderBenefit
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StakeholderGroup { get; set; }
        public string BenefitDescription { get; set; }
        public string BenefitType { get; set; } // Financial, Operational, Strategic, Quality of Life
        public decimal? QuantifiedValue { get; set; }
        public string ValueUnit { get; set; }
        public string RealizationTimeframe { get; set; }
        public decimal Confidence { get; set; } // 0-100%
        public string MeasurementMethod { get; set; }
    }
}