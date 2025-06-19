using System;
using System.ComponentModel.DataAnnotations;

namespace SIMEA.Models.ViewModels
{
    public class ArtifactInfoViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
    }

    public class ArtifactLinkViewModel
    {
        public Guid Id { get; set; }
        public ArtifactInfoViewModel SourceArtifact { get; set; }
        public ArtifactInfoViewModel TargetArtifact { get; set; }
        public string LinkDescription { get; set; }
        
        // Adding DateTime tracking properties for the view model
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime LinkCreatedDate { get; set; }
        
        public string CreatedBy { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? LastModifiedDate { get; set; }
        
        public string LastModifiedBy { get; set; }
    }
}