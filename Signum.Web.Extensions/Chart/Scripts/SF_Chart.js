﻿var SF = SF || {};

SF.Chart = SF.Chart || {};

SF.Chart.Builder = (function () {

    var updateChartBuilder = function ($chartControl) {
        var $chartBuilder = $chartControl.find(".sf-chart-builder");
        $.ajax({
            url: $chartBuilder.attr("data-url"),
            data: $chartControl.find(":input").serialize(),
            success: function (result) {
                $chartBuilder.replaceWith(result);
                SF.triggerNewContent($chartControl.find(".sf-chart-builder"));
            }
        });
    };

    $(".sf-chart-img").live("click", function () {
        var $this = $(this);
        $this.closest(".sf-chart-type").find(".ui-widget-header .sf-chart-type-value").val($this.attr("data-related"));
        var $chartControl = $this.closest(".sf-chart-control");
        updateChartBuilder($chartControl);

        var $resultsContainer = $chartControl.find(".sf-search-results-container");
        if ($this.hasClass("sf-chart-img-equiv")) {
            if ($resultsContainer.find("svg").length > 0) {
                //TODO do not call server again to regenerate chart
                $chartControl.find(".sf-chart-draw").click();
            }
        }
        else {
            $resultsContainer.html("");
        }
    });

    $(".sf-chart-group-trigger").live("change", function () {
        var $this = $(this);
        $this.closest(".sf-chart-builder").find(".sf-chart-group-results").val($this.is(":checked"));
        updateChartBuilder($this.closest(".sf-chart-control"));
    });

    $(".sf-chart-draw").live("click", function (e) {
        e.preventDefault();
        var $this = $(this);
        var $chartControl = $this.closest(".sf-chart-control");
        $.ajax({
            url: $this.attr("data-url"),
            data: $chartControl.find(":input").serialize(),
            success: function (result) {
                if (typeof result === "object") {
                    if (typeof result.ModelState != "undefined") {
                        var modelState = result.ModelState;
                        returnValue = new SF.Validator().showErrors(modelState, true);
                        SF.Notify.error(lang.signum.error, 2000);
                    }
                }
                else {
                    $chartControl.find(".sf-search-results-container").html(result);
                    SF.triggerNewContent($chartControl.find(".sf-search-results-container"));
                }
            }
        });
    });
})();

SF.Chart.Factory = (function () {
    var br = "\n",
        brt = "\n\t";

    var getGraphType = function (key) {
        return new SF.Chart[key]();
    };

    Array.prototype.enterData = function (data, tag, cssClass) {
        return this.selectAll(tag + "." + cssClass).data(data)
        .enter().append("svg:" + tag)
        .attr("class", cssClass);
    };

    return {
        br: br,
        brt: brt,
        getGraphType: getGraphType
    };
})();

SF.Chart.ChartBase = function () { };
SF.Chart.ChartBase.prototype = {

    br: SF.Chart.Factory.br,
    brt: SF.Chart.Factory.brt,
    fontSize: $('<span>&nbsp;</span>').appendTo($('body')).outerHeight(true),
    ticksLength: 4,
    labelMargin: 5,
    chartAxisPadding: 5,
    padding: 5,

    createChartSVG: function (selector) {
        return "var chart = d3.select('" + selector + "')" + this.brt +
            ".append('svg:svg').attr('width', width).attr('height', height);" + this.br + this.br;
    },

    paintXAxis: function () {
        return "//paint x-axis" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".append('svg:line')" + this.brt +
            ".attr('class', 'x-axis')" + this.brt +
            ".attr('x2', width - yAxisLeftPosition - padding);" + this.br + this.br;
    },

    paintYAxis: function () {
        return "//paint y-axis" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + padding + ')')" + this.brt +
            ".append('svg:line')" + this.brt +
            ".attr('class', 'y-axis')" + this.brt +
            ".attr('y2', xAxisTopPosition - padding);" + this.br + this.br;
    },

    paintLegend: function () {
        return "";
    },

    getTokenLabel: function (tokenValue) {
        return tokenValue.toStr || tokenValue;
    },

    getPathPoints: function (points) {
        var result = "";
        var jump = true;
        $.each(points, function (i, p) {
            if (p.x == null || p.y == null) {
                jump = true;
            }
            else {
                result += (jump ? " M " : " ") + p.x + " " + p.y;
                jump = false;
            }
        });
        return result;
    },

    paintChart: function () {
        return this.init() +
        this.getXAxis() +
        this.getYAxis() +
        this.paintXAxisRuler() +
        this.paintYAxisRuler() +
        this.paintGraph() +
        this.paintXAxis() +
        this.paintYAxis() +
        this.paintLegend();
    }
};

SF.Chart.Bars = function () {
    SF.Chart.ChartBase.call(this);
};
SF.Chart.Bars.prototype = $.extend({}, new SF.Chart.ChartBase(), {
    init: function () {
        return "//config variables" + this.br +
            "var yAxisLabelWidth = 150," + this.brt +
            "fontSize= " + this.fontSize + "," + this.brt +
            "ticksLength= " + this.ticksLength + "," + this.brt +
            "labelMargin= " + this.labelMargin + "," + this.brt +
            "chartAxisPadding= " + this.chartAxisPadding + "," + this.brt +
            "padding= " + this.padding + "," + this.brt +
            "yAxisLeftPosition = padding + fontSize + yAxisLabelWidth + (2 * labelMargin) + ticksLength," + this.brt +
            "xAxisTopPosition = height - padding - (fontSize * 2) - (labelMargin * 2) - ticksLength," + this.brt +
            "color = (data.serie.length < 10 ? d3.scale.category10() : d3.scale.category20()).domain([0, $.map(data.serie, function(v) { return JSON.stringify(v); })]);" + this.br + this.br;
    },

    getXAxis: function () {
        return "//x axis scale" + this.br +
            "var x = d3.scale.linear()" + this.brt +
            ".domain([0, d3.max($.map(data.serie, function (e) { return myChart.getTokenLabel(e.value1); }))])" + this.brt +
            ".range([0, width - yAxisLeftPosition - padding]);" + this.br + this.br;
    },

    getYAxis: function () {
        return "//y axis scale" + this.br +
            "var y = d3.scale.ordinal()" + this.brt +
            ".domain($.map(data.serie, function (e) { return JSON.stringify(e); }))" + this.brt +
            ".rangeBands([0, xAxisTopPosition - padding- (2 * chartAxisPadding)]);" + this.br + this.br;
    },

    paintXAxisRuler: function () {
        return "//paint x-axis - ruler" + this.br +
            "chart.append('svg:g').attr('class', 'x-ruler').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + padding + ')')" + this.brt +
            ".enterData(x.ticks(8), 'line', 'x-ruler')" + this.brt +
            ".attr('x1', x)" + this.brt +
            ".attr('x2', x)" + this.brt +
            ".attr('y2', xAxisTopPosition - padding);" + this.br +
            this.br +
            "//paint x-axis - ticks" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-tick').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(x.ticks(8), 'line', 'x-axis-tick')" + this.brt +
            ".attr('x1', x)" + this.brt +
            ".attr('x2', x)" + this.brt +
            ".attr('y2', ticksLength);" + this.br +
            this.br +
            "//paint x-axis - tick labels" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-tick-label').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + (xAxisTopPosition + ticksLength + labelMargin + fontSize) + ')')" + this.brt +
            ".enterData(x.ticks(8), 'text', 'x-axis-tick-label')" + this.brt +
            ".attr('x', x)" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(String);" + this.br +
            this.br +
            "//paint x-axis - token label" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-token-label').attr('transform', 'translate(' + (yAxisLeftPosition + ((width - yAxisLeftPosition) / 2)) + ', ' + height + ')')" + this.brt +
            ".append('svg:text').attr('class', 'x-axis-token-label')" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(data.labels.value1);" + this.br + this.br;
    },

    paintYAxisRuler: function () {
        return "//paint y-axis ticks" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-tick').attr('transform', 'translate(' + (yAxisLeftPosition - ticksLength) + ', ' + (padding + chartAxisPadding + (y.rangeBand() / 2)) + ')')" + this.brt +
            ".enterData(data.serie, 'line', 'y-axis-tick')" + this.brt +
            ".attr('x2', ticksLength)" + this.brt +
            ".attr('y1', function (v) { return y(JSON.stringify(v)); })" + this.brt +
            ".attr('y2', function (v) { return y(JSON.stringify(v)); });" + this.br +
            this.br +
            "//paint y-axis tick labels" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-tick-label').attr('transform', 'translate(' + (yAxisLeftPosition - ticksLength - labelMargin) + ', ' + (padding + chartAxisPadding + (y.rangeBand() * 2 / 3)) + ')')" + this.brt +
            ".enterData(data.serie, 'text', 'y-axis-tick-label')" + this.brt +
            ".attr('y', function (v) { return y(JSON.stringify(v)); })" + this.brt +
            ".attr('text-anchor', 'end')" + this.brt +
            ".attr('width', yAxisLabelWidth)" + this.brt +
            ".text(function (v) { return myChart.getTokenLabel(v.dimension1); });" + this.br +
            this.br +
            "//paint y-axis - token label" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-token-label').attr('transform', 'translate(' + fontSize + ', ' + (xAxisTopPosition / 2) + ') rotate(270)')" + this.brt +
            ".append('svg:text').attr('class', 'y-axis-token-label')" + this.brt +
            ".attr('text-anchor', 'start')" + this.brt +
            ".text(data.labels.dimension1);" + this.br + this.br;
    },

    paintGraph: function () {
        return "//paint graph" + this.br +
            "chart.append('svg:g').attr('class', 'shape').attr('transform' ,'translate(' + yAxisLeftPosition + ', ' + (padding + chartAxisPadding) + ')')" + this.brt +
            ".enterData(data.serie, 'rect', 'shape')" + this.brt +
            ".attr('width', function (v) { return x(myChart.getTokenLabel(v.value1)); })" + this.brt +
            ".attr('height', y.rangeBand)" + this.brt +
            ".attr('y', function (v) { return y(JSON.stringify(v)); })" + this.brt +
            ".attr('fill', function (v) { return color(JSON.stringify(v)); })" + this.brt +
            ".attr('stroke', '#fff')" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(d) { return myChart.getTokenLabel(d.dimension1) + ': ' + myChart.getTokenLabel(d.value1); });" + this.br + this.br;
    }
});

