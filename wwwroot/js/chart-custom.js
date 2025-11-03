//chart report
const chartReport = document.querySelector("#report-chart");
if (chartReport) {
    var options = {
        series: [{
            name: 'series1',
            data: [60, 70, 54, 51, 42, 109, 100]
        }],
        chart: {
            height: 320,
            type: 'area',
            dropShadow: {
                enabled: true,
                top: 10,
                left: 0,
                blur: 3,
                color: '#720f1e',
                opacity: 0.15
            },
            toolbar: {
                show: false
            },
            zoom: {
                enabled: false
            },
        },
        markers: {
            strokeWidth: 4,
            strokeColors: "#ffffff",
            hover: {
                size: 9,
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth',
            lineCap: 'butt',
            width: 4,
        },
        legend: {
            show: false
        },
        colors: ["#0da487"],
        fill: {
            type: 'gradient',
            gradient: {
                shadeIntensity: 1,
                opacityFrom: 0.7,
                opacityTo: 0.6,
                stops: [0, 90, 100]
            }
        },
        grid: {
            xaxis: {
                lines: {
                    borderColor: 'transparent',
                    show: true
                }
            },
            yaxis: {
                lines: {
                    borderColor: 'transparent',
                    show: false,
                }

            },
            padding: {
                right: -112,
                bottom: 0,
                left: 15
            }
        },
        responsive: [{
            breakpoint: 1200,
            options: {
                grid: {
                    padding: {
                        right: -95,
                    }
                },
            },
        },
        {
            breakpoint: 992,
            options: {
                grid: {
                    padding: {
                        right: -69,
                    }
                },
            },
        },
        {
            breakpoint: 767,
            options: {
                chart: {
                    height: 200,
                }
            },
        },
        {
            breakpoint: 576,
            options: {
                yaxis: {
                    labels: {
                        show: false,
                    },
                },
            },
        }
        ],
        yaxis: {
            labels: {
                formatter: function (value) {
                    return "$" + value;
                }
            },
            crosshairs: {
                show: true,
                position: 'back',
                stroke: {
                    color: '#b6b6b6',
                    width: 1,
                    dashArray: 5,
                },
            },
            tooltip: {
                enabled: true,
            },
        },
        xaxis: {
            categories: ["Jan", "Feb", "Mar", "April", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec",],
            range: undefined,
            axisBorder: {
                low: 0,
                offsetX: 0,
                show: false,
            },
            axisTicks: {
                show: false,
            },
        },
    };

    var chart = new ApexCharts(chartReport, options);
    chart.render();
}
//end chart report

//chart earning
const chartEarning = document.querySelector("#earning-chart");
if (chartEarning) {
    var options = {
        series: [{
            data: [35, 41, 62, 42, 13, 18, 29, 37, 36, 51, 32, 35]
        },

        {
            data: [87, 57, 74, 99, 75, 38, 62, 47, 82, 56, 45, 47]
        }
        ],

        chart: {
            height: 320,
            toolbar: {
                show: false
            },
            zoom: {
                enabled: false
            },
            events: {
                mouseMove: function (event, chartContext, config) {
                    chartEarning.style.cursor = "crosshair";
                },
                mouseLeave: function (event, chartContext, config) {
                    chartEarning.style.cursor = "default";
                }
            }
        },

        legend: {
            show: false,
        },

        colors: ['#0da487', '#2483e2'],

        markers: {
            size: 1,
        },
        dataLabels: {
            enabled: false
        },
        xaxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
            labels: {
                show: false,
            }
        },

        responsive: [{
            breakpoint: 1400,
            options: {
                chart: {
                    height: 300,
                },
            },
        },

        {
            breakpoint: 992,
            options: {
                chart: {
                    height: 210,
                    width: "100%",
                    offsetX: 0,
                },
            },
        },

        {
            breakpoint: 578,
            options: {
                chart: {
                    height: 200,
                    width: "105%",
                    offsetX: -20,
                    offsetY: 10,
                },
            },
        },

        {
            breakpoint: 430,
            options: {
                chart: {
                    width: "108%",
                },
            },
        },

        {
            breakpoint: 330,
            options: {
                chart: {
                    width: "112%",
                },
            },
        },
        ],
    };

    const chart = new ApexCharts(chartEarning, options);
    chart.render();
}
//end chart earning

//visitors chart 
const chartVisitors = document.querySelector("#visitors-chart");
if (chartVisitors) {
    var options = {
        series: [44, 55, 41, 17],
        labels: ['The Passersby', 'The Occasionals', 'The Regulars', 'The Superfans'],
        chart: {
            width: "100%",
            height: 275,
            type: 'donut',
        },

        legend: {
            fontSize: '12px',
            position: 'bottom',
            offsetX: 1,
            offsetY: -1,

            markers: {
                width: 10,
                height: 10,
            },

            itemMargin: {
                vertical: 2
            },
        },

        colors: ['#28c870', '#ffa044', '#9e65c2', '#6670bd', '#FF9800'],

        plotOptions: {
            pie: {
                startAngle: -90,
                endAngle: 270
            }
        },

        dataLabels: {
            enabled: false
        },

        responsive: [{
            breakpoint: 1835,
            options: {
                chart: {
                    height: 275,
                },

                legend: {
                    position: 'bottom',

                    itemMargin: {
                        horizontal: 5,
                        vertical: 1
                    },
                },
            },
        },

        {
            breakpoint: 1388,
            options: {
                chart: {
                    height: 330,
                },

                legend: {
                    position: 'bottom',
                },
            },
        },

        {
            breakpoint: 1275,
            options: {
                chart: {
                    height: 300,
                },

                legend: {
                    position: 'bottom',
                },
            },
        },

        {
            breakpoint: 1158,
            options: {
                chart: {
                    height: 280,
                },

                legend: {
                    fontSize: '10px',
                    position: 'bottom',
                    offsetY: 10,
                },
            },
        },

        {
            theme: {
                mode: 'dark',
                palette: 'palette1',
                monochrome: {
                    enabled: true,
                    color: '#255aee',
                    shadeTo: 'dark',
                    shadeIntensity: 0.65
                },
            },
        },

        {
            breakpoint: 598,
            options: {
                chart: {
                    height: 280,
                },

                legend: {
                    fontSize: '12px',
                    position: 'bottom',
                    offsetX: 5,
                    offsetY: -5,

                    markers: {
                        width: 10,
                        height: 10,
                    },

                    itemMargin: {
                        vertical: 1
                    },
                },
            },
        },
        ],
    };

    var chart = new ApexCharts(chartVisitors, options);
    chart.render();
}
//end visitors chart 