$(() => {
    //TODO:b 370. Add administration-book-index.js to capture form submit and show scene for 2 seconds before submitting
    //Demonstrating how to hook into form submit to control the submission
    $('form').on('submit', e => {
        if (!$(e.target).data('delayed')) {
            e.preventDefault();
            $('.scene').show();
            setTimeout(() => $(e.target).data('delayed', true).trigger('submit'),2000);
        }
    }); 
})