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
        console.log($("#ramSection"));
        $("#ramSection").hide();
        $("#hddSection").hide();
        $("#cpuSection").hide();
    }
    else {
        $("#ramSection").show();
        $("#hddSection").show();
        $("#cpuSection").show();
    }
}

function getTemplateSelectorValue() {
    return $('#templatesSelect').find(":selected").attr("value");
}