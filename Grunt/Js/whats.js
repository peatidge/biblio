//TODO:b 331. Add whats.js to Grunt/Js (remember to re-run Grunt)
const whats = el => {
    let edocinu = $('<div class="edoc-inu" style="cursor:help"></div>');
    $(el).append(edocinu).show();
    let l = 32; count = 0;
    setInterval(() => {
        l == 70855 && edocinu.empty() && (l = 32);
        edocinu.append(`<i>&#${l++};<i>`).scrollTop(Math.floor(edocinu[0].scrollHeight - edocinu.height()));
    }, 42);
};
$(()=>$("#chaos").on("click",e=>$(e.target).addClass("fracked") && whats($("#it"))));