SF.Chart.Columns = function () {
    SF.Chart.ChartBase.call(this);
};
SF.Chart.Columns.prototype = $.extend({}, new SF.Chart.ChartBase(), {

    init: function () {
        return "//config variables" + this.br +
            "var yAxisLabelWidth = 25," + this.brt +
            "fontSize= " + this.fontSize + "," + this.brt +
            "ticksLength= " + this.ticksLength + "," + this.brt +
            "labelMargin= " + this.labelMargin + "," + this.brt +
            "chartAxisPadding= " + this.chartAxisPadding + "," + this.brt +
            "padding= " + this.padding + "," + this.brt +
            "yAxisLeftPosition = padding + fontSize + yAxisLabelWidth + (2 * labelMargin) + ticksLength," + this.brt +
            "xAxisTopPosition = height - padding - (fontSize * 3) - (labelMargin * 3) - ticksLength," + this.brt +
            "color = (data.serie.length < 10 ? d3.scale.category10() : d3.scale.category20()).domain([0, $.map(data.serie, function(v) { return JSON.stringify(v); })]);" + this.br + this.br;
    },

    getXAxis: function () {
        return "//x axis scale" + this.br +
            "var x = d3.scale.ordinal()" + this.brt +
            ".domain($.map(data.serie, function (e) { return JSON.stringify(e); }))" + this.brt +
            ".rangeBands([0, width - yAxisLeftPosition - padding - (2 * chartAxisPadding)]);" + this.br + this.br;
    },

    getYAxis: function () {
        return "//y axis scale" + this.br +
            "var y = d3.scale.linear()" + this.brt +
            ".domain([0, d3.max($.map(data.serie, function (e) { return myChart.getTokenLabel(e.value1); }))])" + this.brt +
            ".range([0, xAxisTopPosition - padding]);" + this.br + this.br;
    },

    paintXAxisRuler: function () {
        return "//paint x-axis ticks" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-tick').attr('transform', 'translate(' + (yAxisLeftPosition + (x.rangeBand() / 2)) + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(data.serie, 'line', 'x-axis-tick')" + this.brt +
            ".attr('y2', function (v, i) { return (i%2 == 0 ? ticksLength : (ticksLength + fontSize + labelMargin)); })" + this.brt +
            ".attr('x1', function (v) { return x(JSON.stringify(v)); })" + this.brt +
            ".attr('x2', function (v) { return x(JSON.stringify(v)); });" + this.br +
            this.br +
            "//paint x-axis tick labels" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-tick-label').attr('transform', 'translate(' + (yAxisLeftPosition + (x.rangeBand() / 2)) + ', ' + (xAxisTopPosition + ticksLength + labelMargin + fontSize) + ')')" + this.brt +
            ".enterData(data.serie, 'text', 'x-axis-tick-label')" + this.brt +
            ".attr('x', function (v) { return x(JSON.stringify(v)); })" + this.brt +
            ".attr('y', function (v, i) { return (i%2 == 0 ? 0 : (fontSize + labelMargin)); })" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(function (v) { return myChart.getTokenLabel(v.dimension1); });" + this.br +
            this.br +
            "//paint x-axis - token label" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-token-label').attr('transform', 'translate(' + (yAxisLeftPosition + ((width - yAxisLeftPosition) / 2)) + ', ' + (height) + ')')" + this.brt +
            ".append('svg:text').attr('class', 'x-axis-token-label')" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(data.labels.dimension1);" + this.br + this.br;
    },

    paintYAxisRuler: function () {
        return "//paint y-axis - ruler" + this.br +
            "var yTicks = y.ticks(8);" + this.br +
            "chart.append('svg:g').attr('class', 'y-ruler').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(yTicks, 'line', 'y-ruler')" + this.brt +
            ".attr('x2', width - yAxisLeftPosition - padding)" + this.brt +
            ".attr('y1', function(t) { return -y(t); })" + this.brt +
            ".attr('y2', function(t) { return -y(t); });" + this.br +
            this.br +
            "//paint y-axis - ticks" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-tick').attr('transform', 'translate(' + (yAxisLeftPosition - ticksLength) + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(yTicks, 'line', 'y-axis-tick')" + this.brt +
            ".attr('x2', ticksLength)" + this.brt +
            ".attr('y1', function(t) { return -y(t); })" + this.brt +
            ".attr('y2', function(t) { return -y(t); });" + this.br +
            this.br +
            "//paint y-axis - tick labels" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-tick-label').attr('transform', 'translate(' + (yAxisLeftPosition - ticksLength - labelMargin) + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(yTicks, 'text', 'y-axis-tick-label')" + this.brt +
            ".attr('y', function(t) { return -y(t); })" + this.brt +
            ".attr('dominant-baseline', 'middle')" + this.brt +
            ".attr('text-anchor', 'end')" + this.brt +
            ".text(String);" + this.br +
            this.br +
            "//paint y-axis - token label" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-token-label').attr('transform', 'translate(' + fontSize + ', ' + (xAxisTopPosition / 2) + ') rotate(270)')" + this.brt +
            ".append('svg:text').attr('class', 'y-axis-token-label')" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(data.labels.value1);" + this.br + this.br;
    },

    paintGraph: function () {
        return "//paint graph" + this.br +
            "chart.append('svg:g').attr('class', 'shape').attr('transform' ,'translate(' + (yAxisLeftPosition + chartAxisPadding) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".enterData(data.serie, 'rect', 'shape')" + this.brt +
            ".attr('height', function (v) { return y(myChart.getTokenLabel(v.value1)); })" + this.brt +
            ".attr('width', x.rangeBand)" + this.brt +
            ".attr('x', function (v) { return x(JSON.stringify(v)); })" + this.brt +
            ".attr('fill', function (v) { return color(JSON.stringify(v)); })" + this.brt +
            ".attr('stroke', '#fff')" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(d) { return myChart.getTokenLabel(d.dimension1) + ': ' + myChart.getTokenLabel(d.value1); });" + this.br + this.br;
    }
});

SF.Chart.Lines = function () {
    SF.Chart.Columns.call(this);
}
SF.Chart.Lines.prototype = $.extend({}, new SF.Chart.Columns(), {

    init: function () {
        return "//config variables" + this.br +
            "var yAxisLabelWidth = 25," + this.brt +
            "fontSize= " + this.fontSize + "," + this.brt +
            "ticksLength= " + this.ticksLength + "," + this.brt +
            "labelMargin= " + this.labelMargin + "," + this.brt +
            "chartAxisPadding= " + this.chartAxisPadding + "," + this.brt +
            "padding= " + this.padding + "," + this.brt +
            "yAxisLeftPosition = padding + fontSize + yAxisLabelWidth + (2 * labelMargin) + ticksLength," + this.brt +
            "xAxisTopPosition = height - padding - (fontSize * 3) - (labelMargin * 3) - ticksLength," + this.brt +
            "color = 'steelblue';" + this.br + this.br;
    },

    paintGraph: function () {
        return "//paint graph - line" + this.br +
            "chart.append('svg:g').attr('class', 'shape').attr('transform' ,'translate(' + (yAxisLeftPosition + chartAxisPadding + (x.rangeBand() / 2)) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".append('svg:path').attr('class', 'shape')" + this.brt +
            ".attr('stroke', color)" + this.brt +
            ".attr('fill', 'none')" + this.brt +
            ".attr('stroke-width', 3)" + this.brt +
            ".attr('shape-rendering', 'initial')" + this.brt +
            ".attr('d', myChart.getPathPoints($.map(data.serie, function(v) { return {x: x(JSON.stringify(v)), y: y(myChart.getTokenLabel(v.value1))};})))" + this.br +
            this.br +
            "//paint graph - hover area trigger" + this.br +
            "chart.append('svg:g').attr('class', 'hover-trigger').attr('transform' ,'translate(' + (yAxisLeftPosition + chartAxisPadding + (x.rangeBand() / 2)) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".enterData(data.serie, 'circle', 'hover-trigger')" + this.brt +
            ".attr('cx', function(v) { return x(JSON.stringify(v)); })" + this.brt +
            ".attr('cy', function(v) { return y(myChart.getTokenLabel(v.value1)); })" + this.brt +
            ".attr('r', 15)" + this.brt +
            ".attr('fill', '#fff')" + this.brt +
            ".attr('fill-opacity', 0)" + this.brt +
            ".attr('stroke', 'none')" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(d) { return myChart.getTokenLabel(d.dimension1) + ': ' + myChart.getTokenLabel(d.value1); })" + this.br +
            this.br +
            "//paint graph - points" + this.br +
            "chart.append('svg:g').attr('class', 'point').attr('transform' ,'translate(' + (yAxisLeftPosition + chartAxisPadding + (x.rangeBand() / 2)) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".enterData(data.serie, 'circle', 'point')" + this.brt +
            ".attr('fill', color)" + this.brt +
            ".attr('r', 5)" + this.brt +
            ".attr('cx', function(v) { return x(JSON.stringify(v)); })" + this.brt +
            ".attr('cy', function(v) { return y(myChart.getTokenLabel(v.value1)); });" + this.br + this.br;
    }
});

SF.Chart.TypeTypeValue = function () {
    SF.Chart.ChartBase.call(this);
}
SF.Chart.TypeTypeValue.prototype = $.extend({}, new SF.Chart.ChartBase(), {

    getMaxValue1: function (series) {
        var completeArray = [];
        $.each(series, function (i, s) {
            $.merge(completeArray, s.values);
        });
        return d3.max(completeArray);
    },

    createEmptyCountArray: function (length) {
        var countArray = [];
        for (var i = 0; i < length; i++) {
            countArray.push(0);
        }
        return countArray;
    },

    createCountArray: function (series) {
        var dimensionCount = series[0].values.length;
        var countArray = this.createEmptyCountArray(dimensionCount);

        $.each(series, function (i, serie) {
            for (var i = 0; i < dimensionCount; i++) {
                var v = serie.values[i];
                if (!SF.isEmpty(v)) {
                    countArray[i] += v;
                }
            }
        });

        return countArray;
    },

    init: function () {
        return "//config variables" + this.br +
            "var yAxisLabelWidth = 25," + this.brt +
            "fontSize= " + this.fontSize + "," + this.brt +
            "ticksLength= " + this.ticksLength + "," + this.brt +
            "labelMargin= " + this.labelMargin + "," + this.brt +
            "chartAxisPadding= " + this.chartAxisPadding + "," + this.brt +
            "padding= " + this.padding + "," + this.brt +
            "yAxisLeftPosition = padding + fontSize + yAxisLabelWidth + (2 * labelMargin) + ticksLength," + this.brt +
            "xAxisTopPosition = height - padding - (fontSize * 3) - (labelMargin * 3) - ticksLength," + this.brt +
            "color = (data.series.length < 10 ? d3.scale.category10() : d3.scale.category20()).domain([0, $.map(data.series, function(s) { return JSON.stringify(s.dimension2); })]);" + this.br + this.br;
    },

    getXAxis: function () {
        return "//x axis scale" + this.br +
            "var x = d3.scale.ordinal()" + this.brt +
            ".domain($.map(data.dimension1, function (v) { return JSON.stringify(v); }))" + this.brt +
            ".rangeBands([0, width - yAxisLeftPosition - padding - (2 * chartAxisPadding)]);" + this.br + this.br;
    },

    getYAxis: function () {
        return "//y axis scale" + this.br +
            "var y = d3.scale.linear()" + this.brt +
            ".domain([0, myChart.getMaxValue1(data.series)])" + this.brt +
            ".range([0, xAxisTopPosition - padding - fontSize - labelMargin]);" + this.br + this.br;
    },

    paintXAxis: function () {
        return "//paint x-axis" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".append('svg:line')" + this.brt +
            ".attr('class', 'x-axis')" + this.brt +
            ".attr('x2', width - yAxisLeftPosition - padding);" + this.br + this.br;
    },

    paintYAxis: function () {
        return "//paint y-axis" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + (padding + fontSize + labelMargin) + ')')" + this.brt +
            ".append('svg:line')" + this.brt +
            ".attr('class', 'y-axis')" + this.brt +
            ".attr('y2', xAxisTopPosition - padding - fontSize - labelMargin);" + this.br + this.br;
    },

    paintXAxisRuler: function () {
        return "//paint x-axis ticks" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-tick').attr('transform', 'translate(' + (yAxisLeftPosition + (x.rangeBand() / 2)) + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(data.dimension1, 'line', 'x-axis-tick')" + this.brt +
            ".attr('y2', function (v, i) { return (i%2 == 0 ? ticksLength : (ticksLength + fontSize + labelMargin)); })" + this.brt +
            ".attr('x1', function (v) { return x(JSON.stringify(v)); })" + this.brt +
            ".attr('x2', function (v) { return x(JSON.stringify(v)); });" + this.br +
            this.br +
            "//paint x-axis tick labels" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-tick-label').attr('transform', 'translate(' + (yAxisLeftPosition + (x.rangeBand() / 2)) + ', ' + (xAxisTopPosition + ticksLength + labelMargin + fontSize) + ')')" + this.brt +
            ".enterData(data.dimension1, 'text', 'x-axis-tick-label')" + this.brt +
            ".attr('x', function (v) { return x(JSON.stringify(v)); })" + this.brt +
            ".attr('y', function (v, i) { return (i%2 == 0 ? 0 : (fontSize + labelMargin)); })" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(function (v) { return myChart.getTokenLabel(v); });" + this.br +
            this.br +
            "//paint x-axis - token label" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-token-label').attr('transform', 'translate(' + (yAxisLeftPosition + ((width - yAxisLeftPosition) / 2)) + ', ' + (height) + ')')" + this.brt +
            ".append('svg:text').attr('class', 'x-axis-token-label')" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(data.labels.dimension1);" + this.br + this.br;
    },

    paintYAxisRuler: function () {
        return "//paint y-axis - ruler" + this.br +
            "var yTicks = y.ticks(10);" + this.br +
            "chart.append('svg:g').attr('class', 'y-ruler').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(yTicks, 'line', 'y-ruler')" + this.brt +
            ".attr('x2', width - yAxisLeftPosition - padding)" + this.brt +
            ".attr('y1', function(t) { return -y(t); })" + this.brt +
            ".attr('y2', function(t) { return -y(t); });" + this.br +
            this.br +
            "//paint y-axis - ticks" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-tick').attr('transform', 'translate(' + (yAxisLeftPosition - ticksLength) + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(yTicks, 'line', 'y-axis-tick')" + this.brt +
            ".attr('x2', ticksLength)" + this.brt +
            ".attr('y1', function(t) { return -y(t); })" + this.brt +
            ".attr('y2', function(t) { return -y(t); });" + this.br +
            this.br +
            "//paint y-axis - tick labels" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-tick-label').attr('transform', 'translate(' + (yAxisLeftPosition - ticksLength - labelMargin) + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(yTicks, 'text', 'y-axis-tick-label')" + this.brt +
            ".attr('y', function(t) { return -y(t); })" + this.brt +
            ".attr('dominant-baseline', 'middle')" + this.brt +
            ".attr('text-anchor', 'end')" + this.brt +
            ".text(String);" + this.br +
            this.br +
            "//paint y-axis - token label" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-token-label').attr('transform', 'translate(' + fontSize + ', ' + ((xAxisTopPosition - fontSize - labelMargin) / 2) + ') rotate(270)')" + this.brt +
            ".append('svg:text').attr('class', 'y-axis-token-label')" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(data.labels.value1);" + this.br + this.br;
    },

    paintLegend: function () {
        return "//paint color legend" + this.br +
            "var legendScale = d3.scale.ordinal().domain($.map(data.series, function (s, i) { return i; })).rangeBands([0, width - yAxisLeftPosition - padding])," + this.brt +
            "legendRectWidth = 10," + this.brt +
            "legendLabelWidth = legendScale.rangeBand() - (2 * labelMargin) - legendRectWidth;" + this.br +
            this.br +
            "chart.append('svg:g').attr('class', 'color-legend').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + padding + ')')" + this.brt +
            ".enterData(data.series, 'rect', 'color-rect')" + this.brt +
            ".attr('x', function(e, i) { return (legendRectWidth + legendLabelWidth + (2 * labelMargin)) * i; })" + this.brt +
            ".attr('width', legendRectWidth).attr('height', fontSize)" + this.brt +
            ".attr('fill', function(s) { return color(JSON.stringify(s.dimension2)); });" + this.br +
            this.br +
            "chart.append('svg:g').attr('class', 'color-legend').attr('transform', 'translate(' + (yAxisLeftPosition + labelMargin + legendRectWidth) + ', ' + (padding + fontSize) + ')')" + this.brt +
            ".enterData(data.series, 'text', 'color-text')" + this.brt +
            ".attr('x', function(e, i) { return (legendRectWidth + legendLabelWidth + (2 * labelMargin)) * i; })" + this.brt +
            ".text(function(s) { return myChart.getTokenLabel(s.dimension2); });" + this.br + this.br;
    }
});

SF.Chart.MultiLines = function () {
    SF.Chart.TypeTypeValue.call(this);
}
SF.Chart.MultiLines.prototype = $.extend({}, new SF.Chart.TypeTypeValue(), {

    paintGraph: function () {
        return "//paint graph - line" + this.br +
            "chart.enterData(data.series, 'g', 'shape-serie').attr('transform' ,'translate(' + (yAxisLeftPosition + chartAxisPadding + (x.rangeBand() / 2)) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".append('svg:path').attr('class', 'shape')" + this.brt +
            ".attr('stroke', function(s) { return color(JSON.stringify(s.dimension2)); })" + this.brt +
            ".attr('fill', 'none')" + this.brt +
            ".attr('stroke-width', 3)" + this.brt +
            ".attr('shape-rendering', 'initial')" + this.brt +
            ".attr('d', function(s) { return myChart.getPathPoints($.map(s.values, function(v, i) { return { x: x(JSON.stringify(data.dimension1[i])), y: (SF.isEmpty(v) ? null : y(v)) }; }) )})" + this.br +
            this.br +
            "//paint graph - hover area trigger" + this.br +
            "chart.enterData(data.series, 'g', 'hover-trigger-serie').attr('transform' ,'translate(' + (yAxisLeftPosition + chartAxisPadding + (x.rangeBand() / 2)) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".enterData(function(s) { return s.values; }, 'circle', 'hover-trigger')" + this.brt +
            ".attr('cx', function(v, i) { return x(JSON.stringify(data.dimension1[i])); })" + this.brt +
            ".attr('cy', function(v) { return SF.isEmpty(v) ? 0 : y(myChart.getTokenLabel(v)); })" + this.brt +
            ".attr('r', function(v) { return SF.isEmpty(v) ? 0 : 15; })" + this.brt +
            ".attr('fill', '#fff')" + this.brt +
            ".attr('fill-opacity', 0)" + this.brt +
            ".attr('stroke', 'none')" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(v) { return v; })" + this.br +
            this.br +
            "//paint graph - points" + this.br +
            "chart.enterData(data.series, 'g', 'point-serie').attr('transform' ,'translate(' + (yAxisLeftPosition + chartAxisPadding + (x.rangeBand() / 2)) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".enterData(function(s) { return $.map(s.values, function(v){ return { dimension2: s.dimension2, value: v }; }); }, 'circle', 'point')" + this.brt +
            ".attr('fill', function(pair) { return color(JSON.stringify(pair.dimension2)); })" + this.brt +
            ".attr('fill-opacity', function(pair) { SF.isEmpty(pair.value) ? 0 : 100; })" + this.brt +
            ".attr('r', function(pair) { return SF.isEmpty(pair.value) ? 0 : 5; })" + this.brt +
            ".attr('cx', function(pair, i) { return x(JSON.stringify(data.dimension1[i])); })" + this.brt +
            ".attr('cy', function(pair) { return SF.isEmpty(pair.value) ? 0 : y(myChart.getTokenLabel(pair.value)); });" + this.br + this.br;
    }
});

SF.Chart.MultiColumns = function () {
    SF.Chart.TypeTypeValue.call(this);
};
SF.Chart.MultiColumns.prototype = $.extend({}, new SF.Chart.TypeTypeValue(), {

    paintGraph: function () {
        return "//graph x-subscale" + this.br +
            "var xSubscale = d3.scale.ordinal()" + this.brt +
            ".domain($.map(data.series, function (s) { return JSON.stringify(s.dimension2); }))" + this.brt +
            ".rangeBands([0, x.rangeBand()]);" + this.br +
            this.br +
            "//paint graph" + this.br +
            "chart.enterData(data.series, 'g', 'shape-serie').attr('transform' ,'translate(' + (yAxisLeftPosition + chartAxisPadding) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".enterData(function(s) { return $.map(s.values, function(v){ return { dimension2: s.dimension2, value: v }; }); }, 'rect', 'shape')" + this.brt +
            ".attr('stroke', function(pair) { return SF.isEmpty(pair.value) ? 'none' : '#fff'; })" + this.brt +
            ".attr('fill', function(pair) { return SF.isEmpty(pair.value) ? 'none' : color(JSON.stringify(pair.dimension2)); })" + this.brt +
            ".attr('x', function(pair, i) { return xSubscale(JSON.stringify(pair.dimension2)); })" + this.brt +
            ".attr('transform',  function(pair, i) { return 'translate(' + x(JSON.stringify(data.dimension1[i])) + ', 0)'; })" + this.brt +
            ".attr('width', xSubscale.rangeBand())" + this.brt +
            ".attr('height', function(pair, i) { return SF.isEmpty(pair.value) ? 0 : y(myChart.getTokenLabel(pair.value)); })" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(pair, i) { return SF.isEmpty(pair.value) ? null : myChart.getTokenLabel(data.dimension1[i]) + ', ' + myChart.getTokenLabel(pair.dimension2) + ': ' + myChart.getTokenLabel(pair.value); })" + this.br +
            this.br;
    }
});

SF.Chart.StackedColumns = function () {
    SF.Chart.TypeTypeValue.call(this);
};
SF.Chart.StackedColumns.prototype = $.extend({}, new SF.Chart.TypeTypeValue(), {

    getYAxis: function () {
        return "//y axis scale" + this.br +
            "var y = d3.scale.linear()" + this.brt +
            ".domain([0, d3.max(myChart.createCountArray(data.series))])" + this.brt +
            ".range([0, xAxisTopPosition - padding - fontSize - labelMargin]);" + this.br + this.br;
    },

    paintGraph: function () {
        return "//paint graph" + this.br +
            "var countArray = myChart.createEmptyCountArray(data.dimension1.length);" + this.br +
            "chart.enterData(data.series, 'g', 'shape-serie').attr('transform' ,'translate(' + (yAxisLeftPosition + chartAxisPadding) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".enterData(function(s) { return $.map(s.values, function(v){ return { dimension2: s.dimension2, value: v }; }); }, 'rect', 'shape')" + this.brt +
            ".attr('stroke', function(pair) { return SF.isEmpty(pair.value) ? 'none' : '#fff'; })" + this.brt +
            ".attr('fill', function(pair) { return SF.isEmpty(pair.value) ? 'none' : color(JSON.stringify(pair.dimension2)); })" + this.brt +
            ".attr('transform',  function(pair, i) { return 'translate(' + x(JSON.stringify(data.dimension1[i])) + ', 0)'; })" + this.brt +
            ".attr('width', x.rangeBand())" + this.brt +
            ".attr('height', function(pair, i) { return SF.isEmpty(pair.value) ? 0 : y(myChart.getTokenLabel(pair.value)); })" + this.brt +
            ".attr('y', function(pair, i) { if (SF.isEmpty(pair.value)) { return 0; } else { var offset = y(countArray[i]); countArray[i] += pair.value; return offset; } })" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(pair, i) { return SF.isEmpty(pair.value) ? null : myChart.getTokenLabel(data.dimension1[i]) + ', ' + myChart.getTokenLabel(pair.dimension2) + ': ' + myChart.getTokenLabel(pair.value); })" + this.br +
            this.br;
    }
});

