//Automatically generates name field based on FKs
function nameGen(executionContext) {
    //Get Form context
    var formContext = executionContext.getFormContext();

    try {
        //Retrieve field values
        var vehicleName = formContext.getAttribute("cr90d_vehicleid").getValue()[0].name.replace(/\s/g, "");
        var garageName = formContext.getAttribute("cr90d_garageid").getValue()[0].name.replace(/\s/g, "");
        var orgName = formContext.getAttribute("cr90d_organisationid").getValue()[0].name.replace(/\s/g, "");

        //Set name
        formContext.getAttribute("cr90d_name").setValue(`${orgName}_${vehicleName}_${garageName}`);
        formContext.getControl("cr90d_name").clearNotification("txtNotiName");
    } catch {
        //Set notification
        formContext.getControl("cr90d_name").setNotification("Enter all lookup options to populate this field", "txtNotiName");
    }
}