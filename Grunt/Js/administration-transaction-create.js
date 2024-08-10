//TODO:b 399. Add administration-transaction-create.js 
$(() => { 

    search({ entity: 'Member', key: 'Id', title: ['UserName'], fields: ['Id', 'UserName', 'FirstName', 'LastName'], params: [] });
    search({ entity: 'Book', key: 'UID', title: ['UID', 'Title', 'State'], fields: ['UID', 'Title', 'ISBN', 'State'], params: [{ name: 'state', get value() { return $('#BookState').val() } }] });
})