SF.Chart.TotalColumns = function () {
    SF.Chart.TypeTypeValue.call(this);
};
SF.Chart.TotalColumns.prototype = $.extend({}, new SF.Chart.TypeTypeValue(), {

    getYAxis: function () {
        return "//y axis scale" + this.br +
            "var y = d3.scale.linear()" + this.brt +
            ".domain([0, 100])" + this.brt +
            ".range([0, xAxisTopPosition - padding - fontSize - labelMargin]);" + this.br + this.br;
    },

    paintGraph: function () {
        return "//paint graph" + this.br +
            "var countArray = myChart.createCountArray(data.series);" + this.br +
            "var emptyCountArray = myChart.createEmptyCountArray(data.dimension1.length);" + this.br +
            "chart.enterData(data.series, 'g', 'shape-serie').attr('transform' ,'translate(' + (yAxisLeftPosition + chartAxisPadding) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".enterData(function(s) { return $.map(s.values, function(v){ return { dimension2: s.dimension2, value: v }; }); }, 'rect', 'shape')" + this.brt +
            ".attr('stroke', function(pair) { return SF.isEmpty(pair.value) ? 'none' : '#fff'; })" + this.brt +
            ".attr('fill', function(pair) { return SF.isEmpty(pair.value) ? 'none' : color(JSON.stringify(pair.dimension2)); })" + this.brt +
            ".attr('transform',  function(pair, i) { return 'translate(' + x(JSON.stringify(data.dimension1[i])) + ', 0)'; })" + this.brt +
            ".attr('width', x.rangeBand())" + this.brt +
            ".attr('height', function(pair, i) { return SF.isEmpty(pair.value) ? 0 : y((100 *pair.value) / countArray[i]); })" + this.brt +
            ".attr('y', function(pair, i) { if (SF.isEmpty(pair.value)) { return 0; } else { var offset = emptyCountArray[i]; emptyCountArray[i] += pair.value; return y((100 * offset) / countArray[i]); } })" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(pair, i) { return SF.isEmpty(pair.value) ? null : myChart.getTokenLabel(data.dimension1[i]) + ', ' + myChart.getTokenLabel(pair.dimension2) + ': ' + myChart.getTokenLabel(pair.value); })" + this.br +
            this.br;
    }
});

