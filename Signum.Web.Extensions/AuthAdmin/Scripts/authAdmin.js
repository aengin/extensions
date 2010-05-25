﻿/// <reference path="jquery-1.4.min.vsdoc.js" />"

function magicRadios(ctx) {
    var updateBackground = function() {
        $(this).toggleClass("cbDisabled", !$(":radio", this).attr("checked"));
    }

    $(".ruleTable .cbLink :radio", ctx).hide();
    $(".ruleTable .cbLink", ctx).each(updateBackground);

    $(".ruleTable .cbLink", ctx).live("click", function() {
        var td = $(this).parent("td");
        var tr = $(td).parent("tr");
        var radio = $(":radio", this);
        radio.attr("checked", true);
        $(".overriden", tr).attr("checked", radio.val() != $("input[name$="+this.name+"Base]", tr).val());
        $(".cbLink", tr).each(updateBackground);
    });
}

$(function() {
    magicRadios(document);
});

$(function() {
    $(".ruleTable a.namespace").live("click", function() {
        var ns = $("span.namespace", this).html();
        $(".ruleTable tr").filter(function() {
            return $("td > span.namespace", this).html() == ns;
        }).toggle();
        return false;
    });
});

function openDialog(controllerUrl, data) {
    var navigator = new ViewNavigator({
            controllerUrl: controllerUrl,
            requestExtraJsonData: data,
            type: 'PropertyRulePack',
            onLoaded: function(divId) {
            magicRadios($("#" + divId));
        } 
         });
    navigator.createSave();
}

function postDialog(controllerUrl, prefix) {
    new PartialValidator({ controllerUrl: controllerUrl, prefix: prefix }).trySave();
}