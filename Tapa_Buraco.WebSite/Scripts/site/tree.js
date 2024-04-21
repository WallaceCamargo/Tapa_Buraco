/// <reference path="../jquery-1.9.1.js"/>
/// <reference path="../jquery-ui-1.10.3.custom.js"/>
/// <reference path="../plugins/jquery.blockUI.js"/>
/// <reference path="../plugins/json2.js"/>

function ReplaceAll(string, token, newtoken) {
    while (string.indexOf(token) != -1) {
        string = string.replace(token, newtoken);
    }
    return string;
}

$(document).ready(function () {

    $("#buttonSave").click(function () {
        Load();
    });

});


function Load() {
    var tree = new Array();
    var jsonResult = "{";
    var countTag = 0;
    var countSubTag = 0;


    $("#chapterFolders > ul").each(function (index, tag) {

        countTag++;

        jsonResult += GetJsonTags(tag.children, false);

        jsonResult += "},";

    });

    jsonResult += "}";

    jsonResult = ReplaceAll(jsonResult, ",}", "}");
    jsonResult = ReplaceAll(jsonResult, "},}", "}}");
    jsonResult = ReplaceAll(jsonResult, ",]", "]");

    Save(jsonResult);

}

function Save(jsonDTE) {
    Page.AjaxService.Request('/Tree/Save', ReadData(jsonDTE), ReadCallBack, ReadCallBackError);
}

function ReadCallBack(obj) {
    alert("Alteração efetuada com sucesso!");
    window.location = "/package/index";
}

function ReadCallBackError(obj) {
    alert('Error: ' + obj.Message);
}

function ReadData(jsonDTE) {
    return { Id: $("#hiddenId").val(), FileData: jsonDTE };
}

function GetJsonTags(children, isArray) {

    var jsonReturn = "";

    $(children).each(function (indexSubTag, tagSubTag) {

        if (tagSubTag.nodeName == "LI") {

            var tagText = tagSubTag.innerText;
            tagText = ReplaceAll(tagText, ":", "");
            tagText = ReplaceAll(tagText, "\r", "");
            tagText = ReplaceAll(tagText, "\n", "");
            tagText = ReplaceAll(tagText, " ", "");

            jsonReturn += '"' + tagText + '":"' + $(tagSubTag.innerHTML)[0].children[1].childNodes[0].defaultValue + '",';
        }
        else if (tagSubTag.nodeName == "LABEL") {

            var add = "";

            if (isArray)
                add = "[";

            jsonReturn += '"' + tagSubTag.innerText + '":' + add + '{';


        }
        else if (tagSubTag.nodeName == "UL") {

            var isClassList = false;

            $(tagSubTag.classList).each(function (indexClassTag, classTag) {
                if (classTag == "list")
                    isClassList = true;
            });

            jsonReturn += GetJsonTags(tagSubTag.children, isClassList) + "},";

            if (isClassList)
                jsonReturn += "],";

        }
    });

    return jsonReturn;
}

function ChangeValue(obj) {
    obj.defaultValue = obj.value;
}