SF.Chart.HorizontalTypeTypeValue = function () {
    SF.Chart.TypeTypeValue.call(this);
}
SF.Chart.HorizontalTypeTypeValue.prototype = $.extend({}, new SF.Chart.TypeTypeValue(), {
    init: function () {
        return "//config variables" + this.br +
            "var yAxisLabelWidth = 150," + this.brt +
            "fontSize= " + this.fontSize + "," + this.brt +
            "ticksLength= " + this.ticksLength + "," + this.brt +
            "labelMargin= " + this.labelMargin + "," + this.brt +
            "chartAxisPadding= " + this.chartAxisPadding + "," + this.brt +
            "padding= " + this.padding + "," + this.brt +
            "yAxisLeftPosition = padding + fontSize + yAxisLabelWidth + (2 * labelMargin) + ticksLength," + this.brt +
            "xAxisTopPosition = height - padding - (fontSize * 2) - (labelMargin * 2) - ticksLength," + this.brt +
            "color = (data.series.length < 10 ? d3.scale.category10() : d3.scale.category20()).domain([0, $.map(data.series, function(s) { return JSON.stringify(s.dimension2); })]);" + this.br + this.br;
    },

    getXAxis: function () {
        return "//x axis scale" + this.br +
            "var x = d3.scale.linear()" + this.brt +
            ".domain([0, myChart.getMaxValue1(data.series)])" + this.brt +
            ".range([0, width - yAxisLeftPosition - padding]);" + this.br + this.br;
    },

    getYAxis: function () {
        return "//y axis scale" + this.br +
            "var y = d3.scale.ordinal()" + this.brt +
            ".domain($.map(data.dimension1, function (v) { return JSON.stringify(v); }))" + this.brt +
            ".rangeBands([0, xAxisTopPosition - padding - fontSize - labelMargin - (2 * chartAxisPadding)]);" + this.br + this.br;
    },

    paintXAxisRuler: function () {
        return "//paint x-axis - ruler" + this.br +
            "var xTicks = x.ticks(10);" + this.br +
            "chart.append('svg:g').attr('class', 'x-ruler').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + (padding + fontSize + labelMargin) + ')')" + this.brt +
            ".enterData(xTicks, 'line', 'x-ruler')" + this.brt +
            ".attr('x1', x)" + this.brt +
            ".attr('x2', x)" + this.brt +
            ".attr('y2', xAxisTopPosition - padding - fontSize - labelMargin);" + this.br +
            this.br +
            "//paint x-axis ticks" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-tick').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(xTicks, 'line', 'x-axis-tick')" + this.brt +
            ".attr('y2', ticksLength)" + this.brt +
            ".attr('x1', x)" + this.brt +
            ".attr('x2', x);" + this.br +
            this.br +
            "//paint x-axis tick labels" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-tick-label').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + (xAxisTopPosition + ticksLength + labelMargin + fontSize) + ')')" + this.brt +
            ".enterData(xTicks, 'text', 'x-axis-tick-label')" + this.brt +
            ".attr('x', x)" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(String);" + this.br +
            this.br +
            "//paint x-axis - token label" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-token-label').attr('transform', 'translate(' + (yAxisLeftPosition + ((width - yAxisLeftPosition) / 2)) + ', ' + (height) + ')')" + this.brt +
            ".append('svg:text').attr('class', 'x-axis-token-label')" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(data.labels.value1);" + this.br + this.br;
    },

    paintYAxisRuler: function () {
        return "//paint y-axis - ticks" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-tick').attr('transform', 'translate(' + (yAxisLeftPosition - ticksLength) + ', ' + (padding + chartAxisPadding + (y.rangeBand() / 2)) + ')')" + this.brt +
            ".enterData(data.dimension1, 'line', 'y-axis-tick')" + this.brt +
            ".attr('x2', ticksLength)" + this.brt +
            ".attr('y1', function (v) { return y(JSON.stringify(v)); })" + this.brt +
            ".attr('y2', function (v) { return y(JSON.stringify(v)); });" + this.br +
            this.br +
            "//paint y-axis - tick labels" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-tick-label').attr('transform', 'translate(' + (yAxisLeftPosition - ticksLength - labelMargin) + ', ' + (padding + chartAxisPadding + (y.rangeBand() / 2)) + ')')" + this.brt +
            ".enterData(data.dimension1, 'text', 'y-axis-tick-label')" + this.brt +
            ".attr('y', function (v) { return y(JSON.stringify(v)); })" + this.brt +
            ".attr('dominant-baseline', 'middle')" + this.brt +
            ".attr('text-anchor', 'end')" + this.brt +
            ".text(function (v) { return myChart.getTokenLabel(v); });" + this.br +
            this.br +
            "//paint y-axis - token label" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-token-label').attr('transform', 'translate(' + fontSize + ', ' + ((xAxisTopPosition - fontSize - labelMargin) / 2) + ') rotate(270)')" + this.brt +
            ".append('svg:text').attr('class', 'y-axis-token-label')" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(data.labels.dimension1);" + this.br + this.br;
    }
});

