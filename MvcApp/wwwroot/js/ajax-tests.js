async function AjaxRequestWithOperationName() {
    let OperationName = "Operation1";
    let Args = await tp.AjaxRequest.Execute(OperationName);
    
    let JsonText = tp.ToJson(Args, true); // JSON.stringify(Args, null, 3);    

    let el = document.getElementById("ajax-results");
    el.innerHTML = "";

    let Pre = tp.el(el, "pre");
    Pre.innerHTML = JsonText;
}