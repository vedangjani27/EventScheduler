$(document).ready(function () {
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        events: '/Events/GetEvents',
        editable: true,
        droppable: true,
        eventClick: function (event) {
            $.magnificPopup.open({
                items: {
                    src: '/Events/EditEvent/' + event.id,
                    type: 'iframe'
                },
                iframe: {
                    markup: '<div class="mfp-iframe-scaler">' +
                        '<div class="mfp-close"></div>' +
                        '<iframe class="mfp-iframe" frameborder="0" allowfullscreen></iframe>' +
                        '</div>',
                },
                closeOnBgClick: false,
                closeBtnInside: true
            });
        },
        eventDrop: function (event, delta, revertFunc) {
            var data = {
                id: event.id,
                start: event.start.format("YYYY-MM-DDTHH:mm:ss"), 
                end: event.end.format("YYYY-MM-DDTHH:mm:ss")
            };

            $.ajax({
                url: '/Events/UpdateEventDate',
                type: 'POST',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (response) {
                    alert('Event updated successfully!');
                },
                error: function () {
                    alert('Error updating event');
                    revertFunc();
                }
            });
        }
    });
});

