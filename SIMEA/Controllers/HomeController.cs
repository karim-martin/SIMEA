using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIMEA.Models;
using SIMEA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Linq.Expressions;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<IdentityExtendUser> _userManager;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityExtendUser> userManager, 
        IWebHostEnvironment env, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _env = env;
            _configuration = configuration;
        }

        [Authorize]
        public IActionResult ErrorPage()
        {
            return View();
        }

        public IActionResult Index()
        {
            _logger.LogInformation($"{User.Identity.Name} accessed home page");
            
            // Count EA artifacts by domain
            ViewBag.BusinessArtifactsCount = CountBusinessArtifacts();
            ViewBag.ApplicationArtifactsCount = CountApplicationArtifacts();
            ViewBag.DataArtifactsCount = CountDataArtifacts();
            ViewBag.InfrastructureArtifactsCount = CountInfrastructureArtifacts();
            ViewBag.TotalArtifactsCount = ViewBag.BusinessArtifactsCount + ViewBag.ApplicationArtifactsCount + 
                                          ViewBag.DataArtifactsCount + ViewBag.InfrastructureArtifactsCount;
            
            // Count individual artifact types
            CountIndividualArtifactTypes();
            
            // Count artifact links
            ViewBag.TotalArtifactLinks = _context.ArtifactLinks.Count();

            return View("Index");
        }

        [Authorize(Roles = "Root, EATeam")]
        public IActionResult Setting()
        {
            if (_context.SettingGeneralInfo.Count() > 0)
            {
                var settingGeneralInfo = _context.SettingGeneralInfo.FirstOrDefault();
                ViewBag.Status = "true";
                return View(settingGeneralInfo);
            }
            _logger.LogInformation($"{User.Identity.Name} accessed setting page");
            ViewBag.Status = "false";
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = "Root, EATeam")]
        public async Task<IActionResult> Setting(SettingGeneralInfo model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            try
            {
                var user = await _userManager.GetUserAsync(User);
                
                if (ViewBag.Status == "true")
                {
                    // Update existing record
                    var existingSettings = await _context.SettingGeneralInfo.FindAsync(model.Id);
                    
                    if (existingSettings == null)
                    {
                        return NotFound();
                    }
                    
                    // Update properties
                    existingSettings.OrganizationName = model.OrganizationName;
                    existingSettings.BusinessTRN = model.BusinessTRN;
                    existingSettings.RegistrationNumber = model.RegistrationNumber;
                    existingSettings.Telephone = model.Telephone;
                    existingSettings.Fax = model.Fax;
                    existingSettings.Email = model.Email;
                    existingSettings.Address = model.Address;
                    existingSettings.Parish = model.Parish;
                    existingSettings.LicenseKey = model.LicenseKey;
                    
                    // Update work days
                    existingSettings.Sunday = model.Sunday;
                    existingSettings.Monday = model.Monday;
                    existingSettings.Tuesday = model.Tuesday;
                    existingSettings.Wednesday = model.Wednesday;
                    existingSettings.Thursday = model.Thursday;
                    existingSettings.Friday = model.Friday;
                    existingSettings.Saturday = model.Saturday;
                    
                    // Update audit fields
                    existingSettings.UserModified = user.UserName;
                    existingSettings.DateModified = DateTime.Now;
                    
                    _context.Update(existingSettings);
                }
                else
                {
                    // Create new settings record
                    model.UserCreated = user.UserName;
                    model.DateCreated = DateTime.Now;
                    
                    await _context.SettingGeneralInfo.AddAsync(model);
                }
                
                await _context.SaveChangesAsync();
                _logger.LogInformation($"{User.Identity.Name} updated system settings");
                
                TempData["SuccessMessage"] = "Settings saved successfully.";
                return RedirectToAction(nameof(Setting));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving settings");
                ModelState.AddModelError("", "An error occurred while saving settings. Please try again.");
                return View(model);
            }
        }
        
        // Helper method to count Business artifacts
        private int CountBusinessArtifacts()
        {
            int count = 0;
            count += _context.BusinessStrategyViews.Count();
            count += _context.OperationModels.Count();
            count += _context.OrganizationViews.Count();
            count += _context.CapabilityMaps.Count();
            count += _context.ProcessModels.Count();
            count += _context.BusinessProductServiceViews.Count();
            return count;
        }
        
        // Helper method to count Application artifacts
        private int CountApplicationArtifacts()
        {
            int count = 0;
            count += _context.ApplicationArchitectureFrameworks.Count();
            count += _context.ApplicationBusinessRequirementsViews.Count();
            count += _context.ServiceCatalogues.Count();
            count += _context.ApplicationToApplicationCrossReferences.Count();
            count += _context.ApplicationSecurityModels.Count();
            count += _context.ApplicationDataCrossReferences.Count();
            return count;
        }
        
        // Helper method to count Data artifacts
        private int CountDataArtifacts()
        {
            int count = 0;
            count += _context.DataGovernanceModels.Count();
            count += _context.InformationHierarchyViews.Count();
            count += _context.InformationRequirementsViews.Count();
            count += _context.DataFlowModels.Count();
            count += _context.LogicalDataModels.Count();
            count += _context.DataSecurityModels.Count();
            return count;
        }
        
        // Helper method to count Infrastructure artifacts
        private int CountInfrastructureArtifacts()
        {
            int count = 0;
            count += _context.InfrastructureBusinessRequirementsViews.Count();
            count += _context.SystemToApplicationCrossReferences.Count();
            count += _context.ResourceNeedsModels.Count();
            count += _context.SystemToBusinessCrossReferences.Count();
            count += _context.InfrastructureSecurityModels.Count();
            count += _context.SystemDataCrossReferences.Count();
            return count;
        }
        
        // Helper method to count individual artifact types
        private void CountIndividualArtifactTypes()
        {
            // Business Architecture
            ViewBag.BusinessStrategyCount = _context.BusinessStrategyViews.Count();
            ViewBag.OperationModelCount = _context.OperationModels.Count();
            ViewBag.OrganizationViewCount = _context.OrganizationViews.Count();
            ViewBag.CapabilityMapCount = _context.CapabilityMaps.Count();
            ViewBag.ProcessModelCount = _context.ProcessModels.Count();
            ViewBag.BusinessProductServiceCount = _context.BusinessProductServiceViews.Count();
            
            // Application Architecture
            ViewBag.ApplicationFrameworkCount = _context.ApplicationArchitectureFrameworks.Count();
            ViewBag.ApplicationBusinessRequirementCount = _context.ApplicationBusinessRequirementsViews.Count();
            ViewBag.ServiceCatalogueCount = _context.ServiceCatalogues.Count();
            ViewBag.ApplicationCrossReferenceCount = _context.ApplicationToApplicationCrossReferences.Count();
            ViewBag.ApplicationSecurityCount = _context.ApplicationSecurityModels.Count();
            ViewBag.ApplicationDataCrossReferenceCount = _context.ApplicationDataCrossReferences.Count();
            
            // Data Architecture
            ViewBag.DataGovernanceCount = _context.DataGovernanceModels.Count();
            ViewBag.InformationHierarchyCount = _context.InformationHierarchyViews.Count();
            ViewBag.InformationRequirementsCount = _context.InformationRequirementsViews.Count();
            ViewBag.DataFlowCount = _context.DataFlowModels.Count();
            ViewBag.LogicalDataCount = _context.LogicalDataModels.Count();
            ViewBag.DataSecurityCount = _context.DataSecurityModels.Count();
            
            // Infrastructure Architecture
            ViewBag.InfrastructureBusinessRequirementCount = _context.InfrastructureBusinessRequirementsViews.Count();
            ViewBag.SystemToApplicationCount = _context.SystemToApplicationCrossReferences.Count();
            ViewBag.ResourceNeedsCount = _context.ResourceNeedsModels.Count();
            ViewBag.SystemToBusinessCount = _context.SystemToBusinessCrossReferences.Count();
            ViewBag.InfrastructureSecurityCount = _context.InfrastructureSecurityModels.Count();
            ViewBag.SystemDataCount = _context.SystemDataCrossReferences.Count();
        }

        [HttpPost]
        public async Task<IActionResult> ExportEAReport(string reportType, string reportFormat, bool includeVisualizations = false)
        {
            if (string.IsNullOrEmpty(reportType))
            {
                return BadRequest("Report type is required");
            }

            // Ensure only Excel format is supported
            if (reportFormat?.ToLower() != "excel")
            {
                return BadRequest("Only Excel format is supported for exports");
            }

            _logger.LogInformation($"{User.Identity.Name} requested export of {reportType} in Excel format");
            
            try
            {
                var excelContent = await GenerateExcelReportAsync(reportType);
                string fileName = $"{reportType.Replace("-", "_")}_{DateTime.Now:yyyyMMdd}.xlsx";
                
                return File(excelContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating Excel export for {reportType}");
                return StatusCode(500, "An error occurred while generating the export.");
            }
        }

        private async Task<byte[]> GenerateExcelReportAsync(string reportType)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");
                
                // Row counter
                int row = 1;
                
                // Different artifact types have different structures
                switch (reportType)
                {
                    // Business Architecture artifacts
                    case "business-strategy":
                        var businessStrategies = await _context.BusinessStrategyViews.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Business Goal";
                        worksheet.Cells[row, 3].Value = "Strategic Objective";
                        worksheet.Cells[row, 4].Value = "Key Performance Indicators";
                        worksheet.Cells[row, 5].Value = "Stakeholders";
                        worksheet.Cells[row, 6].Value = "Timeframe";
                        worksheet.Cells[row, 7].Value = "Vision Alignment";
                        worksheet.Cells[row, 8].Value = "Risks";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var strategy in businessStrategies)
                        {
                            worksheet.Cells[row, 1].Value = strategy.Id;
                            worksheet.Cells[row, 2].Value = strategy.BusinessGoal;
                            worksheet.Cells[row, 3].Value = strategy.StrategicObjective;
                            worksheet.Cells[row, 4].Value = string.Join("; ", strategy.KeyPerformanceIndicators ?? new List<string>());
                            worksheet.Cells[row, 5].Value = string.Join("; ", strategy.Stakeholders ?? new List<string>());
                            worksheet.Cells[row, 6].Value = strategy.Timeframe;
                            worksheet.Cells[row, 7].Value = strategy.OrganizationalVisionAlignment;
                            worksheet.Cells[row, 8].Value = string.Join("; ", strategy.Risks ?? new List<string>());
                            worksheet.Cells[row, 9].Value = string.Join("; ", strategy.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "operation-model":
                        var operationModels = await _context.OperationModels.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Process Name";
                        worksheet.Cells[row, 3].Value = "Process Owner";
                        worksheet.Cells[row, 4].Value = "Inputs";
                        worksheet.Cells[row, 5].Value = "Outputs";
                        worksheet.Cells[row, 6].Value = "Key Activities";
                        worksheet.Cells[row, 7].Value = "Resources Required";
                        worksheet.Cells[row, 8].Value = "Performance Metrics";
                        worksheet.Cells[row, 9].Value = "Challenges";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var model in operationModels)
                        {
                            worksheet.Cells[row, 1].Value = model.Id;
                            worksheet.Cells[row, 2].Value = model.OperationalProcess;
                            worksheet.Cells[row, 3].Value = model.ProcessOwner;
                            worksheet.Cells[row, 4].Value = string.Join("; ", model.Inputs ?? new List<string>());
                            worksheet.Cells[row, 5].Value = string.Join("; ", model.Outputs ?? new List<string>());
                            worksheet.Cells[row, 6].Value = string.Join("; ", model.KeyActivities ?? new List<string>());
                            worksheet.Cells[row, 7].Value = string.Join("; ", model.ResourcesRequired ?? new List<string>());
                            worksheet.Cells[row, 8].Value = string.Join("; ", model.PerformanceMetrics ?? new List<string>());
                            worksheet.Cells[row, 9].Value = string.Join("; ", model.Challenges ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "organization-view":
                        var orgViews = await _context.OrganizationViews.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Department/Unit";
                        worksheet.Cells[row, 3].Value = "Roles & Responsibilities";
                        worksheet.Cells[row, 4].Value = "Reporting Structure";
                        worksheet.Cells[row, 5].Value = "Key Personnel";
                        worksheet.Cells[row, 6].Value = "Collaboration Points";
                        worksheet.Cells[row, 7].Value = "Organizational Goals";
                        worksheet.Cells[row, 8].Value = "Challenges";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 8])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var view in orgViews)
                        {
                            worksheet.Cells[row, 1].Value = view.Id;
                            worksheet.Cells[row, 2].Value = view.DepartmentOrUnit;
                            worksheet.Cells[row, 3].Value = view.RolesAndResponsibilities;
                            worksheet.Cells[row, 4].Value = view.ReportingStructure;
                            worksheet.Cells[row, 5].Value = string.Join("; ", view.KeyPersonnel ?? new List<string>());
                            worksheet.Cells[row, 6].Value = string.Join("; ", view.CollaborationPoints ?? new List<string>());
                            worksheet.Cells[row, 7].Value = string.Join("; ", view.OrganizationalGoals ?? new List<string>());
                            worksheet.Cells[row, 8].Value = string.Join("; ", view.Challenges ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "capability-map":
                        var capabilityMaps = await _context.CapabilityMaps.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Capability Name";
                        worksheet.Cells[row, 3].Value = "Description";
                        worksheet.Cells[row, 4].Value = "Maturity Level";
                        worksheet.Cells[row, 5].Value = "Owner";
                        worksheet.Cells[row, 6].Value = "Dependencies";
                        worksheet.Cells[row, 7].Value = "Key Processes";
                        worksheet.Cells[row, 8].Value = "Gaps";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 8])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var map in capabilityMaps)
                        {
                            worksheet.Cells[row, 1].Value = map.Id;
                            worksheet.Cells[row, 2].Value = map.CapabilityName;
                            worksheet.Cells[row, 3].Value = map.CapabilityDescription;
                            worksheet.Cells[row, 4].Value = map.MaturityLevel;
                            worksheet.Cells[row, 5].Value = map.Owner;
                            worksheet.Cells[row, 6].Value = string.Join("; ", map.Dependencies ?? new List<string>());
                            worksheet.Cells[row, 7].Value = string.Join("; ", map.KeyProcesses ?? new List<string>());
                            worksheet.Cells[row, 8].Value = string.Join("; ", map.Gaps ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "process-model":
                        var processModels = await _context.ProcessModels.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Process Name";
                        worksheet.Cells[row, 3].Value = "Description";
                        worksheet.Cells[row, 4].Value = "Inputs";
                        worksheet.Cells[row, 5].Value = "Outputs";
                        worksheet.Cells[row, 6].Value = "Steps/Workflow";
                        worksheet.Cells[row, 7].Value = "Owner";
                        worksheet.Cells[row, 8].Value = "Tools/Systems Used";
                        worksheet.Cells[row, 9].Value = "Performance Metrics";
                        worksheet.Cells[row, 10].Value = "Customer Type";
                        worksheet.Cells[row, 11].Value = "Key Metrics";
                        worksheet.Cells[row, 12].Value = "Cycle Efficiency";
                        worksheet.Cells[row, 13].Value = "Lead Time";
                        worksheet.Cells[row, 14].Value = "Bottlenecks";
                        worksheet.Cells[row, 15].Value = "Improvement Opportunities";
                        worksheet.Cells[row, 16].Value = "Mapping Date";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 16])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var model in processModels)
                        {
                            worksheet.Cells[row, 1].Value = model.Id;
                            worksheet.Cells[row, 2].Value = model.ProcessName;
                            worksheet.Cells[row, 3].Value = model.ProcessDescription;
                            worksheet.Cells[row, 4].Value = string.Join("; ", model.Inputs ?? new List<string>());
                            worksheet.Cells[row, 5].Value = string.Join("; ", model.Outputs ?? new List<string>());
                            worksheet.Cells[row, 6].Value = string.Join("; ", model.StepsWorkflow ?? new List<string>());
                            worksheet.Cells[row, 7].Value = model.Owner;
                            worksheet.Cells[row, 8].Value = string.Join("; ", model.ToolsSystemsUsed ?? new List<string>());
                            worksheet.Cells[row, 9].Value = string.Join("; ", model.PerformanceMetrics ?? new List<string>());
                            worksheet.Cells[row, 10].Value = model.CustomerType;
                            worksheet.Cells[row, 11].Value = string.Join("; ", model.KeyMetrics ?? new List<string>());
                            worksheet.Cells[row, 12].Value = model.CycleEfficiency;
                            worksheet.Cells[row, 13].Value = model.LeadTime?.ToString() ?? string.Empty;
                            worksheet.Cells[row, 14].Value = string.Join("; ", model.Bottlenecks ?? new List<string>());
                            worksheet.Cells[row, 15].Value = string.Join("; ", model.ImprovementOpportunities ?? new List<string>());
                            worksheet.Cells[row, 16].Value = model.MappingDate?.ToString("yyyy-MM-dd") ?? string.Empty;
                            row++;
                        }
                        
                        // Create Process Stages worksheet
                        var stagesWorksheet = package.Workbook.Worksheets.Add("Process Stages");
                        int stageRow = 1;
                        
                        // Add headers for stages
                        stagesWorksheet.Cells[stageRow, 1].Value = "Process ID";
                        stagesWorksheet.Cells[stageRow, 2].Value = "Process Name";
                        stagesWorksheet.Cells[stageRow, 3].Value = "Stage ID";
                        stagesWorksheet.Cells[stageRow, 4].Value = "Name";
                        stagesWorksheet.Cells[stageRow, 5].Value = "Description";
                        stagesWorksheet.Cells[stageRow, 6].Value = "Process Time";
                        stagesWorksheet.Cells[stageRow, 7].Value = "Wait Time";
                        stagesWorksheet.Cells[stageRow, 8].Value = "Is Value Add";
                        stagesWorksheet.Cells[stageRow, 9].Value = "Percent Complete";
                        stagesWorksheet.Cells[stageRow, 10].Value = "Defect Rate";
                        stagesWorksheet.Cells[stageRow, 11].Value = "Issues";
                        stagesWorksheet.Cells[stageRow, 12].Value = "Process IDs";
                        stagesWorksheet.Cells[stageRow, 13].Value = "Application IDs";
                        
                        // Format header row for stages
                        using (var range = stagesWorksheet.Cells[stageRow, 1, stageRow, 13])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        stageRow++;
                        
                        // Add data rows for stages
                        foreach (var model in processModels)
                        {
                            if (model.Stages != null && model.Stages.Any())
                            {
                                foreach (var stage in model.Stages)
                                {
                                    stagesWorksheet.Cells[stageRow, 1].Value = model.Id;
                                    stagesWorksheet.Cells[stageRow, 2].Value = model.ProcessName;
                                    stagesWorksheet.Cells[stageRow, 3].Value = stage.Id;
                                    stagesWorksheet.Cells[stageRow, 4].Value = stage.Name;
                                    stagesWorksheet.Cells[stageRow, 5].Value = stage.Description;
                                    stagesWorksheet.Cells[stageRow, 6].Value = stage.ProcessTime.ToString();
                                    stagesWorksheet.Cells[stageRow, 7].Value = stage.WaitTime.ToString();
                                    stagesWorksheet.Cells[stageRow, 8].Value = stage.IsValueAdd;
                                    stagesWorksheet.Cells[stageRow, 9].Value = stage.PercentComplete;
                                    stagesWorksheet.Cells[stageRow, 10].Value = stage.DefectRate;
                                    stagesWorksheet.Cells[stageRow, 11].Value = string.Join("; ", stage.Issues ?? new List<string>());
                                    stagesWorksheet.Cells[stageRow, 12].Value = string.Join("; ", stage.ProcessIds ?? new List<string>());
                                    stagesWorksheet.Cells[stageRow, 13].Value = string.Join("; ", stage.ApplicationIds ?? new List<string>());
                                    stageRow++;
                                }
                            }
                        }
                        
                        // Create Stakeholder Values worksheet
                        var stakeholderWorksheet = package.Workbook.Worksheets.Add("Stakeholder Values");
                        int stakeholderRow = 1;
                        
                        // Add headers for stakeholder values
                        stakeholderWorksheet.Cells[stakeholderRow, 1].Value = "Process ID";
                        stakeholderWorksheet.Cells[stakeholderRow, 2].Value = "Process Name";
                        stakeholderWorksheet.Cells[stakeholderRow, 3].Value = "Stakeholder Value ID";
                        stakeholderWorksheet.Cells[stakeholderRow, 4].Value = "Stakeholder Type";
                        stakeholderWorksheet.Cells[stakeholderRow, 5].Value = "Value Description";
                        stakeholderWorksheet.Cells[stakeholderRow, 6].Value = "Priority";
                        stakeholderWorksheet.Cells[stakeholderRow, 7].Value = "Process ID Reference";
                        stakeholderWorksheet.Cells[stakeholderRow, 8].Value = "Metric of Success";
                        
                        // Format header row for stakeholder values
                        using (var range = stakeholderWorksheet.Cells[stakeholderRow, 1, stakeholderRow, 8])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        stakeholderRow++;
                        
                        // Add data rows for stakeholder values
                        foreach (var model in processModels)
                        {
                            if (model.StakeholderValues != null && model.StakeholderValues.Any())
                            {
                                foreach (var value in model.StakeholderValues)
                                {
                                    stakeholderWorksheet.Cells[stakeholderRow, 1].Value = model.Id;
                                    stakeholderWorksheet.Cells[stakeholderRow, 2].Value = model.ProcessName;
                                    stakeholderWorksheet.Cells[stakeholderRow, 3].Value = value.Id;
                                    stakeholderWorksheet.Cells[stakeholderRow, 4].Value = value.StakeholderType;
                                    stakeholderWorksheet.Cells[stakeholderRow, 5].Value = value.ValueDescription;
                                    stakeholderWorksheet.Cells[stakeholderRow, 6].Value = value.Priority;
                                    stakeholderWorksheet.Cells[stakeholderRow, 7].Value = value.ProcessId;
                                    stakeholderWorksheet.Cells[stakeholderRow, 8].Value = value.MetricOfSuccess;
                                    stakeholderRow++;
                                }
                            }
                        }
                        
                        break;
                        
                    case "business-product-service":
                        var productServices = await _context.BusinessProductServiceViews.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Product/Service Name";
                        worksheet.Cells[row, 3].Value = "Description";
                        worksheet.Cells[row, 4].Value = "Target Market/Customer";
                        worksheet.Cells[row, 5].Value = "Value Proposition";
                        worksheet.Cells[row, 6].Value = "Revenue Model";
                        worksheet.Cells[row, 7].Value = "Key Features";
                        worksheet.Cells[row, 8].Value = "Dependencies";
                        worksheet.Cells[row, 9].Value = "Competitors";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var ps in productServices)
                        {
                            worksheet.Cells[row, 1].Value = ps.Id;
                            worksheet.Cells[row, 2].Value = ps.ProductServiceName;
                            worksheet.Cells[row, 3].Value = ps.Description;
                            worksheet.Cells[row, 4].Value = ps.TargetMarketCustomer;
                            worksheet.Cells[row, 5].Value = ps.ValueProposition;
                            worksheet.Cells[row, 6].Value = ps.RevenueModel;
                            worksheet.Cells[row, 7].Value = string.Join("; ", ps.KeyFeatures ?? new List<string>());
                            worksheet.Cells[row, 8].Value = string.Join("; ", ps.Dependencies ?? new List<string>());
                            worksheet.Cells[row, 9].Value = string.Join("; ", ps.Competitors ?? new List<string>());
                            row++;
                        }
                        break;
                    
                    // Application Architecture artifacts
                    case "application-framework":
                        var frameworks = await _context.ApplicationArchitectureFrameworks.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Application Name";
                        worksheet.Cells[row, 3].Value = "Description";
                        worksheet.Cells[row, 4].Value = "Business Function";
                        worksheet.Cells[row, 5].Value = "Technology Stack";
                        worksheet.Cells[row, 6].Value = "Integration Points";
                        worksheet.Cells[row, 7].Value = "Owner";
                        worksheet.Cells[row, 8].Value = "Version";
                        worksheet.Cells[row, 9].Value = "Deployment Environment";
                        worksheet.Cells[row, 10].Value = "Rationalization Category";
                        worksheet.Cells[row, 11].Value = "Business Value";
                        worksheet.Cells[row, 12].Value = "Technical Fit";
                        worksheet.Cells[row, 13].Value = "Annual Cost";
                        worksheet.Cells[row, 14].Value = "Risk Score";
                        worksheet.Cells[row, 15].Value = "Current State";
                        worksheet.Cells[row, 16].Value = "Future State";
                        worksheet.Cells[row, 17].Value = "Rationalization Rationale";
                        worksheet.Cells[row, 18].Value = "Redundant Applications";
                        worksheet.Cells[row, 19].Value = "Replacement Options";
                        worksheet.Cells[row, 20].Value = "Assessment Date";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 20])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var framework in frameworks)
                        {
                            worksheet.Cells[row, 1].Value = framework.Id;
                            worksheet.Cells[row, 2].Value = framework.ApplicationName;
                            worksheet.Cells[row, 3].Value = framework.ApplicationDescription;
                            worksheet.Cells[row, 4].Value = framework.BusinessFunctionSupported;
                            worksheet.Cells[row, 5].Value = string.Join("; ", framework.TechnologyStack ?? new List<string>());
                            worksheet.Cells[row, 6].Value = string.Join("; ", framework.IntegrationPoints ?? new List<string>());
                            worksheet.Cells[row, 7].Value = framework.Owner;
                            worksheet.Cells[row, 8].Value = framework.Version;
                            worksheet.Cells[row, 9].Value = framework.DeploymentEnvironment;
                            worksheet.Cells[row, 10].Value = framework.RationalizationCategory;
                            worksheet.Cells[row, 11].Value = framework.BusinessValue;
                            worksheet.Cells[row, 12].Value = framework.TechnicalFit;
                            worksheet.Cells[row, 13].Value = framework.AnnualCost;
                            worksheet.Cells[row, 14].Value = framework.RiskScore;
                            worksheet.Cells[row, 15].Value = framework.CurrentState;
                            worksheet.Cells[row, 16].Value = framework.FutureState;
                            worksheet.Cells[row, 17].Value = framework.RationalizationRationale;
                            worksheet.Cells[row, 18].Value = string.Join("; ", framework.RedundantApplications ?? new List<string>());
                            worksheet.Cells[row, 19].Value = string.Join("; ", framework.ReplacementOptions ?? new List<string>());
                            worksheet.Cells[row, 20].Value = framework.AssessmentDate?.ToString("yyyy-MM-dd") ?? string.Empty;
                            row++;
                        }
                        
                        // Create Application Recommendations worksheet
                        var recommendationsWorksheet = package.Workbook.Worksheets.Add("Application Recommendations");
                        int recRow = 1;
                        
                        // Add headers for recommendations
                        recommendationsWorksheet.Cells[recRow, 1].Value = "Application ID";
                        recommendationsWorksheet.Cells[recRow, 2].Value = "Application Name";
                        recommendationsWorksheet.Cells[recRow, 3].Value = "Recommendation ID";
                        recommendationsWorksheet.Cells[recRow, 4].Value = "Recommendation Type";
                        recommendationsWorksheet.Cells[recRow, 5].Value = "Description";
                        recommendationsWorksheet.Cells[recRow, 6].Value = "Business";
                        recommendationsWorksheet.Cells[recRow, 7].Value = "Estimated Cost Savings";
                        recommendationsWorksheet.Cells[recRow, 8].Value = "Estimated Implementation Cost";
                        recommendationsWorksheet.Cells[recRow, 9].Value = "Timeline";
                        recommendationsWorksheet.Cells[recRow, 10].Value = "Dependencies";
                        recommendationsWorksheet.Cells[recRow, 11].Value = "Status";
                        
                        // Format header row for recommendations
                        using (var range = recommendationsWorksheet.Cells[recRow, 1, recRow, 11])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        recRow++;
                        
                        // Add data rows for recommendations
                        foreach (var framework in frameworks)
                        {
                            if (framework.Recommendations != null && framework.Recommendations.Any())
                            {
                                foreach (var rec in framework.Recommendations)
                                {
                                    recommendationsWorksheet.Cells[recRow, 1].Value = framework.Id;
                                    recommendationsWorksheet.Cells[recRow, 2].Value = framework.ApplicationName;
                                    recommendationsWorksheet.Cells[recRow, 3].Value = rec.Id;
                                    recommendationsWorksheet.Cells[recRow, 4].Value = rec.RecommendationType;
                                    recommendationsWorksheet.Cells[recRow, 5].Value = rec.Description;
                                    recommendationsWorksheet.Cells[recRow, 6].Value = rec.Business;
                                    recommendationsWorksheet.Cells[recRow, 7].Value = rec.EstimatedCostSavings;
                                    recommendationsWorksheet.Cells[recRow, 8].Value = rec.EstimatedImplementationCost;
                                    recommendationsWorksheet.Cells[recRow, 9].Value = rec.Timeline;
                                    recommendationsWorksheet.Cells[recRow, 10].Value = string.Join("; ", rec.Dependencies ?? new List<string>());
                                    recommendationsWorksheet.Cells[recRow, 11].Value = rec.Status;
                                    recRow++;
                                }
                            }
                        }
                        
                        break;
                        
                    case "application-business-requirement":
                        var appRequirements = await _context.ApplicationBusinessRequirementsViews.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Requirement ID";
                        worksheet.Cells[row, 3].Value = "Description";
                        worksheet.Cells[row, 4].Value = "Priority";
                        worksheet.Cells[row, 5].Value = "Stakeholders";
                        worksheet.Cells[row, 6].Value = "Status";
                        worksheet.Cells[row, 7].Value = "Assigned To";
                        worksheet.Cells[row, 8].Value = "Due Date";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var req in appRequirements)
                        {
                            worksheet.Cells[row, 1].Value = req.Id;
                            worksheet.Cells[row, 2].Value = req.RequirementId;
                            worksheet.Cells[row, 3].Value = req.RequirementDescription;
                            worksheet.Cells[row, 4].Value = req.Priority;
                            worksheet.Cells[row, 5].Value = string.Join("; ", req.Stakeholders ?? new List<string>());
                            worksheet.Cells[row, 6].Value = req.Status;
                            worksheet.Cells[row, 7].Value = req.AssignedTo;
                            worksheet.Cells[row, 8].Value = req.DueDate;
                            worksheet.Cells[row, 9].Value = string.Join("; ", req.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;

                    case "service-catalogue":
                        var serviceCatalogues = await _context.ServiceCatalogues.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Service Name";
                        worksheet.Cells[row, 3].Value = "Description";
                        worksheet.Cells[row, 4].Value = "Service Owner";
                        worksheet.Cells[row, 5].Value = "SLA";
                        worksheet.Cells[row, 6].Value = "Cost";
                        worksheet.Cells[row, 7].Value = "Supported Applications";
                        worksheet.Cells[row, 8].Value = "Availability";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var service in serviceCatalogues)
                        {
                            worksheet.Cells[row, 1].Value = service.Id;
                            worksheet.Cells[row, 2].Value = service.ServiceName;
                            worksheet.Cells[row, 3].Value = service.ServiceDescription;
                            worksheet.Cells[row, 4].Value = service.ServiceOwner;
                            worksheet.Cells[row, 5].Value = service.SLA;
                            worksheet.Cells[row, 6].Value = service.Cost;
                            worksheet.Cells[row, 7].Value = string.Join("; ", service.SupportedApplications ?? new List<string>());
                            worksheet.Cells[row, 8].Value = service.Availability;
                            worksheet.Cells[row, 9].Value = string.Join("; ", service.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "application-cross-reference":
                        var appCrossRefs = await _context.ApplicationToApplicationCrossReferences.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Source Application";
                        worksheet.Cells[row, 3].Value = "Target Application";
                        worksheet.Cells[row, 4].Value = "Integration Type";
                        worksheet.Cells[row, 5].Value = "Data Exchanged";
                        worksheet.Cells[row, 6].Value = "Frequency";
                        worksheet.Cells[row, 7].Value = "Owner";
                        worksheet.Cells[row, 8].Value = "Security Requirements";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var crossRef in appCrossRefs)
                        {
                            worksheet.Cells[row, 1].Value = crossRef.Id;
                            worksheet.Cells[row, 2].Value = crossRef.SourceApplication;
                            worksheet.Cells[row, 3].Value = crossRef.TargetApplication;
                            worksheet.Cells[row, 4].Value = crossRef.IntegrationType;
                            worksheet.Cells[row, 5].Value = crossRef.DataExchanged;
                            worksheet.Cells[row, 6].Value = crossRef.Frequency;
                            worksheet.Cells[row, 7].Value = crossRef.Owner;
                            worksheet.Cells[row, 8].Value = crossRef.SecurityRequirements;
                            worksheet.Cells[row, 9].Value = string.Join("; ", crossRef.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "application-security":
                        var appSecurityModels = await _context.ApplicationSecurityModels.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Application Name";
                        worksheet.Cells[row, 3].Value = "Security Requirement";
                        worksheet.Cells[row, 4].Value = "Compliance Standards";
                        worksheet.Cells[row, 5].Value = "Vulnerabilities";
                        worksheet.Cells[row, 6].Value = "Mitigation Strategies";
                        worksheet.Cells[row, 7].Value = "Owner";
                        worksheet.Cells[row, 8].Value = "Last Audit Date";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var security in appSecurityModels)
                        {
                            worksheet.Cells[row, 1].Value = security.Id;
                            worksheet.Cells[row, 2].Value = security.ApplicationName;
                            worksheet.Cells[row, 3].Value = security.SecurityRequirement;
                            worksheet.Cells[row, 4].Value = string.Join("; ", security.ComplianceStandards ?? new List<string>());
                            worksheet.Cells[row, 5].Value = string.Join("; ", security.Vulnerabilities ?? new List<string>());
                            worksheet.Cells[row, 6].Value = string.Join("; ", security.MitigationStrategies ?? new List<string>());
                            worksheet.Cells[row, 7].Value = security.Owner;
                            worksheet.Cells[row, 8].Value = security.LastAuditDate;
                            worksheet.Cells[row, 9].Value = string.Join("; ", security.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "application-data-cross-reference":
                        var appDataCrossRefs = await _context.ApplicationDataCrossReferences.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Application Name";
                        worksheet.Cells[row, 3].Value = "Data Entity";
                        worksheet.Cells[row, 4].Value = "Data Usage";
                        worksheet.Cells[row, 5].Value = "Data Sensitivity";
                        worksheet.Cells[row, 6].Value = "Access Controls";
                        worksheet.Cells[row, 7].Value = "Owner";
                        worksheet.Cells[row, 8].Value = "Data Retention Policy";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var crossRef in appDataCrossRefs)
                        {
                            worksheet.Cells[row, 1].Value = crossRef.Id;
                            worksheet.Cells[row, 2].Value = crossRef.ApplicationName;
                            worksheet.Cells[row, 3].Value = crossRef.DataEntity;
                            worksheet.Cells[row, 4].Value = crossRef.DataUsage;
                            worksheet.Cells[row, 5].Value = crossRef.DataSensitivity;
                            worksheet.Cells[row, 6].Value = crossRef.AccessControls;
                            worksheet.Cells[row, 7].Value = crossRef.Owner;
                            worksheet.Cells[row, 8].Value = crossRef.DataRetentionPolicy;
                            worksheet.Cells[row, 9].Value = string.Join("; ", crossRef.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;

                    // Data Architecture artifacts
                    case "data-governance":
                        var dataGovernanceModels = await _context.DataGovernanceModels.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Data Entity";
                        worksheet.Cells[row, 3].Value = "Data Owner";
                        worksheet.Cells[row, 4].Value = "Data Steward";
                        worksheet.Cells[row, 5].Value = "Data Quality Metrics";
                        worksheet.Cells[row, 6].Value = "Data Policies";
                        worksheet.Cells[row, 7].Value = "Compliance Requirements";
                        worksheet.Cells[row, 8].Value = "Data Lifecycle";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var governance in dataGovernanceModels)
                        {
                            worksheet.Cells[row, 1].Value = governance.Id;
                            worksheet.Cells[row, 2].Value = governance.DataEntity;
                            worksheet.Cells[row, 3].Value = governance.DataOwner;
                            worksheet.Cells[row, 4].Value = governance.DataSteward;
                            worksheet.Cells[row, 5].Value = governance.DataQualityMetrics;
                            worksheet.Cells[row, 6].Value = governance.DataPolicies;
                            worksheet.Cells[row, 7].Value = governance.ComplianceRequirements;
                            worksheet.Cells[row, 8].Value = governance.DataLifecycle;
                            worksheet.Cells[row, 9].Value = string.Join("; ", governance.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "information-hierarchy":
                        var informationHierarchies = await _context.InformationHierarchyViews.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Information Level";
                        worksheet.Cells[row, 3].Value = "Parent Information";
                        worksheet.Cells[row, 4].Value = "Child Information";
                        worksheet.Cells[row, 5].Value = "Description";
                        worksheet.Cells[row, 6].Value = "Owner";
                        worksheet.Cells[row, 7].Value = "Usage";
                        worksheet.Cells[row, 8].Value = "Dependencies";
                        worksheet.Cells[row, 9].Value = "Security Level";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var hierarchy in informationHierarchies)
                        {
                            worksheet.Cells[row, 1].Value = hierarchy.Id;
                            worksheet.Cells[row, 2].Value = hierarchy.InformationLevel;
                            worksheet.Cells[row, 3].Value = hierarchy.ParentInformation;
                            worksheet.Cells[row, 4].Value = hierarchy.ChildInformation;
                            worksheet.Cells[row, 5].Value = hierarchy.Description;
                            worksheet.Cells[row, 6].Value = hierarchy.Owner;
                            worksheet.Cells[row, 7].Value = hierarchy.Usage;
                            worksheet.Cells[row, 8].Value = string.Join("; ", hierarchy.Dependencies ?? new List<string>());
                            worksheet.Cells[row, 9].Value = hierarchy.SecurityLevel;
                            row++;
                        }
                        break;
                        
                    case "information-requirements":
                        var informationRequirements = await _context.InformationRequirementsViews.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Requirement ID";
                        worksheet.Cells[row, 3].Value = "Description";
                        worksheet.Cells[row, 4].Value = "Data Entity";
                        worksheet.Cells[row, 5].Value = "Priority";
                        worksheet.Cells[row, 6].Value = "Stakeholders";
                        worksheet.Cells[row, 7].Value = "Status";
                        worksheet.Cells[row, 8].Value = "Assigned To";
                        worksheet.Cells[row, 9].Value = "Due Date";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var requirement in informationRequirements)
                        {
                            worksheet.Cells[row, 1].Value = requirement.Id;
                            worksheet.Cells[row, 2].Value = requirement.RequirementId;
                            worksheet.Cells[row, 3].Value = requirement.RequirementDescription;
                            worksheet.Cells[row, 4].Value = requirement.DataEntity;
                            worksheet.Cells[row, 5].Value = requirement.Priority;
                            worksheet.Cells[row, 6].Value = string.Join("; ", requirement.Stakeholders ?? new List<string>());
                            worksheet.Cells[row, 7].Value = requirement.Status;
                            worksheet.Cells[row, 8].Value = requirement.AssignedTo;
                            worksheet.Cells[row, 9].Value = requirement.DueDate;
                            row++;
                        }
                        break;
                        
                    case "data-flow":
                        var dataFlows = await _context.DataFlowModels.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Data Source";
                        worksheet.Cells[row, 3].Value = "Data Destination";
                        worksheet.Cells[row, 4].Value = "Data Flow Description";
                        worksheet.Cells[row, 5].Value = "Data Type";
                        worksheet.Cells[row, 6].Value = "Frequency";
                        worksheet.Cells[row, 7].Value = "Owner";
                        worksheet.Cells[row, 8].Value = "Security Requirements";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var flow in dataFlows)
                        {
                            worksheet.Cells[row, 1].Value = flow.Id;
                            worksheet.Cells[row, 2].Value = flow.DataSource;
                            worksheet.Cells[row, 3].Value = flow.DataDestination;
                            worksheet.Cells[row, 4].Value = flow.DataFlowDescription;
                            worksheet.Cells[row, 5].Value = flow.DataTypeName;
                            worksheet.Cells[row, 6].Value = flow.Frequency;
                            worksheet.Cells[row, 7].Value = flow.Owner;
                            worksheet.Cells[row, 8].Value = flow.SecurityRequirements;
                            worksheet.Cells[row, 9].Value = string.Join("; ", flow.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "logical-data-model":
                        var logicalDataModels = await _context.LogicalDataModels.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Entity Name";
                        worksheet.Cells[row, 3].Value = "Attributes";
                        worksheet.Cells[row, 4].Value = "Relationships";
                        worksheet.Cells[row, 5].Value = "Primary Key";
                        worksheet.Cells[row, 6].Value = "Foreign Key";
                        worksheet.Cells[row, 7].Value = "Owner";
                        worksheet.Cells[row, 8].Value = "Description";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var model in logicalDataModels)
                        {
                            worksheet.Cells[row, 1].Value = model.Id;
                            worksheet.Cells[row, 2].Value = model.EntityName;
                            worksheet.Cells[row, 3].Value = string.Join("; ", model.Attributes ?? new List<string>());
                            worksheet.Cells[row, 4].Value = string.Join("; ", model.Relationships ?? new List<string>());
                            worksheet.Cells[row, 5].Value = model.PrimaryKey;
                            worksheet.Cells[row, 6].Value = model.ForeignKey;
                            worksheet.Cells[row, 7].Value = model.Owner;
                            worksheet.Cells[row, 8].Value = model.Description;
                            worksheet.Cells[row, 9].Value = string.Join("; ", model.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "data-security":
                        var dataSecurityModels = await _context.DataSecurityModels.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Data Entity";
                        worksheet.Cells[row, 3].Value = "Security Requirement";
                        worksheet.Cells[row, 4].Value = "Access Controls";
                        worksheet.Cells[row, 5].Value = "Encryption Requirements";
                        worksheet.Cells[row, 6].Value = "Compliance Standards";
                        worksheet.Cells[row, 7].Value = "Owner";
                        worksheet.Cells[row, 8].Value = "Last Audit Date";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var security in dataSecurityModels)
                        {
                            worksheet.Cells[row, 1].Value = security.Id;
                            worksheet.Cells[row, 2].Value = security.DataEntity;
                            worksheet.Cells[row, 3].Value = security.SecurityRequirement;
                            worksheet.Cells[row, 4].Value = security.AccessControls;
                            worksheet.Cells[row, 5].Value = security.EncryptionRequirements;
                            worksheet.Cells[row, 6].Value = string.Join("; ", security.ComplianceStandards ?? new List<string>());
                            worksheet.Cells[row, 7].Value = security.Owner;
                            worksheet.Cells[row, 8].Value = security.LastAuditDate;
                            worksheet.Cells[row, 9].Value = string.Join("; ", security.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;
                    
                    // Infrastructure Architecture artifacts
                    case "infrastructure-business-requirement":
                        var infrastructureRequirements = await _context.InfrastructureBusinessRequirementsViews.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Requirement ID";
                        worksheet.Cells[row, 3].Value = "Description";
                        worksheet.Cells[row, 4].Value = "Business Unit";
                        worksheet.Cells[row, 5].Value = "Priority";
                        worksheet.Cells[row, 6].Value = "Stakeholders";
                        worksheet.Cells[row, 7].Value = "Status";
                        worksheet.Cells[row, 8].Value = "Assigned To";
                        worksheet.Cells[row, 9].Value = "Due Date";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var requirement in infrastructureRequirements)
                        {
                            worksheet.Cells[row, 1].Value = requirement.Id;
                            worksheet.Cells[row, 2].Value = requirement.RequirementId;
                            worksheet.Cells[row, 3].Value = requirement.RequirementDescription;
                            worksheet.Cells[row, 4].Value = requirement.BusinessUnit;
                            worksheet.Cells[row, 5].Value = requirement.Priority;
                            worksheet.Cells[row, 6].Value = string.Join("; ", requirement.Stakeholders ?? new List<string>());
                            worksheet.Cells[row, 7].Value = requirement.Status;
                            worksheet.Cells[row, 8].Value = requirement.AssignedTo;
                            worksheet.Cells[row, 9].Value = requirement.DueDate;
                            row++;
                        }
                        break;
                        
                    case "system-application":
                        var systemAppCrossRefs = await _context.SystemToApplicationCrossReferences.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "System Name";
                        worksheet.Cells[row, 3].Value = "Application Name";
                        worksheet.Cells[row, 4].Value = "Integration Type";
                        worksheet.Cells[row, 5].Value = "Data Exchanged";
                        worksheet.Cells[row, 6].Value = "Frequency";
                        worksheet.Cells[row, 7].Value = "Owner";
                        worksheet.Cells[row, 8].Value = "Security Requirements";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var crossRef in systemAppCrossRefs)
                        {
                            worksheet.Cells[row, 1].Value = crossRef.Id;
                            worksheet.Cells[row, 2].Value = crossRef.SystemName;
                            worksheet.Cells[row, 3].Value = crossRef.ApplicationName;
                            worksheet.Cells[row, 4].Value = crossRef.IntegrationType;
                            worksheet.Cells[row, 5].Value = crossRef.DataExchanged;
                            worksheet.Cells[row, 6].Value = crossRef.Frequency;
                            worksheet.Cells[row, 7].Value = crossRef.Owner;
                            worksheet.Cells[row, 8].Value = crossRef.SecurityRequirements;
                            worksheet.Cells[row, 9].Value = string.Join("; ", crossRef.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "resource-needs":
                        var resourceNeeds = await _context.ResourceNeedsModels.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "Resource Type";
                        worksheet.Cells[row, 3].Value = "Resource Name";
                        worksheet.Cells[row, 4].Value = "Description";
                        worksheet.Cells[row, 5].Value = "Quantity";
                        worksheet.Cells[row, 6].Value = "Owner";
                        worksheet.Cells[row, 7].Value = "Cost";
                        worksheet.Cells[row, 8].Value = "Dependencies";
                        worksheet.Cells[row, 9].Value = "Allocation Status";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var resource in resourceNeeds)
                        {
                            worksheet.Cells[row, 1].Value = resource.Id;
                            worksheet.Cells[row, 2].Value = resource.ResourceType;
                            worksheet.Cells[row, 3].Value = resource.ResourceName;
                            worksheet.Cells[row, 4].Value = resource.Description;
                            worksheet.Cells[row, 5].Value = resource.Quantity;
                            worksheet.Cells[row, 6].Value = resource.Owner;
                            worksheet.Cells[row, 7].Value = resource.Cost;
                            worksheet.Cells[row, 8].Value = string.Join("; ", resource.Dependencies ?? new List<string>());
                            worksheet.Cells[row, 9].Value = resource.AllocationStatus;
                            row++;
                        }
                        break;
                        
                    case "system-business":
                        var systemBusinessCrossRefs = await _context.SystemToBusinessCrossReferences.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "System Name";
                        worksheet.Cells[row, 3].Value = "Business Unit";
                        worksheet.Cells[row, 4].Value = "Business Process Supported";
                        worksheet.Cells[row, 5].Value = "Owner";
                        worksheet.Cells[row, 6].Value = "Criticality";
                        worksheet.Cells[row, 7].Value = "Dependencies";
                        worksheet.Cells[row, 8].Value = "Performance Metrics";
                        worksheet.Cells[row, 9].Value = "Security Requirements";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var crossRef in systemBusinessCrossRefs)
                        {
                            worksheet.Cells[row, 1].Value = crossRef.Id;
                            worksheet.Cells[row, 2].Value = crossRef.SystemName;
                            worksheet.Cells[row, 3].Value = crossRef.BusinessUnit;
                            worksheet.Cells[row, 4].Value = crossRef.BusinessProcessSupported;
                            worksheet.Cells[row, 5].Value = crossRef.Owner;
                            worksheet.Cells[row, 6].Value = crossRef.Criticality;
                            worksheet.Cells[row, 7].Value = string.Join("; ", crossRef.Dependencies ?? new List<string>());
                            worksheet.Cells[row, 8].Value = string.Join("; ", crossRef.PerformanceMetrics ?? new List<string>());
                            worksheet.Cells[row, 9].Value = crossRef.SecurityRequirements;
                            row++;
                        }
                        break;
                        
                    case "infrastructure-security":
                        var infrastructureSecurityModels = await _context.InfrastructureSecurityModels.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "System Name";
                        worksheet.Cells[row, 3].Value = "Security Requirement";
                        worksheet.Cells[row, 4].Value = "Compliance Standards";
                        worksheet.Cells[row, 5].Value = "Vulnerabilities";
                        worksheet.Cells[row, 6].Value = "Mitigation Strategies";
                        worksheet.Cells[row, 7].Value = "Owner";
                        worksheet.Cells[row, 8].Value = "Last Audit Date";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var security in infrastructureSecurityModels)
                        {
                            worksheet.Cells[row, 1].Value = security.Id;
                            worksheet.Cells[row, 2].Value = security.SystemName;
                            worksheet.Cells[row, 3].Value = security.SecurityRequirement;
                            worksheet.Cells[row, 4].Value = string.Join("; ", security.ComplianceStandards ?? new List<string>());
                            worksheet.Cells[row, 5].Value = string.Join("; ", security.Vulnerabilities ?? new List<string>());
                            worksheet.Cells[row, 6].Value = string.Join("; ", security.MitigationStrategies ?? new List<string>());
                            worksheet.Cells[row, 7].Value = security.Owner;
                            worksheet.Cells[row, 8].Value = security.LastAuditDate;
                            worksheet.Cells[row, 9].Value = string.Join("; ", security.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;
                        
                    case "system-data":
                        var systemDataCrossRefs = await _context.SystemDataCrossReferences.ToListAsync();
                        
                        // Write headers
                        worksheet.Cells[row, 1].Value = "ID";
                        worksheet.Cells[row, 2].Value = "System Name";
                        worksheet.Cells[row, 3].Value = "Data Entity";
                        worksheet.Cells[row, 4].Value = "Data Usage";
                        worksheet.Cells[row, 5].Value = "Data Sensitivity";
                        worksheet.Cells[row, 6].Value = "Access Controls";
                        worksheet.Cells[row, 7].Value = "Owner";
                        worksheet.Cells[row, 8].Value = "Data Retention Policy";
                        worksheet.Cells[row, 9].Value = "Dependencies";
                        
                        // Format header row
                        using (var range = worksheet.Cells[row, 1, row, 9])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                        
                        row++;
                        
                        // Write data rows
                        foreach (var crossRef in systemDataCrossRefs)
                        {
                            worksheet.Cells[row, 1].Value = crossRef.Id;
                            worksheet.Cells[row, 2].Value = crossRef.SystemName;
                            worksheet.Cells[row, 3].Value = crossRef.DataEntity;
                            worksheet.Cells[row, 4].Value = crossRef.DataUsage;
                            worksheet.Cells[row, 5].Value = crossRef.DataSensitivity;
                            worksheet.Cells[row, 6].Value = crossRef.AccessControls;
                            worksheet.Cells[row, 7].Value = crossRef.Owner;
                            worksheet.Cells[row, 8].Value = crossRef.DataRetentionPolicy;
                            worksheet.Cells[row, 9].Value = string.Join("; ", crossRef.Dependencies ?? new List<string>());
                            row++;
                        }
                        break;
                    
                    default:
                        throw new ArgumentException($"Export not implemented for artifact type: {reportType}");
                }
                
                // Auto-size columns for better readability
                worksheet.Cells.AutoFitColumns();
                
                return package.GetAsByteArray();
            }
        }

        [HttpGet]
        public IActionResult DownloadTemplate(string artifactType)
        {
            if (string.IsNullOrEmpty(artifactType))
            {
                return BadRequest("Artifact type is required");
            }

            _logger.LogInformation($"{User.Identity.Name} requested template download for {artifactType}");
            
            try
            {
                var templateContent = GenerateTemplateExcel(artifactType);
                string fileName = $"{artifactType.Replace("-", "_")}_template.xlsx";
                
                return File(templateContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating template for {artifactType}");
                return StatusCode(500, "An error occurred while generating the template.");
            }
        }

        private byte[] GenerateTemplateExcel(string artifactType)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Template");
                
                // Provide the headers for the selected artifact type
                switch (artifactType)
                {
                    // Business Architecture artifacts
                    case "business-strategy":
                        SetHeaders(worksheet, new[] { 
                            "Business Goal*", "Strategic Objective*", "Key Performance Indicators*", 
                            "Stakeholders*", "Timeframe*", "Vision Alignment*", "Risks", "Dependencies" 
                        });
                        break;
                        
                    case "operation-model":
                        SetHeaders(worksheet, new[] { 
                            "Process Name*", "Process Owner*", "Inputs*", "Outputs*", 
                            "Key Activities*", "Required Resources*", "Performance Metrics*", "Challenges" 
                        });
                        break;
                        
                    case "organization-view":
                        SetHeaders(worksheet, new[] { 
                            "Department/Unit*", "Roles & Responsibilities*", "Reporting Structure*", 
                            "Key Personnel*", "Collaboration Points*", "Organizational Goals*", "Challenges" 
                        });
                        break;
                        
                    case "capability-map":
                        SetHeaders(worksheet, new[] { 
                            "Capability Name*", "Capability Description*", "Maturity Level*", 
                            "Owner*", "Dependencies", "Key Processes*", "Gaps" 
                        });
                        break;
                        
                    case "process-model":
                        SetHeaders(worksheet, new[] { 
                            "Process Name*", "Process Description*", "Inputs*", "Outputs*", 
                            "Steps/Workflow*", "Owner*", "Tools/Systems Used*", "Performance Metrics*",
                            "Customer Type", "Key Metrics", "Cycle Efficiency", "Lead Time",
                            "Bottlenecks", "Improvement Opportunities", "Mapping Date"
                        });
                        
                        // Add a Process Stages template worksheet
                        var stagesWorksheet = package.Workbook.Worksheets.Add("Process Stages Template");
                        SetHeaders(stagesWorksheet, new[] {
                            "Process Name*", "Stage Name*", "Description*", "Process Time (hh:mm:ss)*", 
                            "Wait Time (hh:mm:ss)*", "Is Value Add*", "Percent Complete", 
                            "Defect Rate", "Issues", "Process IDs", "Application IDs"
                        });
                        AddSampleRow(stagesWorksheet, new[] {
                            "Order Processing", "Order Entry", "Customer order entry into system", "00:10:00", 
                            "00:05:00", "TRUE", "100", "0.5", "None", "PROC-001", "APP-001; APP-002"
                        });
                        
                        // Add a Stakeholder Values template worksheet
                        var stakeholderWorksheet = package.Workbook.Worksheets.Add("Stakeholder Values Template");
                        SetHeaders(stakeholderWorksheet, new[] {
                            "Process Name*", "Stakeholder Type*", "Value Description*", "Priority (1-10)*", 
                            "Related Process ID", "Metric of Success*"
                        });
                        AddSampleRow(stakeholderWorksheet, new[] {
                            "Order Processing", "Customer", "Fast order fulfillment", "9",
                            "PROC-001", "Order fulfilled within 24 hours"
                        });
                        break;
                        
                    case "business-product-service":
                        SetHeaders(worksheet, new[] { 
                            "Product/Service Name*", "Description*", "Target Market/Customer*", 
                            "Value Proposition*", "Revenue Model*", "Key Features*", "Dependencies", "Competitors*" 
                        });
                        break;
                        
                    // Application Architecture artifacts
                    case "application-framework":
                        SetHeaders(worksheet, new[] { 
                            "Application Name*", "Application Description*", "Business Function Supported*", 
                            "Technology Stack", "Integration Points", "Owner*", "Version*", "Deployment Environment*",
                            "Rationalization Category", "Business Value", "Technical Fit", "Annual Cost",
                            "Risk Score", "Current State", "Future State", "Rationalization Rationale",
                            "Redundant Applications", "Replacement Options", "Assessment Date"
                        });
                        
                        // Add Application Recommendations template worksheet
                        var recommendationsWorksheet = package.Workbook.Worksheets.Add("Recommendations Template");
                        SetHeaders(recommendationsWorksheet, new[] {
                            "Application Name*", "Recommendation Type*", "Description*", "Business*",
                            "Estimated Cost Savings*", "Estimated Implementation Cost*", "Timeline*",
                            "Dependencies", "Status*"
                        });
                        AddSampleRow(recommendationsWorksheet, new[] {
                            "CRM System", "Consolidate", "Merge with ERP system", "Sales",
                            "50000", "25000", "Q3 2023", "ERP System", "Pending"
                        });
                        break;
                        
                    case "application-business-requirement":
                        SetHeaders(worksheet, new[] { 
                            "Requirement ID*", "Requirement Description*", "Priority*", 
                            "Stakeholders", "Status*", "Assigned To", "Due Date*", "Dependencies" 
                        });
                        break;
                        
                    case "service-catalogue":
                        SetHeaders(worksheet, new[] { 
                            "Service Name*", "Description*", "Service Owner*", "SLA*", 
                            "Cost*", "Supported Applications", "Availability*", "Dependencies" 
                        });
                        break;
                        
                    case "application-cross-reference":
                        SetHeaders(worksheet, new[] { 
                            "Source Application*", "Target Application*", "Integration Type*", 
                            "Data Exchanged*", "Frequency*", "Owner*", "Security Requirements", "Dependencies" 
                        });
                        break;
                        
                    case "application-security":
                        SetHeaders(worksheet, new[] { 
                            "Application Name*", "Security Requirement*", "Compliance Standards", 
                            "Vulnerabilities", "Mitigation Strategies", "Owner*", "Last Audit Date*", "Dependencies" 
                        });
                        break;
                        
                    case "application-data-cross-reference":
                        SetHeaders(worksheet, new[] { 
                            "Application Name*", "Data Entity*", "Data Usage*", 
                            "Data Sensitivity*", "Access Controls*", "Owner*", "Data Retention Policy*", "Dependencies" 
                        });
                        break;
                        
                    // Data Architecture artifacts
                    case "data-governance":
                        SetHeaders(worksheet, new[] { 
                            "Data Entity*", "Data Owner*", "Data Steward*", 
                            "Data Classification*", "Data Lifecycle*", "Retention Policy*", "Dependencies" 
                        });
                        break;
                        
                    case "information-hierarchy":
                        SetHeaders(worksheet, new[] { 
                            "Information Level*", "Description*", "Parent Level", 
                            "Data Elements*", "Owner*", "Classification*", "Dependencies" 
                        });
                        break;
                        
                    case "information-requirements":
                        SetHeaders(worksheet, new[] { 
                            "Requirement ID*", "Requirement Description*", "Priority*", 
                            "Data Elements*", "Owner*", "Status*", "Dependencies" 
                        });
                        break;
                        
                    case "data-flow":
                        SetHeaders(worksheet, new[] { 
                            "Data Source*", "Data Destination*", "Data Flow Description*", 
                            "Data Elements*", "Frequency*", "Volume*", "Dependencies" 
                        });
                        break;
                        
                    case "logical-data":
                        SetHeaders(worksheet, new[] { 
                            "Entity Name*", "Description*", "Attributes*", 
                            "Relationships*", "Owner*", "Classification*", "Dependencies" 
                        });
                        break;
                        
                    case "data-security":
                        SetHeaders(worksheet, new[] { 
                            "Data Entity*", "Security Requirement*", "Access Controls*", 
                            "Encryption*", "Masking Requirements*", "Owner*", "Last Review Date*", "Dependencies" 
                        });
                        break;
                        
                    // Infrastructure Architecture artifacts
                    case "infrastructure-business-requirement":
                        SetHeaders(worksheet, new[] { 
                            "Requirement ID*", "Requirement Description*", "Priority*", 
                            "Infrastructure Components*", "Owner*", "Status*", "Dependencies" 
                        });
                        break;
                        
                    case "system-application":
                        SetHeaders(worksheet, new[] { 
                            "System Name*", "Application Name*", "Integration Type*", 
                            "System Role*", "Owner*", "Dependencies", "Performance Requirements" 
                        });
                        break;
                        
                    case "resource-needs":
                        SetHeaders(worksheet, new[] { 
                            "Resource Type*", "Resource Name*", "Description*", 
                            "Quantity*", "Owner*", "Cost*", "Dependencies", "Allocation Status*" 
                        });
                        break;
                        
                    case "system-business":
                        SetHeaders(worksheet, new[] { 
                            "System Name*", "Business Unit*", "Business Process Supported*", 
                            "Owner*", "Criticality*", "Dependencies", "Performance Metrics", "Security Requirements" 
                        });
                        break;
                        
                    case "infrastructure-security":
                        SetHeaders(worksheet, new[] { 
                            "System Name*", "Security Requirement*", "Compliance Standards", 
                            "Vulnerabilities", "Mitigation Strategies*", "Owner*", "Last Audit Date*", "Dependencies" 
                        });
                        break;
                        
                    case "system-data":
                        SetHeaders(worksheet, new[] { 
                            "System Name*", "Data Entity*", "Data Usage*", 
                            "Data Sensitivity*", "Access Controls*", "Owner*", "Data Retention Policy*", "Dependencies" 
                        });
                        break;
                        
                    default:
                        throw new ArgumentException($"Template not implemented for artifact type: {artifactType}");
                }
                        
                // Add a note about required fields and sample data
                var noteRow = worksheet.Dimension.End.Row + 2;
                worksheet.Cells[noteRow, 1].Value = "Note: Fields marked with * are required.";
                worksheet.Cells[noteRow, 1].Style.Font.Bold = true;
                worksheet.Cells[noteRow, 1].Style.Font.Italic = true;
                
                // Auto-size columns
                worksheet.Cells.AutoFitColumns();
                        
                return package.GetAsByteArray();
            }
        }

        // Helper method to set up headers for templates
        private void SetHeaders(ExcelWorksheet worksheet, string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }
        }

        // Helper method to add a sample row to template worksheets
        private void AddSampleRow(ExcelWorksheet worksheet, string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                worksheet.Cells[2, i + 1].Value = values[i];
                worksheet.Cells[2, i + 1].Style.Font.Italic = true;
                worksheet.Cells[2, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[2, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportEAArtifacts(string artifactType, IFormFile importFile, bool updateExisting = false, bool skipHeaderRow = true)
        {
            if (string.IsNullOrEmpty(artifactType))
            {
                return BadRequest(new { success = false, message = "Artifact type is required" });
            }

            if (importFile == null || importFile.Length == 0)
            {
                return BadRequest(new { success = false, message = "No file was uploaded" });
            }

            // Check file extension (only allow Excel files)
            var fileExtension = Path.GetExtension(importFile.FileName).ToLowerInvariant();
            if (fileExtension != ".xlsx" && fileExtension != ".xls")
            {
                return BadRequest(new { success = false, message = "Only Excel files (.xlsx or .xls) are supported" });
            }

            _logger.LogInformation($"{User.Identity.Name} is importing {artifactType} from file {importFile.FileName}");

            try
            {
                // Process the Excel file
                using (var stream = new MemoryStream())
                {
                    await importFile.CopyToAsync(stream);
                    stream.Position = 0;

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            return BadRequest(new { success = false, message = "The Excel file does not contain any worksheets" });
                        }

                        // Determine the start row (skip header if specified)
                        int startRow = skipHeaderRow ? 2 : 1;
                        
                        // Process the data based on artifact type
                        var (importedCount, updatedCount, errorCount) = await ImportArtifactsFromExcelAsync(
                            artifactType, worksheet, startRow, updateExisting);

                        // Return success response with import details
                        return Ok(new { 
                            success = true, 
                            message = $"Import successful. Imported {importedCount} new records, updated {updatedCount} existing records. {errorCount} errors encountered.",
                            imported = importedCount,
                            updated = updatedCount,
                            errors = errorCount
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error importing {artifactType} from Excel file");
                return StatusCode(500, new { success = false, message = $"An error occurred during import: {ex.Message}" });
            }
        }

        private async Task<(int imported, int updated, int errors)> ImportArtifactsFromExcelAsync(
            string artifactType, ExcelWorksheet worksheet, int startRow, bool updateExisting)
        {
            int importedCount = 0;
            int updatedCount = 0;
            int errorCount = 0;
            
            int rowCount = worksheet.Dimension.Rows;
            int colCount = worksheet.Dimension.Columns;
            
            // Get required field configurations for the artifact type
            var fieldConfig = GetArtifactFieldConfiguration(artifactType);
            if (fieldConfig == null)
            {
                throw new InvalidOperationException($"Configuration not found for artifact type: {artifactType}");
            }
            
            // Get the DbSet for the specified artifact type
            var entityType = GetEntityTypeForArtifactType(artifactType);
            if (entityType == null)
            {
                throw new InvalidOperationException($"Entity type not found for artifact type: {artifactType}");
            }
            
            var dbSet = _context.GetType().GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType && 
                                    p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                                    p.PropertyType.GetGenericArguments()[0] == entityType)
                ?.GetValue(_context);
            
            if (dbSet == null)
            {
                throw new InvalidOperationException($"DbSet not found for entity type: {entityType.Name}");
            }
            
            // Parse and validate headers (optional but recommended)
            var headers = new Dictionary<string, int>();
            for (int col = 1; col <= colCount; col++)
            {
                string headerText = worksheet.Cells[1, col].Text.Trim();
                if (!string.IsNullOrEmpty(headerText))
                {
                    headers[headerText] = col;
                }
            }
            
            // Process each row
            for (int row = startRow; row <= rowCount; row++)
            {
                try
                {
                    // Check if row is empty
                    bool isEmptyRow = true;
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Text))
                        {
                            isEmptyRow = false;
                            break;
                        }
                    }
                    
                    if (isEmptyRow)
                    {
                        continue; // Skip empty rows
                    }
                    
                    // Create a new instance of the entity or find existing one
                    object entity = null;
                    bool isNewEntity = true;
                    
                    if (updateExisting)
                    {
                        // Build a query to find existing entity based on key fields
                        var keyValues = new Dictionary<string, object>();
                        foreach (var keyField in fieldConfig.KeyFields)
                        {
                            int col = headers.ContainsKey(keyField) ? headers[keyField] : fieldConfig.ColumnMappings[keyField];
                            keyValues[keyField] = GetCellValue(worksheet.Cells[row, col]);
                        }
                        
                        // Use reflection to find the entity
                        var method = typeof(EntityFrameworkQueryableExtensions).GetMethods()
                            .FirstOrDefault(m => m.Name == "FirstOrDefaultAsync" && m.GetParameters().Length == 2);
                        
                        if (method != null)
                        {
                            var parameter = Expression.Parameter(entityType, "e");
                            Expression predicate = null;
                            
                            foreach (var kvp in keyValues)
                            {
                                var property = Expression.Property(parameter, kvp.Key);
                                var constant = Expression.Constant(kvp.Value);
                                var equality = Expression.Equal(property, constant);
                                
                                predicate = predicate == null ? equality : Expression.AndAlso(predicate, equality);
                            }
                            
                            var lambda = Expression.Lambda(predicate, parameter);
                            var genericMethod = method.MakeGenericMethod(entityType);
                            
                            // Get the queryable from DbSet
                            var queryable = dbSet.GetType().GetMethod("AsQueryable").Invoke(dbSet, null);
                            
                            // Execute the query
                            entity = await (dynamic)genericMethod.Invoke(null, new[] { queryable, lambda });
                            
                            if (entity != null)
                            {
                                isNewEntity = false;
                            }
                        }
                    }
                    
                    // If no existing entity found or not updating, create new instance
                    if (entity == null)
                    {
                        entity = Activator.CreateInstance(entityType);
                    }
                    
                    // Validate required fields
                    bool hasRequiredFields = true;
                    foreach (var requiredField in fieldConfig.RequiredFields)
                    {
                        int col = headers.ContainsKey(requiredField) ? headers[requiredField] : fieldConfig.ColumnMappings[requiredField];
                        var value = GetCellValue(worksheet.Cells[row, col]);
                        
                        if (value == null || (value is string strValue && string.IsNullOrWhiteSpace(strValue)))
                        {
                            hasRequiredFields = false;
                            _logger.LogWarning($"Row {row}: Missing required field '{requiredField}' for {artifactType}");
                            break;
                        }
                    }
                    
                    if (!hasRequiredFields)
                    {
                        errorCount++;
                        continue;
                    }
                    
                    // Map the fields to the entity
                    foreach (var mapping in fieldConfig.ColumnMappings)
                    {
                        string fieldName = mapping.Key;
                        int col = headers.ContainsKey(fieldName) ? headers[fieldName] : mapping.Value;
                        
                        if (col <= colCount) // Make sure the column exists in the worksheet
                        {
                            var property = entityType.GetProperty(fieldName);
                            if (property != null)
                            {
                                var cellValue = worksheet.Cells[row, col].Value;
                                var typedValue = ConvertValueToPropertyType(cellValue, property.PropertyType, fieldConfig.ListFields.Contains(fieldName));
                                
                                property.SetValue(entity, typedValue);
                            }
                        }
                    }
                    
                    // Add or update the entity
                    if (isNewEntity)
                    {
                        // Add new entity to DbSet
                        var addMethod = dbSet.GetType().GetMethod("Add");
                        addMethod.Invoke(dbSet, new[] { entity });
                        importedCount++;
                    }
                    else
                    {
                        // Entity is already being tracked for update
                        updatedCount++;
                    }
                }
                catch (Exception ex)
                {
                    errorCount++;
                    _logger.LogError(ex, $"Error importing {artifactType} at row {row}");
                }
            }
            
            // Save changes to database
            await _context.SaveChangesAsync();
            
            return (importedCount, updatedCount, errorCount);
        }

        private object GetCellValue(ExcelRange cell)
        {
            if (cell.Value == null)
            {
                return null;
            }
            
            return cell.Value;
        }

        private object ConvertValueToPropertyType(object value, Type propertyType, bool isList)
        {
            if (value == null)
            {
                return propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
            }
            
            if (isList)
            {
                // Handle list types (assuming string lists for simplicity)
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var elementType = propertyType.GetGenericArguments()[0];
                    
                    if (elementType == typeof(string) && value is string stringValue)
                    {
                        // Split by semicolon and trim each value
                        return stringValue.Split(';')
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                            .Select(s => s.Trim())
                            .ToList();
                    }
                }
            }
            
            // Handle simple type conversions
            if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                if (value is DateTime dateValue)
                {
                    return dateValue;
                }
                
                if (value is string stringValue && DateTime.TryParse(stringValue, out DateTime parsedDate))
                {
                    return parsedDate;
                }
                
                if (propertyType == typeof(DateTime?))
                {
                    return null;
                }
            }
            
            // Try simple conversion for other types
            try
            {
                return Convert.ChangeType(value, propertyType);
            }
            catch
            {
                return propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
            }
        }

        private class ArtifactFieldConfiguration
        {
            public List<string> RequiredFields { get; set; } = new List<string>();
            public List<string> KeyFields { get; set; } = new List<string>();
            public List<string> ListFields { get; set; } = new List<string>();
            public Dictionary<string, int> ColumnMappings { get; set; } = new Dictionary<string, int>();
        }

        private ArtifactFieldConfiguration GetArtifactFieldConfiguration(string artifactType)
        {
            // This method would return the field configuration for each artifact type
            // This could be stored in a JSON file or in a database table
            switch (artifactType)
            {
                case "infrastructure-security":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "SystemName", "SecurityRequirement", "Owner" },
                        KeyFields = new List<string> { "SystemName", "SecurityRequirement" },
                        ListFields = new List<string> { "ComplianceStandards", "Vulnerabilities", "MitigationStrategies", "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "SystemName", 1 },
                            { "SecurityRequirement", 2 },
                            { "ComplianceStandards", 3 },
                            { "Vulnerabilities", 4 },
                            { "MitigationStrategies", 5 },
                            { "Owner", 6 },
                            { "LastAuditDate", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "business-strategy":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "BusinessGoal", "StrategicObjective", "Timeframe" },
                        KeyFields = new List<string> { "BusinessGoal", "StrategicObjective" },
                        ListFields = new List<string> { "KeyPerformanceIndicators", "Stakeholders", "Risks", "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "BusinessGoal", 1 },
                            { "StrategicObjective", 2 },
                            { "KeyPerformanceIndicators", 3 },
                            { "Stakeholders", 4 },
                            { "Timeframe", 5 },
                            { "OrganizationalVisionAlignment", 6 },
                            { "Risks", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "operation-model":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "OperationalProcess", "ProcessOwner" },
                        KeyFields = new List<string> { "OperationalProcess", "ProcessOwner" },
                        ListFields = new List<string> { "Inputs", "Outputs", "KeyActivities", "ResourcesRequired", "PerformanceMetrics", "Challenges" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "OperationalProcess", 1 },
                            { "ProcessOwner", 2 },
                            { "Inputs", 3 },
                            { "Outputs", 4 },
                            { "KeyActivities", 5 },
                            { "ResourcesRequired", 6 },
                            { "PerformanceMetrics", 7 },
                            { "Challenges", 8 }
                        }
                    };
                    
                case "organization-view":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "DepartmentOrUnit", "RolesAndResponsibilities", "ReportingStructure" },
                        KeyFields = new List<string> { "DepartmentOrUnit" },
                        ListFields = new List<string> { "KeyPersonnel", "CollaborationPoints", "OrganizationalGoals", "Challenges" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "DepartmentOrUnit", 1 },
                            { "RolesAndResponsibilities", 2 },
                            { "ReportingStructure", 3 },
                            { "KeyPersonnel", 4 },
                            { "CollaborationPoints", 5 },
                            { "OrganizationalGoals", 6 },
                            { "Challenges", 7 }
                        }
                    };
                    
                case "capability-map":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "CapabilityName", "CapabilityDescription", "MaturityLevel", "Owner" },
                        KeyFields = new List<string> { "CapabilityName" },
                        ListFields = new List<string> { "Dependencies", "KeyProcesses", "Gaps" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "CapabilityName", 1 },
                            { "CapabilityDescription", 2 },
                            { "MaturityLevel", 3 },
                            { "Owner", 4 },
                            { "Dependencies", 5 },
                            { "KeyProcesses", 6 },
                            { "Gaps", 7 }
                        }
                    };
                    
                case "process-model":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "ProcessName", "ProcessDescription", "Owner" },
                        KeyFields = new List<string> { "ProcessName" },
                        ListFields = new List<string> { "Inputs", "Outputs", "StepsWorkflow", "ToolsSystemsUsed", "PerformanceMetrics" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "ProcessName", 1 },
                            { "ProcessDescription", 2 },
                            { "Inputs", 3 },
                            { "Outputs", 4 },
                            { "StepsWorkflow", 5 },
                            { "Owner", 6 },
                            { "ToolsSystemsUsed", 7 },
                            { "PerformanceMetrics", 8 }
                        }
                    };
                    
                case "business-product-service":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "ProductServiceName", "Description", "TargetMarketCustomer", "ValueProposition" },
                        KeyFields = new List<string> { "ProductServiceName" },
                        ListFields = new List<string> { "KeyFeatures", "Dependencies", "Competitors" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "ProductServiceName", 1 },
                            { "Description", 2 },
                            { "TargetMarketCustomer", 3 },
                            { "ValueProposition", 4 },
                            { "RevenueModel", 5 },
                            { "KeyFeatures", 6 },
                            { "Dependencies", 7 },
                            { "Competitors", 8 }
                        }
                    };
                    
                case "application-framework":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "ApplicationName", "ApplicationDescription", "BusinessFunctionSupported", "Owner" },
                        KeyFields = new List<string> { "ApplicationName" },
                        ListFields = new List<string> { "TechnologyStack", "IntegrationPoints" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "ApplicationName", 1 },
                            { "ApplicationDescription", 2 },
                            { "BusinessFunctionSupported", 3 },
                            { "TechnologyStack", 4 },
                            { "IntegrationPoints", 5 },
                            { "Owner", 6 },
                            { "Version", 7 },
                            { "DeploymentEnvironment", 8 }
                        }
                    };
                    
                case "application-business-requirement":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "RequirementId", "RequirementDescription", "Priority", "Status" },
                        KeyFields = new List<string> { "RequirementId" },
                        ListFields = new List<string> { "Stakeholders", "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "RequirementId", 1 },
                            { "RequirementDescription", 2 },
                            { "Priority", 3 },
                            { "Stakeholders", 4 },
                            { "Status", 5 },
                            { "AssignedTo", 6 },
                            { "DueDate", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "service-catalogue":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "ServiceName", "ServiceDescription", "ServiceOwner", "SLA" },
                        KeyFields = new List<string> { "ServiceName" },
                        ListFields = new List<string> { "SupportedApplications", "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "ServiceName", 1 },
                            { "ServiceDescription", 2 },
                            { "ServiceOwner", 3 },
                            { "SLA", 4 },
                            { "Cost", 5 },
                            { "SupportedApplications", 6 },
                            { "Availability", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "application-cross-reference":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "SourceApplication", "TargetApplication", "IntegrationType", "DataExchanged" },
                        KeyFields = new List<string> { "SourceApplication", "TargetApplication" },
                        ListFields = new List<string> { "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "SourceApplication", 1 },
                            { "TargetApplication", 2 },
                            { "IntegrationType", 3 },
                            { "DataExchanged", 4 },
                            { "Frequency", 5 },
                            { "Owner", 6 },
                            { "SecurityRequirements", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "application-security":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "ApplicationName", "SecurityRequirement", "Owner" },
                        KeyFields = new List<string> { "ApplicationName", "SecurityRequirement" },
                        ListFields = new List<string> { "ComplianceStandards", "Vulnerabilities", "MitigationStrategies", "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "ApplicationName", 1 },
                            { "SecurityRequirement", 2 },
                            { "ComplianceStandards", 3 },
                            { "Vulnerabilities", 4 },
                            { "MitigationStrategies", 5 },
                            { "Owner", 6 },
                            { "LastAuditDate", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "application-data-cross-reference":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "ApplicationName", "DataEntity", "DataUsage", "DataSensitivity" },
                        KeyFields = new List<string> { "ApplicationName", "DataEntity" },
                        ListFields = new List<string> { "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "ApplicationName", 1 },
                            { "DataEntity", 2 },
                            { "DataUsage", 3 },
                            { "DataSensitivity", 4 },
                            { "AccessControls", 5 },
                            { "Owner", 6 },
                            { "DataRetentionPolicy", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "data-governance":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "DataEntity", "DataOwner", "DataSteward", "DataPolicies" },
                        KeyFields = new List<string> { "DataEntity" },
                        ListFields = new List<string> { "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "DataEntity", 1 },
                            { "DataOwner", 2 },
                            { "DataSteward", 3 },
                            { "DataQualityMetrics", 4 },
                            { "DataPolicies", 5 },
                            { "ComplianceRequirements", 6 },
                            { "DataLifecycle", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "information-hierarchy":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "InformationLevel", "Description", "Owner", "Usage" },
                        KeyFields = new List<string> { "InformationLevel", "ParentInformation", "ChildInformation" },
                        ListFields = new List<string> { "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "InformationLevel", 1 },
                            { "ParentInformation", 2 },
                            { "ChildInformation", 3 },
                            { "Description", 4 },
                            { "Owner", 5 },
                            { "Usage", 6 },
                            { "Dependencies", 7 },
                            { "SecurityLevel", 8 }
                        }
                    };
                    
                case "information-requirements":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "RequirementId", "RequirementDescription", "DataEntity", "Priority" },
                        KeyFields = new List<string> { "RequirementId" },
                        ListFields = new List<string> { "Stakeholders" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "RequirementId", 1 },
                            { "RequirementDescription", 2 },
                            { "DataEntity", 3 },
                            { "Priority", 4 },
                            { "Stakeholders", 5 },
                            { "Status", 6 },
                            { "AssignedTo", 7 },
                            { "DueDate", 8 }
                        }
                    };
                    
                case "data-flow":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "DataSource", "DataDestination", "DataFlowDescription", "DataTypeName" },
                        KeyFields = new List<string> { "DataSource", "DataDestination" },
                        ListFields = new List<string> { "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "DataSource", 1 },
                            { "DataDestination", 2 },
                            { "DataFlowDescription", 3 },
                            { "DataTypeName", 4 },
                            { "Frequency", 5 },
                            { "Owner", 6 },
                            { "SecurityRequirements", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "logical-data":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "EntityName", "Description", "Owner" },
                        KeyFields = new List<string> { "EntityName" },
                        ListFields = new List<string> { "Attributes", "Relationships", "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "EntityName", 1 },
                            { "Attributes", 2 },
                            { "Relationships", 3 },
                            { "PrimaryKey", 4 },
                            { "ForeignKey", 5 },
                            { "Owner", 6 },
                            { "Description", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "data-security":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "DataEntity", "SecurityRequirement", "AccessControls", "EncryptionRequirements" },
                        KeyFields = new List<string> { "DataEntity", "SecurityRequirement" },
                        ListFields = new List<string> { "ComplianceStandards", "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "DataEntity", 1 },
                            { "SecurityRequirement", 2 },
                            { "AccessControls", 3 },
                            { "EncryptionRequirements", 4 },
                            { "ComplianceStandards", 5 },
                            { "Owner", 6 },
                            { "LastAuditDate", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "infrastructure-business-requirement":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "RequirementId", "RequirementDescription", "BusinessUnit", "Priority" },
                        KeyFields = new List<string> { "RequirementId" },
                        ListFields = new List<string> { "Stakeholders" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "RequirementId", 1 },
                            { "RequirementDescription", 2 },
                            { "BusinessUnit", 3 },
                            { "Priority", 4 },
                            { "Stakeholders", 5 },
                            { "Status", 6 },
                            { "AssignedTo", 7 },
                            { "DueDate", 8 }
                        }
                    };
                    
                case "system-application":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "SystemName", "ApplicationName", "IntegrationType", "DataExchanged" },
                        KeyFields = new List<string> { "SystemName", "ApplicationName" },
                        ListFields = new List<string> { "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "SystemName", 1 },
                            { "ApplicationName", 2 },
                            { "IntegrationType", 3 },
                            { "DataExchanged", 4 },
                            { "Frequency", 5 },
                            { "Owner", 6 },
                            { "SecurityRequirements", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                case "resource-needs":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "ResourceType", "ResourceName", "Description", "Quantity" },
                        KeyFields = new List<string> { "ResourceType", "ResourceName" },
                        ListFields = new List<string> { "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "ResourceType", 1 },
                            { "ResourceName", 2 },
                            { "Description", 3 },
                            { "Quantity", 4 },
                            { "Owner", 5 },
                            { "Cost", 6 },
                            { "Dependencies", 7 },
                            { "AllocationStatus", 8 }
                        }
                    };
                    
                case "system-business":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "SystemName", "BusinessUnit", "BusinessProcessSupported", "Owner" },
                        KeyFields = new List<string> { "SystemName", "BusinessUnit" },
                        ListFields = new List<string> { "Dependencies", "PerformanceMetrics" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "SystemName", 1 },
                            { "BusinessUnit", 2 },
                            { "BusinessProcessSupported", 3 },
                            { "Owner", 4 },
                            { "Criticality", 5 },
                            { "Dependencies", 6 },
                            { "PerformanceMetrics", 7 },
                            { "SecurityRequirements", 8 }
                        }
                    };
                    
                case "system-data":
                    return new ArtifactFieldConfiguration
                    {
                        RequiredFields = new List<string> { "SystemName", "DataEntity", "DataUsage", "DataSensitivity" },
                        KeyFields = new List<string> { "SystemName", "DataEntity" },
                        ListFields = new List<string> { "Dependencies" },
                        ColumnMappings = new Dictionary<string, int>
                        {
                            { "SystemName", 1 },
                            { "DataEntity", 2 },
                            { "DataUsage", 3 },
                            { "DataSensitivity", 4 },
                            { "AccessControls", 5 },
                            { "Owner", 6 },
                            { "DataRetentionPolicy", 7 },
                            { "Dependencies", 8 }
                        }
                    };
                    
                default:
                    throw new NotImplementedException($"Configuration for artifact type '{artifactType}' is not implemented");
            }
        }

        private Type GetEntityTypeForArtifactType(string artifactType)
        {
            // Map artifact type to entity type
            switch (artifactType)
            {
                case "business-strategy":
                    return typeof(BusinessStrategyView);
                case "operation-model":
                    return typeof(OperationModel);
                case "organization-view":
                    return typeof(OrganizationView);
                case "capability-map":
                    return typeof(CapabilityMap);
                case "process-model":
                    return typeof(ProcessModel);
                case "business-product-service":
                    return typeof(BusinessProductServiceView);
                case "application-framework":
                    return typeof(ApplicationArchitectureFramework);
                case "application-business-requirement":
                    return typeof(ApplicationBusinessRequirementsView);
                case "service-catalogue":
                    return typeof(ServiceCatalogue);
                case "application-cross-reference":
                    return typeof(ApplicationToApplicationCrossReference);
                case "application-security":
                    return typeof(ApplicationSecurityModel);
                case "application-data-cross-reference":
                    return typeof(ApplicationDataCrossReference);
                case "data-governance":
                    return typeof(DataGovernanceModel);
                case "information-hierarchy":
                    return typeof(InformationHierarchyView);
                case "information-requirements":
                    return typeof(InformationRequirementsView);
                case "data-flow":
                    return typeof(DataFlowModel);
                case "logical-data":
                    return typeof(LogicalDataModel);
                case "data-security":
                    return typeof(DataSecurityModel);
                case "infrastructure-business-requirement":
                    return typeof(InfrastructureBusinessRequirementsView);
                case "system-application":
                    return typeof(SystemToApplicationCrossReference);
                case "resource-needs":
                    return typeof(ResourceNeedsModel);
                case "system-business":
                    return typeof(SystemToBusinessCrossReference);
                case "infrastructure-security":
                    return typeof(InfrastructureSecurityModel);
                case "system-data":
                    return typeof(SystemDataCrossReference);
                default:
                    throw new NotImplementedException($"Entity type for artifact type '{artifactType}' is not implemented");
            }
        }
    }
}