SF.Chart.MultiBars = function () {
    SF.Chart.HorizontalTypeTypeValue.call(this);
};
SF.Chart.MultiBars.prototype = $.extend({}, new SF.Chart.HorizontalTypeTypeValue(), {

    paintGraph: function () {
        return "//graph y-subscale" + this.br +
            "var ySubscale = d3.scale.ordinal()" + this.brt +
            ".domain($.map(data.series, function (s) { return JSON.stringify(s.dimension2); }))" + this.brt +
            ".rangeBands([0, y.rangeBand()]);" + this.br +
            this.br +
            "//paint graph" + this.br +
            "chart.enterData(data.series, 'g', 'shape-serie').attr('transform' ,'translate(' + yAxisLeftPosition + ', ' + (padding + fontSize + labelMargin + chartAxisPadding) + ')')" + this.brt +
            ".enterData(function(s) { return $.map(s.values, function(v){ return { dimension2: s.dimension2, value: v }; }); }, 'rect', 'shape')" + this.brt +
            ".attr('stroke', function(pair) { return SF.isEmpty(pair.value) ? 'none' : '#fff'; })" + this.brt +
            ".attr('fill', function(pair) { return SF.isEmpty(pair.value) ? 'none' : color(JSON.stringify(pair.dimension2)); })" + this.brt +
            ".attr('y', function(pair, i) { return ySubscale(JSON.stringify(pair.dimension2)); })" + this.brt +
            ".attr('transform',  function(pair, i) { return 'translate(0, ' + y(JSON.stringify(data.dimension1[i])) + ')'; })" + this.brt +
            ".attr('height', ySubscale.rangeBand())" + this.brt +
            ".attr('width', function(pair, i) { return SF.isEmpty(pair.value) ? 0 : x(myChart.getTokenLabel(pair.value)); })" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(pair, i) { return SF.isEmpty(pair.value) ? null : myChart.getTokenLabel(data.dimension1[i]) + ', ' + myChart.getTokenLabel(pair.dimension2) + ': ' + myChart.getTokenLabel(pair.value); })" + this.br +
             this.br;
    }
});

