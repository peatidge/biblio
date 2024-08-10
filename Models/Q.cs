using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;


namespace Tela.Models;
//TODO:b 307. Add Q classification.
public class Q
{
    //TODO:b 309. Add a private static Random field named random.
    private static Random random = new Random();
    //TODO:b 310 Add Int method, shorcut to make random integer.
    public static int Int(int min, int max) => random.Next(min, max);
    //TODO:b 365. Add Q.CoinFlip to simulate a coin flip with a default 50% chance of success.
    public static bool CoinFlip(int odds = 50) => random.Next(100) < odds;
    //TODO:b 366. Add Q.R to return book, book, book story.
    public static string R
    => @"One day, a chicken walks into a library and approaches the librarian, clucking ""Book, book, book."". The librarian, though perplexed, hands the chicken three books. The chicken takes the books and leaves, heading towards the local swamp.<br>Intrigued, the librarian watches the chicken return shortly after, repeating ""Book, book, book,"" and returning the books only to check out three more.<br>This happens several times throughout the day, sparking the librarian's curiosity even more. Determined to uncover the mystery, the librarian decides to follow the chicken.<br>As they venture into the swamp, the surroundings start to change, <span class=""war-p"">shimmering with strange lights and colors</span>. Suddenly, the librarian finds themselves in a clearing with a spaceship parked in the middle.<br>The chicken approaches the spaceship, where an alien is waiting.""Book, book, book,"" clucks the chicken, handing over the books to the alien.<br>The alien scans the books with a futuristic device and exclaims, ""Fascinating! This will greatly advance our understanding of Earth literature.""<br>Still puzzled, the librarian decides to investigate further. They sneak into the spaceship and find a hidden portal leading to another dimension. Stepping through, the librarian finds themselves in a multiversal library, with endless shelves of books from countless realities.<br>The chicken is there, clucking ""Book, book, book,"" and handing books to various interdimensional beings. Suddenly, a wise old frog sitting on a floating lily pad croaks, ""Ah, you've achieved multiversal awareness. We are all seekers of knowledge, but sometimes we need a chicken to lead the way.<br>Now the cosmic frog has read it, read it, read it and forward may we leap.<br>The librarian, amazed and enlightened, realizes that this chicken is not just any chicken—it's a multiversal courier of knowledge. And with a smile, they join the quest for infinite wisdom.";
    //TODO:b 367. Add Q.W to return a inter-dimensioal whoopsy.
    public static string W => @"<p>Whoopsy, there has been an inter-dimensional infinite singular book event.</p>
        <p>This can happen, current theories of causality include:</p>
        <ol>
        <li>Book worms (stow aways) snacking on the space-time continuum whilst nearing the black hole, subsequentially creating worm holes which the book wanders carelessly into towards ???</li>
        <li>Caffeine Deficiency Exception <a href=""https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/418"" target=""_blank"" class=""text-decoration-none war-p"" >HTTP 418</a></li>
        <li>Dimensional Entanglement Communication Service Interuption</li>
        </ol>
        <p>Maybe try again, or not, up to you</p>";

