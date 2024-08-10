//TODO:b 439. Add lemur.js to project
const superThanksForAskingPosition = () => {
    $('.schrodinger').css('opacity', 0.01);
    $('#lemurs').fadeIn();
    let i = 1, max = 9;
    const inOuterval = () => {
        $('#lemurs-over-and-out').css('background-image', `url('https://quiz-engin-cdn-dtbyasb5ekdvc6h5.z01.azurefd.net/images/lemur/${i}.webp?v=chippies')`);
        i = i == max ? 1 : i + 1;
    }; 
    setInterval(inOuterval, 512); 
    $('#lemurs-over-and-out').show(); 
    const dance = (o, d) => setTimeout(() => {
        fetch(`/api/reports/gravitons/${o}`)
            .then(r => r.json())
            .then(j => {
                $('#lemurs').find('pre').html(j.wave);          
                if (j.clocked) {
                    localStorage.setItem('lemur',100);
                    $('#lemur-progress').html(`100%`);
                    $('#lemurs').fadeOut();
                    clearInterval(inOuterval);
                    $('#lemurs-over-and-out').hide(); 
                }
                else {
                    let ingress = Math.round(Math.random() * 100 + (-4 * 2));
                    localStorage.setItem('lemur',ingress);
                    $('#lemur-progress').html(`${ingress}%`);
                    dance(j.order, j.duration);
                }               
            })
            .catch(e => console.error(e.message));
    },d)  
    dance(0,256);
}
$(() => {
    new Hammer($('#Possibility')[0])
        .on("panstart", ev => $('#Possibility').addClass('dragging'))
        .on("panmove", ev => {
            $('#axolert').hide(); 
            $('#Possibility').addClass('dragging');
            const touch = ev.changedPointers[0];
            const target = $($('#axolotransmission').html())[0];           
            target.style.position = 'absolute';
            target.style.left = `${touch.pageX - target.offsetWidth / 2}px`;
            target.style.top = `${touch.pageY - target.offsetHeight / 2}px`;
            $('body').append(target);
        })
        .on("panend pancancel", ev => {
            let t = $('.axoclone').not('#axolotransmission .axoclone').last()[0];
            if (ev.type === "panend" && t) {            
                const r2 = t.getBoundingClientRect(), d2 = $('#Ytilibissop')[0].getBoundingClientRect();          
                if (r2.top < d2.bottom && r2.bottom > r2.top && r2.left < d2.right && r2.right > d2.left) {                  
                    superThanksForAskingPosition();
                }
                else {
                    $('#axolert').html(`Axological Target Missed ~${Math.floor(Math.random() * 420)}^ (texalotl units)`).show(); 
                }
                $('.axoclone').not('#axolotransmission .axoclone').remove();
            }
        });
});