using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMEA.Data;
using SIMEA.Models;
using SIMEA.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class EAOutputController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EAOutputController> _logger;

        public EAOutputController(ApplicationDbContext context, ILogger<EAOutputController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: EAOutput/ViewA
        public async Task<IActionResult> ViewA()
        {
            _logger.LogInformation("EAOutputController.ViewA called by {user}", User.Identity.Name);
            // Strategic Alignment View
            var model = new StrategicAlignmentViewModel
            {
                Title = "Strategic Alignment View",
                Description = "Visualizes how business strategies align with capabilities and applications",
                BusinessStrategies = await _context.BusinessStrategyViews.ToListAsync(),
                Capabilities = await _context.CapabilityMaps.ToListAsync(),
                Applications = await _context.ApplicationArchitectureFrameworks.ToListAsync(),
                GeneratedDate = DateTime.Now,
                GeneratedBy = User.Identity.Name,
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = User.Identity.Name
            };

            // Get relevant artifact links
            var links = await _context.ArtifactLinks.ToListAsync();
            
            // Strategy to Capability links
            model.StrategyCapabilityLinks = links.Where(l => 
                l.SourceArtifactType == ArtifactType.BusinessStrategy && 
                l.TargetArtifactType == ArtifactType.CapabilityMap).ToList();
            
            // Capability to Application links
            model.CapabilityApplicationLinks = links.Where(l => 
                l.SourceArtifactType == ArtifactType.CapabilityMap && 
                l.TargetArtifactType == ArtifactType.ApplicationFramework).ToList();

            return View(model);
        }

        // GET: EAOutput/ViewB
        public async Task<IActionResult> ViewB()
        {
            _logger.LogInformation("EAOutputController.ViewB called by {user}", User.Identity.Name);
            // Application Portfolio Dashboard
            var model = new ApplicationPortfolioViewModel
            {
                Title = "Application Portfolio Dashboard",
                Description = "Overview of all applications, their statuses, and relationships",
                Applications = await _context.ApplicationArchitectureFrameworks.ToListAsync(),
                Services = await _context.ServiceCatalogues.ToListAsync(),
                AppDependencies = await _context.ApplicationToApplicationCrossReferences.ToListAsync(),
                BusinessRequirements = await _context.ApplicationBusinessRequirementsViews.ToListAsync(),
                GeneratedDate = DateTime.Now,
                GeneratedBy = User.Identity.Name,
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = User.Identity.Name
            };

            return View(model);
        }

        // GET: EAOutput/ViewC
        public async Task<IActionResult> ViewC()
        {
            _logger.LogInformation("EAOutputController.ViewC called by {user}", User.Identity.Name);
            // Architecture Roadmap
            var model = new ArchitectureRoadmapViewModel
            {
                Title = "Architecture Roadmap",
                Description = "Timeline view of planned architecture changes and implementations",
                TimelinePhases = new List<string> { "Current State", "Transition (6 mo)", "Transition (12 mo)", "Future State" },
                GeneratedDate = DateTime.Now,
                GeneratedBy = User.Identity.Name,
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = User.Identity.Name
            };

            // Create sample roadmap items based on existing artifacts
            var roadmapItems = new List<RoadmapItem>();
            
            // Add business strategy items
            var businessStrategies = await _context.BusinessStrategyViews.Take(3).ToListAsync();
            foreach (var strategy in businessStrategies)
            {
                roadmapItems.Add(new RoadmapItem
                {
                    Id = $"BS-{strategy.Id}",
                    Name = strategy.BusinessGoal,
                    Phase = "Current State",
                    Domain = "Business",
                    Description = strategy.StrategicObjective,
                    Dependencies = new List<string>(),
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(6),
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                });
            }
            
            // Add application items
            var applications = await _context.ApplicationArchitectureFrameworks.Take(5).ToListAsync();
            foreach (var app in applications)
            {
                roadmapItems.Add(new RoadmapItem
                {
                    Id = $"APP-{app.Id}",
                    Name = app.ApplicationName,
                    Phase = "Transition (6 mo)",
                    Domain = "Application",
                    Description = app.ApplicationDescription,
                    Dependencies = new List<string> { roadmapItems.FirstOrDefault()?.Id },
                    StartDate = DateTime.Now.AddMonths(3),
                    EndDate = DateTime.Now.AddMonths(9),
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                });
            }
            
            // Add data items
            var dataModels = await _context.LogicalDataModels.Take(2).ToListAsync();
            foreach (var data in dataModels)
            {
                roadmapItems.Add(new RoadmapItem
                {
                    Id = $"DATA-{data.Id}",
                    Name = data.EntityName,
                    Phase = "Transition (12 mo)",
                    Domain = "Data",
                    Description = data.Description,
                    Dependencies = new List<string> { roadmapItems.FirstOrDefault(r => r.Domain == "Application")?.Id },
                    StartDate = DateTime.Now.AddMonths(6),
                    EndDate = DateTime.Now.AddMonths(12),
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                });
            }
            
            // Add infrastructure items
            var resources = await _context.ResourceNeedsModels.Take(2).ToListAsync();
            foreach (var resource in resources)
            {
                roadmapItems.Add(new RoadmapItem
                {
                    Id = $"INFR-{resource.Id}",
                    Name = resource.ResourceName,
                    Phase = "Future State",
                    Domain = "Infrastructure",
                    Description = resource.Description,
                    Dependencies = new List<string> { roadmapItems.FirstOrDefault(r => r.Domain == "Data")?.Id },
                    StartDate = DateTime.Now.AddMonths(9),
                    EndDate = DateTime.Now.AddMonths(18),
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                });
            }
            
            model.RoadmapItems = roadmapItems;
            return View(model);
        }

        // GET: EAOutput/ViewD
        public async Task<IActionResult> ViewD()
        {
            _logger.LogInformation("EAOutputController.ViewD called by {user}", User.Identity.Name);
            // Cross-Domain Impact Analysis
            var model = new CrossDomainImpactViewModel
            {
                Title = "Cross-Domain Impact Analysis",
                Description = "Analysis of how changes in one domain impact artifacts in other domains"
            };

            // Get all artifacts
            var allArtifacts = new List<ArtifactNodeViewModel>();
            allArtifacts.AddRange(await GetArtifactsByDomain(ArtifactDomain.Business));
            allArtifacts.AddRange(await GetArtifactsByDomain(ArtifactDomain.Application));
            allArtifacts.AddRange(await GetArtifactsByDomain(ArtifactDomain.Data));
            allArtifacts.AddRange(await GetArtifactsByDomain(ArtifactDomain.Infrastructure));
            model.AllArtifacts = allArtifacts;
            
            // Get cross-domain connections
            var links = await _context.ArtifactLinks.ToListAsync();
            var crossDomainConnections = await BuildArtifactConnections(links.Where(l => 
                GetDomainForArtifactType(l.SourceArtifactType.ToString()) != 
                GetDomainForArtifactType(l.TargetArtifactType.ToString())).ToList());
            model.CrossDomainConnections = crossDomainConnections;
            
            // Build impact analysis items
            var impactItems = new List<ImpactAnalysisItem>();
            
            // Sample key artifacts to analyze
            var keyArtifacts = allArtifacts.Take(5).ToList();
            
            foreach (var artifact in keyArtifacts)
            {
                var impactedArtifacts = new List<ArtifactNodeViewModel>();
                
                // Find artifacts directly impacted (target of links)
                var directImpacts = crossDomainConnections
                    .Where(c => c.SourceNodeId == artifact.NodeId)
                    .Select(c => c.TargetNodeId)
                    .ToList();
                
                foreach (var targetId in directImpacts)
                {
                    var impactedArtifact = allArtifacts.FirstOrDefault(a => a.NodeId == targetId);
                    if (impactedArtifact != null)
                    {
                        impactedArtifacts.Add(impactedArtifact);
                    }
                }
                
                impactItems.Add(new ImpactAnalysisItem
                {
                    SourceId = artifact.Id,
                    SourceName = artifact.Name,
                    SourceDomain = artifact.Domain,
                    ImpactedArtifacts = impactedArtifacts,
                    TotalImpactScore = impactedArtifacts.Count
                });
            }
            
            model.ImpactItems = impactItems;
            
            return View(model);
        }

        // GET: EAOutput/ViewE
        public async Task<IActionResult> ViewE()
        {
            _logger.LogInformation("EAOutputController.ViewE called by {user}", User.Identity.Name);
            // Technology Standards Compliance
            var model = new TechnologyStandardsViewModel
            {
                Title = "Technology Standards Compliance",
                Description = "Analysis of compliance with technology standards across the enterprise",
                StandardsItems = new List<TechnologyStandardItem>(),
                ComplianceByDomain = new Dictionary<string, int>(),
                NonCompliantApplications = new List<ApplicationArchitectureFramework>()
            };

            // Get all applications
            var applications = await _context.ApplicationArchitectureFrameworks.ToListAsync();
            
            // Create sample technology standards
            var standards = new List<TechnologyStandardItem>
            {
                new TechnologyStandardItem {
                    Category = "Application Platform",
                    StandardName = ".NET Core",
                    Description = "Enterprise standard for application development",
                    Version = "6.0+",
                    CompliantCount = 0,
                    NonCompliantCount = 0,
                    CompliantApplications = new List<string>(),
                    NonCompliantApplications = new List<string>()
                },
                new TechnologyStandardItem {
                    Category = "Database",
                    StandardName = "SQL Server",
                    Description = "Primary database platform",
                    Version = "2019+",
                    CompliantCount = 0,
                    NonCompliantCount = 0,
                    CompliantApplications = new List<string>(),
                    NonCompliantApplications = new List<string>()
                },
                new TechnologyStandardItem {
                    Category = "Web Frontend",
                    StandardName = "Angular",
                    Description = "JavaScript framework for web applications",
                    Version = "13+",
                    CompliantCount = 0,
                    NonCompliantCount = 0,
                    CompliantApplications = new List<string>(),
                    NonCompliantApplications = new List<string>()
                }
            };
            
            // Generate sample compliance data
            var random = new Random();
            foreach (var app in applications)
            {
                // Analyze tech stack
                var techStack = app.TechnologyStack ?? new List<string>();
                
                foreach (var standard in standards)
                {
                    bool isCompliant = false;
                    
                    // Simulate compliance check
                    if (techStack.Any(t => t?.Contains(standard.StandardName) == true))
                    {
                        isCompliant = random.Next(10) < 7; // 70% chance of compliance
                    }
                    
                    if (isCompliant)
                    {
                        standard.CompliantCount++;
                        standard.CompliantApplications.Add(app.ApplicationName);
                    }
                    else
                    {
                        standard.NonCompliantCount++;
                        standard.NonCompliantApplications.Add(app.ApplicationName);
                        
                        if (!model.NonCompliantApplications.Contains(app))
                        {
                            model.NonCompliantApplications.Add(app);
                        }
                    }
                }
            }
            
            model.StandardsItems = standards;
            
            // Add domain compliance rates
            model.ComplianceByDomain.Add("Application", 75);
            model.ComplianceByDomain.Add("Data", 85);
            model.ComplianceByDomain.Add("Infrastructure", 60);
            
            return View(model);
        }

        // GET: EAOutput/ViewF
        public async Task<IActionResult> ViewF()
        {
            _logger.LogInformation("EAOutputController.ViewF called by {user}", User.Identity.Name);
            // Data Flow Diagram
            var model = new DataFlowDiagramViewModel
            {
                Title = "Data Flow Diagram",
                Description = "Visualization of data flows between applications and systems",
                DataFlows = await _context.DataFlowModels.ToListAsync(),
                LogicalDataModels = await _context.LogicalDataModels.ToListAsync(),
                Applications = await _context.ApplicationArchitectureFrameworks.ToListAsync(),
                AppDataReferences = await _context.ApplicationDataCrossReferences.ToListAsync(),
                SystemDataReferences = await _context.SystemDataCrossReferences.ToListAsync()
            };

            return View(model);
        }

        // GET: EAOutput/ViewG
        public async Task<IActionResult> ViewG()
        {
            _logger.LogInformation("EAOutputController.ViewG called by {user}", User.Identity.Name);
            // Capability to Application Mapping
            var model = new CapabilityApplicationMappingViewModel
            {
                Title = "Capability to Application Mapping",
                Description = "Maps business capabilities to the applications that support them",
                Capabilities = await _context.CapabilityMaps.ToListAsync(),
                Applications = await _context.ApplicationArchitectureFrameworks.ToListAsync(),
                GeneratedDate = DateTime.Now,
                GeneratedBy = User.Identity.Name,
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = User.Identity.Name
            };

            // Generate sample mappings based on existing data and artifact links
            var links = await _context.ArtifactLinks
                .Where(l => l.SourceArtifactType == ArtifactType.CapabilityMap && 
                       l.TargetArtifactType == ArtifactType.ApplicationFramework)
                .ToListAsync();
            
            var mappings = new List<CapabilityApplicationMapping>();
            
            // First use existing links if available
            foreach (var link in links)
            {
                var capability = await _context.CapabilityMaps.FindAsync(int.Parse(link.SourceArtifactId));
                var application = await _context.ApplicationArchitectureFrameworks.FindAsync(int.Parse(link.TargetArtifactId));
                
                if (capability != null && application != null)
                {
                    mappings.Add(new CapabilityApplicationMapping
                    {
                        CapabilityId = capability.Id.ToString(),
                        CapabilityName = capability.CapabilityName,
                        ApplicationId = application.Id.ToString(),
                        ApplicationName = application.ApplicationName,
                        SupportLevel = GetRandomSupportLevel(),
                        Description = link.LinkDescription ?? $"Support for {capability.CapabilityName}",
                        MappingDate = DateTime.Now,
                        MappedBy = User.Identity.Name,
                        ReviewDate = DateTime.Now.AddMonths(6)
                    });
                }
            }
            
            // If we don't have enough links, create some random mappings
            if (mappings.Count < 10 && model.Capabilities.Any() && model.Applications.Any())
            {
                var rand = new Random();
                var supportLevels = new[] { "Full", "Partial", "None" };
                
                foreach (var capability in model.Capabilities.Take(5))
                {
                    // Randomly assign 1-3 applications to each capability
                    var appCount = rand.Next(1, 4);
                    var apps = model.Applications.OrderBy(x => Guid.NewGuid()).Take(appCount);
                    
                    foreach (var app in apps)
                    {
                        // Check if mapping already exists
                        if (!mappings.Any(m => m.CapabilityId == capability.Id.ToString() && 
                                         m.ApplicationId == app.Id.ToString()))
                        {
                            mappings.Add(new CapabilityApplicationMapping
                            {
                                CapabilityId = capability.Id.ToString(),
                                CapabilityName = capability.CapabilityName,
                                ApplicationId = app.Id.ToString(),
                                ApplicationName = app.ApplicationName,
                                SupportLevel = supportLevels[rand.Next(supportLevels.Length)],
                                Description = $"Support for {capability.CapabilityName}",
                                MappingDate = DateTime.Now.AddDays(-rand.Next(0, 90)),
                                MappedBy = User.Identity.Name,
                                ReviewDate = DateTime.Now.AddMonths(rand.Next(3, 9))
                            });
                        }
                    }
                }
            }
            
            model.Mappings = mappings;
            return View(model);
        }

        private string GetRandomSupportLevel()
        {
            var levels = new[] { "Full", "Partial", "None" };
            return levels[new Random().Next(levels.Length)];
        }

        // GET: EAOutput/ViewH
        public async Task<IActionResult> ViewH()
        {
            _logger.LogInformation("EAOutputController.ViewH called by {user}", User.Identity.Name);
            // Business Value Heat Map
            var model = new BusinessValueHeatMapViewModel
            {
                Title = "Business Value Heat Map",
                Description = "Visualizes the business value and strategic importance of various architecture components",
                Products = await _context.BusinessProductServiceViews.ToListAsync(),
                Capabilities = await _context.CapabilityMaps.ToListAsync(),
                GeneratedDate = DateTime.Now,
                GeneratedBy = User.Identity.Name,
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = User.Identity.Name
            };

            // Generate heat map items
            var heatMapItems = new List<HeatMapItem>();
            var random = new Random();
            
            // Add heat map items for products/services
            foreach (var product in model.Products)
            {
                heatMapItems.Add(new HeatMapItem
                {
                    ItemId = $"PS-{product.Id}",
                    ItemName = product.ProductServiceName,
                    ItemType = "Product/Service",
                    BusinessValue = random.Next(1, 11), // 1-10
                    StrategicImportance = random.Next(1, 11), // 1-10
                    RiskLevel = random.Next(1, 11), // 1-10
                    Domain = "Business",
                    AssessmentDate = DateTime.Now.AddDays(-random.Next(0, 30))
                });
            }
            
            // Add heat map items for capabilities
            foreach (var capability in model.Capabilities)
            {
                heatMapItems.Add(new HeatMapItem
                {
                    ItemId = $"CAP-{capability.Id}",
                    ItemName = capability.CapabilityName,
                    ItemType = "Capability",
                    BusinessValue = random.Next(1, 11), // 1-10
                    StrategicImportance = random.Next(1, 11), // 1-10
                    RiskLevel = random.Next(1, 11), // 1-10
                    Domain = "Business",
                    AssessmentDate = DateTime.Now.AddDays(-random.Next(0, 30))
                });
            }
            
            // Add heat map items for applications (just a few)
            var applications = await _context.ApplicationArchitectureFrameworks.Take(5).ToListAsync();
            foreach (var app in applications)
            {
                heatMapItems.Add(new HeatMapItem
                {
                    ItemId = $"APP-{app.Id}",
                    ItemName = app.ApplicationName,
                    ItemType = "Application",
                    BusinessValue = random.Next(1, 11), // 1-10
                    StrategicImportance = random.Next(1, 11), // 1-10
                    RiskLevel = random.Next(1, 11), // 1-10
                    Domain = "Application",
                    AssessmentDate = DateTime.Now.AddDays(-random.Next(0, 30))
                });
            }
            
            model.HeatMapItems = heatMapItems;
            return View(model);
        }

        // GET: EAOutput/ViewI
        public async Task<IActionResult> ViewI()
        {
            _logger.LogInformation("EAOutputController.ViewI called by {user}", User.Identity.Name);
            // Integration Landscape
            var model = new IntegrationLandscapeViewModel
            {
                Title = "Integration Landscape",
                Description = "Overview of all integration points between systems and applications",
                AppIntegrations = await _context.ApplicationToApplicationCrossReferences.ToListAsync(),
                SystemAppIntegrations = await _context.SystemToApplicationCrossReferences.ToListAsync(),
                IntegrationPatterns = new List<IntegrationPattern>()
            };

            // Generate integration patterns based on data
            var patterns = new Dictionary<string, int>();
            var patternExamples = new Dictionary<string, List<string>>();
            
            // Count patterns from app integrations
            foreach (var integration in model.AppIntegrations)
            {
                var patternType = integration.IntegrationType ?? "Unknown";
                
                if (!patterns.ContainsKey(patternType))
                {
                    patterns[patternType] = 0;
                    patternExamples[patternType] = new List<string>();
                }
                
                patterns[patternType]++;
                patternExamples[patternType].Add($"{integration.SourceApplication} → {integration.TargetApplication}");
            }
            
            // Count patterns from system-app integrations
            foreach (var integration in model.SystemAppIntegrations)
            {
                var patternType = integration.IntegrationType ?? "Unknown";
                
                if (!patterns.ContainsKey(patternType))
                {
                    patterns[patternType] = 0;
                    patternExamples[patternType] = new List<string>();
                }
                
                patterns[patternType]++;
                patternExamples[patternType].Add($"{integration.SystemName} → {integration.ApplicationName}");
            }
            
            // Create integration pattern objects
            foreach (var pattern in patterns)
            {
                model.IntegrationPatterns.Add(new IntegrationPattern
                {
                    PatternType = pattern.Key,
                    Count = pattern.Value,
                    Examples = patternExamples[pattern.Key].Take(3).ToList() // Limit examples to 3
                });
            }
            
            // If no patterns found, add sample data
            if (model.IntegrationPatterns.Count == 0)
            {
                model.IntegrationPatterns.Add(new IntegrationPattern
                {
                    PatternType = "API/REST",
                    Count = 5,
                    Examples = new List<string> { "CRM → ERP", "Customer Portal → CRM", "Mobile App → Customer Portal" }
                });
                
                model.IntegrationPatterns.Add(new IntegrationPattern
                {
                    PatternType = "File Transfer",
                    Count = 3,
                    Examples = new List<string> { "ERP → Data Warehouse", "Vendor System → ERP" }
                });
                
                model.IntegrationPatterns.Add(new IntegrationPattern
                {
                    PatternType = "Messaging",
                    Count = 2,
                    Examples = new List<string> { "Order System → Fulfillment System" }
                });
            }

            return View(model);
        }

        // GET: EAOutput/ViewJ
        public async Task<IActionResult> ViewJ()
        {
            _logger.LogInformation("EAOutputController.ViewJ called by {user}", User.Identity.Name);
            // Business Process to Tech Stack Alignment
            var model = new BusinessProcessTechAlignmentViewModel
            {
                Title = "Business Process to Tech Stack Alignment",
                Description = "Maps business processes to supporting technology stacks",
                BusinessProcesses = await _context.ProcessModels.ToListAsync(),
                Applications = await _context.ApplicationArchitectureFrameworks.ToListAsync(),
                GeneratedDate = DateTime.Now,
                GeneratedBy = User.Identity.Name,
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = User.Identity.Name
            };

            // Generate mappings
            var mappings = new List<ProcessTechMapping>();
            var random = new Random();
            
            // Match processes with applications
            foreach (var process in model.BusinessProcesses)
            {
                // Randomly assign 1-2 applications to each process
                var appCount = random.Next(1, 3);
                var apps = model.Applications.OrderBy(x => Guid.NewGuid()).Take(appCount);
                
                foreach (var app in apps)
                {
                    mappings.Add(new ProcessTechMapping
                    {
                        ProcessId = process.Id.ToString(),
                        ProcessName = process.ProcessName,
                        ApplicationId = app.Id.ToString(),
                        ApplicationName = app.ApplicationName,
                        TechnologyStack = app.TechnologyStack.FirstOrDefault() ?? "Unknown",
                        AlignmentScore = random.Next(1, 6), // 1-5
                        MappingDate = DateTime.Now.AddDays(-random.Next(0, 60))
                    });
                }
            }
            
            model.Mappings = mappings;
            return View(model);
        }

        // GET: EAOutput/ViewK
        public async Task<IActionResult> ViewK()
        {
            _logger.LogInformation("EAOutputController.ViewK called by {user}", User.Identity.Name);
            // Service Dependency Map
            var model = new ServiceDependencyMapViewModel
            {
                Title = "Service Dependency Map",
                Description = "Visualizes dependencies between services in the architecture",
                Services = await _context.ServiceCatalogues.ToListAsync(),
                Applications = await _context.ApplicationArchitectureFrameworks.ToListAsync(),
                GeneratedDate = DateTime.Now,
                GeneratedBy = User.Identity.Name,
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = User.Identity.Name
            };

            // Generate dependencies
            var dependencies = new List<ServiceDependency>();
            var random = new Random();
            
            // For each service, create 1-3 dependencies to other services
            foreach (var service in model.Services)
            {
                var targetCount = random.Next(1, 4); // 1-3 dependencies
                var targets = model.Services
                    .Where(s => s.Id != service.Id) // Exclude self
                    .OrderBy(x => Guid.NewGuid()) // Random order
                    .Take(targetCount);
                
                foreach (var target in targets)
                {
                    dependencies.Add(new ServiceDependency
                    {
                        SourceServiceId = service.Id.ToString(),
                        SourceServiceName = service.ServiceName,
                        TargetServiceId = target.Id.ToString(),
                        TargetServiceName = target.ServiceName,
                        DependencyType = GetRandomDependencyType(),
                        CriticalityLevel = GetRandomCriticalityLevel(),
                        IdentifiedDate = DateTime.Now.AddDays(-random.Next(0, 90))
                    });
                }
            }
            
            model.Dependencies = dependencies;
            return View(model);
        }
        
        private string GetRandomDependencyType()
        {
            var types = new[] { "Data", "Functional", "API", "Infrastructure", "Security" };
            return types[new Random().Next(types.Length)];
        }
        
        private string GetRandomCriticalityLevel()
        {
            var levels = new[] { "Critical", "High", "Medium", "Low" };
            return levels[new Random().Next(levels.Length)];
        }

        // GET: EAOutput/ViewL
        public async Task<IActionResult> ViewL()
        {
            _logger.LogInformation("EAOutputController.ViewL called by {user}", User.Identity.Name);
            // Value Stream Modeling
            var model = new ValueStreamModelingViewModel
            {
                Title = "Value Stream Modeling",
                Description = "Visualization of value streams, identifying waste and opportunities for improvement",
                BusinessProcesses = await _context.ProcessModels.ToListAsync(),
                GeneratedDate = DateTime.Now,
                GeneratedBy = User.Identity.Name,
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = User.Identity.Name
            };

            // Filter to include only processes that have value stream data
            model.BusinessProcesses = model.BusinessProcesses
                .Where(p => p.CustomerType != null && p.Stages != null && p.Stages.Any())
                .ToList();
            
            // If we don't have any value stream processes, populate with sample data
            if (!model.BusinessProcesses.Any())
            {
                var processes = await _context.ProcessModels.ToListAsync();
                var applications = await _context.ApplicationArchitectureFrameworks.ToListAsync();
                var random = new Random();
                
                if (processes.Any())
                {
                    // Create a working copy of processes to avoid modifying the database
                    var workingProcesses = processes.Select(p => new ProcessModel
                    {
                        Id = p.Id,
                        ProcessName = p.ProcessName,
                        ProcessDescription = p.ProcessDescription,
                        Owner = p.Owner,
                        DateCreated = p.DateCreated,
                        DateModified = p.DateModified,
                        UserCreated = p.UserCreated,
                        UserModified = p.UserModified
                    }).ToList();

                    // Create sample value stream data for Order to Cash
                    var orderToCash = workingProcesses.FirstOrDefault(p => p.ProcessName.Contains("Order") || p.Id == workingProcesses.First().Id);
                    if (orderToCash != null)
                    {
                        orderToCash.CustomerType = "External Customer";
                        orderToCash.KeyMetrics = new List<string> { "Order Fulfillment Time", "Cash Conversion Cycle", "Order Accuracy" };
                        orderToCash.CycleEfficiency = 0.35m;
                        orderToCash.LeadTime = TimeSpan.FromDays(12);
                        orderToCash.Bottlenecks = new List<string> { "Credit approval process", "Inventory management delays" };
                        orderToCash.ImprovementOpportunities = new List<string> { "Automate credit approvals", "Implement real-time inventory tracking" };
                        orderToCash.MappingDate = DateTime.Now.AddDays(-30);
                        orderToCash.Stages = new List<ProcessStage>();
                        
                        // Add stages to Order to Cash
                        orderToCash.Stages.Add(new ProcessStage
                        {
                            Id = "VS-001-S1",
                            Name = "Order Receipt",
                            Description = "Customer places order through various channels",
                            ProcessTime = TimeSpan.FromHours(2),
                            WaitTime = TimeSpan.FromHours(6),
                            ProcessIds = processes.Take(1).Select(p => p.Id.ToString()).ToList(),
                            ApplicationIds = applications.Take(1).Select(a => a.Id.ToString()).ToList(),
                            IsValueAdd = true,
                            PercentComplete = 98.5m,
                            DefectRate = 2.1m,
                            Issues = new List<string> { "Manual entry errors" }
                        });
                        
                        orderToCash.Stages.Add(new ProcessStage
                        {
                            Id = "VS-001-S2",
                            Name = "Credit Approval",
                            Description = "Validate customer credit and approve order",
                            ProcessTime = TimeSpan.FromHours(4),
                            WaitTime = TimeSpan.FromDays(2),
                            ProcessIds = processes.Skip(1).Take(1).Select(p => p.Id.ToString()).ToList(),
                            ApplicationIds = processes.Skip(1).Take(1).Select(p => p.Id.ToString()).ToList(),
                            IsValueAdd = false,
                            PercentComplete = 82.5m,
                            DefectRate = 5.0m,
                            Issues = new List<string> { "Manual approval required for most orders", "Duplicate credit checks" }
                        });
                        
                        orderToCash.Stages.Add(new ProcessStage
                        {
                            Id = "VS-001-S3",
                            Name = "Fulfillment",
                            Description = "Pick, pack and ship order",
                            ProcessTime = TimeSpan.FromDays(1),
                            WaitTime = TimeSpan.FromDays(2),
                            ProcessIds = processes.Skip(2).Take(1).Select(p => p.Id.ToString()).ToList(),
                            ApplicationIds = processes.Skip(2).Take(1).Select(p => p.Id.ToString()).ToList(),
                            IsValueAdd = true,
                            PercentComplete = 95.0m,
                            DefectRate = 1.2m,
                            Issues = new List<string> { "Inventory discrepancies" }
                        });
                        
                        orderToCash.Stages.Add(new ProcessStage
                        {
                            Id = "VS-001-S4",
                            Name = "Invoicing",
                            Description = "Generate and send invoice",
                            ProcessTime = TimeSpan.FromHours(4),
                            WaitTime = TimeSpan.FromDays(1),
                            ProcessIds = processes.Skip(3).Take(1).Select(p => p.Id.ToString()).ToList(),
                            ApplicationIds = processes.Skip(3).Take(1).Select(p => p.Id.ToString()).ToList(),
                            IsValueAdd = true,
                            PercentComplete = 99.1m,
                            DefectRate = 0.5m,
                            Issues = new List<string>()
                        });
                        
                        orderToCash.Stages.Add(new ProcessStage
                        {
                            Id = "VS-001-S5",
                            Name = "Payment Processing",
                            Description = "Receive and process payment",
                            ProcessTime = TimeSpan.FromHours(2),
                            WaitTime = TimeSpan.FromDays(5),
                            ProcessIds = processes.Skip(4).Take(1).Select(p => p.Id.ToString()).ToList(),
                            ApplicationIds = processes.Skip(4).Take(1).Select(p => p.Id.ToString()).ToList(),
                            IsValueAdd = true,
                            PercentComplete = 97.8m,
                            DefectRate = 0.8m,
                            Issues = new List<string> { "Delayed payment reconciliation" }
                        });
                        
                        // Add stakeholder values
                        orderToCash.StakeholderValues = new List<StakeholderValueItem>
                        {
                            new StakeholderValueItem
                            {
                                StakeholderType = "Customer",
                                ValueDescription = "Fast, accurate order fulfillment",
                                Priority = 9,
                                ProcessId = orderToCash.Id.ToString(),
                                MetricOfSuccess = "Order fulfillment time < 5 days"
                            },
                            new StakeholderValueItem
                            {
                                StakeholderType = "Finance",
                                ValueDescription = "Reduced days sales outstanding",
                                Priority = 8,
                                ProcessId = orderToCash.Id.ToString(),
                                MetricOfSuccess = "DSO < 30 days"
                            },
                            new StakeholderValueItem
                            {
                                StakeholderType = "Operations",
                                ValueDescription = "Efficient inventory management",
                                Priority = 7,
                                ProcessId = orderToCash.Id.ToString(),
                                MetricOfSuccess = "Inventory accuracy > 98%"
                            }
                        };
                    }
                    
                    // Create sample value stream data for another process
                    var secondProcess = workingProcesses.Skip(1).FirstOrDefault() ?? workingProcesses.FirstOrDefault();
                    if (secondProcess != null && secondProcess != orderToCash)
                    {
                        secondProcess.CustomerType = "Internal and External Stakeholders";
                        secondProcess.KeyMetrics = new List<string> { "Time to Market", "Development Costs", "First-Year ROI" };
                        secondProcess.CycleEfficiency = 0.28m;
                        secondProcess.LeadTime = TimeSpan.FromDays(120);
                        secondProcess.Bottlenecks = new List<string> { "Approval gates", "Resource constraints", "Testing cycles" };
                        secondProcess.ImprovementOpportunities = new List<string> { "Implement agile methodologies", "Parallel testing", "Early stakeholder involvement" };
                        secondProcess.MappingDate = DateTime.Now.AddDays(-45);
                        secondProcess.Stages = new List<ProcessStage>();
                        
                        // Add stages
                        secondProcess.Stages.Add(new ProcessStage
                        {
                            Id = "VS-002-S1",
                            Name = "Concept Development",
                            Description = "Generate and refine product ideas",
                            ProcessTime = TimeSpan.FromDays(15),
                            WaitTime = TimeSpan.FromDays(5),
                            ProcessIds = processes.Skip(1).Take(1).Select(p => p.Id.ToString()).ToList(),
                            ApplicationIds = processes.Skip(1).Take(1).Select(p => p.Id.ToString()).ToList(),
                            IsValueAdd = true,
                            PercentComplete = 90.0m,
                            DefectRate = 15.0m,
                            Issues = new List<string> { "Unclear requirements", "Limited market research" }
                        });
                        
                        secondProcess.StakeholderValues = new List<StakeholderValueItem>
                        {
                            new StakeholderValueItem
                            {
                                StakeholderType = "Product Management",
                                ValueDescription = "Faster time to market",
                                Priority = 9,
                                ProcessId = secondProcess.Id.ToString(),
                                MetricOfSuccess = "Time to market < 120 days"
                            },
                            new StakeholderValueItem
                            {
                                StakeholderType = "Engineering",
                                ValueDescription = "Reduced technical debt",
                                Priority = 8,
                                ProcessId = secondProcess.Id.ToString(),
                                MetricOfSuccess = "Technical debt < 15% of development time"
                            }
                        };
                    }
                    
                    // Use the modified in-memory models for the view
                    model.BusinessProcesses = workingProcesses
                        .Where(p => p.CustomerType != null && p.Stages != null && p.Stages.Any())
                        .ToList();
                }
            }
            
            return View(model);
        }

        // GET: EAOutput/ViewM
        public async Task<IActionResult> ViewM()
        {
            _logger.LogInformation("EAOutputController.ViewM called by {user}", User.Identity.Name);
            // Application Rationalization
            var model = new ApplicationRationalizationViewModel
            {
                Title = "Application Rationalization",
                Description = "Analysis of application portfolio to determine the optimal future state",
                Applications = await _context.ApplicationArchitectureFrameworks.ToListAsync(),
                CategoryCounts = new Dictionary<string, int>(),
                GeneratedDate = DateTime.Now,
                GeneratedBy = User.Identity.Name,
                LastModifiedDate = DateTime.Now,
                LastModifiedBy = User.Identity.Name
            };

            // Check if rationalization data exists and populate if needed
            var categoryCounts = new Dictionary<string, int>
            {
                { "Tolerate", 0 },
                { "Invest", 0 },
                { "Migrate", 0 },
                { "Eliminate", 0 }
            };
            
            // Filter apps with rationalization data and count categories
            foreach (var app in model.Applications)
            {
                if (!string.IsNullOrEmpty(app.RationalizationCategory))
                {
                    if (categoryCounts.ContainsKey(app.RationalizationCategory))
                    {
                        categoryCounts[app.RationalizationCategory]++;
                    }
                }
            }
            
            // If we don't have enough rationalization data, populate with sample data
            if (categoryCounts.Sum(kv => kv.Value) < model.Applications.Count / 2)
            {
                var random = new Random();
                
                // Create working copies of applications to avoid modifying the database
                var workingApps = model.Applications.Select(a => new ApplicationArchitectureFramework
                {
                    Id = a.Id,
                    ApplicationName = a.ApplicationName,
                    ApplicationDescription = a.ApplicationDescription,
                    BusinessFunctionSupported = a.BusinessFunctionSupported,
                    TechnologyStack = a.TechnologyStack,
                    IntegrationPoints = a.IntegrationPoints,
                    Owner = a.Owner,
                    Version = a.Version,
                    DeploymentEnvironment = a.DeploymentEnvironment,
                    DateCreated = a.DateCreated,
                    DateModified = a.DateModified,
                    UserCreated = a.UserCreated,
                    UserModified = a.UserModified,
                    RationalizationCategory = a.RationalizationCategory,
                    BusinessValue = a.BusinessValue,
                    TechnicalFit = a.TechnicalFit,
                    AnnualCost = a.AnnualCost,
                    RiskScore = a.RiskScore,
                    CurrentState = a.CurrentState,
                    FutureState = a.FutureState,
                    RationalizationRationale = a.RationalizationRationale,
                    AssessmentDate = a.AssessmentDate,
                    AssessedBy = a.AssessedBy,
                    RedundantWith = a.RedundantWith,
                    ReplacementOptions = a.ReplacementOptions,
                    Recommendations = a.Recommendations != null 
                        ? new List<ApplicationRecommendation>(a.Recommendations) 
                        : new List<ApplicationRecommendation>()
                }).ToList();
                
                // Assess each application that doesn't already have rationalization data
                foreach (var app in workingApps.Where(a => string.IsNullOrEmpty(a.RationalizationCategory)))
                {
                    // Randomly distribute applications across categories with some weighting
                    string category;
                    if (random.Next(100) < 30)
                    {
                        category = "Tolerate"; // 30%
                    }
                    else if (random.Next(100) < 50)
                    {
                        category = "Invest"; // 35%
                    }
                    else if (random.Next(100) < 60)
                    {
                        category = "Migrate"; // 20%
                    }
                    else
                    {
                        category = "Eliminate"; // 15%
                    }
                    
                    categoryCounts[category]++;
                    
                    // Generate business and technical scores with some correlation to the category
                    int businessValue = 0;
                    int technicalFit = 0;
                    
                    switch (category)
                    {
                        case "Tolerate":
                            businessValue = random.Next(5, 8); // Medium-high business value
                            technicalFit = random.Next(2, 6); // Low-medium technical fit
                            break;
                        case "Invest":
                            businessValue = random.Next(7, 11); // High business value
                            technicalFit = random.Next(7, 11); // High technical fit
                            break;
                        case "Migrate":
                            businessValue = random.Next(7, 11); // High business value
                            technicalFit = random.Next(1, 5); // Low technical fit
                            break;
                        case "Eliminate":
                            businessValue = random.Next(1, 5); // Low business value
                            technicalFit = random.Next(1, 6); // Low-medium technical fit
                            break;
                    }
                    
                    // Update application with rationalization data
                    app.RationalizationCategory = category;
                    app.BusinessValue = businessValue;
                    app.TechnicalFit = technicalFit;
                    app.AnnualCost = random.Next(50000, 500000);
                    app.RiskScore = Math.Round((decimal)random.Next(1, 11) / 2, 1);
                    app.CurrentState = GetCurrentStateDescription(category);
                    app.FutureState = GetFutureStateDescription(category);
                    app.RationalizationRationale = GetRationaleDescription(category, businessValue, technicalFit);
                    app.AssessmentDate = DateTime.Now.AddDays(-random.Next(1, 90));
                    app.AssessedBy = User.Identity.Name;
                    app.RedundantWith = GetPotentialRedundancies(app.Id.ToString(), workingApps, category, random);
                    app.ReplacementOptions = GetReplacementOptions(category, random);
                    
                    // Create recommendations for this application
                    if (app.Recommendations == null)
                    {
                        app.Recommendations = new List<ApplicationRecommendation>();
                    }
                    
                    if (category == "Migrate" || category == "Eliminate")
                    {
                        app.Recommendations.Add(new ApplicationRecommendation
                        {
                            Id = $"REC-{app.Id}",
                            ApplicationId = app.Id.ToString(),
                            RecommendationType = category == "Migrate" ? "Migrate" : "Retire",
                            Description = category == "Migrate" 
                                ? $"Migrate {app.ApplicationName} to modern platform" 
                                : $"Retire {app.ApplicationName} and consolidate functionality",
                            Business = app.BusinessFunctionSupported,
                            EstimatedCostSavings = category == "Eliminate" 
                                ? app.AnnualCost.Value 
                                : app.AnnualCost.Value * 0.3m,
                            EstimatedImplementationCost = category == "Eliminate"
                                ? app.AnnualCost.Value * 0.5m
                                : app.AnnualCost.Value * 0.8m,
                            Timeline = category == "Eliminate" ? "6 months" : "12 months",
                            Dependencies = app.RedundantWith ?? new List<string>(),
                            Status = GetRandomStatus()
                        });
                    }
                    else if (category == "Invest" && random.Next(100) < 60)
                    {
                        app.Recommendations.Add(new ApplicationRecommendation
                        {
                            Id = $"REC-{app.Id}",
                            ApplicationId = app.Id.ToString(),
                            RecommendationType = "Enhance",
                            Description = $"Add features to {app.ApplicationName} to improve business capabilities",
                            Business = app.BusinessFunctionSupported,
                            EstimatedCostSavings = app.AnnualCost.Value * 0.1m,
                            EstimatedImplementationCost = app.AnnualCost.Value * 0.4m,
                            Timeline = "9 months",
                            Dependencies = new List<string>(),
                            Status = GetRandomStatus()
                        });
                    }
                }
                
                // Use the working in-memory applications for the view
                model.Applications = workingApps;
            }
            
            model.CategoryCounts = categoryCounts;
            
            return View(model);
        }
        
        private string GetCurrentStateDescription(string category)
        {
            switch (category)
            {
                case "Tolerate":
                    return "Functional but with technical limitations";
                case "Invest":
                    return "Strategic application with strong alignment";
                case "Migrate":
                    return "Business critical but technically outdated";
                case "Eliminate":
                    return "Limited business value and high technical debt";
                default:
                    return "Unknown status";
            }
        }
        
        private string GetFutureStateDescription(string category)
        {
            switch (category)
            {
                case "Tolerate":
                    return "Maintain as-is until replacement is feasible";
                case "Invest":
                    return "Enhance with additional capabilities";
                case "Migrate":
                    return "Migrate to modern platform/architecture";
                case "Eliminate":
                    return "Decommission and consolidate functionality";
                default:
                    return "TBD";
            }
        }
        
        private string GetRationaleDescription(string category, int businessValue, int technicalFit)
        {
            switch (category)
            {
                case "Tolerate":
                    return $"Business value ({businessValue}/10) justifies continued use despite technical limitations ({technicalFit}/10)";
                case "Invest":
                    return $"Strong business value ({businessValue}/10) and technical fit ({technicalFit}/10) makes this a strategic asset";
                case "Migrate":
                    return $"High business value ({businessValue}/10) but poor technical fit ({technicalFit}/10) indicates modernization needed";
                case "Eliminate":
                    return $"Low business value ({businessValue}/10) and poor technical fit ({technicalFit}/10) does not justify continued investment";
                default:
                    return "Additional assessment needed";
            }
        }
        
        private List<string> GetPotentialRedundancies(string appId, List<ApplicationArchitectureFramework> allApps, string category, Random random)
        {
            if (category == "Eliminate" || (category == "Migrate" && random.Next(100) < 30))
            {
                // For Eliminate and some Migrate, find potential redundant applications
                return allApps
                    .Where(a => a.Id.ToString() != appId)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(random.Next(1, 3))
                    .Select(a => a.ApplicationName)
                    .ToList();
            }
            
            return new List<string>();
        }
        
        private List<string> GetReplacementOptions(string category, Random random)
        {
            if (category == "Migrate" || category == "Eliminate")
            {
                string[] options = { 
                    "Commercial off-the-shelf (COTS) solution", 
                    "Cloud-based SaaS alternative",
                    "Modern custom-developed application",
                    "Extend existing enterprise platform",
                    "Consolidate with other applications"
                };
                
                return options
                    .OrderBy(x => Guid.NewGuid())
                    .Take(random.Next(1, 3))
                    .ToList();
            }
            
            return new List<string>();
        }
        
        private string GetRandomStatus()
        {
            var statuses = new[] { "Pending", "Approved", "In Progress", "Completed" };
            return statuses[new Random().Next(statuses.Length)];
        }

        #region Helper Methods
        // Helper method to categorize artifacts by domain
        private async Task<List<ArtifactNodeViewModel>> GetArtifactsByDomain(ArtifactDomain domain)
        {
            var artifacts = new List<ArtifactNodeViewModel>();
            
            // Filter artifact types by domain
            var domainTypes = GetArtifactTypesByDomain(domain);
            
            foreach (var artifactType in domainTypes)
            {
                // Get artifacts for each type
                var typeArtifacts = await GetArtifactsOfType(artifactType);
                artifacts.AddRange(typeArtifacts);
            }
            
            return artifacts;
        }

        // Helper method to get artifact types by domain
        private List<ArtifactType> GetArtifactTypesByDomain(ArtifactDomain domain)
        {
            switch (domain)
            {
                case ArtifactDomain.Business:
                    return new List<ArtifactType>
                    {
                        ArtifactType.BusinessStrategy,
                        ArtifactType.OperationModel,
                        ArtifactType.OrganizationView,
                        ArtifactType.CapabilityMap,
                        ArtifactType.ProcessModel,
                        ArtifactType.BusinessProductService
                    };
                    
                case ArtifactDomain.Application:
                    return new List<ArtifactType>
                    {
                        ArtifactType.ApplicationFramework,
                        ArtifactType.ApplicationBusinessRequirement,
                        ArtifactType.ServiceCatalogue,
                        ArtifactType.ApplicationCrossReference,
                        ArtifactType.ApplicationSecurity,
                        ArtifactType.ApplicationDataCrossReference
                    };
                    
                case ArtifactDomain.Data:
                    return new List<ArtifactType>
                    {
                        ArtifactType.DataGovernance,
                        ArtifactType.InformationHierarchy,
                        ArtifactType.InformationRequirements,
                        ArtifactType.DataFlow,
                        ArtifactType.LogicalData,
                        ArtifactType.DataSecurity
                    };
                    
                case ArtifactDomain.Infrastructure:
                    return new List<ArtifactType>
                    {
                        ArtifactType.InfrastructureBusinessRequirement,
                        ArtifactType.SystemToApplication,
                        ArtifactType.ResourceNeeds,
                        ArtifactType.SystemToBusiness,
                        ArtifactType.InfrastructureSecurity,
                        ArtifactType.SystemData
                    };
                    
                default:
                    return new List<ArtifactType>();
            }
        }

        // Helper method to get artifacts by type
        private async Task<List<ArtifactNodeViewModel>> GetArtifactsOfType(ArtifactType artifactType)
        {
            var artifacts = new List<ArtifactNodeViewModel>();
            
            try
            {
                switch (artifactType)
                {
                    case ArtifactType.BusinessStrategy:
                        var strategies = await _context.BusinessStrategyViews.ToListAsync();
                        artifacts.AddRange(strategies.Select(s => new ArtifactNodeViewModel
                        {
                            Id = s.Id.ToString(),
                            Name = s.BusinessGoal,
                            Type = artifactType,
                            Domain = ArtifactDomain.Business,
                            Description = s.StrategicObjective
                        }));
                        break;
                        
                    case ArtifactType.OperationModel:
                        var operations = await _context.OperationModels.ToListAsync();
                        artifacts.AddRange(operations.Select(o => new ArtifactNodeViewModel
                        {
                            Id = o.Id.ToString(),
                            Name = o.OperationalProcess,
                            Type = artifactType,
                            Domain = ArtifactDomain.Business,
                            Description = string.Join(", ", o.KeyActivities?.Take(2) ?? new List<string>())
                        }));
                        break;
                        
                    case ArtifactType.OrganizationView:
                        var orgViews = await _context.OrganizationViews.ToListAsync();
                        artifacts.AddRange(orgViews.Select(o => new ArtifactNodeViewModel
                        {
                            Id = o.Id.ToString(),
                            Name = o.DepartmentOrUnit,
                            Type = artifactType,
                            Domain = ArtifactDomain.Business,
                            Description = o.RolesAndResponsibilities
                        }));
                        break;
                        
                    case ArtifactType.CapabilityMap:
                        var capabilities = await _context.CapabilityMaps.ToListAsync();
                        artifacts.AddRange(capabilities.Select(c => new ArtifactNodeViewModel
                        {
                            Id = c.Id.ToString(),
                            Name = c.CapabilityName,
                            Type = artifactType,
                            Domain = ArtifactDomain.Business,
                            Description = c.CapabilityDescription
                        }));
                        break;
                        
                    case ArtifactType.ProcessModel:
                        var processes = await _context.ProcessModels.ToListAsync();
                        artifacts.AddRange(processes.Select(p => new ArtifactNodeViewModel
                        {
                            Id = p.Id.ToString(),
                            Name = p.ProcessName,
                            Type = artifactType,
                            Domain = ArtifactDomain.Business,
                            Description = p.ProcessDescription
                        }));
                        break;
                        
                    case ArtifactType.BusinessProductService:
                        var products = await _context.BusinessProductServiceViews.ToListAsync();
                        artifacts.AddRange(products.Select(p => new ArtifactNodeViewModel
                        {
                            Id = p.Id.ToString(),
                            Name = p.ProductServiceName,
                            Type = artifactType,
                            Domain = ArtifactDomain.Business,
                            Description = p.Description
                        }));
                        break;
                        
                    case ArtifactType.ApplicationFramework:
                        var frameworks = await _context.ApplicationArchitectureFrameworks.ToListAsync();
                        artifacts.AddRange(frameworks.Select(f => new ArtifactNodeViewModel
                        {
                            Id = f.Id.ToString(),
                            Name = f.ApplicationName,
                            Type = artifactType,
                            Domain = ArtifactDomain.Application,
                            Description = f.ApplicationDescription
                        }));
                        break;
                        
                    case ArtifactType.ApplicationBusinessRequirement:
                        var appRequirements = await _context.ApplicationBusinessRequirementsViews.ToListAsync();
                        artifacts.AddRange(appRequirements.Select(r => new ArtifactNodeViewModel
                        {
                            Id = r.Id.ToString(),
                            Name = r.RequirementId,
                            Type = artifactType,
                            Domain = ArtifactDomain.Application,
                            Description = r.RequirementDescription
                        }));
                        break;
                        
                    case ArtifactType.ServiceCatalogue:
                        var services = await _context.ServiceCatalogues.ToListAsync();
                        artifacts.AddRange(services.Select(s => new ArtifactNodeViewModel
                        {
                            Id = s.Id.ToString(),
                            Name = s.ServiceName,
                            Type = artifactType,
                            Domain = ArtifactDomain.Application,
                            Description = s.ServiceDescription
                        }));
                        break;
                        
                    case ArtifactType.ApplicationCrossReference:
                        var appCrossRefs = await _context.ApplicationToApplicationCrossReferences.ToListAsync();
                        artifacts.AddRange(appCrossRefs.Select(a => new ArtifactNodeViewModel
                        {
                            Id = a.Id.ToString(),
                            Name = a.SourceApplication + " → " + a.TargetApplication,
                            Type = artifactType,
                            Domain = ArtifactDomain.Application,
                            Description = a.IntegrationType
                        }));
                        break;
                        
                    case ArtifactType.ApplicationSecurity:
                        var appSecurity = await _context.ApplicationSecurityModels.ToListAsync();
                        artifacts.AddRange(appSecurity.Select(a => new ArtifactNodeViewModel
                        {
                            Id = a.Id.ToString(),
                            Name = a.ApplicationName,
                            Type = artifactType,
                            Domain = ArtifactDomain.Application,
                            Description = a.SecurityRequirement
                        }));
                        break;
                        
                    case ArtifactType.ApplicationDataCrossReference:
                        var appDataRef = await _context.ApplicationDataCrossReferences.ToListAsync();
                        artifacts.AddRange(appDataRef.Select(a => new ArtifactNodeViewModel
                        {
                            Id = a.Id.ToString(),
                            Name = a.ApplicationName + " → " + a.DataEntity,
                            Type = artifactType,
                            Domain = ArtifactDomain.Application,
                            Description = a.DataUsage
                        }));
                        break;
                        
                    case ArtifactType.DataGovernance:
                        var governances = await _context.DataGovernanceModels.ToListAsync();
                        artifacts.AddRange(governances.Select(g => new ArtifactNodeViewModel
                        {
                            Id = g.Id.ToString(),
                            Name = g.DataEntity,
                            Type = artifactType,
                            Domain = ArtifactDomain.Data,
                            Description = g.DataLifecycle
                        }));
                        break;
                        
                    case ArtifactType.InformationHierarchy:
                        var hierarchies = await _context.InformationHierarchyViews.ToListAsync();
                        artifacts.AddRange(hierarchies.Select(h => new ArtifactNodeViewModel
                        {
                            Id = h.Id.ToString(),
                            Name = h.InformationLevel,
                            Type = artifactType,
                            Domain = ArtifactDomain.Data,
                            Description = h.Description
                        }));
                        break;
                        
                    case ArtifactType.InformationRequirements:
                        var infoRequirements = await _context.InformationRequirementsViews.ToListAsync();
                        artifacts.AddRange(infoRequirements.Select(i => new ArtifactNodeViewModel
                        {
                            Id = i.Id.ToString(),
                            Name = i.RequirementId,
                            Type = artifactType,
                            Domain = ArtifactDomain.Data,
                            Description = i.RequirementDescription
                        }));
                        break;
                        
                    case ArtifactType.DataFlow:
                        var dataFlows = await _context.DataFlowModels.ToListAsync();
                        artifacts.AddRange(dataFlows.Select(d => new ArtifactNodeViewModel
                        {
                            Id = d.Id.ToString(),
                            Name = d.DataSource + " → " + d.DataDestination,
                            Type = artifactType,
                            Domain = ArtifactDomain.Data,
                            Description = d.DataFlowDescription
                        }));
                        break;
                        
                    case ArtifactType.LogicalData:
                        var logicalData = await _context.LogicalDataModels.ToListAsync();
                        artifacts.AddRange(logicalData.Select(l => new ArtifactNodeViewModel
                        {
                            Id = l.Id.ToString(),
                            Name = l.EntityName,
                            Type = artifactType,
                            Domain = ArtifactDomain.Data,
                            Description = l.Description
                        }));
                        break;
                        
                    case ArtifactType.DataSecurity:
                        var dataSecurity = await _context.DataSecurityModels.ToListAsync();
                        artifacts.AddRange(dataSecurity.Select(d => new ArtifactNodeViewModel
                        {
                            Id = d.Id.ToString(),
                            Name = d.DataEntity,
                            Type = artifactType,
                            Domain = ArtifactDomain.Data,
                            Description = d.SecurityRequirement
                        }));
                        break;
                        
                    case ArtifactType.InfrastructureBusinessRequirement:
                        var requirements = await _context.InfrastructureBusinessRequirementsViews.ToListAsync();
                        artifacts.AddRange(requirements.Select(r => new ArtifactNodeViewModel
                        {
                            Id = r.Id.ToString(),
                            Name = r.RequirementId,
                            Type = artifactType,
                            Domain = ArtifactDomain.Infrastructure,
                            Description = r.RequirementDescription
                        }));
                        break;
                        
                    case ArtifactType.SystemToApplication:
                        var sysAppRefs = await _context.SystemToApplicationCrossReferences.ToListAsync();
                        artifacts.AddRange(sysAppRefs.Select(s => new ArtifactNodeViewModel
                        {
                            Id = s.Id.ToString(),
                            Name = s.SystemName + " → " + s.ApplicationName,
                            Type = artifactType,
                            Domain = ArtifactDomain.Infrastructure,
                            Description = s.IntegrationType
                        }));
                        break;
                        
                    case ArtifactType.ResourceNeeds:
                        var resources = await _context.ResourceNeedsModels.ToListAsync();
                        artifacts.AddRange(resources.Select(r => new ArtifactNodeViewModel
                        {
                            Id = r.Id.ToString(),
                            Name = r.ResourceName,
                            Type = artifactType,
                            Domain = ArtifactDomain.Infrastructure,
                            Description = r.Description
                        }));
                        break;
                        
                    case ArtifactType.SystemToBusiness:
                        var sysBiz = await _context.SystemToBusinessCrossReferences.ToListAsync();
                        artifacts.AddRange(sysBiz.Select(s => new ArtifactNodeViewModel
                        {
                            Id = s.Id.ToString(),
                            Name = s.SystemName + " → " + s.BusinessUnit,
                            Type = artifactType,
                            Domain = ArtifactDomain.Infrastructure,
                            Description = s.BusinessProcessSupported
                        }));
                        break;
                        
                    case ArtifactType.InfrastructureSecurity:
                        var infraSecurity = await _context.InfrastructureSecurityModels.ToListAsync();
                        artifacts.AddRange(infraSecurity.Select(i => new ArtifactNodeViewModel
                        {
                            Id = i.Id.ToString(),
                            Name = i.SystemName,
                            Type = artifactType,
                            Domain = ArtifactDomain.Infrastructure,
                            Description = i.SecurityRequirement
                        }));
                        break;
                        
                    case ArtifactType.SystemData:
                        var sysDataRefs = await _context.SystemDataCrossReferences.ToListAsync();
                        artifacts.AddRange(sysDataRefs.Select(s => new ArtifactNodeViewModel
                        {
                            Id = s.Id.ToString(),
                            Name = s.SystemName + " → " + s.DataEntity,
                            Type = artifactType,
                            Domain = ArtifactDomain.Infrastructure,
                            Description = s.DataUsage
                        }));
                        break;
                            
                    default:
                        // For types not explicitly handled, try to get a generic representation
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error getting artifacts of type {artifactType}: {ex.Message}");
            }
            
            return artifacts;
        }

        // Build connections between artifacts
        private async Task<List<ArtifactConnectionViewModel>> BuildArtifactConnections(List<ArtifactLink> links)
        {
            var connections = new List<ArtifactConnectionViewModel>();
            
            foreach (var link in links)
            {
                try
                {
                    var sourceInfo = await GetArtifactInfo(link.SourceArtifactType, link.SourceArtifactId);
                    var targetInfo = await GetArtifactInfo(link.TargetArtifactType, link.TargetArtifactId);
                    
                    var connection = new ArtifactConnectionViewModel
                    {
                        Id = link.Id.ToString(),
                        SourceId = link.SourceArtifactId,
                        SourceType = link.SourceArtifactType,
                        SourceName = sourceInfo.Name,
                        TargetId = link.TargetArtifactId,
                        TargetType = link.TargetArtifactType,
                        TargetName = targetInfo.Name,
                        Description = link.LinkDescription,
                        IsCrossDomain = GetDomainForArtifactType(link.SourceArtifactType.ToString()) != 
                                       GetDomainForArtifactType(link.TargetArtifactType.ToString())
                    };
                    
                    connections.Add(connection);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    _logger.LogError(ex, "Error building connection for link {linkId}: {message}", link.Id, ex.Message);
                }
            }
            
            return connections;
        }

        // Helper method to get the domain for an artifact type
        private ArtifactDomain GetDomainForArtifactType(string artifactType)
        {
            if (new[] { 
                "BusinessStrategy", "OperationModel", "OrganizationView", 
                "CapabilityMap", "ProcessModel", "BusinessProductService" 
            }.Any(t => artifactType.Contains(t)))
            {
                return ArtifactDomain.Business;
            }
            
            if (new[] { 
                "ApplicationFramework", "ApplicationBusinessRequirement", 
                "ServiceCatalogue", "ApplicationCrossReference", 
                "ApplicationSecurity", "ApplicationDataCrossReference" 
            }.Any(t => artifactType.Contains(t)))
            {
                return ArtifactDomain.Application;
            }
            
            if (new[] { 
                "DataGovernance", "InformationHierarchy", 
                "InformationRequirements", "DataFlow", 
                "LogicalData", "DataSecurity" 
            }.Any(t => artifactType.Contains(t)))
            {
                return ArtifactDomain.Data;
            }
            
            if (new[] { 
                "InfrastructureBusinessRequirement", "SystemToApplication", 
                "ResourceNeeds", "SystemToBusiness", 
                "InfrastructureSecurity", "SystemData" 
            }.Any(t => artifactType.Contains(t)))
            {
                return ArtifactDomain.Infrastructure;
            }
            
            return ArtifactDomain.Unknown;
        }

        // Helper method to get artifact source/target details
        private async Task<dynamic> GetArtifactInfo(ArtifactType artifactType, string artifactId)
        {
            int id;
            if (!int.TryParse(artifactId, out id))
            {
                _logger.LogWarning("Failed to parse artifact ID '{artifactId}' for artifact type {artifactType}", artifactId, artifactType);
                return new { Name = "Unknown", Type = artifactType.ToString() };
            }

            try
            {
                switch (artifactType)
                {
                    case ArtifactType.BusinessStrategy:
                        var strategy = await _context.BusinessStrategyViews.FindAsync(id);
                        return strategy != null 
                            ? new { Name = strategy.BusinessGoal, Type = "Business Strategy" } 
                            : new { Name = "Unknown", Type = "Business Strategy" };

                    case ArtifactType.OperationModel:
                        var operation = await _context.OperationModels.FindAsync(id);
                        return operation != null 
                            ? new { Name = operation.OperationalProcess, Type = "Operation Model" }
                            : new { Name = "Unknown", Type = "Operation Model" };

                    case ArtifactType.OrganizationView:
                        var org = await _context.OrganizationViews.FindAsync(id);
                        return org != null 
                            ? new { Name = org.DepartmentOrUnit, Type = "Organization View" }
                            : new { Name = "Unknown", Type = "Organization View" };

                    case ArtifactType.CapabilityMap:
                        var capability = await _context.CapabilityMaps.FindAsync(id);
                        return capability != null 
                            ? new { Name = capability.CapabilityName, Type = "Capability Map" }
                            : new { Name = "Unknown", Type = "Capability Map" };

                    case ArtifactType.ProcessModel:
                        var process = await _context.ProcessModels.FindAsync(id);
                        return process != null 
                            ? new { Name = process.ProcessName, Type = "Process Model" }
                            : new { Name = "Unknown", Type = "Process Model" };

                    case ArtifactType.BusinessProductService:
                        var product = await _context.BusinessProductServiceViews.FindAsync(id);
                        return product != null 
                            ? new { Name = product.ProductServiceName, Type = "Business Product/Service" }
                            : new { Name = "Unknown", Type = "Business Product/Service" };

                    case ArtifactType.ApplicationFramework:
                        var appFramework = await _context.ApplicationArchitectureFrameworks.FindAsync(id);
                        return appFramework != null 
                            ? new { Name = appFramework.ApplicationName, Type = "Application Framework" }
                            : new { Name = "Unknown", Type = "Application Framework" };

                    case ArtifactType.ApplicationBusinessRequirement:
                        var appRequirement = await _context.ApplicationBusinessRequirementsViews.FindAsync(id);
                        return appRequirement != null 
                            ? new { Name = appRequirement.RequirementId, Type = "Application Requirement" }
                            : new { Name = "Unknown", Type = "Application Requirement" };

                    case ArtifactType.ServiceCatalogue:
                        var service = await _context.ServiceCatalogues.FindAsync(id);
                        return service != null 
                            ? new { Name = service.ServiceName, Type = "Service Catalogue" }
                            : new { Name = "Unknown", Type = "Service Catalogue" };

                    case ArtifactType.ApplicationCrossReference:
                        var appCrossRef = await _context.ApplicationToApplicationCrossReferences.FindAsync(id);
                        return appCrossRef != null 
                            ? new { Name = $"{appCrossRef.SourceApplication} → {appCrossRef.TargetApplication}", Type = "App Cross-Reference" }
                            : new { Name = "Unknown", Type = "App Cross-Reference" };

                    case ArtifactType.ApplicationSecurity:
                        var appSecurity = await _context.ApplicationSecurityModels.FindAsync(id);
                        return appSecurity != null 
                            ? new { Name = appSecurity.ApplicationName, Type = "App Security" }
                            : new { Name = "Unknown", Type = "App Security" };

                    case ArtifactType.ApplicationDataCrossReference:
                        var appDataRef = await _context.ApplicationDataCrossReferences.FindAsync(id);
                        return appDataRef != null 
                            ? new { Name = $"{appDataRef.ApplicationName} → {appDataRef.DataEntity}", Type = "App-Data Reference" }
                            : new { Name = "Unknown", Type = "App-Data Reference" };

                    case ArtifactType.DataGovernance:
                        var governance = await _context.DataGovernanceModels.FindAsync(id);
                        return governance != null 
                            ? new { Name = governance.DataEntity, Type = "Data Governance" }
                            : new { Name = "Unknown", Type = "Data Governance" };

                    case ArtifactType.InformationHierarchy:
                        var hierarchy = await _context.InformationHierarchyViews.FindAsync(id);
                        return hierarchy != null 
                            ? new { Name = hierarchy.InformationLevel, Type = "Information Hierarchy" }
                            : new { Name = "Unknown", Type = "Information Hierarchy" };

                    case ArtifactType.InformationRequirements:
                        var infoRequirement = await _context.InformationRequirementsViews.FindAsync(id);
                        return infoRequirement != null 
                            ? new { Name = infoRequirement.RequirementId, Type = "Information Requirement" }
                            : new { Name = "Unknown", Type = "Information Requirement" };

                    case ArtifactType.DataFlow:
                        var dataFlow = await _context.DataFlowModels.FindAsync(id);
                        return dataFlow != null 
                            ? new { Name = $"{dataFlow.DataSource} → {dataFlow.DataDestination}", Type = "Data Flow" }
                            : new { Name = "Unknown", Type = "Data Flow" };

                    case ArtifactType.LogicalData:
                        var logicalData = await _context.LogicalDataModels.FindAsync(id);
                        return logicalData != null 
                            ? new { Name = logicalData.EntityName, Type = "Logical Data" }
                            : new { Name = "Unknown", Type = "Logical Data" };

                    case ArtifactType.DataSecurity:
                        var dataSecurity = await _context.DataSecurityModels.FindAsync(id);
                        return dataSecurity != null 
                            ? new { Name = dataSecurity.DataEntity, Type = "Data Security" }
                            : new { Name = "Unknown", Type = "Data Security" };

                    case ArtifactType.InfrastructureBusinessRequirement:
                        var infraRequirement = await _context.InfrastructureBusinessRequirementsViews.FindAsync(id);
                        return infraRequirement != null 
                            ? new { Name = infraRequirement.RequirementId, Type = "Infra Requirement" }
                            : new { Name = "Unknown", Type = "Infra Requirement" };

                    case ArtifactType.SystemToApplication:
                        var sysAppRef = await _context.SystemToApplicationCrossReferences.FindAsync(id);
                        return sysAppRef != null 
                            ? new { Name = $"{sysAppRef.SystemName} → {sysAppRef.ApplicationName}", Type = "System-App Link" }
                            : new { Name = "Unknown", Type = "System-App Link" };

                    case ArtifactType.ResourceNeeds:
                        var resource = await _context.ResourceNeedsModels.FindAsync(id);
                        return resource != null 
                            ? new { Name = resource.ResourceName, Type = "Resource Need" }
                            : new { Name = "Unknown", Type = "Resource Need" };

                    case ArtifactType.SystemToBusiness:
                        var sysBiz = await _context.SystemToBusinessCrossReferences.FindAsync(id);
                        return sysBiz != null 
                            ? new { Name = $"{sysBiz.SystemName} → {sysBiz.BusinessUnit}", Type = "System-Business Link" }
                            : new { Name = "Unknown", Type = "System-Business Link" };

                    case ArtifactType.InfrastructureSecurity:
                        var infraSecurity = await _context.InfrastructureSecurityModels.FindAsync(id);
                        return infraSecurity != null 
                            ? new { Name = infraSecurity.SystemName, Type = "Infra Security" }
                            : new { Name = "Unknown", Type = "Infra Security" };

                    case ArtifactType.SystemData:
                        var sysData = await _context.SystemDataCrossReferences.FindAsync(id);
                        return sysData != null 
                            ? new { Name = $"{sysData.SystemName} → {sysData.DataEntity}", Type = "System-Data Link" }
                            : new { Name = "Unknown", Type = "System-Data Link" };
                            
                    default:
                        return new { Name = artifactId, Type = artifactType.ToString() };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting artifact info for {artifactType} with ID {artifactId}: {message}", 
                    artifactType, artifactId, ex.Message);
                return new { Name = artifactId, Type = artifactType.ToString() };
            }
        } 
        #endregion
    }
}