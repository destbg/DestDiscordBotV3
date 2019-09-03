const connection = new signalR.HubConnectionBuilder()
    .withUrl("/report")
    .build();

function AppViewModel() {
    var self = this;

    self.reports = ko.observableArray([]);

    connection.on("ReportsRecieved", reports => {
        for (var i = 0; i < reports.length; i++) {
            var exists = false;
            var koReports = self.reports();
            for (var j = 0; j < koReports.length; j++)
                if (koReports[j].id === reports[i].id) {
                    exists = true;
                    break;
                }
            if (!exists)
                self.reports.push(reports[i]);
        }
    });

    self.refreshReports = function () {
        connection.send("GiveReports");
    };

    self.removeReport = function () {
        connection.send("RemoveReports", this.id);
        self.reports.remove(this);
    };
}

ko.applyBindings(new AppViewModel());

(async () => {
    await connection.start()
        .catch(console.error);
    await connection.send("GiveReports");
})();