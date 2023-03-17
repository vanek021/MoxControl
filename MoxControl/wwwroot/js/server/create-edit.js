"use strict";

// On document ready
$(document).ready(function () {

    processAuthorizationType(getAuthorizationSelectorValue());

    $("#authorizationSelect").change(function () {
        var selectorValue = getAuthorizationSelectorValue();
        processAuthorizationType(selectorValue);
    });
});

function processAuthorizationType(str) {
    if (str === 'UserCredentials') {
        $("#serverRootPassword").css("display", "none");
        $("#serverRootUsername").css("display", "none");
    }
    else {
        $("#serverRootPassword").css("display", "flex");
        $("#serverRootUsername").css("display", "flex");
    }
}

function getAuthorizationSelectorValue() {
    return $('#authorizationSelect').find(":selected")[0].label;
}