function InputMaskFull() {

    $(".mask").each(function (index, mask) {
        var getMask = mask.attributes.mask.value;

        getMask = ReplaceAll(getMask, "0", "9");
        getMask = ReplaceAll(getMask, "1", "9");
        getMask = ReplaceAll(getMask, "2", "9");
        getMask = ReplaceAll(getMask, "3", "9");
        getMask = ReplaceAll(getMask, "4", "9");
        getMask = ReplaceAll(getMask, "5", "9");
        getMask = ReplaceAll(getMask, "6", "9");
        getMask = ReplaceAll(getMask, "7", "9");
        getMask = ReplaceAll(getMask, "8", "9");

        getMask = ReplaceAll(getMask, "D", "9");
        getMask = ReplaceAll(getMask, "M", "9");
        getMask = ReplaceAll(getMask, "A", "9");
        getMask = ReplaceAll(getMask, "Y", "9");
        getMask = ReplaceAll(getMask, "H", "9");
        getMask = ReplaceAll(getMask, "S", "9");
        getMask = ReplaceAll(getMask, "N", "9");

        getMask = ReplaceAll(getMask, "d", "9");
        getMask = ReplaceAll(getMask, "m", "9");
        getMask = ReplaceAll(getMask, "a", "9");
        getMask = ReplaceAll(getMask, "y", "9");
        getMask = ReplaceAll(getMask, "h", "9");
        getMask = ReplaceAll(getMask, "s", "9");
        getMask = ReplaceAll(getMask, "n", "9");

        getMask = ReplaceAll(getMask, "(S/N)", "X");
        getMask = ReplaceAll(getMask, "(s/n)", "X");
        getMask = ReplaceAll(getMask, "S/N", "X");
        getMask = ReplaceAll(getMask, "s/n", "X");
        getMask = ReplaceAll(getMask, "SN", "X");
        getMask = ReplaceAll(getMask, "sn", "X");

        getMask = ReplaceAll(getMask, "999(X)", "ZDFG");
        getMask = ReplaceAll(getMask, "999(x)", "ZDFG");
        getMask = ReplaceAll(getMask, "99(X)", "ZDFG");
        getMask = ReplaceAll(getMask, "99(x)", "ZDFG");
        getMask = ReplaceAll(getMask, "9(X)", "ZDFG");
        getMask = ReplaceAll(getMask, "9(x)", "ZDFG");

        getMask = ReplaceAll(getMask, "(X)999", "ZDFG");
        getMask = ReplaceAll(getMask, "(x)999", "ZDFG");
        getMask = ReplaceAll(getMask, "(X)99", "ZDFG");
        getMask = ReplaceAll(getMask, "(x)99", "ZDFG");
        getMask = ReplaceAll(getMask, "(X)9", "ZDFG");
        getMask = ReplaceAll(getMask, "(x)9", "ZDFG");

        if (getMask.length == 1 && (getMask == "X" || getMask == "x"))
            getMask = "ZDFG";

        var zdfg = "";

        for (var i = 0; i < mask.attributes.maxlength.value; i++) {
            zdfg += "X";
        }

        getMask = ReplaceAll(getMask, "ZDFG", zdfg);

        var formatedMask = GetMask(getMask);

        if (formatedMask != "")
            $(mask).mask(formatedMask);

    });
}

function InputMask() {


}

function GetMask(maskFormat) {

    var maskArray = new Array();

    var count = 0;

    for (var i = 0; i < maskFormat.length; i++) {

        if (i != maskFormat.length) {
            var unit = maskFormat.substring(i, (i + 1));
            var select = "";

            select = SelectMaskUnit(unit);

            if (select != "") {
                maskArray[count] = select;
                count++;
            }

        }
    }

    var ret = "";
    var countMaskArray = 0;
    var retOld = "";

    for (var i = 0; i < maskArray.length; i++) {
        ret += maskArray[i];

        if (retOld == maskArray[i])
            countMaskArray++;

        retOld = maskArray[i];
    }

    if ((countMaskArray + 1) == maskArray.length && retOld == "a")
        ret = "";

    return ret;
}

function SelectMaskUnit(unit) {
    var Units = new Array("9", " ", "-", "(", ")", "/", "\\", ".", ",");

    for (var i = 0; i < Units.length; i++) {

        if (unit == Units[i]) {
            return unit;
        }
    }

    if (unit == "X" || unit == "x") {
        return "a";
    }

    return "";
}

function MountTextBox(obj) {
    var TagKey = obj.Key;
    var TagValue = obj.Value;

    var textBox = "<label old=" + TagValue + ">" + TagKey + ": <input type=\"text\" name=" + TagKey + " maxlength=\"99999\" placeholder=" + TagKey + " value=" + TagValue + " onblur=\"ChangeValue(this)\"";

    if (TagKey == "TP_REG") {
        return " disabled=\"disabled\" ";
    } else {
        return "";
    }

    textBox += " /> </label>";
}