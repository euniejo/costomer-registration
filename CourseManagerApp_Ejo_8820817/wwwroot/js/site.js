$(document).ready(function () {
    // setup the jQuerui datepicker component:
    $('input[type=datetime]').datepicker({
        dateFormat: 'mm/dd/yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+100"
    });
});