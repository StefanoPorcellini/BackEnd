using Esercizio_M4_W2_D5.Models;

namespace Esercizio_M4_W2_D5.Services
{
    public class ProductService : IProductService
    {
        private static List<Product> _products = new List<Product>
        {
            new Product
           {
            Id = 1,
            Brand = "Nike",
            Name = "Air Zoom Vapor X",
            Price = 129.99m,
            Description = "Scarpe da tennis leggere e reattive con tecnologia Air Zoom.",
            ImgCover = "nike_air_zoom_vapor_x.webp",
            OtherImg = new List<string> { "nike_air_zoom_vapor_x_2.webp", "nike_air_zoom_vapor_x_3.webp" }
        },
        new Product
        {
            Id = 2,
            Brand = "Adidas",
            Name = "Adizero Ubersonic 4",
            Price = 149.99m,
            Description = "Scarpe da tennis ultraleggere per massima velocità e agilità.",
            ImgCover = "adidas_adizero_ubersonic_4.webp",
            OtherImg = new List<string> { "adidas_adizero_ubersonic_4_2.webp", "adidas_adizero_ubersonic_4_3.webp" }
        },
        new Product
        {
            Id = 3,
            Brand = "Asics",
            Name = "Gel-Resolution 8",
            Price = 159.99m,
            Description = "Scarpe da tennis stabili con ammortizzazione Gel per comfort duraturo.",
            ImgCover = "asics_gel_resolution_8.webp",
            OtherImg = new List<string> { "asics_gel_resolution_8_2.webp", "asics_gel_resolution_8_3.webp" }
        },
        new Product
        {
            Id = 4,
            Brand = "Nike",
            Name = "Air Jordan 13 Retro",
            Price = 119.99m,
            Description = "La Air Jordan 13 Retro 'Dune Red' presenta una tomaia in pelle bianca con sovrapposizioni in rete a fossette in una tonalità cremisi scuro. Il rivestimento in pelle scamosciata sintetica Terra Blush si estende all'intersuola leggera in Phylon, dotata di ammortizzazione Zoom Air nell'avampiede e nel tallone. Gli elementi del marchio includono il logo Jumpman ricamato sulla linguetta e un occhio olografico incastonato nel colletto laterale. Sotto il piede, la suola in gomma a zampa di pantera offre una trazione aderente.",
            ImgCover = "air_jordan_13.webp",
            OtherImg = new List<string> { "air_jordan_13_2.webp", "air_jordan_13_3.webp" }
        },
        new Product
        {
            Id = 5,
            Brand = "Adidas",
            Name = "Bad Bunny x Campus",
            Price = 139.99m,
            Description = "La Bad Bunny x adidas Campus 'The Last Campus' utilizza una pelle scamosciata marrone pelosa sulla tomaia, accentuata da tre strisce in pelle marrone e dotata di un collare pesantemente imbottito. I dettagli unici includono linguette raddoppiate, il logo dell'occhio di Benito sulla linguetta superiore e un parafango in gomma ondulata. La low-top poggia su un'intersuola color crema, sostenuta da una suola a spina di pesce.",
            ImgCover = "bad_bunny_x_campus.webp",
            OtherImg = new List<string> { "bad_bunny_x_campus_2.webp", "bad_bunny_x_campus_3.webp" }
        },
        new Product
        {
            Id = 6,
            Brand = "New Balance",
            Name = "Action Bronson x 1906R",
            Price = 169.99m,
            Description = "La Action Bronson x New Balance 1906R 'Rosewater' dona un tocco di colore pastello alla lifestyle runner. I lacci in corda verde lime fissano la tomaia in mesh bianco traspirante, accentuata da un logo 'N' blu bicolore e da un sottile tocco di rosa sul pannello posteriore. Una vivace tonalità fucsia ricopre la gabbia del tallone e la linguetta in TPU, contrassegnata dai marchi Baklava e New Balance 1906. L'intersuola ABZORB, dipinta di arancione e sostenuta da una suola in gomma multicolore, garantisce un'ammortizzazione leggera.",
            ImgCover = "action_bronson.webp",
            OtherImg = new List<string> { "action_bronson_2.webp", "action_bronson_3.webp" }
        },
        new Product
        {
            Id = 7,
            Brand = "New Balance",
            Name = "JJJJound x 2002R GORE-TEX",
            Price = 159.99m,
            Description = "Le JJJJound x New Balance 2002R GORE-TEX \"Green\" sono caratterizzate da un mesh nero traspirante con sovrapposizioni in pelle scamosciata in una tenue tonalità verde. Oltre a una linguetta a soffietto, una fodera impermeabile in GORE-TEX assicura che i piedi rimangano asciutti. Una finitura riflettente è applicata al logo \"N\" e alla linguetta posteriore in pelle con il marchio JJJJound. L'ammortizzazione leggera è garantita da un'intersuola ABZORB nera con baccelli SBS nel tallone.",
            ImgCover = "jjjjound.webp",
            OtherImg = new List<string> { "jjjjound_2.webp", "jjjjound_3.webp" }
        },
        new Product
        {
            Id = 8,
            Brand = "Nike",
            Name = "Dunk Low 2024",
            Price = 179.99m,
            Description = "L'edizione 2024 della Nike Dunk Low CO.JP \"Ultraman\" ripropone un'ambita colorazione del 1999, rilasciata esclusivamente in Giappone. Chiamato così in onore del supereroe giapponese degli anni '60, il modello retrò utilizza una tomaia in pelle liscia, caratterizzata da un colore di base in Varsity Red con sovrapposizioni in grigio neutro a contrasto. Il marchio Nike è presente sulla linguetta, sulla soletta e sulla linguetta posteriore. Alla base della sneaker c'è una tradizionale suola a cupsole che abbina i fianchi bianchi a una suola in gomma grigia.",
            ImgCover = "dunk_low.webp",
            OtherImg = new List<string> { "dunk_low_2.webp", "dunk_low_3.webp" }
        },
        new Product
        {
            Id = 9,
            Brand = "New Balance",
            Name = "Fresh Foam Lav",
            Price = 129.99m,
            Description = "Scarpe da tennis con intersuola Fresh Foam per comfort e leggerezza.",
            ImgCover = "new_balance_fresh_foam_lav.webp",
            OtherImg = new List<string> { "new_balance_fresh_foam_lav_2.webp", "new_balance_fresh_foam_lav_3.webp" }
        },
        new Product
        {
            Id = 10,
            Brand = "Adidas",
            Name = "Response CL",
            Price = 139.99m,
            Description = "Le adidas Response CL sono realizzate con un arioso mesh bianco gesso, arricchito da sovrapposizioni tonali 'fuse' in pelle e camoscio. Le tre strisce avorio delineate da finiture nere adornano i pannelli posteriori, mentre il logo \"eye\" di Bad Bunny compare sulla linguetta posteriore. L'intersuola in schiuma fornisce un'ammortizzazione leggera, resa in una finitura bianco sporco per mantenere l'estetica monocromatica.",
            ImgCover = "response_cl.webp",
            OtherImg = new List<string> { "response_cl_2.webp", "response_cl_3.webp" }
        },
        new Product
        {
            Id = 11,
            Brand = "Puma",
            Name = "LQDCELL Omega",
            Price = 119.99m,
            Description = "Scarpe da tennis con tecnologia LQDCELL per supporto e stabilità.",
            ImgCover = "puma_lqdcell_omega.webp",
            OtherImg = new List<string> { "puma_lqdcell_omega_2.webp", "puma_lqdcell_omega_3.webp" }
        },
        new Product
        {
            Id = 12,
            Brand = "Asics",
            Name = "Court FF Novak",
            Price = 169.99m,
            Description = "Scarpe da tennis premium con supporto eccellente.",
            ImgCover = "asics_court_ff_novak.webp",
            OtherImg = new List<string> { "asics_court_ff_novak_2.webp", "asics_court_ff_novak_3.webp" }
        }
        };

        public List<Product> GetProducts()
        {
            return _products;
        }

        public Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
    }
}
