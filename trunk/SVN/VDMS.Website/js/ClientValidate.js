// JScript File
//var arrObj = new Array();
function noNumbers(e)
{
var keynum
var keychar
var numcheck
    if(window.event) // IE
    {
        keynum = e.keyCode
    }
    else if(e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)
    numcheck = /\d/
    return !numcheck.test(keychar)
}

function onlyNumbers(e)
{
var keynum
var keychar
    var numcheck
    if(window.event) // IE
    {
        keynum = e.keyCode
    }
    else if(e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which
    }
    keychar = String.fromCharCode(keynum)
    numcheck = /\d/
    return numcheck.test(keychar)
}


function Import_ExceptionCheck(txt, txtNum, errMsg, confMsg)
{
    var btnOk = true;
    //alert(txtNum);
    for (var i=0; i<txtNum; i++)
    {
        btnOk = (txt[i].disabled == true) || (txt[i].value.trim() != '');
        if (!btnOk) break;
    }    
    if (!btnOk) 
    {
        //alert(errMsg);
        btnAccept.disabled = 'disabled';
        return false;
    } else 
    { 
        btnAccept.disabled = '';
        return true;    //confirm(confMsg);
    }
}

function CheckStatusList(funcNum)
{
    for (var i=0; i<funcNum; i++)
    {
        //alert('Alo');
        checkFuncList[i]();
    }    
    
}

function Import_StatusChanged(btn,txt,rbl,chb, rb0,txtItemDate,txtImportDate, imgDt)
{
    
    if ((chb.checked == false) || (rbl.checked == false)) 
        txt.disabled = '';
    else 
        txt.disabled = 'disabled';
        
    if (rb0.checked == true)
    {
        txtItemDate.value = "";
        txtItemDate.disabled = 'disabled';
        imgDt.className = "hidden";
    }
    else
    {
        txtItemDate.value = txtImportDate.value;
        txtItemDate.disabled = '';
        imgDt.className = "";
    }
    
    Import_ExceptionChecker();
    return;
}

function Import_SetFocus(e)
{
    var keynum;

    if(window.event) // IE
    {
        keynum = e.keyCode;
    }
    else if(e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
    }
    if(keynum==13) { btnTest.click();  return false; }
    else { return true; }
}

function Import_AddExceptionTextbox(arr,i,obj)
{
    arr[i]=obj;
}
function Import_CheckUserInput()
{

    return;
}