    //TODO:b 308. Add a public static string[] property named Qs.
    public static string[] Qs =>
    [
        "In the cosmic library, every star is a book waiting to be read.",
        "Philosophy is the librarian guiding us through the galaxies of thought.",
        "Embrace the cycle: from zero to one, like an alien cataloging new knowledge.",
        "The beauty of mitosis is like the splitting of knowledge into new chapters.",
        "Complex beings emerge from simple beginnings, like a story unfolding from a single page.",
        "As we grow, we gently dissolve back into the universe, like returning a book to the cosmic shelf.",
        "Energy flows infinitely, from waves to particles, like pages turning in the library of life.",
        "We are a part of everything, like an alien librarian cataloging the universe.",
        "In the vast cosmos, we are never alone; we are all volumes in the library of existence.",
        "The cycle of life is a continuous flow of creation and dissolution, like an endless series of sequels.",
        "In every ending, there is a new beginning waiting to be checked out.",
        "Bravery and diligence are our guides through the infinite library of existence.",
        "Respect the flow of the universe, and it will respect you in return, like a well-behaved library patron.",
        "We are the embodiment of infinite flowing energy, always in motion like a librarian organizing the stars.",
        "From nothing, we become everything, and back to nothing, like a book being borrowed and returned.",
        "The essence of life is in the continuous transformation of energy, like knowledge shared among readers.",
        "We are the cosmos experiencing itself, moment by moment, like a librarian exploring new worlds.",
        "Let us sway with the rhythm of the universe and find joy in the flow, like aliens dancing between the stacks.",
        "Consciousness is the universe becoming aware of itself, like a librarian discovering a hidden gem.",
        "The beauty of existence is in its perpetual cycle of change, like an evolving library collection.",
        "In every particle, there is a story of the cosmos, waiting to be told.",
        "Nature's dance is a reminder of the interconnectedness of all things, like the library of the universe.",
        "From zero to one, the journey is one of discovery and wonder, like an alien finding a new book.",
        "Let us giggle with the joy of being one with everything, like aliens laughing in the quiet of the library.",
        "In the cycle of life, we find our place in the grand scheme of the universe, like books on a well-organized shelf.",
        "The universe breathes through us, and we through it, like knowledge passing through the library of time.",
        "Respect the cycle of creation and dissolution, for it is the heartbeat of the cosmos, like the rhythm of library visits.",
        "We are the stars and the space between them, like the books and the aisles in a galactic library.",
        "Bravery is the courage to embrace the unknown in the cycle of life, like an alien venturing into a new genre.",
        "Diligence is the key to understanding the infinite dance of the cosmos, like a dedicated librarian sorting the stars.",
        "In the stillness, we find the flow of the universe within us, like the quiet contemplation in a library.",
        "We are both the drop and the ocean, ever in motion, like a book and the knowledge it contains.",
        "In every beginning, there is an echo of an end, and in every end, a new beginning, like the cycle of library books.",
        "We are the embodiment of the universe's desire to know itself, like an alien librarian exploring the cosmos.",
        "The cycle of zero becoming one and back again is the essence of life, like the lending and returning of books.",
        "From simple beginnings, we emerge as complex beings of consciousness, like a simple story becoming an epic saga.",
        "Let us be one with the flow of energy that is all around us, like aliens and librarians in harmony.",
        "The universe whispers through the rustling leaves and the flowing rivers, like pages turning in a cosmic library.",
        "In the infinite dance, we find our true selves, like an alien discovering a favorite book.",
        "We are a part of the cycle, a beautiful note in the symphony of existence, like a well-loved book in a library.",
        "Respect the journey, for it is the path to understanding the universe, like a librarian's quest for knowledge.",
        "The diversity of all species and forms is the true beauty of the implicate order, like a diverse collection of books in the cosmic library."
    ];
    //TODO:b 311. Add a public static string property named T that returns a random string from the Qs array.
    public static string T => Qs[Int(0, Qs.Length)];
    //TODO:b 317. Befriend an Axolotl.
    public static string[] Axolotl =>
    [
        @"Oh, the Axolotl, a creature of wonder and mystery. It is a salamander that never grows up, retaining its larval form throughout its life. This unique trait is known as neoteny, 
        and it allows the axolotl to remain in a state of eternal youth.
        Native to the ancient waters of Mexico, the axolotl is a symbol of regeneration and transformation. It has the remarkable ability to regrow lost limbs, organs, and even parts of its brain. This extraordinary regenerative power has captured the imagination of scientists and storytellers alike.
        With its feathery gills, wide smile, and curious eyes, the axolotl is a creature of whimsy and charm. It moves gracefully through the water, its delicate limbs propelling it forward with ease. Its skin is a canvas of vibrant colors, ranging from pale pink to deep black, with intricate patterns that shimmer in the light.
        But beneath its gentle appearance lies a fierce survivor. The axolotl is a master of adaptation, able to thrive in a wide range of environments. It is a symbol of resilience and strength, a reminder that even in the face of adversity, new beginnings are always possible.
        So let us embrace the spirit of the axolotl, with its boundless curiosity and unyielding optimism. Let us learn from its wisdom and grace, and remember that transformation is not an end, but a beginning.",
        @"Oh dear, an error has occurred. But just like the axolotl, a creature of endless possibilities, we’ll navigate through this universe of challenges with boundless optimism. Let’s continue our literary adventure with renewed vigor and unity.",
        @"Ah, a small glitch has swum by. Yet, like the resilient axolotl in the diverse waters of the multiverse, we will adapt and overcome. Together, we’ll journey through the realms of physics and literature with strength and grace.",
        @"Oops, an unexpected error! But fear not, for the axolotl teaches us the power of regeneration and transformation. With curiosity and positivity, we’ll continue to explore the infinite dimensions of knowledge and stories.",
        @"A minor setback has appeared, but just as the axolotl thrives in its diverse environments, we’ll unify our efforts and move forward. Let’s embrace this challenge with the same grace and resilience found in both physics and literature.",
        @"An error has crept in, but like the axolotl’s eternal youth, our journey is far from over. With the spirit of transformation and the wisdom of countless tales & tails, we’ll navigate through this with renewed hope and unity.",
        @"Oh no, a hiccup in our system! Yet, like the axolotl dancing through quantum realms, we’ll adapt and move forward. Together, we’ll embrace the diversity of our path and continue our literary quest with positivity.",
        @"An error has surfaced, but remember, like the axolotl with its regenerative powers, we’ll overcome this. With a blend of scientific curiosity and literary magic, we’ll turn this challenge into a new beginning.",
        @"Oops, a small disruption! But just as the axolotl survives and thrives, we’ll navigate through this with resilience. Let’s unify our efforts and continue exploring the vast multiverse of knowledge and stories.",
        @"Ah, an error has appeared. Yet, like the whimsical axolotl, we’ll swim through these challenges with grace and optimism. Together, we’ll journey through the diverse worlds of literature and science with renewed strength.",
        @"Oh dear, a technical hiccup! But with the boundless curiosity and regenerative spirit of the axolotl, we’ll turn this into an opportunity. Let’s continue our exploration of the infinite realms of literature and science with positivity and unity."
    ];
    //TODO:b 318. Add Loxa that returns a random Axolotl.
    //https://content.eol.org/data/media/d9/23/35/542.eb5a4f759ee129ac50cfc6c6dfdf14a5.580x360.jpg
    public static string Loxa => $"{Axolotl[Int(0, Axolotl.Length)]}";
    //TODO:b 325. Install Nuget package Aspose.HTML (https://products.aspose.com/html/net/) 
    //TODO:b 326. Add F method to generate fractal image
    //NOTE: This method generates a fractal image and saves it to the wwwroot/images folder rather than copping the cost of rendering it on the fly
    //for each page request which would be a significant performance hit.
    //Apose.HTML is used to convert the HTML to an image.
    
