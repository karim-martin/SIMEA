/**
 * EA Charts Helper Functions
 * Contains utility functions for working with charts in the EA application
 */

// Check if a chart has data and display "No Data" message if empty
function handleEmptyChart(chartElement, data, renderCallback) {
    if (!chartElement) return false;
    
    // Clear previous content
    chartElement.innerHTML = '';
    
    // Check if data is empty
    if (!data || 
        (Array.isArray(data) && data.length === 0) || 
        (typeof data === 'object' && Object.keys(data).length === 0)) {
        
        // Display No Data message
        chartElement.innerHTML = '<div class="alert alert-warning text-center">' +
            '<i class="fas fa-exclamation-circle mr-2"></i> No Data</div>';
        return false;
    }
    
    // Initialize chart and call the provided render callback
    var chart = echarts.init(chartElement, null, { renderer: 'canvas' });
    if (typeof renderCallback === 'function') {
        renderCallback(chart);
    }
    
    // Handle window resize
    window.addEventListener('resize', function() {
        if (chart) chart.resize();
    });
    
    return chart;
}

// Format tooltip to handle null/undefined values
function formatTooltipValue(value, defaultValue = '0') {
    if (value === null || value === undefined || isNaN(value)) {
        return defaultValue;
    }
    return value;
}

// Safe data access helper
function safeGet(obj, path, defaultValue = '') {
    if (!obj) return defaultValue;
    
    const keys = path.split('.');
    let current = obj;
    
    for (let i = 0; i < keys.length; i++) {
        if (current === null || current === undefined) {
            return defaultValue;
        }
        current = current[keys[i]];
    }
    
    return (current === null || current === undefined) ? defaultValue : current;
} 