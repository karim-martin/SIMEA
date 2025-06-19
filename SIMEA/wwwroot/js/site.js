// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#IndexDataTable').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'csvHtml5'
        ]
    });
});

// JavaScript for the ArtifactLink functionality

$(document).ready(function () {
    // Check if we're on a page with artifact type dropdowns
    if ($('#sourceArtifactType').length && $('#targetArtifactType').length) {
        // Event handlers for source artifact type selection
        $('#sourceArtifactType').change(function () {
            var selectedType = $(this).val();
            if (selectedType) {
                loadArtifactIds(selectedType, '#sourceArtifactId');
            } else {
                $('#sourceArtifactId').empty().append('<option value="">-- Select Source Artifact --</option>');
            }
        });
        
        // Event handlers for target artifact type selection
        $('#targetArtifactType').change(function () {
            var selectedType = $(this).val();
            if (selectedType) {
                loadArtifactIds(selectedType, '#targetArtifactId');
            } else {
                $('#targetArtifactId').empty().append('<option value="">-- Select Target Artifact --</option>');
            }
        });
        
        // Initialize dropdowns if we're on an edit page
        if ($('#Id').length) { // Check if this is the edit page
            // Load initial source artifact options
            if ($('#sourceArtifactType').val()) {
                loadArtifactIds($('#sourceArtifactType').val(), '#sourceArtifactId', $('#sourceArtifactId').val());
            }
            
            // Load initial target artifact options
            if ($('#targetArtifactType').val()) {
                loadArtifactIds($('#targetArtifactType').val(), '#targetArtifactId', $('#targetArtifactId').val());
            }
        }
    }
});

// Function to load artifact IDs based on selected type
function loadArtifactIds(artifactType, selectElement, currentValue) {
    $.ajax({
        url: '/ArtifactLink/GetArtifactIds',
        type: 'GET',
        data: { artifactType: artifactType },
        success: function (data) {
            var select = $(selectElement);
            select.empty();
            select.append('<option value="">-- Select Artifact --</option>');
            
            if (data && data.length > 0) {
                $.each(data, function (i, item) {
                    var option = $('<option>').val(item.id).text(item.name);
                    if (currentValue && item.id == currentValue) {
                        option.prop('selected', true);
                    }
                    select.append(option);
                });
            } else if (data && data.error) {
                console.error('Error loading artifacts:', data.error);
                alert('Error loading artifacts: ' + data.error);
            } else {
                select.append('<option value="">No artifacts found</option>');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error loading artifacts:', error);
            alert('Error loading artifacts. Please try again.');
        }
    });
}