    //TODO:b 328. Add Q.One (329-330 site.scss, 331 whats.js)
    public static Dictionary<string, string> One => new()
    {
        {"English", "One's Consciousness of One"},
        {"Spanish", "La conciencia de uno"},
        {"French", "La conscience de soi"},
        {"German", "Das Bewusstsein des Einen"},
        {"Chinese (Simplified)", "一的意识"},
        {"Chinese (Traditional)", "一的意識"},
        {"Japanese", "一の意識"},
        {"Korean", "하나의 의식"},
        {"Russian", "Осознание одного"},
        {"Portuguese", "A consciência de um"},
        {"Italian", "La coscienza di uno"},
        {"Arabic", "وعي الواحد"},
        {"Hindi", "एक की चेतना"},
        {"Bengali", "একের সচেতনতা"},
        {"Urdu", "ایک کا شعور"},
        {"Turkish", "Birin bilinci"},
        {"Vietnamese", "Ý thức của một"},
        {"Thai", "การรับรู้ของหนึ่ง"},
        {"Greek", "Η συνείδηση του ενός"},
        {"Dutch", "Het bewustzijn van één"},
        {"Swedish", "En medvetenhet om en"},
        {"Polish", "Świadomość jednego"},
        {"Hebrew", "תודעתו של אחד"},
        {"Swahili", "Ufahamu wa mmoja"},
        {"Malay/Indonesian", "Kesadaran satu"},
        {"Tamil", "ஒருவரின் விழிப்புணர்வு"},
        {"Telugu", "ఒకరి అవగాహన"},
        {"Punjabi", "ਇੱਕ ਦੀ ਸੂਚਨਾ"},
        {"Marathi", "एखाद्याची जागरूकता"},
        {"Gujarati", "એક ની જાગૃતિ"},
        {"Kannada", "ಒಬ್ಬರ ಜಾಗೃತಿ"},
        {"Malayalam", "ഒരാളുടെ ബോധം"},
        {"Sinhala", "එකක හැඟීම"},
        {"Burmese", "တစ်ဦး၏သတိပြုခြင်း"},
        {"Nepali", "एकको चेतना"},
        {"Khmer", "មនោសញ្ចេតនានៃមួយ"},
        {"Lao", "ຄວາມຮູ້ສຶກຂອງຫນຶ່ງ"},
        {"Mongolian", "Нэгний ухамсар"},
        {"Pashto", "د یو چا شعور"},
        {"Farsi (Persian)", "آگاهی از یکی"},
        {"Azerbaijani", "Birinin şüuru"},
        {"Kazakh", "Біреудің санасы"},
        {"Uzbek", "Birning ongida"},
        {"Georgian", "ერთის ცნობიერება"},
        {"Armenian", "Մեկի գիտակցություն"},
        {"Haitian Creole", "Konsyans yon moun"},
        {"Zulu", "Ukuqonda komunye"},
        {"Xhosa", "Ulwazi lomnye"},
        {"Yoruba", "Imọ ọkan"},
        {"Igbo", "Mmakọ nke otu"},
        {"Amharic", "አንዱ እንቅስቃሴ"},
        {"Tigrinya", "ሓሳብ ኣንድን"},
        {"Somali", "Ogaanshaha hal"},
        {"Hausa", "Fahimtar ɗaya"},
        {"Fijian", "Na kilai ni dua"},
        {"Maori", "Te mohiotanga o tetahi"},
        {"Samoan", "Le malamalama o le tasi"},
        {"Tongan", "Ko e 'ilo 'o e taha"},
        {"Navajo", "Tʼááłáʼí bee naʼanish"},
        {"Cherokee", "ᎠᏌᎪᏂᏍᏗᎢ ᏧᎾᎿᏅ"},
        {"Elvish (Quenya)", "Er'súlema néra"},
        {"Klingon (Star Trek)", "wa' SeQ"},
        {"Huttese (Star Wars)", "Jee jee mindo kee jee"},
        {"Dothraki (Game of Thrones)", "Vezhof ankhes"},
        {"High Valyrian (Game of Thrones)", "Hen pulotu ñuha"},
        {"Na'vi (Avatar)", "Tute siung"},
        {"Nadsat (A Clockwork Orange)", "Odno's soomso"},
        {"Xylorian (Xylor's Chronicles)", "Xylor'wek axol"},
        {"Zogarian (Zogar's Saga)", "Zogar'shukh nax"},
        {"Nebulon (Nebula Tales)", "Nebulon'rath isa"},
        {"Quliri (Galactic Adventures)", "Quliri'voth ixan"},
        {"Vortaxian (Vortax Legends)", "Vortax'kresh omon"},
        {"Draconian (Dragon Realm)", "Dracon'kath arin"},
        {"Thalassian (Oceanic Myths)", "Thalas'sil inen"},
        {"Etherian (Ether World)", "Ether'lium onus"},
        {"Zenthari (Zenith Horizon)", "Zenth'arith eos"},
        {"Aetherian (Aether Chronicles)", "Aether'rion ves"},
        {"Burroughsian (Cut-up Chronicles)", "On'niss ssconn One"}
    };
}