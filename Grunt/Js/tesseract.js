const tesseract = () => {

    if (!localStorage.getItem('Tesseract')) {
        localStorage.setItem('Tesseract', JSON.stringify({ time: new Date(), stability: 0 }));
    }
    else {
        let t = JSON.parse(localStorage.getItem('Tesseract'));
        if (new Date() - new Date(t.time) > 2000) {

            t.stability += 2.5 * (Math.random() < 0.33 ? -1 : 1);
            $('#tesseract').find('.progress-bar').css('width','0%');

            if (t.stability < 34) {
                $('#tesseract').find('.progress-bar')[0].style.width = `${t.stability}%`;
                $('#q-bit').css('color','green');
            }
            else if (t.stability < 67) {
                $('#tesseract').find('.progress-bar')[0].style.width = `100%`;
                $('#tesseract').find('.progress-bar')[1].style.width = `${t.stability}%`;
                $('#q-bit').css('color','yellow');
            }
            else if (t.stability < 101) {
                $('#tesseract').find('.progress-bar')[0].style.width = `100%`;
                $('#tesseract').find('.progress-bar')[1].style.width = `100%`;
                $('#tesseract').find('.progress-bar')[2].style.width = `${t.stability}%`;
                $('#q-bit').css('color','red');
            }
            else {             
                bootstrap.Toast.getOrCreateInstance($('#tesseract-toast')[0]).show();
            }
            t.time = new Date();
            localStorage.setItem('Tesseract', JSON.stringify(t));
            $('#q-bit').show();
            setTimeout(() => { $('#q-bit').hide();  }, 1000); 
        }
    }
}

var timeMachine = new Interval(tesseract, 500);

$(() => {
    $('body').on('mouseover', () => {
        $('#q-bit').off('click').on('click',() => localStorage.setItem('Tesseract', JSON.stringify({ time: new Date(), stability: 100 })))
        $('#tesseract').show(); 

        $('#initiate-axelotl-vaporization-fluctuation').off('click').on('click', e => {
            bootstrap.Toast.getOrCreateInstance($('#tesseract-toast')[0]).hide();
            localStorage.setItem('Tesseract', JSON.stringify({ time: new Date(), stability: 0 }));
            let axe = $('<img src="/images/axe.webp" alt="Axolotl" class="axe" />');
            $('body').prepend(axe);
            setTimeout(() => axe.remove(),1500);
        }); 
        if (!timeMachine.isRunning()) {
            timeMachine.start();
        }
    });
});

function Interval(fn, time) {
    var timer = false;
    this.start = function () {
        if (!this.isRunning())
            timer = setInterval(fn, time);
    };
    this.stop = function () {
        clearInterval(timer);
        timer = false;
    };
    this.isRunning = function () {
        return timer !== false;
    };
}
