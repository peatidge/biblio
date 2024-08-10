//TODO:b 421. Add administration-restoration-schedule.js
$(() => {
    search({ entity: 'Book', key: 'UID', title: ['UID', 'Title', 'State'], fields: ['UID', 'Title', 'ISBN', 'State'], params: [{ name: 'state', get value() { return $('#BookState').val() } }] });
})