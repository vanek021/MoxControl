$(document).ready(async function () {
    $("tr[data-server-row]").each(async function () {
        var health = await getServerHealth($(this).attr("data-virtualizationSystem"), $(this).attr("data-server-id"));

        var cpuProgress = $(this).find("div[data-server-cpu]");
        var hddProgress = $(this).find("div[data-server-hdd]");
        var ramProgress = $(this).find("div[data-server-ram]");

        cpuProgress.find("div.progress-bar").attr("style", `width: ${health.cpuUsedPercent}%;`);
        hddProgress.find("div.progress-bar").attr("style", `width: ${health.hddUsedPercent}%;`);
        ramProgress.find("div.progress-bar").attr("style", `width: ${health.memoryUsedPercent}%;`);

        cpuProgress.find("span").text(`CPU: ${health.cpuUsedPercent}%`);
        hddProgress.find("span").text(`HDD: ${health.hddUsedPercent}%`);
        ramProgress.find("span").text(`RAM: ${health.memoryUsedPercent}%`);
    });
});

async function getServerHealth(virtualizationSystem, serverId) {
    const response = await fetch(`/Server/GetServerHealth/${serverId}?virtualizationSystem=${virtualizationSystem}`);

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