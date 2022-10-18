import * as ko from "knockout"
import * as $ from "jquery"

console.log("TestModel page initialized");

$(async () => {
    console.log("Document Ready");
    let testModel = new TestModel("Bijay", "Maharjan");
    ko.applyBindings(testModel, document.getElementById("myview-container"));

});

class TestModel {
    firstname: KnockoutObservable<string>;
    lastname: KnockoutObservable<string>;

    constructor(firstname: string, lastname: string) {
        this.firstname = ko.observable(firstname);
        this.lastname = ko.observable(lastname);
    }
}