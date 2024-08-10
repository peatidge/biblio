//TODO:b 378. Add administration-book-details.js for calendar view of book events
$(() => {
    var tau = new FullCalendar.Calendar($('#tau')[0], {
        initialView: 'dayGridMonth',
        events: `/api/reports/books/tau/${$('#UID').val()}`,
        eventSourceSuccess: (j, r) => j.records.map(  r => { return {...r, title: r.type };  }),
    });
    $('#btn-tau-cal').on('click', () =>  tau.render());
})