SF.Chart.StackedBars = function () {
    SF.Chart.HorizontalTypeTypeValue.call(this);
};
SF.Chart.StackedBars.prototype = $.extend({}, new SF.Chart.HorizontalTypeTypeValue(), {

    getXAxis: function () {
        return "//x axis scale" + this.br +
            "var x = d3.scale.linear()" + this.brt +
            ".domain([0, d3.max(myChart.createCountArray(data.series))])" + this.brt +
            ".range([0, width - yAxisLeftPosition - padding]);" + this.br + this.br;
    },

    paintGraph: function () {
        return "//paint graph" + this.br +
            "var countArray = myChart.createEmptyCountArray(data.dimension1.length);" + this.br +
            "chart.enterData(data.series, 'g', 'shape-serie').attr('transform' ,'translate(' + yAxisLeftPosition + ', ' + (padding + fontSize + labelMargin + chartAxisPadding) + ')')" + this.brt +
            ".enterData(function(s) { return $.map(s.values, function(v){ return { dimension2: s.dimension2, value: v }; }); }, 'rect', 'shape')" + this.brt +
            ".attr('stroke', function(pair) { return SF.isEmpty(pair.value) ? 'none' : '#fff'; })" + this.brt +
            ".attr('fill', function(pair) { return SF.isEmpty(pair.value) ? 'none' : color(JSON.stringify(pair.dimension2)); })" + this.brt +
            ".attr('transform',  function(pair, i) { return 'translate(0, ' + y(JSON.stringify(data.dimension1[i])) + ')'; })" + this.brt +
            ".attr('height', y.rangeBand())" + this.brt +
            ".attr('width', function(pair, i) { return SF.isEmpty(pair.value) ? 0 : x(myChart.getTokenLabel(pair.value)); })" + this.brt +
            ".attr('x', function(pair, i) { if (SF.isEmpty(pair.value)) { return 0; } else { var offset = x(countArray[i]); countArray[i] += pair.value; return offset; } })" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(pair, i) { return SF.isEmpty(pair.value) ? null : myChart.getTokenLabel(data.dimension1[i]) + ', ' + myChart.getTokenLabel(pair.dimension2) + ': ' + myChart.getTokenLabel(pair.value); })" + this.br +
             this.br;
    }
});

SF.Chart.TotalBars = function () {
    SF.Chart.HorizontalTypeTypeValue.call(this);
};
SF.Chart.TotalBars.prototype = $.extend({}, new SF.Chart.HorizontalTypeTypeValue(), {

    getXAxis: function () {
        return "//x axis scale" + this.br +
            "var x = d3.scale.linear()" + this.brt +
            ".domain([0, 100])" + this.brt +
            ".range([0, width - yAxisLeftPosition - padding]);" + this.br + this.br;
    },

    paintGraph: function () {
        return "//paint graph" + this.br +
            "var countArray = myChart.createCountArray(data.series);" + this.br +
            "var emptyCountArray = myChart.createEmptyCountArray(data.dimension1.length);" + this.br +
            "chart.enterData(data.series, 'g', 'shape-serie').attr('transform' ,'translate(' + yAxisLeftPosition + ', ' + (padding + fontSize + labelMargin + chartAxisPadding) + ')')" + this.brt +
            ".enterData(function(s) { return $.map(s.values, function(v){ return { dimension2: s.dimension2, value: v }; }); }, 'rect', 'shape')" + this.brt +
            ".attr('stroke', function(pair) { return SF.isEmpty(pair.value) ? 'none' : '#fff'; })" + this.brt +
            ".attr('fill', function(pair) { return SF.isEmpty(pair.value) ? 'none' : color(JSON.stringify(pair.dimension2)); })" + this.brt +
            ".attr('transform',  function(pair, i) { return 'translate(0, ' + y(JSON.stringify(data.dimension1[i])) + ')'; })" + this.brt +
            ".attr('height', y.rangeBand())" + this.brt +
            ".attr('width', function(pair, i) { return SF.isEmpty(pair.value) ? 0 : x((100 * pair.value) / countArray[i]); })" + this.brt +
            ".attr('x', function(pair, i) { if (SF.isEmpty(pair.value)) { return 0; } else { var offset = emptyCountArray[i]; emptyCountArray[i] += pair.value; return x((100 * offset) / countArray[i]); } })" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(pair, i) { return SF.isEmpty(pair.value) ? null : myChart.getTokenLabel(data.dimension1[i]) + ', ' + myChart.getTokenLabel(pair.dimension2) + ': ' + myChart.getTokenLabel(pair.value); })" + this.br +
             +this.br + this.br;
    }
});

