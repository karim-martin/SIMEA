using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIMEA.Data;
using SIMEA.Models;
using SIMEA.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, EATeam")]
    public class ArtifactLinkController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ArtifactLinkController> _logger;

        public ArtifactLinkController(ILogger<ArtifactLinkController> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: ArtifactLink
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArtifactLinks.ToListAsync());
        }

        // GET: ArtifactLink/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artifactLink = await _context.ArtifactLinks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artifactLink == null)
            {
                return NotFound();
            }

            // Get source and target artifacts information to display in the diagram
            var sourceInfo = await GetArtifactInfo(artifactLink.SourceArtifactType, artifactLink.SourceArtifactId);
            var targetInfo = await GetArtifactInfo(artifactLink.TargetArtifactType, artifactLink.TargetArtifactId);
            
            // Add to ViewBag for the view
            ViewBag.SourceArtifactInfo = sourceInfo;
            ViewBag.TargetArtifactInfo = targetInfo;

            // Find related links (links that share source or target with this link)
            var relatedLinks = await _context.ArtifactLinks
                .Where(l => 
                    (l.SourceArtifactType == artifactLink.SourceArtifactType && l.SourceArtifactId == artifactLink.SourceArtifactId) ||
                    (l.TargetArtifactType == artifactLink.TargetArtifactType && l.TargetArtifactId == artifactLink.TargetArtifactId) ||
                    (l.SourceArtifactType == artifactLink.TargetArtifactType && l.SourceArtifactId == artifactLink.TargetArtifactId) ||
                    (l.TargetArtifactType == artifactLink.SourceArtifactType && l.TargetArtifactId == artifactLink.SourceArtifactId)
                )
                .Where(l => l.Id != artifactLink.Id) // Exclude the current link
                .ToListAsync();
            
            // Enhance related links with artifact names
            var enhancedRelatedLinks = new List<dynamic>();
            foreach (var link in relatedLinks)
            {
                var sourceName = (await GetArtifactInfo(link.SourceArtifactType, link.SourceArtifactId)).Name;
                var targetName = (await GetArtifactInfo(link.TargetArtifactType, link.TargetArtifactId)).Name;
                
                enhancedRelatedLinks.Add(new
                {
                    Id = link.Id,
                    SourceArtifactType = link.SourceArtifactType,
                    SourceArtifactId = link.SourceArtifactId,
                    SourceArtifactName = sourceName,
                    TargetArtifactType = link.TargetArtifactType,
                    TargetArtifactId = link.TargetArtifactId,
                    TargetArtifactName = targetName,
                    LinkDescription = link.LinkDescription,
                    LinkCreatedDate = link.LinkCreatedDate
                });
            }
            
            ViewBag.RelatedLinks = enhancedRelatedLinks;
            
            // Create view model for visualization
            var viewModel = new ArtifactLinkViewModel
            {
                Id = artifactLink.Id,
                LinkDescription = artifactLink.LinkDescription,
                SourceArtifact = new ArtifactInfoViewModel 
                { 
                    Name = sourceInfo.Name, 
                    Type = sourceInfo.Type, 
                    Id = artifactLink.SourceArtifactId 
                },
                TargetArtifact = new ArtifactInfoViewModel 
                { 
                    Name = targetInfo.Name, 
                    Type = targetInfo.Type, 
                    Id = artifactLink.TargetArtifactId 
                },
                // Include datetime properties
                LinkCreatedDate = artifactLink.LinkCreatedDate,
                CreatedBy = artifactLink.CreatedBy,
                LastModifiedDate = artifactLink.LastModifiedDate,
                LastModifiedBy = artifactLink.LastModifiedBy
            };

            ViewBag.ViewModel = viewModel;

            return View(artifactLink);
        }
        
        // GET: ArtifactLink/FullView
        public async Task<IActionResult> FullView(string domain)
        {
            // Get all artifact links
            var allLinks = await _context.ArtifactLinks.ToListAsync();
            
            // Build the complex visualization data
            var visualizationData = new VisualizationViewModel();
            
            // Get artifacts by domain
            visualizationData.BusinessArtifacts = await GetArtifactsByDomain(ArtifactDomain.Business);
            visualizationData.ApplicationArtifacts = await GetArtifactsByDomain(ArtifactDomain.Application);
            visualizationData.DataArtifacts = await GetArtifactsByDomain(ArtifactDomain.Data);
            visualizationData.InfrastructureArtifacts = await GetArtifactsByDomain(ArtifactDomain.Infrastructure);
            
            // Build connections between artifacts
            visualizationData.Connections = await BuildArtifactConnections(allLinks);
            
            // Filter by domain if specified
            if (!string.IsNullOrEmpty(domain))
            {
                visualizationData.SelectedDomain = domain;
            }
            
            return View(visualizationData);
        }

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
                        var appDataCrossRefs = await _context.ApplicationDataCrossReferences.ToListAsync();
                        artifacts.AddRange(appDataCrossRefs.Select(a => new ArtifactNodeViewModel
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
                        var sysBusinessRefs = await _context.SystemToBusinessCrossReferences.ToListAsync();
                        artifacts.AddRange(sysBusinessRefs.Select(s => new ArtifactNodeViewModel
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
                _logger.LogError(ex, $"Error getting artifacts of type {artifactType}");
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
                        IsCrossDomain = GetDomainForArtifactType(link.SourceArtifactType) != GetDomainForArtifactType(link.TargetArtifactType)
                    };
                    
                    connections.Add(connection);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    _logger.LogError(ex, $"Error building connection for link {link.Id}");
                }
            }
            
            return connections;
        }

        // Helper method to get the domain for an artifact type
        private ArtifactDomain GetDomainForArtifactType(ArtifactType artifactType)
        {
            if (new[] { ArtifactType.BusinessStrategy, ArtifactType.OperationModel, ArtifactType.OrganizationView, 
                        ArtifactType.CapabilityMap, ArtifactType.ProcessModel, ArtifactType.BusinessProductService }
                    .Contains(artifactType))
            {
                return ArtifactDomain.Business;
            }
            
            if (new[] { ArtifactType.ApplicationFramework, ArtifactType.ApplicationBusinessRequirement, 
                        ArtifactType.ServiceCatalogue, ArtifactType.ApplicationCrossReference, 
                        ArtifactType.ApplicationSecurity, ArtifactType.ApplicationDataCrossReference }
                    .Contains(artifactType))
            {
                return ArtifactDomain.Application;
            }
            
            if (new[] { ArtifactType.DataGovernance, ArtifactType.InformationHierarchy, 
                        ArtifactType.InformationRequirements, ArtifactType.DataFlow, 
                        ArtifactType.LogicalData, ArtifactType.DataSecurity }
                    .Contains(artifactType))
            {
                return ArtifactDomain.Data;
            }
            
            if (new[] { ArtifactType.InfrastructureBusinessRequirement, ArtifactType.SystemToApplication, 
                        ArtifactType.ResourceNeeds, ArtifactType.SystemToBusiness, 
                        ArtifactType.InfrastructureSecurity, ArtifactType.SystemData }
                    .Contains(artifactType))
            {
                return ArtifactDomain.Infrastructure;
            }
            
            return ArtifactDomain.Unknown;
        }

        // GET: ArtifactLink/Create
        public IActionResult Create()
        {
            ViewBag.ArtifactTypes = GetArtifactTypesSelectList();
            return View();
        }

        // POST: ArtifactLink/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SourceArtifactType,SourceArtifactId,TargetArtifactType,TargetArtifactId,LinkDescription")] ArtifactLink artifactLink)
        {
            if (ModelState.IsValid)
            {
                artifactLink.Id = Guid.NewGuid();
                artifactLink.LinkCreatedDate = DateTime.Now;
                artifactLink.CreatedBy = User.Identity.Name;
                _context.Add(artifactLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ArtifactTypes = GetArtifactTypesSelectList();
            return View(artifactLink);
        }

        // GET: ArtifactLink/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artifactLink = await _context.ArtifactLinks.FindAsync(id);
            if (artifactLink == null)
            {
                return NotFound();
            }
            ViewBag.ArtifactTypes = GetArtifactTypesSelectList();
            return View(artifactLink);
        }

        // POST: ArtifactLink/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,SourceArtifactType,SourceArtifactId,TargetArtifactType,TargetArtifactId,LinkDescription,LinkCreatedDate,CreatedBy")] ArtifactLink artifactLink)
        {
            if (id != artifactLink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    artifactLink.LastModifiedDate = DateTime.Now;
                    artifactLink.LastModifiedBy = User.Identity.Name;
                    _context.Update(artifactLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtifactLinkExists(artifactLink.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ArtifactTypes = GetArtifactTypesSelectList();
            return View(artifactLink);
        }

        // GET: ArtifactLink/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artifactLink = await _context.ArtifactLinks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artifactLink == null)
            {
                return NotFound();
            }

            // Get source and target artifacts information to display in the diagram
            var sourceInfo = await GetArtifactInfo(artifactLink.SourceArtifactType, artifactLink.SourceArtifactId);
            var targetInfo = await GetArtifactInfo(artifactLink.TargetArtifactType, artifactLink.TargetArtifactId);
            
            // Add to ViewBag for the view
            ViewBag.SourceArtifactInfo = sourceInfo;
            ViewBag.TargetArtifactInfo = targetInfo;

            // Find related links (links that share source or target with this link)
            var relatedLinks = await _context.ArtifactLinks
                .Where(l => 
                    (l.SourceArtifactType == artifactLink.SourceArtifactType && l.SourceArtifactId == artifactLink.SourceArtifactId) ||
                    (l.TargetArtifactType == artifactLink.TargetArtifactType && l.TargetArtifactId == artifactLink.TargetArtifactId) ||
                    (l.SourceArtifactType == artifactLink.TargetArtifactType && l.SourceArtifactId == artifactLink.TargetArtifactId) ||
                    (l.TargetArtifactType == artifactLink.SourceArtifactType && l.TargetArtifactId == artifactLink.SourceArtifactId)
                )
                .Where(l => l.Id != artifactLink.Id) // Exclude the current link
                .ToListAsync();
            
            // Enhance related links with artifact names
            var enhancedRelatedLinks = new List<dynamic>();
            foreach (var link in relatedLinks)
            {
                var sourceName = (await GetArtifactInfo(link.SourceArtifactType, link.SourceArtifactId)).Name;
                var targetName = (await GetArtifactInfo(link.TargetArtifactType, link.TargetArtifactId)).Name;
                
                enhancedRelatedLinks.Add(new
                {
                    Id = link.Id,
                    SourceArtifactType = link.SourceArtifactType,
                    SourceArtifactId = link.SourceArtifactId,
                    SourceArtifactName = sourceName,
                    TargetArtifactType = link.TargetArtifactType,
                    TargetArtifactId = link.TargetArtifactId,
                    TargetArtifactName = targetName,
                    LinkDescription = link.LinkDescription,
                    LinkCreatedDate = link.LinkCreatedDate
                });
            }
            
            ViewBag.RelatedLinks = enhancedRelatedLinks;
            
            // Create view model for visualization
            var viewModel = new ArtifactLinkViewModel
            {
                Id = artifactLink.Id,
                LinkDescription = artifactLink.LinkDescription,
                SourceArtifact = new ArtifactInfoViewModel 
                { 
                    Name = sourceInfo.Name, 
                    Type = sourceInfo.Type, 
                    Id = artifactLink.SourceArtifactId 
                },
                TargetArtifact = new ArtifactInfoViewModel 
                { 
                    Name = targetInfo.Name, 
                    Type = targetInfo.Type, 
                    Id = artifactLink.TargetArtifactId 
                }
            };

            ViewBag.ViewModel = viewModel;

            return View(artifactLink);
        }

        // POST: ArtifactLink/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var artifactLink = await _context.ArtifactLinks.FindAsync(id);
            _context.ArtifactLinks.Remove(artifactLink);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtifactLinkExists(Guid id)
        {
            return _context.ArtifactLinks.Any(e => e.Id == id);
        }

        private SelectList GetArtifactTypesSelectList()
        {
            return new SelectList(Enum.GetValues(typeof(ArtifactType))
                .Cast<ArtifactType>()
                .Select(a => new SelectListItem
                {
                    Value = a.ToString(),
                    Text = a.ToString()
                }), "Value", "Text");
        }

        // Helper method to get artifact source/target details for the diagram
        private async Task<dynamic> GetArtifactInfo(ArtifactType artifactType, string artifactId)
        {
            if (string.IsNullOrEmpty(artifactId))
            {
                _logger.LogWarning("GetArtifactInfo called with null or empty artifactId for type {type}", artifactType);
                return new { Name = "Unknown", Type = artifactType.ToString() };
            }

            if (!int.TryParse(artifactId, out int id))
            {
                _logger.LogWarning("GetArtifactInfo failed to parse artifactId {id} for type {type}", artifactId, artifactType);
                return new { Name = artifactId, Type = artifactType.ToString() };
            }

            try
            {
                switch (artifactType)
                {
                    case ArtifactType.BusinessStrategy:
                        var strategy = await _context.BusinessStrategyViews.FindAsync(id);
                        return strategy != null 
                            ? new { Name = strategy.BusinessGoal, Type = "Business Strategy" } 
                            : new { Name = "Unknown Strategy #" + id, Type = "Business Strategy" };

                    case ArtifactType.OperationModel:
                        var operation = await _context.OperationModels.FindAsync(id);
                        return operation != null 
                            ? new { Name = operation.OperationalProcess, Type = "Operation Model" }
                            : new { Name = "Unknown Operation #" + id, Type = "Operation Model" };

                    case ArtifactType.OrganizationView:
                        var org = await _context.OrganizationViews.FindAsync(id);
                        return org != null 
                            ? new { Name = org.DepartmentOrUnit, Type = "Organization View" }
                            : new { Name = "Unknown Organization #" + id, Type = "Organization View" };

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
                            
                    // Add cases for other artifact types as needed
                    
                    default:
                        _logger.LogWarning("GetArtifactInfo called with unhandled artifact type {type} for ID {id}", artifactType, id);
                        return new { Name = "Unknown " + artifactType.ToString() + " #" + id, Type = artifactType.ToString() };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving artifact info for type {type} with ID {id}", artifactType, id);
                return new { Name = "Error retrieving " + artifactType.ToString() + " #" + id, Type = artifactType.ToString() };
            }
        }

        // GET: ArtifactLink/GetArtifactIds
        [HttpGet]
        public async Task<JsonResult> GetArtifactIds(ArtifactType artifactType)
        {
            var artifactIds = new List<object>();

            try
            {
                _logger.LogInformation("GetArtifactIds called for artifact type {type}", artifactType);
                
                switch (artifactType)
                {
                    case ArtifactType.BusinessStrategy:
                        try
                        {
                            artifactIds = await _context.BusinessStrategyViews
                                .Select(a => new { Id = a.Id.ToString(), Name = a.BusinessGoal })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving BusinessStrategy artifacts");
                        }
                        break;

                    case ArtifactType.OperationModel:
                        try
                        {
                            artifactIds = await _context.OperationModels
                                .Select(a => new { Id = a.Id.ToString(), Name = a.OperationalProcess })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving OperationModel artifacts");
                        }
                        break;

                    case ArtifactType.OrganizationView:
                        try
                        {
                            artifactIds = await _context.OrganizationViews
                                .Select(a => new { Id = a.Id.ToString(), Name = a.DepartmentOrUnit })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving OrganizationView artifacts");
                        }
                        break;

                    case ArtifactType.CapabilityMap:
                        try
                        {
                            artifactIds = await _context.CapabilityMaps
                                .Select(a => new { Id = a.Id.ToString(), Name = a.CapabilityName })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving CapabilityMap artifacts");
                        }
                        break;

                    case ArtifactType.ProcessModel:
                        try
                        {
                            artifactIds = await _context.ProcessModels
                                .Select(a => new { Id = a.Id.ToString(), Name = a.ProcessName })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving ProcessModel artifacts");
                        }
                        break;

                    case ArtifactType.BusinessProductService:
                        try
                        {
                            artifactIds = await _context.BusinessProductServiceViews
                                .Select(a => new { Id = a.Id.ToString(), Name = a.ProductServiceName })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving BusinessProductService artifacts");
                        }
                        break;

                    case ArtifactType.ApplicationFramework:
                        try
                        {
                            artifactIds = await _context.ApplicationArchitectureFrameworks
                                .Select(a => new { Id = a.Id.ToString(), Name = a.ApplicationName })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving ApplicationFramework artifacts");
                        }
                        break;
                        
                    case ArtifactType.ApplicationBusinessRequirement:
                        try
                        {
                            artifactIds = await _context.ApplicationBusinessRequirementsViews
                                .Select(a => new { Id = a.Id.ToString(), Name = a.RequirementId })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving ApplicationBusinessRequirement artifacts");
                        }
                        break;
                        
                    case ArtifactType.ServiceCatalogue:
                        try
                        {
                            artifactIds = await _context.ServiceCatalogues
                                .Select(a => new { Id = a.Id.ToString(), Name = a.ServiceName })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving ServiceCatalogue artifacts");
                        }
                        break;
                        
                    case ArtifactType.ApplicationCrossReference:
                        try
                        {
                            artifactIds = await _context.ApplicationToApplicationCrossReferences
                                .Select(a => new { Id = a.Id.ToString(), Name = a.SourceApplication + " → " + a.TargetApplication })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving ApplicationCrossReference artifacts");
                        }
                        break;
                        
                    case ArtifactType.ApplicationSecurity:
                        try
                        {
                            artifactIds = await _context.ApplicationSecurityModels
                                .Select(a => new { Id = a.Id.ToString(), Name = a.ApplicationName })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving ApplicationSecurity artifacts");
                        }
                        break;
                        
                    case ArtifactType.ApplicationDataCrossReference:
                        try
                        {
                            artifactIds = await _context.ApplicationDataCrossReferences
                                .Select(a => new { Id = a.Id.ToString(), Name = a.ApplicationName + " → " + a.DataEntity })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving ApplicationDataCrossReference artifacts");
                        }
                        break;
                        
                    case ArtifactType.DataGovernance:
                        try
                        {
                            artifactIds = await _context.DataGovernanceModels
                                .Select(a => new { Id = a.Id.ToString(), Name = a.DataEntity })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving DataGovernance artifacts");
                        }
                        break;
                        
                    case ArtifactType.InformationHierarchy:
                        try
                        {
                            artifactIds = await _context.InformationHierarchyViews
                                .Select(a => new { Id = a.Id.ToString(), Name = a.InformationLevel })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving InformationHierarchy artifacts");
                        }
                        break;
                        
                    case ArtifactType.InformationRequirements:
                        try
                        {
                            artifactIds = await _context.InformationRequirementsViews
                                .Select(a => new { Id = a.Id.ToString(), Name = a.RequirementId })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving InformationRequirements artifacts");
                        }
                        break;
                        
                    case ArtifactType.DataFlow:
                        try
                        {
                            artifactIds = await _context.DataFlowModels
                                .Select(a => new { Id = a.Id.ToString(), Name = a.DataSource + " → " + a.DataDestination })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving DataFlow artifacts");
                        }
                        break;
                        
                    case ArtifactType.LogicalData:
                        try
                        {
                            artifactIds = await _context.LogicalDataModels
                                .Select(a => new { Id = a.Id.ToString(), Name = a.EntityName })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving LogicalData artifacts");
                        }
                        break;
                        
                    case ArtifactType.DataSecurity:
                        try
                        {
                            artifactIds = await _context.DataSecurityModels
                                .Select(a => new { Id = a.Id.ToString(), Name = a.DataEntity })
                                .Cast<object>().ToListAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving DataSecurity artifacts");
                        }
                        break;
                        
                    case ArtifactType.InfrastructureBusinessRequirement:
                        artifactIds = await _context.InfrastructureBusinessRequirementsViews
                            .Select(a => new { Id = a.Id.ToString(), Name = a.RequirementId })
                            .Cast<object>().ToListAsync();
                        break;
                        
                    case ArtifactType.SystemToApplication:
                        artifactIds = await _context.SystemToApplicationCrossReferences
                            .Select(a => new { Id = a.Id.ToString(), Name = a.SystemName + " → " + a.ApplicationName })
                            .Cast<object>().ToListAsync();
                        break;
                        
                    case ArtifactType.ResourceNeeds:
                        artifactIds = await _context.ResourceNeedsModels
                            .Select(a => new { Id = a.Id.ToString(), Name = a.ResourceName })
                            .Cast<object>().ToListAsync();
                        break;
                        
                    case ArtifactType.SystemToBusiness:
                        artifactIds = await _context.SystemToBusinessCrossReferences
                            .Select(a => new { Id = a.Id.ToString(), Name = a.SystemName + " → " + a.BusinessUnit })
                            .Cast<object>().ToListAsync();
                        break;
                        
                    case ArtifactType.InfrastructureSecurity:
                        artifactIds = await _context.InfrastructureSecurityModels
                            .Select(a => new { Id = a.Id.ToString(), Name = a.SystemName })
                            .Cast<object>().ToListAsync();
                        break;
                        
                    case ArtifactType.SystemData:
                        artifactIds = await _context.SystemDataCrossReferences
                            .Select(a => new { Id = a.Id.ToString(), Name = a.SystemName + " → " + a.DataEntity })
                            .Cast<object>().ToListAsync();
                        break;
                        
                    default:
                        _logger.LogWarning("GetArtifactIds called with unhandled artifact type: {type}", artifactType);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetArtifactIds for type {type}", artifactType);
            }

            return Json(artifactIds);
        }
    }
}