function setUpCalendar() {
    //globals
    var calEvents = {},
        cal = null,
        selectedDate = null;
        
    initCalendarControl();
   
    refreshCalendarEventsData().done(function () {       
        bindCalendarHeaderEvents();
        bindModalDialogEvents();
        validateModalInputFields();
    });
  
    // METHODS //

    // Init
    function refreshCalendarEventsData() {
        var $d = $.Deferred();
           
        $.getJSON("/api/jobs/events/" + cal.getMonth() + "/" + cal.getYear()).done(function (data) {
            if (data) {
                // clear events count spans and events
                $(".js-eventCount").remove();
                calEvents = {};
                // fill the list of events
                $.each(data, function (index, calEventItem) {
                    calEvents[calEventItem.Date] = '<span class="js-eventCount">' + calEventItem.Count + '</span>';
                });
                // set event number per date
                cal.setData(calEvents);

                // set red color for number of events per day == 10
                $(".js-eventCount:contains('10')").css("background", "#b32d00");
                
            }
        }).always(function () {
            $d.resolve();
        });

        return $d.promise();
    }

    function initCalendarControl() {

        cal = $('#calendar').calendario({
            onDayClick: function ($el, $contentEl, dateProperties) {
                selectedDate = dateProperties.month + "-" + dateProperties.day + "-" + dateProperties.year;

                openOrCloseEventsDiv($el);
                getEventsForSpecificDate(selectedDate).done(function () {
                    bindEventsForIndividualJob();
                    // set Add button invisible if number of jobs is > 9
                    if ($(".js-job").length > 9)
                        $(".js-addNewJob").toggle();
                });
            },
            caldata: calEvents
        }),
           $month = $('#custom-month').html(cal.getMonthName()),
           $year = $('#custom-year').html(cal.getYear());
    }

    // Events binding  
    function bindCalendarHeaderEvents() {
        $('#custom-next').on('click', function () {
            cal.gotoNextMonth(updateMonthYear);
            refreshCalendarEventsData();
        });
        $('#custom-prev').on('click', function () {
            cal.gotoPreviousMonth(updateMonthYear);
            refreshCalendarEventsData();
        });
        $('#custom-current').on('click', function () {
            cal.gotoNow(updateMonthYear);            
            refreshCalendarEventsData();
        });
    }

    function bindModalDialogEvents() {
        $(".js-delete").on("click", function () {
            var id = $(".js-jobId").val();
            deleteJob(id);
        });

        $(".js-save").on("click", function () {
            var id = $(".js-jobId").val();
            if (id)
                editJob();
            else addJob();
        });

        $("#myModalHorizontal").find("input[required]").on("keyup", function () {
            validateModalInputFields();
        });

        $(".action-next, .action-prev").on("click", function () {
            validateModalInputFields();
        });
    }

    function bindEventsForIndividualJob() {
        $(".js-job").on("click", function () {           
            var id = $(this).attr("jobId");

            // enable delete button
            $(".js-delete").show();

            populateModalWithJobData(id).done(function () {
                validateModalInputFields();
            });          
        });

        $(".js-addNewJob").on("click", function () {
            resetModalDialogControls();
            validateModalInputFields();
        })
    }

    // Events div
    function openOrCloseEventsDiv(dateDiv) {
        var isEventDivOpen = $("#js-eventDiv").length > 0,
			isTheSameDateClicked = dateDiv.hasClass("js-activeDate");

        if (!isEventDivOpen) {
            openEventsDiv(dateDiv);
        }
        else if (isEventDivOpen && isTheSameDateClicked) {
            closeEventsDiv();
        }
        else { // events div open but different date is clicked
            closeEventsDiv();
            openEventsDiv(dateDiv);
        }
    }

    function closeEventsDiv() {
        $("#js-eventDiv").remove();
        refreshLowerBorders();
    }

    function openEventsDiv(dateDiv) {
        var calendarRow = dateDiv.parents(".fc-row").first();

        // set only one active dateCell
        $(".js-activeDate").removeClass('js-activeDate');
        dateDiv.addClass("js-activeDate");

        // set lower border invisible for all nodes in a row
        calendarRow.css("box-shadow", "0px 0px 0px rgba(255, 255, 255, 0.2) inset");

        // set lower border for all date cells in a row 
        calendarRow.children().css("box-shadow", "-1px -1px 0px rgba(255, 255, 255, 0.2) inset");
        $(calendarRow).children().last().css("box-shadow", "0px -1px 0px rgba(255, 255, 255, 0.2) inset");

        // set lower border invisible for the active date cell
        dateDiv.css("box-shadow", "-1px 0px 0px rgba(255, 255, 255, 0.2) inset");

        // inserts space for events list
        calendarRow.after('<div id="js-eventDiv" class="fc-row"></div>');

        // insert button inside of events div
        $("#js-eventDiv").append('<button data-toggle="modal" class="js-addNewJob btn btn-primary btn-sm" style="float:right; margin-top:1%;" data-target="#myModalHorizontal">Add</button>');
       
        // fade in effect simulation :)
        $("#js-eventDiv").hide();
        $("#js-eventDiv").show(200);
    }

    function refreshLowerBorders() {
        // set lower border visible for row in which the active date is in
        $(".js-activeDate").parents(".fc-row").first().css("box-shadow", "0px 0px 0px rgba(255, 255, 255, 0.2) inset");
        // set lower border for the active date cell
        $(".js-activeDate").css("box-shadow", "-1px -1px 0px rgba(255, 255, 255, 0.2) inset");
        // set box shadow for active date
        $(".js-today").css("box-shadow", "inset -1px -1px 100px");
        // hide lower border for last row
        $(".fc-body").children(".fc-row").last().children().css("box-shadow", "-1px 0px 0px rgba(255, 255, 255, 0.2) inset").last().css("box-shadow", "0px 0px");
    }

    function getEventsForSpecificDate(date) {
        var $d = $.Deferred(),
            url = "/api/jobs/events/" + date;

        $.getJSON(url).done(function (data) {
            if (data && data.length > 0) {
                // add modal trigger to every job calendar event
                $.each(data, function (index, value) {
                    $("#js-eventDiv").append("<button data-toggle='modal' jobId ='" + value.MontazaId + "' class='js-job btn btn-success btn-xs' data-target='#myModalHorizontal'>"
                        + value.Adresa + "</button>")
                });
            }
        }).always(function () {
            $d.resolve();
        });

        return $d.promise();
    }

    // modal
    function populateModalWithJobData(id) {
        var $d = $.Deferred(),
               url = "/api/jobs/" + id;

        $.getJSON(url).done(function (data) {
            if (data) {
                $("#radnik").val(data.Radnik);
                $("#adresa").val(data.Adresa); 
                $("#vreme").val(setTimeString(data.Vreme));
                $(".js-jobId").val(data.MontazaId);
            }
        }).always(function () {
            $d.resolve();
        });

        return $d.promise();  
    }

    function setTimeString(inputTime) {
        var timeWithooutSeconds = inputTime.substring(0, inputTime.lastIndexOf(':')),
            hour, min;
     
        // get hours
        hour = timeWithooutSeconds.substring(0, timeWithooutSeconds.indexOf(':'))
        // get mins
        min = timeWithooutSeconds.substring(timeWithooutSeconds.indexOf(':') + 1);
        // format string
        return hour + " : " + min;
    }

    function validateModalInputFields() {
        var $requiredFields = $("#myModalHorizontal").find("input[required]"),
            $saveButton = $(".js-save"),
            isValid = true;

        // check if fields are valid
        $.each($requiredFields, function (index, field) {
            if (field.value === "") {
                $(this).parents(".form-group").first().addClass("has-error");
                isValid = false;
            }
            else
                $(this).parents(".form-group").first().removeClass("has-error");

        });
        // disable or enable save button
        if (!isValid) {
            $saveButton.attr("disabled", true);
        }
        else
            $saveButton.attr("disabled", false);
    }

    function resetModalDialogControls() {
        $(".js-jobId").val("");
        $("#adresa").val("");
        $("#radnik").val("");
        $("#vreme").val("00 : 00");

        // disable delete button
        $(".js-delete").hide();
    }

    // Crud
    function deleteJob(id) {
        $.ajax({
            url: 'api/jobs/' + id,
            type: 'DELETE',
            success: function (result) {
                if (result === true) {
                    $(".js-cancel").click(); // close modal
                    refreshCalendarEventsData().done(function () {
                        toastr.success("Event has been deleted.");
                        resetModalDialogControls();
                    });
                }
                else {
                    toastr.error("Error deleting event!");
                }
            }
        });
    }

    function addJob() {

        var url = "/api/jobs",
            // serialize form input data and add Date to query string
            encodedQueryString = $('#montazaForma').serialize() + '&Datum=' + selectedDate,
            // decode query string (because of time which has %3A instead of ':')
            data = decodeURIComponent(encodedQueryString);

        $.post(url, data).done(function (isSaved) {
            if (isSaved) {
                $(".js-cancel").click(); // close modal
                refreshCalendarEventsData().done(function (result) {
                    toastr.success("Event has been added.");
                    resetModalDialogControls();
                });
            }
            else
                toastr.error("Error adding event!");
        });
    }

    function editJob() {

        var url = "/api/jobs",
           data = {
               MontazaId: $(".js-jobId").val(),
               Adresa: $("#adresa").val(),
               Radnik: $("#radnik").val(),
               Vreme: $("#vreme").val().replace(/ /g, ''),
               Datum: selectedDate
           };

        $.ajax({
            url: url,
            type: 'PUT',
            data: data,
            success: function (isEdited) {
                if (isEdited === true) {
                    $(".js-cancel").click(); // close modal
                    refreshCalendarEventsData().done(function () {
                        toastr.success("Event is updated.");
                        resetModalDialogControls();
                    });
                }
                else {
                    toastr.error("Error updating event!");
                }
            }
        });
    }

    // Other
    function updateMonthYear() {
        $month.html(cal.getMonthName());
        $year.html(cal.getYear());
    }
}