SF.Chart.StackedAreas = function () {
    SF.Chart.TypeTypeValue.call(this);
};
SF.Chart.StackedAreas.prototype = $.extend({}, new SF.Chart.TypeTypeValue(), {

    getYAxis: function () {
        return "//y axis scale" + this.br +
            "var y = d3.scale.linear()" + this.brt +
            ".domain([0, d3.max(myChart.createCountArray(data.series))])" + this.brt +
            ".range([0, xAxisTopPosition - padding - fontSize - labelMargin]);" + this.br + this.br;
    },

    paintGraph: function () {
        return "//paint graph" + this.br +
            "var countArray = myChart.createEmptyCountArray(data.dimension1.length);" + this.br +
            "chart.enterData(data.series, 'g', 'shape-serie').attr('transform' ,'translate(' + (yAxisLeftPosition + (x.rangeBand() / 2)) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".append('svg:path').attr('class', 'shape')" + this.brt +
            ".attr('stroke', function(s) { return color(JSON.stringify(s.dimension2)); })" + this.brt +
            ".attr('fill', function(s) { return color(JSON.stringify(s.dimension2)); })" + this.brt +
            ".attr('shape-rendering', 'initial')" + this.brt +
            ".attr('d', function(s) { return myChart.getPathPoints($.merge(" + this.brt + "\t" +
            "$.map(countArray, function(v, i) { return { x: x(JSON.stringify(data.dimension1[i])), y: y(countArray[i]) }; }).reverse(), " + this.brt + "\t" +
            "$.map(s.values, function(v, i) { var offset = y(countArray[i]); countArray[i] += v; return { x: x(JSON.stringify(data.dimension1[i])), y: offset + y(SF.isEmpty(v) ? 0 : v) }; }) ))})" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(s) { return myChart.getTokenLabel(s.dimension2); })" + this.br +
             this.br;
    }
});

SF.Chart.TotalAreas = function () {
    SF.Chart.TypeTypeValue.call(this);
};
SF.Chart.TotalAreas.prototype = $.extend({}, new SF.Chart.TypeTypeValue(), {

    getYAxis: function () {
        return "//y axis scale" + this.br +
            "var y = d3.scale.linear()" + this.brt +
            ".domain([0, 100])" + this.brt +
            ".range([0, xAxisTopPosition - padding - fontSize - labelMargin]);" + this.br + this.br;
    },

    paintGraph: function () {
        return "//paint graph" + this.br +
            "var countArray = myChart.createCountArray(data.series);" + this.br +
            "var emptyCountArray = myChart.createEmptyCountArray(data.dimension1.length);" + this.br +
            "chart.enterData(data.series, 'g', 'shape-serie').attr('transform' ,'translate(' + (yAxisLeftPosition + (x.rangeBand() / 2)) + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".append('svg:path').attr('class', 'shape')" + this.brt +
            ".attr('stroke', function(s) { return color(JSON.stringify(s.dimension2)); })" + this.brt +
            ".attr('fill', function(s) { return color(JSON.stringify(s.dimension2)); })" + this.brt +
            ".attr('shape-rendering', 'initial')" + this.brt +
            ".attr('d', function(s) { return myChart.getPathPoints($.merge(" + this.brt + "\t" +
            "$.map(countArray, function(v, i) { return { x: x(JSON.stringify(data.dimension1[i])), y: y((100 * emptyCountArray[i]) / countArray[i]) }; }).reverse(), " + this.brt + "\t" +
            "$.map(s.values, function(v, i) { var offset = emptyCountArray[i]; emptyCountArray[i] += v; return { x: x(JSON.stringify(data.dimension1[i])), y: y((100 * (offset + (SF.isEmpty(v) ? 0 : v))) / countArray[i]) }; }) ))})" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(s) { return myChart.getTokenLabel(s.dimension2); })" + this.br +
             this.br;
    }
});

SF.Chart.Points = function () {
    SF.Chart.ChartBase.call(this);
};
SF.Chart.Points.prototype = $.extend({}, new SF.Chart.ChartBase(), {

    init: function () {
        return "//config variables" + this.br +
            "var yAxisLabelWidth = 25," + this.brt +
            "fontSize= " + this.fontSize + "," + this.brt +
            "ticksLength= " + this.ticksLength + "," + this.brt +
            "labelMargin= " + this.labelMargin + "," + this.brt +
            "chartAxisPadding= " + this.chartAxisPadding + "," + this.brt +
            "padding= " + this.padding + "," + this.brt +
            "yAxisLeftPosition = padding + fontSize + yAxisLabelWidth + (2 * labelMargin) + ticksLength," + this.brt +
            "xAxisTopPosition = height - padding - (fontSize * 2) - (labelMargin * 2) - ticksLength," + this.brt +
            "color = (data.points.length < 10 ? d3.scale.category10() : d3.scale.category20()).domain([0, $.map(data.points, function(v) { return JSON.stringify(v); })]);" + this.br + this.br;
    },

    getXAxis: function () {
        return "//x axis scale" + this.br +
            "var x = d3.scale.linear()" + this.brt +
            ".domain([0, d3.max($.map(data.points, function (e) { return e.dimension1; }))])" + this.brt +
            ".range([0, width - yAxisLeftPosition - padding]);" + this.br + this.br;
    },

    getYAxis: function () {
        return "//y axis scale" + this.br +
            "var y = d3.scale.linear()" + this.brt +
            ".domain([0, d3.max($.map(data.points, function (e) { return e.dimension2; }))])" + this.brt +
            ".range([0, xAxisTopPosition - padding]);" + this.br + this.br;
    },

    paintXAxisRuler: function () {
        return "//paint x-axis - ticks" + this.br +
            "var xTicks = x.ticks(10);" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-tick').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(xTicks, 'line', 'x-axis-tick')" + this.brt +
            ".attr('x1', x)" + this.brt +
            ".attr('x2', x)" + this.brt +
            ".attr('y2', ticksLength);" + this.br +
            this.br +
            "//paint x-axis - tick labels" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-tick-label').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + (xAxisTopPosition + ticksLength + labelMargin + fontSize) + ')')" + this.brt +
            ".enterData(xTicks, 'text', 'x-axis-tick-label')" + this.brt +
            ".attr('x', x)" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(String);" + this.br +
            this.br +
            "//paint x-axis - token label" + this.br +
            "chart.append('svg:g').attr('class', 'x-axis-token-label').attr('transform', 'translate(' + (yAxisLeftPosition + ((width - yAxisLeftPosition) / 2)) + ', ' + height + ')')" + this.brt +
            ".append('svg:text').attr('class', 'x-axis-token-label')" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(data.labels.dimension1);" + this.br + this.br;
    },

    paintYAxisRuler: function () {
        return "//paint y-axis - ruler" + this.br +
            "var yTicks = y.ticks(8);" + this.br +
            "chart.append('svg:g').attr('class', 'y-ruler').attr('transform', 'translate(' + yAxisLeftPosition + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(yTicks, 'line', 'y-ruler')" + this.brt +
            ".attr('x2', width - yAxisLeftPosition - padding)" + this.brt +
            ".attr('y1', function(t) { return -y(t); })" + this.brt +
            ".attr('y2', function(t) { return -y(t); });" + this.br +
            this.br +
            "//paint y-axis - ticks" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-tick').attr('transform', 'translate(' + (yAxisLeftPosition - ticksLength) + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(yTicks, 'line', 'y-axis-tick')" + this.brt +
            ".attr('x2', ticksLength)" + this.brt +
            ".attr('y1', function(t) { return -y(t); })" + this.brt +
            ".attr('y2', function(t) { return -y(t); });" + this.br +
            this.br +
            "//paint y-axis - tick labels" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-tick-label').attr('transform', 'translate(' + (yAxisLeftPosition - ticksLength - labelMargin) + ', ' + xAxisTopPosition + ')')" + this.brt +
            ".enterData(yTicks, 'text', 'y-axis-tick-label')" + this.brt +
            ".attr('y', function(t) { return -y(t); })" + this.brt +
            ".attr('dominant-baseline', 'middle')" + this.brt +
            ".attr('text-anchor', 'end')" + this.brt +
            ".text(String);" + this.br +
            this.br +
            "//paint y-axis - token label" + this.br +
            "chart.append('svg:g').attr('class', 'y-axis-token-label').attr('transform', 'translate(' + fontSize + ', ' + (xAxisTopPosition / 2) + ') rotate(270)')" + this.brt +
            ".append('svg:text').attr('class', 'y-axis-token-label')" + this.brt +
            ".attr('text-anchor', 'middle')" + this.brt +
            ".text(data.labels.dimension2);" + this.br + this.br;
    },

    paintGraph: function () {
        return "//paint graph" + this.br +
            "chart.enterData(data.points, 'g', 'shape-serie').attr('transform' ,'translate(' + yAxisLeftPosition + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".append('svg:circle').attr('class', 'shape')" + this.brt +
            ".attr('stroke', function(p) { return color(JSON.stringify(p)); })" + this.brt +
            ".attr('fill', function(p) { return color(JSON.stringify(p)); })" + this.brt +
            ".attr('shape-rendering', 'initial')" + this.brt +
            ".attr('r', 5)" + this.brt +
            ".attr('cx', function(p) { return x(p.dimension1); })" + this.brt +
            ".attr('cy', function(p) { return y(p.dimension2); })" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(p) { return myChart.getTokenLabel(p.value1) + ': ' + p.dimension1 + ', ' + p.dimension2; })" + this.br +
             this.br;
    }
});

