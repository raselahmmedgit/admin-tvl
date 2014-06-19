//-----------------------------------------------------
//start Morris Charts Methods

function LoadMorrisDonutChart(elementId, postUrl) {

    $.ajax({
        url: postUrl,
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {

        },
        success: function (result) {

            console.log(result);

            //DONUT CHART
            Morris.Donut({
                element: elementId,
                resize: true,
                //colors: ["#3c8dbc", "#f56954", "#00a65a"],
                data: result.data,
                hideHover: 'auto'
            });
        },
        error: function (objAjaxRequest, strError) {
            var respText = objAjaxRequest.responseText;
            var messageText = respText;
            LoadErrorAppMessageWindowWithText(messageText);
        }

    });

}


function LoadMorrisBarChart(elementId, postUrl) {

    $.ajax({
        url: postUrl,
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {

        },
        success: function (result) {

            console.log(result);

            //BAR CHART
            Morris.Bar({
                element: elementId,
                resize: true,
                data: result.data,
                //barColors: ['#00a65a', '#f56954'],
                xkey: result.xkey,
                ykeys: result.ykeys,
                labels: result.labels,
                hideHover: 'auto'
            });

        },
        error: function (objAjaxRequest, strError) {
            var respText = objAjaxRequest.responseText;
            var messageText = respText;
            LoadErrorAppMessageWindowWithText(messageText);
        }

    });

}


//end Morris Charts Methods
//-----------------------------------------------------

//-----------------------------------------------------
//start Flot Charts Methods

function labelFormatter(label, series) {
    return "<div style='font-size:13px; text-align:center; padding:2px; color: #fff; font-weight: 600;'>"
                        + label
                        + "<br/>"
                        + Math.round(series.percent) + "%</div>";
}

function LoadFlotPieChart(elementId, postUrl) {

    $.ajax({
        url: postUrl,
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {

        },
        success: function (result) {

            console.log(result);

            /* START PIE CHART */
            $.plot(elementId, result.data, {
                series: {
                    pie: {
                        show: true,
                        radius: 1,
                        tilt: 0.5,
                        label: {
                            show: true,
                            radius: 1,
                            formatter: labelFormatter,
                            background: {
                                opacity: 0.8
                            }
                        },
                        combine: {
                            color: '#999',
                            threshold: 0.1
                        }
                    }
                },
                legend: {
                    show: false
                }
            });
            /* END PIE CHART */

        },
        error: function (objAjaxRequest, strError) {
            var respText = objAjaxRequest.responseText;
            var messageText = respText;
            LoadErrorAppMessageWindowWithText(messageText);
        }

    });

}


function LoadFlotBarChart(elementId, postUrl) {

    $.ajax({
        url: postUrl,
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {

        },
        success: function (result) {

            console.log(result.data);

            /* START BAR CHART */
            $.plot(elementId, result.data, {
                grid: {
                    borderWidth: 1,
                    borderColor: "#f3f3f3",
                    tickColor: "#f3f3f3"
                },
                series: {
                    bars: {
                        show: true,
                        barWidth: 0.5,
                        align: "center"
                    }
                },
                xaxis: {
                    mode: "categories",
                    tickLength: 0
                }
            });
            /* END BAR CHART */

        },
        error: function (objAjaxRequest, strError) {
            var respText = objAjaxRequest.responseText;
            var messageText = respText;
            LoadErrorAppMessageWindowWithText(messageText);
        }

    });

}


//end Flot Charts Methods
//-----------------------------------------------------