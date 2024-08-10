//TODO:b 398. Add search.js
const value = (o, k) => o[Object.keys(o).find(key => key.toLowerCase() === k.toLowerCase())];

const search = s => {
    $(`#${s.entity}_Search`).on('keyup', e => e.target.value.length > 1 &&
        fetch(`/Administration/${s.entity}/Search?searchTerm=${e.target.value}${s.params.map(p => `&${p.name}=${p.value}`).join()}`)
            .then(r => r.json())
            .then(d => $(`#${s.entity}_Result`).empty().append(d.map(m => {
                let el = $(`<div class="search-result" role="button" tabindex="0" title="Select Item">${s.title.map(t => value(m, t)).join('<br />')}</div>`);
                el.on('click keypress', e => {
                    if (e.type == 'click' || (e.type == 'keypress' && e.which == 13)) {
                        s.fields.forEach(f => $(`#${s.entity}_${f}`).val(value(m, f)));
                        $(`#${s.entity}_Result`).empty().hide();
                        $(`#${s.entity}`).show()
                            .find('button').on('click', e => $(`#${s.entity}`).hide().find('input').val('') && e.preventDefault());
                    }
                });
                return el;
            })
            ).show())
            .catch(error => console.error(error))
    );
    $(`#${s.entity}_Search_Clear`).on('click', () => $(`#${s.entity}_Search`).val('') && $(`#${s.entity}_Result`).empty().hide());
};

$(() => {
    $('.input-container-readonly').find('.btn-danger')
        .on('click', e => $(`#${$(e.target).closest('.input-container-readonly').attr('id')}`).hide().find('input').val('') && e.preventDefault());
})