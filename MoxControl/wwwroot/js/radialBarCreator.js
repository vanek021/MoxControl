var options = {
    series: [0],
    chart: {
        type: 'radialBar',
        offsetY: -20,
        sparkline: {
            enabled: true
        }
    },
    plotOptions: {
        radialBar: {
            inverseOrder: false,
            startAngle: 0,
            endAngle: 360,
            offsetX: 0,
            offsetY: 0,
            hollow: {
                margin: 5,
                size: '50%',
                background: 'transparent',
                image: undefined,
                imageWidth: 150,
                imageHeight: 150,
                imageOffsetX: 0,
                imageOffsetY: 0,
                imageClipped: true,
                position: 'front',
                dropShadow: {
                    enabled: false,
                    top: 0,
                    left: 0,
                    blur: 3,
                    opacity: 0.5
                }
            },
            dataLabels: {
                name: {
                    show: true,
                    fontSize: '16px',
                    fontFamily: undefined,
                    fontWeight: 600,
                    color: '#000000',
                    offsetY: -10
                },
                value: {
                    show: true,
                    fontSize: '14px',
                    fontFamily: undefined,
                    fontWeight: 400,
                    color: undefined,
                    offsetY: 16,
                    formatter: function (val) {
                        return val + '%'
                    }
                },
            }
        }
    },
    grid: {
        padding: {
            top: -10
        }
    },
    fill: {
        type: 'gradient',
        colors: ['#00B2FF'],
        gradient: {
            shade: 'light',
            shadeIntensity: 0,
            inverseColors: false,
            opacityFrom: 1,
            opacityTo: 1,
            stops: [0, 50, 53, 91]
        },
    },
    labels: ['Average Results'],
};

function createAndRenderRadialBar(element, color, value, label) {
    options.series = [value];
    options.labels = [label];
    options.colors = [color];
    var chart = new ApexCharts(document.querySelector(`#${element}`), options);
    chart.render();

    return chart;
}

function updateRadialBar(chart, color, value, label) {
    options.series = [value];
    options.labels = [label];
    options.colors = [color];

    chart.updateOptions(options);
}