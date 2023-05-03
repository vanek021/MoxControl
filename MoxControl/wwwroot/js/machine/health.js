$(document).ready(async function () {
    var tableRows = $("tr[data-machine-row]").each(async function () {
        var health = await getMachineHealth($(this).attr("data-virtualizationSystem"), $(this).attr("data-machine-id"));

        var cpuProgress = $(this).find("div[data-machine-cpu]");
        var hddProgress = $(this).find("div[data-machine-hdd]");
        var ramProgress = $(this).find("div[data-machine-ram]");

        cpuProgress.find("div.progress-bar").attr("style", `width: ${health.cpuUsedPercent}%;`);
        hddProgress.find("div.progress-bar").attr("style", `width: ${health.hddUsedPercent}%;`);
        ramProgress.find("div.progress-bar").attr("style", `width: ${health.memoryUsedPercent}%;`);

        cpuProgress.find("span").text(`CPU: ${health.cpuUsedPercent}%`);
        hddProgress.find("span").text(`HDD: ${health.hddUsedPercent}%`);
        ramProgress.find("span").text(`RAM: ${health.memoryUsedPercent}%`);
    });
});

async function getMachineHealth(virtualizationSystem, machineId) {
    const response = await fetch(`/Machine/GetMachineHealth/${machineId}?virtualizationSystem=${virtualizationSystem}`);

    if (response.ok) {
        const jsonData = await response.json();
        jsonData.isSuccess = true;
        return jsonData;
    }

    var emptyData = {
        cpuUsedPercent: 0,
        memoryUsedPercent: 0,
        hddUsedPercent: 0,
        isSuccess: false
    }

    return emptyData;
}