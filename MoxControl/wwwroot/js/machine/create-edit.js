"use strict";

// On document ready
$(document).ready(function () {

    processTemplateType(getTemplateSelectorValue());

    $("#templatesSelect").change(function () {
        var selectorValue = getTemplateSelectorValue();
        processTemplateType(selectorValue);
    });
});

function processTemplateType(str) {
    if (str != 0) {
        $("#ramSection").hide();
        $("#hddSection").hide();
        $("#cpuSection").hide();
        $("#imageSection").hide();
    }
    else {
        $("#ramSection").show();
        $("#hddSection").show();
        $("#cpuSection").show();
        $("#imageSection").show();
    }
}

function getTemplateSelectorValue() {
    return $('#templatesSelect').find(":selected").attr("value");
}