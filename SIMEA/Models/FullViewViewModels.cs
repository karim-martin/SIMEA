using System;
using System.Collections.Generic;

namespace SIMEA.Models.ViewModels
{
    // Enum to represent architecture domains
    public enum ArtifactDomain
    {
        Unknown,
        Business,
        Application,
        Data,
        Infrastructure
    }
    
    // ViewModel for the FullView visualization
    public class VisualizationViewModel
    {
        public List<ArtifactNodeViewModel> BusinessArtifacts { get; set; } = new List<ArtifactNodeViewModel>();
        public List<ArtifactNodeViewModel> ApplicationArtifacts { get; set; } = new List<ArtifactNodeViewModel>();
        public List<ArtifactNodeViewModel> DataArtifacts { get; set; } = new List<ArtifactNodeViewModel>();
        public List<ArtifactNodeViewModel> InfrastructureArtifacts { get; set; } = new List<ArtifactNodeViewModel>();
        public List<ArtifactConnectionViewModel> Connections { get; set; } = new List<ArtifactConnectionViewModel>();
        public string SelectedDomain { get; set; }
        
        // Helper method to get all artifacts across domains
        public List<ArtifactNodeViewModel> GetAllArtifacts()
        {
            var allArtifacts = new List<ArtifactNodeViewModel>();
            allArtifacts.AddRange(BusinessArtifacts);
            allArtifacts.AddRange(ApplicationArtifacts);
            allArtifacts.AddRange(DataArtifacts);
            allArtifacts.AddRange(InfrastructureArtifacts);
            return allArtifacts;
        }
    }
    
    // ViewModel for each artifact node in the visualization
    public class ArtifactNodeViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ArtifactType Type { get; set; }
        public ArtifactDomain Domain { get; set; }
        public string Description { get; set; }
        
        // Helper properties for visualization
        public string DomainCssClass
        {
            get
            {
                return Domain switch
                {
                    ArtifactDomain.Business => "business",
                    ArtifactDomain.Application => "application",
                    ArtifactDomain.Data => "data",
                    ArtifactDomain.Infrastructure => "infrastructure",
                    _ => "unknown"
                };
            }
        }
        
        public string TypeShortName
        {
            get
            {
                // Create abbreviated/shorthand name for the type for display in diagram
                var typeName = Type.ToString();
                
                // Split by uppercase letters and take first letter of each part
                var shortName = "";
                bool isPreviousCharLower = false;
                
                foreach (var c in typeName)
                {
                    if (char.IsUpper(c) && isPreviousCharLower)
                    {
                        shortName += c;
                    }
                    else if (shortName.Length == 0)
                    {
                        shortName += c;
                    }
                    
                    isPreviousCharLower = char.IsLower(c);
                }
                
                return shortName;
            }
        }
        
        public string NodeId => $"node_{Type}_{Id}".Replace(" ", "_");
    }
    
    // ViewModel for connections between artifacts
    public class ArtifactConnectionViewModel
    {
        public string Id { get; set; }
        public string SourceId { get; set; }
        public ArtifactType SourceType { get; set; }
        public string SourceName { get; set; }
        public string TargetId { get; set; }
        public ArtifactType TargetType { get; set; }
        public string TargetName { get; set; }
        public string Description { get; set; }
        public bool IsCrossDomain { get; set; }
        
        // Helper properties
        public string SourceNodeId => $"node_{SourceType}_{SourceId}".Replace(" ", "_");
        public string TargetNodeId => $"node_{TargetType}_{TargetId}".Replace(" ", "_");
        public string LinkClass => IsCrossDomain ? "cross-domain-link" : "same-domain-link";
    }
}