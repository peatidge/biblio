//TODO:b 483. Add js and css files for driver js to _Scripts and _CSS partials
//TODO:b 484. Create a new js file in the Grunt Js folder called temere-in-ignotum-praecipitare
//TODO:b 485. Follow the instruction from driver.js website for accessing the global driver object
const driver = window.driver.js.driver;
//TODO:b 486. Create a helper function for building the steps
const stepulate = (e, t, d) => ({ element: e, popover: { title: t, description: d } });
const tour = () => window.location.pathname.replace(/\//g, '');
//TODO:b 487. Create a helper function to train (build) a driver
const trainDriver = steps =>
    driver({
        showProgress: true, nextBtnText: ">", prevBtnText: "<",
        steps: steps.map(s => stepulate(s.el, s.title, s.msg))
            .filter(step => !step.element || document.querySelector(step.element)),
        onHighlightStarted: (el, step, options, state) => { $('#tesseract-toast').css('opacity', 0); $('#q-bit').show(); },
        onDeselected: (el, step, o) => $('#tesseract-toast').css('opacity', 1),
        onDestroyed: (el, step, o) =>  localStorage.setItem(tour(),1),
    });
//TODO: 489. Add functions for each page that requires a driver
const startHomeDriver = () =>
    trainDriver([
        { el: '#a-wake-N', title: 'Welcome', msg: '<div class="d-flex flex-column align-items-center justify-content-center"><span style="font-size:x-large;font-weight:bolder;">BIBLIOTHECA ~ DOMINIUM </span><span class="war-p m-2">&gt;^-^&lt;</span><span>WELCOMES YOU</span></div>' },
        { el: '#chaos', title: 'Emporium Scientiae Multidimensionale', msg: 'Unlock Mandelbrot with the push of a mouse click-e thing-e' },
        { el: '#boggle', title: 'Themeless', msg: 'Dark to light, light to dark, up to you...' },
        { el: '#know-where', title: 'Home Sweet Gnome', msg: 'Just click here and tap your heels x 3' },
        { el: '#q-bit', title: 'Tesseract', msg: 'Occasional "FLUX LOXA" required, your contribution is greatly appreciated.' },
        { el: '#commence-self-implication', title: 'Join Us', msg: `${'Join Us, '.repeat(5)}...` },
        { el: '#trance-end', title: 'Entree', msg: 'Already implicated?' },
        { el: '#toes', title: 'FOOTNOTE (new feeture):', msg: `Howdy, just wanted to say g'day, I will be your footer on this journey. (s)` }
    ]).drive();

const IdentityAccountRegister = () =>
    trainDriver([
        { el: '#Input_LibraryId', title: 'Library', msg: 'Select the library you wish to join' },
        { el: '#Input_FirstName', title: 'First Name', msg: 'Enter your preferred first name e.g. Scribbly, Boblio, Bookmarcius etc...' },
        { el: '#Input_LastName', title: 'Last Name', msg: 'Enter your preferred last name e.g. Whatsi, Chupta etc...' },
        { el: '#Input_Email', title: 'Email', msg: 'Enter your email address (this will also be your username)' },
        { el: '#Input_Password', title: 'Password', msg: 'Provide a self-synthesized key composed of complex gobbledygook' },
        { el: '#Input_ConfirmPassword', title: 'Confirm Your Password', msg: `Repeat gobbledygook` },
        { el: '#registerSubmit', title: 'Submit', msg: `${'Press me, '.repeat(5)}...` },
        { el: '#toes', title: 'FOOTNOTE: (existing feeture)', msg: `Howdy, just wanted to say g'day again and let you know I'm still here. (u)` }
    ]).drive();

const IdentityAccountLogin = () =>
    trainDriver([
        { el: '#Input_Email', title: 'Email', msg: 'Enter your registered email address (this is also considered as your username)' },
        { el: '#Input_Password', title: 'Password', msg: 'Provide gobbledygook' },
        { el: '#login-submit', title: 'Submit', msg: `${'Press me, '.repeat(5)}...` },
        { el: '#toes', title: 'FOOTNOTE: (existing feeture)', msg: `Boooopy Wooopy Doooo... (c)` }
    ]).drive();

const Administration = () =>
    trainDriver([
        { el: '#t-settings', title: 'Administration Home', msg: '<div class="d-flex align-items-center justify-content-center gap-2">C.TRL CENTRAL<span style="font-size:2.5rem;" class="war-p">&#127899;</span><div>' },
        { el: '#library-logo-modal', title: 'Library', msg: 'View details of the library' },
        { el: '#t-members', title: 'Members', msg: `Manage Members` },
        { el: '#t-references', title: 'References', msg: `Manage Book References (Titles)` },
        { el: '#t-transactions', title: 'Transactions', msg: `Manage Book Transactions: Restorations, Holds & Loans` },
        { el: '#t-hold', title: 'Hold Book', msg: `Hold a book for a member` },
        { el: '#t-loan', title: 'Loan Book', msg: `Loan a book to a member` },
        { el: '#t-restore', title: 'Schedule Restoration', msg: `Schedule a book in to be restored` },
        { el: '#start-status-workshop', title: 'DataViz (count)', msg: 'View the sum of book and their status in graphic detail' },
        { el: '#trance-out', title: 'DataViz (percentage)', msg: 'Visualize the percentage of loans, holds and restorations by book title reference.' },
        { el: '#toes', title: 'FOOTNOTE: (base feeture)', msg: `Hello there, it’s me, your trusty footer! I may be at the bottom, but I hold everything together. A little appreciation goes a long way 👣 (h)` }
    ]).drive();

const AdministrationMember = () =>
    trainDriver([
        { el: '#t-members-table', title: 'Members', msg: 'View Member data in tabular format' },
        { el: '#t-members-create-btn', title: 'Register Member', msg: 'Implicate a new specimen for knowledge absorption' },
        { el: '.t-members-loan-link', title: 'RE:Member Loans', msg: `Easily navigate to loan a book to the selected member` },
        { el: '.t-members-hold-link', title: 'RE:Member Holds', msg: `Effortlessly navigate to hold a book for the selected member` },
        { el: '.pagination', title: 'Page Turner', msg: `Use your imagination` },
        { el: '#toes', title: 'FOOTNOTE: (billing feeture)', msg: `Hi! I’m the footer, quietly supporting every page you visit. It’s a pleasure to keep you grounded across dimensions 💸 (a)` }
    ]).drive();

const AdministrationReference = () =>
    trainDriver([
        { el: '#t-references-table', title: 'Book Titles', msg: 'View book reference data in tabular format' },
        { el: '#t-reference-create-btn', title: 'Inject Knowledge', msg: 'Add a new reference to the library' },
        { el: '.t-reference-books-link', title: 'RE:Books', msg: `View all currently existing synthesized instances of books referenced by this reference` },
        { el: '.t-reference-edit-link', title: 'RE:MUTATE', msg: `Tinker with book DNA` },
        { el: '.t-reference-details-link', title: 'RE:KNOWING', msg: `Become familiar with the simplistic intricacies details of this reference` },
        { el: '.pagination', title: 'Page Turner', msg: `Use your imagination` },
        { el: '#toes', title: 'FOOTNOTE: (regenerative feeture)', msg: `Just like an axolotl, I’m here to support and regenerate your experience. I hope my efforts make your journey smoother 🦎 (h)` }
    ]).drive();

const AdministrationReferenceCreate = () =>
    trainDriver([
        { el: '#Title', title: 'Reference Book Title', msg: 'Add the book title for the reference' },
        { el: '#ISBN', title: 'ISBN', msg: 'Provide the reference ISBN. Must be in the format of NNN-NNNNNNNNNN (where N = any real number)' },
        { el: '#Author', title: 'Author(s)', msg: `Provide details of the specimens responsible for ... i.e. authors` },
        { el: '#Books', title: 'SYNTHESIZE N BOOKS', msg: `Provide the number of book instances you would like synthesized during the initial reference creation process` },
        { el: '#apply-knowledge', title: 'APPLY KNOWLEDGE', msg: `Commence knowledge ingestion process` },
        { el: '.pagination', title: 'Page Turner', msg: `Use your imagination` },
        { el: '#toes', title: 'FOOTNOTE: (loxa feeture)', msg: `Without me, things might get a bit buggy. I’m here to keep everything stable, and your recognition means a lot (e)` }
    ]).drive();

const AdministrationTransaction = () =>
    trainDriver([
        { el: '#t-transactions-table', title: 'Transactions', msg: 'View book transactions data in tabular format' },
        { el: '#start', title: 'Temporal Limitations', msg: 'Control time and limit how far back you want to see' },
        { el: '.q', title: 'Q', msg: `Kickstart Q eerie temporal limitation engine ` },
        { el: '#t-transactions-actions', title: 'Release & Return', msg: `Action available processes based on the book's current state` },
        { el: '.pagination', title: 'Page Turner', msg: `Use your imagination` },
        { el: '#toes', title: 'FOOTNOTE: (inter-dimensional feeture)', msg: `I’m your constant companion, no matter the dimension. I’m here to ensure everything stays in place 🌌 (a)` }
    ]).drive();

const AdministrationTransactionHold = () =>
    trainDriver([
        { el: '#Member_Search', title: 'RE:MEMBER:SEARCH', msg: '<div style="text-align:center">Search for and select a target member for the hold transaction.<br />(just start typing in the search box)</div>' },
        { el: '#Book_Search', title: 'FIND A BOOK', msg: 'Start typing reference "Title" or "ISBN" to search for and select a book' },
        { el: '#BookState', title: 'STATE FILTER', msg: `Limit search results based on a book's current state` },
        { el: '#submit-transaction-request', title: 'Submit', msg: `Submit request to process book hold request` },
        { el: '#toes', title: 'FOOTNOTE: (founding feeture)', msg: `In every dimension, I stand firm. I’m the foundation of your browsing experience, quietly supporting you 🌠 (v)` }
    ]).drive();

const AdministrationTransactionLoan = () =>
    trainDriver([
        { el: '#Member_Search', title: 'RE:MEMBER:SEARCH', msg: '<div style="text-align:center">Search for and select a target member for the loan transaction.<br />(just start typing in the search box)</div>' },
        { el: '#Book_Search', title: 'FIND A BOOK', msg: 'Start typing reference "Title" or "ISBN" to search for and select a book' },
        { el: '#BookState', title: 'STATE FILTER', msg: `Limit search results based on a book's current state` },
        { el: '#submit-transaction-request', title: 'Submit', msg: `Submit request to process book loan request` },
        { el: '#toes', title: 'FOOTNOTE: (deprecated feeture)', msg: `Huh? What? When? 😕 (y)` }
    ]).drive();

const AdministrationRestoration = () =>
    trainDriver([
        { el: '#t-restoration-table', title: 'Restorations', msg: 'View book restorations data in tabular format' },
        { el: '#biblio-resurrection', title: 'Schedule Restoration', msg: `Navigate to schedule a book's literal resurrection` },
        { el: '.restoration-book-link', title: `Book 'Em, Danno!`, msg: `Peek into the ever evolving pages of a book` },
        { el: '.pagination', title: 'Page Turner', msg: `Use your imagination` },
        { el: '#toes', title: 'FOOTNOTE: (stellar feeture)', msg: `I might not shine as bright as a quasar, but I’m always here, lighting the way at the bottom of the page. 🌟 (b)` }
    ]).drive();
const AdministrationRestorationSchedule = () =>
    trainDriver([
        { el: '#Start', title: 'Determine Beginning', msg: 'Tell the book when to expect the restoration process to begin<br/> Sufficient notice is adviced' },
        { el: '#End', title: 'Prognosis', msg: `Each book has it's own process, keep that it mind when determining the book's discharge (restoration end) date` },
        { el: '#Book_Search', title: `BOOK LOOK-UP (azure magnum)`, msg: `Search for a book by "Title" or "ISBN"` },
        { el: '#submit-restoration', title: `Book 'Em In, Danno!`, msg: `Submit scheduled book restoration for processing` },
        { el: '#toes', title: 'FOOTNOTE: (anchoring feeture)', msg: `Just like a kilstar, I anchor this page through the cosmic chaos.<br/>Thanks for recognizing my support! 🌌 (u)` } 
    ]).drive();
const Member = () =>
    trainDriver([
        { el: '#t-member-home', title: 'RE:MEMBER HOME', msg: 'Make yourself feel @home-sweet-home' },
        { el: '#Possibility', title: 'Axoposibility', msg: `Endeavour to drag possibility onto the tesseract for enhanced User Experience (UX)` },
        { el: '#Ytilibissop', title: `Schrodinger's Axolotl`, msg: `Drop the other me on me to commence deterministic UX pixel wave function collapse` },
        { el: '#photo-warning', title: `Specimen Diversity Note`, msg: `Take note, not all implicated specimens require UX feature as member's with photosensitivity are naturally calibrated with the system'` },
        { el: '#toes', title: 'FOOTNOTE: (singularity feeture)', msg: `At the singularity of every page, I’m the point where everything converges. Thanks for the appreciation! 🕳️(r)` }
    ]).drive();
//TODO:b 488. Add driver ignition logic, checks the current page and starts the driver based on matching function name
//try catch is an expection for pages that do not have a driver defined
$(() => {
    const accelerate = () => {
        try {
            if (!localStorage.getItem(tour())) {
                tour() == '' ? startHomeDriver() : eval(`${tour()}()`);
            }
        }
        catch { };
    };   
    $('#yelp').on('click', () => { localStorage.removeItem(tour()); accelerate(); });  
    accelerate();
});