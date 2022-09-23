//Checks stock vs restock quantity to determine if the restock toggle should be activated
function restockUpdate(executionContext) {
    var formContext = executionContext.getFormContext();

    var stock = formContext.getAttribute("grocery_stock").getValue();
    var restockQty = formContext.getAttribute("grocery_restockquantity").getValue();

    if (restockQty > stock) {
        
    }

}

//Alerts user to write an informative name
function alertName(executionContext) {
    var formContext = executionContext.getFormContext();

    var name = formContext.getAttribute("grocery_name").getValue();

    if (name.length < 3) {
        formContext.getControl("grocery_name").setNotification("Please enter an informative name");
    }
    else {
        formContext.getControl("grocery_name").clearNotification();
    }
}