using Intercon.Domain.ComplexTypes;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;

namespace Intercon.Infrastructure.Persistence.DataSeeder.Seeds;

public class BusinessesSeed
{
    public static List<Business> SeedBusinesses()
    {
        return new List<Business>()
        {
            new Business
            {
                Id = 1,
                OwnerId = 1,
                Title = "Linella",
                ShortDescription =
                    "Fresh Produce Paradise: Our supermarket boasts a colorful array of crisp, farm-fresh fruits and vegetables.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Mioriţa 6, MD-2028, Chișinău, Moldova",
                    Latitude: "46.997268711131156",
                    Longitude: "28.808228905548145"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 2,
                OwnerId = 2,
                Title = "Linella",
                ShortDescription =
                    "Deli Delights: Savor the finest cuts and artisanal cheeses at our deli counter, perfect for creating gourmet sandwiches.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Mioriţa 11, MD-2028, Chișinău, Moldova",
                    Latitude: "46.99245259019449",
                    Longitude: "28.8195631373346"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 3,
                OwnerId = 3,
                Title = "Linella",
                ShortDescription =
                    "Bakery Bliss: Indulge your senses with the aroma of freshly baked bread, pastries, and cakes from our in-house bakery.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Nicolae Testemițanu 39, MD-2025, Chișinău, Moldova",
                    Latitude: "46.99033662341652",
                    Longitude: "28.827228161847135"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 4,
                OwnerId = 4,
                Title = "Linella",
                ShortDescription =
                    "International Aisles: Explore global flavors in our international section, featuring a diverse range of spices, sauces, and exotic ingredients.",
                FullDescription = "",
                Address = new Address(
                    Street: "Grenoble St 128, MD-2019, Chișinău, Moldova",
                    Latitude: "46.98159778023099",
                    Longitude: "28.838277932155503"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 5,
                OwnerId = 5,
                Title = "Linella",
                ShortDescription =
                    "Organic Oasis: Embrace a healthier lifestyle with our extensive selection of organic products, from produce to pantry staples.",
                FullDescription = "",
                Address = new Address(
                    Street: "Trajan Blvd 22, MD-2060, Chișinău, Moldova",
                    Latitude: "46.97901985761206",
                    Longitude: "28.84628902068298"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 6,
                OwnerId = 6,
                Title = "Linella",
                ShortDescription =
                    "Tech-Savvy Shopping: Enjoy a seamless experience with state-of-the-art checkout systems and contactless payment options.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Cuza Vodă 42, MD-2060, Chișinău, Moldova",
                    Latitude: "46.97440110685183",
                    Longitude: "28.8484844829509"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 7,
                OwnerId = 7,
                Title = "Linella",
                ShortDescription =
                    "Coffee Corner: Energize your shopping spree with a stop at our coffee kiosk, offering a variety of brews to suit every taste.",
                FullDescription = "",
                Address = new Address(
                    Street: "Dacia Blvd 44, MD-2062, Chișinău, Moldova",
                    Latitude: "46.97683767341616",
                    Longitude: "28.86946481346383"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 8,
                OwnerId = 8,
                Title = "Linella",
                ShortDescription =
                    "Budget-Friendly Buys: Discover unbeatable deals and discounts on everyday essentials throughout our aisles.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Cuza Vodă 24, MD-2072, Chișinău, Moldova",
                    Latitude: "46.98006234030243",
                    Longitude: "28.85696562791677"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 9,
                OwnerId = 9,
                Title = "Linella",
                ShortDescription =
                    "Kid-Friendly Zone: Keep the little ones entertained with a dedicated kids' corner featuring snacks, toys, and games.",
                FullDescription = "",
                Address = new Address(
                    Street: "Trajan Blvd 13/1, MD-2060, Chișinău, Moldova",
                    Latitude: "46.98254643240371",
                    Longitude: "28.852738466974355"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 10,
                OwnerId = 10,
                Title = "Linella",
                ShortDescription =
                    "Wine and Spirits Wonderland: Choose from a curated selection of wines, spirits, and craft beers to complement any occasion.",
                FullDescription = "",
                Address = new Address(
                    Street: "Independentei St 5A, Chișinău, Moldova",
                    Latitude: "46.98363535990976",
                    Longitude: "28.848487165341734"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 11,
                OwnerId = 11,
                Title = "Linella",
                ShortDescription =
                    "Health and Wellness Hub: Find a range of health-conscious products, including vitamins, supplements, and natural remedies.",
                FullDescription = "",
                Address = new Address(
                    Street: "Independentei St 4/1, MD-2043, Chișinău, Moldova",
                    Latitude: "46.9854276408486",
                    Longitude: "28.84450810863036"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 12,
                OwnerId = 12,
                Title = "Linella",
                ShortDescription =
                    "Pet Paradise: Treat your furry friends with a selection of premium pet foods, toys, and accessories.",
                FullDescription = "",
                Address = new Address(
                    Street: "Hristo Botev St 17, MD-2043, Chișinău, Moldova",
                    Latitude: "46.98837896380982",
                    Longitude: "28.847787780138415"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 13,
                OwnerId = 13,
                Title = "Linella",
                ShortDescription =
                    "Floral Fantasy: Brighten your home with fresh flowers from our floral department, offering a vibrant assortment for any occasion.",
                FullDescription = "",
                Address = new Address(
                    Street: "Dacia Blvd 16/1, MD-2043, Chișinău, Moldova",
                    Latitude: "46.989030778364764",
                    Longitude: "28.8523951505316"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 14,
                OwnerId = 14,
                Title = "Linella",
                ShortDescription =
                    "Bulk Bargains: Reduce waste and save money with our bulk bins filled with grains, cereals, and snacks.",
                FullDescription = "",
                Address = new Address(
                    Street: "Sarmizegetusa St 35/1, Chișinău, Moldova",
                    Latitude: "46.98675285653647",
                    Longitude: "28.873743911958265"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 15,
                OwnerId = 15,
                Title = "Linella",
                ShortDescription =
                    "Grill Masters' Haven: Elevate your BBQ game with our selection of premium meats, marinades, and grilling accessories.",
                FullDescription = "",
                Address = new Address(
                    Street: "Nicolae Titulescu St 47, MD-2032, Chișinău, Moldova",
                    Latitude: "46.99398475692658",
                    Longitude: "28.868086811800232"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 16,
                OwnerId = 16,
                Title = "Linella",
                ShortDescription =
                    "Seasonal Sensations: Experience the best of each season with rotating displays of seasonal fruits, vegetables, and decor.",
                FullDescription = "",
                Address = new Address(
                    Street: "Șoseaua Muncești 388, MD-2029, Chișinău, Moldova",
                    Latitude: "46.9840704074349",
                    Longitude: "28.89469700456433"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 17,
                OwnerId = 17,
                Title = "Linella",
                ShortDescription =
                    "Convenient Meal Solutions: Explore our ready-to-eat section for quick and delicious meal options for busy days.",
                FullDescription = "",
                Address = new Address(
                    Street: "Trandafirilor Street 13/1, MD-2038, Chișinău, Moldova",
                    Latitude: "46.99821020387124",
                    Longitude: "28.852975914683928"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 18,
                OwnerId = 18,
                Title = "Linella",
                ShortDescription =
                    "DIY Cooking Central: Unleash your inner chef with a comprehensive range of fresh ingredients and culinary tools.",
                FullDescription = "",
                Address = new Address(
                    Street: "Decebal Blvd 59, MD-2015, Chișinău, Moldova",
                    Latitude: "47.00055475652262",
                    Longitude: "28.860421333180224"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 19,
                OwnerId = 19,
                Title = "Linella",
                ShortDescription =
                    "Customer Rewards Program: Enjoy exclusive discounts and perks with our loyalty program, making every visit more rewarding.",
                FullDescription = "",
                Address = new Address(
                    Street: "Șoseaua Muncești 162a, MD-2002, Chișinău, Moldova",
                    Latitude: "47.002573085802815",
                    Longitude: "28.86912407950438"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 20,
                OwnerId = 20,
                Title = "Linella",
                ShortDescription =
                    "Environmental Initiatives: We're committed to sustainability with eco-friendly packaging and efforts to reduce our carbon footprint.",
                FullDescription = "",
                Address = new Address(
                    Street: "str. Ştefan cel Mare 6/1, MD-2001, Chișinău, Moldova",
                    Latitude: "47.014784396197186",
                    Longitude: "28.84825418047755"
                ),
                Category = BusinessCategory.Supermarket
            },
            new Business
            {
                Id = 21,
                OwnerId = 21,
                Title = "Granier",
                ShortDescription =
                    "Artisanal Delights: Step into our bakery and savor the aroma of freshly baked artisanal bread, crafted with care and passion.",
                FullDescription = "",
                Address = new Address(
                    Street: "Dacia Blvd 23, Chișinău, Moldova",
                    Latitude: "46.987043756473525",
                    Longitude: "28.858158245545862"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 22,
                OwnerId = 22,
                Title = "Granier",
                ShortDescription =
                    "Sweet Symphony: Indulge your sweet tooth with our delectable array of pastries, cakes, and cookies, each a symphony of flavors.",
                FullDescription = "",
                Address = new Address(
                    Street: "Bulevardul Moscova 2, Chișinău, Moldova",
                    Latitude: "47.04659714748263",
                    Longitude: "28.862784138001565"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 23,
                OwnerId = 23,
                Title = "Granier",
                ShortDescription =
                    "Daily Dough Delights: Start your day right with our daily selection of freshly baked goods, from flaky croissants to hearty bagels.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Ion Creangă 1/3, Chișinău, Moldova",
                    Latitude: "47.03707074148245",
                    Longitude: "28.812059282788898"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 24,
                OwnerId = 24,
                Title = "Granier",
                ShortDescription =
                    "Custom Cake Creations: Celebrate life's special moments with our custom cake service, where we turn your visions into delicious realities.",
                FullDescription = "",
                Address = new Address(
                    Street: "Ion Creanga Street 78, MD-2064, Chișinău, Moldova",
                    Latitude: "47.02794057099621",
                    Longitude: "28.794680547206337"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 25,
                OwnerId = 25,
                Title = "Granier",
                ShortDescription =
                    "Wholesome and Hearty: Embrace the goodness of our wholesome, hearty bread varieties, perfect for sandwiches or standalone enjoyment.",
                FullDescription = "",
                Address = new Address(
                    Street: "Stefan cel Mare si Sfant Boulevard 69, Chișinău, Moldova",
                    Latitude: "47.01735225708259",
                    Longitude: "28.84274959996724"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 26,
                OwnerId = 26,
                Title = "Granier",
                ShortDescription =
                    "Gourmet Gluten-Free Options: Delight in our gourmet gluten-free treats, proving that dietary restrictions shouldn't compromise flavor.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Ismail 86/4, Chișinău, Moldova",
                    Latitude: "47.01704785707604",
                    Longitude: "28.848374799967235"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 27,
                OwnerId = 27,
                Title = "Granier",
                ShortDescription =
                    "Seasonal Sensations: Experience the changing seasons through our rotating menu of seasonal specialties, each a taste of something new.",
                FullDescription = "",
                Address = new Address(
                    Street: "Dacia Blvd 23, Chișinău, Moldova",
                    Latitude: "46.98704398518555",
                    Longitude: "28.858158246593405"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 28,
                OwnerId = 28,
                Title = "Granier",
                ShortDescription =
                    "Coffee Companions: Pair your favorite brew with our selection of muffins, scones, and Danish pastries for a delightful coffee break.",
                FullDescription = "",
                Address = new Address(
                    Street: "Alba-Iulia St 75/6, Chișinău, Moldova",
                    Latitude: "47.03817349730268",
                    Longitude: "28.769902617145586"
                ),
                Category = BusinessCategory.Bakery
            },
            new Business
            {
                Id = 29,
                OwnerId = 29,
                Title = "Big sport gym",
                ShortDescription = "Protein? We have protein. We love protein. Protein is our friend, is our life.",
                FullDescription = "",
                Address = new Address(
                    Street: "Alba-Iulia St 168, MD-2051, Chișinău, Moldova",
                    Latitude: "47.034170414465535",
                    Longitude: "28.77906807051942"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 30,
                OwnerId = 30,
                Title = "Big sport gym",
                ShortDescription =
                    "Fitness Oasis: Step into our sport gym, where health and wellness come together in a dynamic and energizing environment.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Calea Ieşilor 10, MD-2069, Chișinău, Moldova",
                    Latitude: "47.040153314542394",
                    Longitude: "28.802261799967244"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 31,
                OwnerId = 31,
                Title = "Jungle Fitness",
                ShortDescription =
                    "State-of-the-Art Equipment: Experience the latest in fitness technology with our state-of-the-art exercise machines designed for optimal results.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Ceucari 2/3, Chișinău, Moldova",
                    Latitude: "47.061499693722865",
                    Longitude: "28.846700904257833"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 32,
                OwnerId = 32,
                Title = "Big sport gym",
                ShortDescription =
                    "Group Fitness Fiesta: Join our invigorating group fitness classes, from heart-pounding HIIT sessions to calming yoga, catering to all fitness levels.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Socoleni 7, MD-2020, Chișinău, Moldova",
                    Latitude: "47.061454471605394",
                    Longitude: "28.846780029415065"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 33,
                OwnerId = 33,
                Title = "Big sport gym",
                ShortDescription =
                    "Personalized Training Programs: Achieve your fitness goals with our personalized training programs, crafted by experienced fitness professionals.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Ion Dumeniuc 12, MD-2075, Chișinău, Moldova",
                    Latitude: "47.05619741474855",
                    Longitude: "28.89190357051942"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 34,
                OwnerId = 34,
                Title = "Big sport gym",
                ShortDescription =
                    "Wellness Community Hub: Connect with like-minded individuals in our vibrant fitness community, fostering motivation and support.",
                FullDescription = "",
                Address = new Address(
                    Street: "Constantin Brâncuși St 3, MD-2060, Chișinău, Moldova",
                    Latitude: "46.9894162312499",
                    Longitude: "28.860072521799268"
                ),
                Category = BusinessCategory.Gym
            },
            new Business
            {
                Id = 35,
                OwnerId = 35,
                Title = "McDonald's",
                ShortDescription =
                    "Golden Arches Haven: Enter the iconic world of McDonald's, where the unmistakable golden arches promise a familiar and delicious experience.",
                FullDescription = "",
                Address = new Address(
                    Street: "Dacia Blvd 21/1, MD-2038, Chișinău, Moldova",
                    Latitude: "46.98718779604783",
                    Longitude: "28.857078982756153"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 36,
                OwnerId = 36,
                Title = "McDonald's",
                ShortDescription =
                    "Fast-Food Excellence: Enjoy a quick and satisfying meal with our fast-food excellence, featuring classic favorites and innovative menu additions.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Haiducilor 31, Codru, Moldova",
                    Latitude: "46.99556232794662",
                    Longitude: "28.797881468852168"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 37,
                OwnerId = 37,
                Title = "McDonald's",
                ShortDescription =
                    "Happy Meal Magic: Delight the little ones with our famous Happy Meals, complete with a toy and a menu designed just for kids.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Arborilor 21, Chișinău, Moldova",
                    Latitude: "47.004621527706284",
                    Longitude: "28.840940282175964"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 38,
                OwnerId = 38,
                Title = "McDonald's",
                ShortDescription =
                    "Drive-Thru Convenience: Experience the ease of our drive-thru service, ensuring your favorite McDonald's meals are just a window away.",
                FullDescription = "",
                Address = new Address(
                    Street: "Stefan cel Mare si Sfant Boulevard 8, Chișinău, Moldova",
                    Latitude: "47.01666195706779",
                    Longitude: "28.84588759996724"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 39,
                OwnerId = 39,
                Title = "McDonald's",
                ShortDescription =
                    "McCafé Elegance: Elevate your coffee experience at our McCafé, offering a variety of expertly crafted coffees and delightful pastries.",
                FullDescription = "",
                Address = new Address(
                    Street: "Stefan cel Mare si Sfant Boulevard 134/1, MD-2001, Chișinău, Moldova",
                    Latitude: "47.023347857210915",
                    Longitude: "28.834795729415063"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 40,
                OwnerId = 40,
                Title = "McDonald's",
                ShortDescription =
                    "Global Flavors, Local Favorites: Explore our diverse menu that brings global flavors to local communities, ensuring a taste for every palate.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Calea Ieşilor 8, Chișinău, Moldova",
                    Latitude: "47.03967041453619",
                    Longitude: "28.80318217051942"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 41,
                OwnerId = 41,
                Title = "McDonald's",
                ShortDescription =
                    "McDelivery Magic: Enjoy the convenience of McDelivery, bringing your McDonald's favorites directly to your doorstep with just a few taps on your phone.",
                FullDescription = "",
                Address = new Address(
                    Street: "Alba-Iulia St 194/A, MD-2071, Chișinău, Moldova",
                    Latitude: "47.038700828492104",
                    Longitude: "28.770121370519423"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 42,
                OwnerId = 42,
                Title = "McDonald's",
                ShortDescription =
                    "Nutritional Transparency: Make informed choices with our nutritional information readily available, allowing you to balance taste and wellness.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Alecu Russo 2, Chișinău, Moldova",
                    Latitude: "47.04458835766566",
                    Longitude: "28.861046799967244"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 43,
                OwnerId = 43,
                Title = "McDonald's",
                ShortDescription =
                    "Ronald McDonald House Charities: Support a good cause with your meal – a portion of our proceeds goes to Ronald McDonald House Charities, helping families in need.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Mihai Sadoveanu 42/6, Chișinău, Moldova",
                    Latitude: "47.07014241492778",
                    Longitude: "28.888394929415064"
                ),
                Category = BusinessCategory.FastFood
            },
            new Business
            {
                Id = 44,
                OwnerId = 44,
                Title = "Darwin",
                ShortDescription =
                    "Tech Wonderland: Step into our electronics store, where cutting-edge gadgets and innovative devices create a tech enthusiast's paradise.",
                FullDescription = "",
                Address = new Address(
                    Street: "Independentei St 13, MD-2043, Chișinău, Moldova",
                    Latitude: "46.97775792300121",
                    Longitude: "28.85631288155769"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 45,
                OwnerId = 45,
                Title = "Darwin",
                ShortDescription =
                    "Smart Solutions Hub: Explore a world of smart solutions for your home and lifestyle, featuring the latest in connected devices and automation.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Arborilor 21, Chișinău, Moldova",
                    Latitude: "47.00430172863937",
                    Longitude: "28.840684799967242"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 46,
                OwnerId = 46,
                Title = "Darwin",
                ShortDescription =
                    "Gaming Galore: Immerse yourself in the ultimate gaming experience with our extensive collection of gaming consoles, accessories, and high-performance PCs.",
                FullDescription = "",
                Address = new Address(
                    Street: "Hincesti Hwy 60/4, MD-2028, Chișinău, Moldova",
                    Latitude: "47.00273211406181",
                    Longitude: "28.816402429415064"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 47,
                OwnerId = 47,
                Title = "Darwin",
                ShortDescription =
                    "Home Entertainment Haven: Elevate your home entertainment with state-of-the-art TVs, sound systems, and streaming devices for a cinematic experience.",
                FullDescription = "",
                Address = new Address(
                    Street: "Centru, Chișinău MD, Bulevardul Ștefan cel Mare și Sfînt 71, MD-2012, Moldova",
                    Latitude: "47.01822345710117",
                    Longitude: "28.84141332941506"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 48,
                OwnerId = 48,
                Title = "Darwin",
                ShortDescription =
                    "Mobile Marvels: Discover the latest smartphones, tablets, and wearables, offering sleek design and powerful features to keep you connected.",
                FullDescription = "",
                Address = new Address(
                    Street: "Mitropolit Varlaam St 58, Chișinău, Moldova",
                    Latitude: "47.019306871424895",
                    Longitude: "28.845023929415063"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 49,
                OwnerId = 49,
                Title = "Darwin",
                ShortDescription =
                    "Tech Accessories Alley: Enhance your devices with our wide range of accessories, from stylish phone cases to high-quality cables and chargers.",
                FullDescription = "",
                Address = new Address(
                    Street: "Stefan cel Mare si Sfant Boulevard 132, Chișinău, Moldova",
                    Latitude: "47.02248167143844",
                    Longitude: "28.836127599967234"
                ),
                Category = BusinessCategory.Electronics
            },
            new Business
            {
                Id = 50,
                OwnerId = 50,
                Title = "Darwin",
                ShortDescription =
                    "DIY Tech Projects Corner: Embark on your DIY tech projects with our selection of components, tools, and kits, empowering you to create and innovate.",
                FullDescription = "",
                Address = new Address(
                    Street: "Strada Ion Creangă 49/5, Сhisinau, Moldova",
                    Latitude: "47.02755544258538, ",
                    Longitude: "28.795076827982474"
                ),
                Category = BusinessCategory.Electronics
            }
        };
    }
}