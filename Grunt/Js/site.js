//TODO:b 240. Add site.js file to Grunt/Js folder (logic coming soon, stay tooned!)
//*For now just checking that uglified and Grunt is working
//TODO:b 241. Add libman.json file to Tela project (this defines the libraries in the wwwroot/lib folder)
//*NOTE: You may need to change the file (i.e. add a space or something) and save it to trigger the restore
//TODO:b 369a. Add wibble wobble to site.js (used in book index but adding here in case I want to add wibbles & wobbles to other pages)
$(() => {
    $('*[data-wibble]').on('mouseenter', e => $('#wibble').show().find('#wobble').html($(e.target).data('wibble')));
    $(".page-starter #start").on('blur', () => $('#page-turner').trigger('click'))
});