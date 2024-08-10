//TODO:b 429. Add administration-home-index.js file to the project.
//This adds dataviz to the administration home page (bar chart for books status)
$(() => { 

    //TODO:b 474. Update administration.home.index.js to include the percentage chart etc...
    const count = j => new Chart($('#count-ch-ch-ch-art')[0], {
        type: 'bar',
        data: {
            labels: j.map(b => b.title),
            datasets: [
                { label: 'Count', backgroundColor: '#ADD8E6', data: j.map(b => b.count) },
                { label: 'On Loan', backgroundColor: '#F08080', data: j.map(b => b.onLoan) },
                { label: 'On Hold', backgroundColor: '#FFFACD', data: j.map(b => b.onHold) },
                { label: 'Available', backgroundColor: '#98FB98', data: j.map(b => b.available) }
            ]
        }
    });

    fetch('/api/reports/books/status')
        .then(r => r.json())
        .then(count)
        .catch(e => console.error(e.message)); 

    let percentageChChChArt; 

    const percentage = j => {     
        let options = { responsive: true,plugins: {legend: { position: 'top' },title:{ display:true,text:'Percentage'}}}; 
        let datasets = [
            {label:'% Holds',backgroundColor:'#ed1f1f',data: j.map(b => b.holdPercentage)},
            {label:'% Loans',backgroundColor:'#851379',data: j.map(b => b.loanPercentage)},
            {label: '% Restorations',backgroundColor: `#f2ee07`, data: j.map(b => b.restorationPercentage)}
        ]; 
        let labels = j.map(t => t.title);
        percentageChChChArt?.destroy(); 
        percentageChChChArt = new Chart($('#percentage-ch-ch-ch-art')[0], { type: 'bar', data: { labels, datasets, options } });
    }; 

    fetch(`/api/reports/transactions/stat?start=${$('#start').val()}`) 
        .then(r => r.json())
        .then(percentage)
        .catch(e => console.error(e.message));

    $('#artist .mojo').on('click', e => {
        $('.hide-me').css('visibility','visible').hide(); 
        $(`.${$(e.target).data('chart')}`).show();
        e.preventDefault(); e.stopPropagation();
    }); 

    $('#start').on('blur',() =>
        fetch(`/api/reports/transactions/stat?start=${$('#start')[0]?.valueAsDate?.toISOString()}`)
            .then(r => r.json())
            .then(percentage)
            .catch(e => console.error(e.message))         
    );
});