SF.Chart.Bubbles = function () {
    SF.Chart.Points.call(this);
};
SF.Chart.Bubbles.prototype = $.extend({}, new SF.Chart.Points(), {

    getSizeScale: function (data, area) {
        var sum = 0;
        $.each(data.points, function (i, p) {
            sum += p.value2;
        });

        return d3.scale.linear()
            .domain([0, sum])
            .range([0, area]);
    },

    paintGraph: function () {
        return "//paint graph" + this.br +
            "var sizeScale = myChart.getSizeScale(data, (width - yAxisLeftPosition) * (height - xAxisTopPosition));" + this.br +
            "chart.enterData(data.points, 'g', 'shape-serie').attr('transform' ,'translate(' + yAxisLeftPosition + ', ' + xAxisTopPosition + ') scale(1, -1)')" + this.brt +
            ".append('svg:circle').attr('class', 'shape')" + this.brt +
            ".attr('stroke', function(p) { return color(JSON.stringify(p)); })" + this.brt +
            ".attr('fill', function(p) { return color(JSON.stringify(p)); })" + this.brt +
            ".attr('shape-rendering', 'initial')" + this.brt +
            ".attr('r', function(p) { return Math.sqrt(sizeScale(p.value2)/Math.PI); })" + this.brt +
            ".attr('cx', function(p) { return x(p.dimension1); })" + this.brt +
            ".attr('cy', function(p) { return y(p.dimension2); })" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(p) { return '(' + p.dimension1 + ', ' + p.dimension2 + ') ' + myChart.getTokenLabel(p.value1) + ': ' + p.value2; })" + this.br +
             this.br;
    }
});

SF.Chart.Pie = function () {
    SF.Chart.ChartBase.call(this);
};
SF.Chart.Pie.prototype = $.extend({}, new SF.Chart.ChartBase(), {

    init: function () {
        return "//config variables" + this.br +
            "var fontSize= " + this.fontSize + "," + this.brt +
            "labelMargin= " + this.labelMargin + "," + this.brt +
            "padding= " + this.padding + "," + this.brt +
            "r = d3.min([width, height - fontSize - labelMargin]) / 3;" + this.brt +
            "rInner = 0," + this.brt +
            "color = (data.serie.length < 10 ? d3.scale.category10() : d3.scale.category20()).domain([0, $.map(data.serie, function(v) { return JSON.stringify(v); })]);" + this.br + this.br;
    },

    getXAxis: function () { return ""; },

    getYAxis: function () { return ""; },

    paintXAxisRuler: function () { return ""; },

    paintYAxisRuler: function () { return ""; },

    paintXAxis: function () { return ""; },

    paintYAxis: function () { return ""; },

    paintLegend: function () {
        return "//paint color legend" + this.br +
            "var legendScale = d3.scale.ordinal().domain($.map(data.serie, function (s, i) { return i; })).rangeBands([0, width])," + this.brt +
            "legendRectWidth = 10," + this.brt +
            "legendLabelWidth = legendScale.rangeBand() - (2 * labelMargin) - legendRectWidth;" + this.br +
            this.br +
            "chart.append('svg:g').attr('class', 'color-legend').attr('transform', 'translate(0, ' + (height - fontSize) + ')')" + this.brt +
            ".enterData(data.serie, 'rect', 'color-rect')" + this.brt +
            ".attr('x', function(e, i) { return (legendRectWidth + legendLabelWidth + (2 * labelMargin)) * i; })" + this.brt +
            ".attr('width', legendRectWidth).attr('height', fontSize)" + this.brt +
            ".attr('fill', function(v) { return color(JSON.stringify(v)); });" + this.br +
            this.br +
            "chart.append('svg:g').attr('class', 'color-legend').attr('transform', 'translate(' + (legendRectWidth + labelMargin) + ', ' + height + ')')" + this.brt +
            ".enterData(data.serie, 'text', 'color-text')" + this.brt +
            ".attr('x', function(e, i) { return (legendRectWidth + legendLabelWidth + (2 * labelMargin)) * i; })" + this.brt +
            ".text(function(v) { return myChart.getTokenLabel(v.dimension1); });" + this.br + this.br;
    },

    paintGraph: function () {
        return "//paint graph" + this.br +
            "var arc = d3.svg.arc().outerRadius(r).innerRadius(rInner);" + this.br +
            "var pie = d3.layout.pie().value(function(v) { return v.value1; });" + this.br + this.br +
            "chart.append('svg:g').data([data.serie]).attr('class', 'shape').attr('transform', 'translate(' + (width / 2) + ', ' + ((height - fontSize - labelMargin) / 2) + ')')" + this.brt +
            ".enterData(pie, 'g', 'slice')" + this.brt +
            ".append('svg:path').attr('class', 'shape')" + this.brt +
            ".attr('d', arc)" + this.brt +
            ".attr('fill', function(slice) { return color(JSON.stringify(slice.data)); })" + this.brt +
            ".attr('shape-rendering', 'initial')" + this.brt +
            ".append('svg:title')" + this.brt +
            ".text(function(slice) { return myChart.getTokenLabel(slice.data.dimension1) + ': ' + slice.data.value1; });" + this.br + this.br;
    }
});

SF.Chart.Doughnout = function () {
    SF.Chart.Pie.call(this);
};
SF.Chart.Doughnout.prototype = $.extend({}, new SF.Chart.Pie(), {

    init: function () {
        return "//config variables" + this.br +
            "var fontSize= " + this.fontSize + "," + this.brt +
            "labelMargin= " + this.labelMargin + "," + this.brt +
            "padding= " + this.padding + "," + this.brt +
            "r = d3.min([width, height - fontSize - labelMargin]) / 3;" + this.brt +
            "rInner = r / 2," + this.brt +
            "color = (data.serie.length < 10 ? d3.scale.category10() : d3.scale.category20()).domain([0, $.map(data.serie, function(v) { return JSON.stringify(v); })]);" + this.br + this.br;
    }
});