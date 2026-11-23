using Ferreteria.Api.Models;

namespace Ferreteria.Api.Data;

/// <summary>
/// Datos semilla del catálogo. Se aplican con <c>HasData</c>, por lo que la migración inicial
/// los inserta una sola vez y el arranque de la API nunca los duplica.
/// </summary>
public static class SeedData
{
    public static readonly Categoria[] Categorias =
    [
        new() { Id = 1, Nombre = "Herramientas", Slug = "herramientas" },
        new() { Id = 2, Nombre = "Plomería", Slug = "plomeria" },
        new() { Id = 3, Nombre = "Electricidad", Slug = "electricidad" },
        new() { Id = 4, Nombre = "Pintura", Slug = "pintura" },
        new() { Id = 5, Nombre = "Tornillería", Slug = "tornilleria" },
        new() { Id = 6, Nombre = "Jardín", Slug = "jardin" },
        new() { Id = 7, Nombre = "Seguridad industrial", Slug = "seguridad-industrial" },
    ];

    public static readonly Producto[] Productos =
    [
        // Herramientas
        new() { Id = 1, Nombre = "Taladro percutor 750W", Descripcion = "Taladro percutor de 750W con mandril de 13 mm, velocidad variable y empuñadura ergonómica ideal para concreto y madera.", PrecioUsd = 68.90m, CategoriaId = 1, Stock = 18, Sku = "HER-TAL-001", ImagenUrl = Imagen("her-tal-001") },
        new() { Id = 2, Nombre = "Juego de destornilladores 12 piezas", Descripcion = "Juego de destornilladores planos y de estrella con puntas imantadas y mangos de goma antideslizante.", PrecioUsd = 14.50m, CategoriaId = 1, Stock = 42, Sku = "HER-DES-012", ImagenUrl = Imagen("her-des-012") },
        new() { Id = 3, Nombre = "Sierra circular 7 1/4 pulgadas", Descripcion = "Sierra circular de 1400W con disco de carburo, guía paralela y protección ajustable para cortes rectos en madera.", PrecioUsd = 89.00m, CategoriaId = 1, Stock = 9, Sku = "HER-SIE-185", ImagenUrl = Imagen("her-sie-185") },
        new() { Id = 4, Nombre = "Martillo de uña 16 oz", Descripcion = "Martillo de acero forjado con mango de fibra de vidrio y agarre de goma, balanceado para trabajo prolongado.", PrecioUsd = 11.75m, CategoriaId = 1, Stock = 35, Sku = "HER-MAR-016", ImagenUrl = Imagen("her-mar-016") },
        new() { Id = 5, Nombre = "Cinta métrica 8 metros", Descripcion = "Cinta métrica retráctil de 8 m con carcasa reforzada, freno automático y gancho magnético.", PrecioUsd = 6.40m, CategoriaId = 1, Stock = 60, Sku = "HER-CIN-008", ImagenUrl = Imagen("her-cin-008") },
        new() { Id = 6, Nombre = "Llave de tubo ajustable 10 pulgadas", Descripcion = "Llave ajustable de 10 pulgadas en acero cromado, mandíbula móvil calibrada para tuercas y tuberías.", PrecioUsd = 9.80m, CategoriaId = 1, Stock = 27, Sku = "HER-LLA-010", ImagenUrl = Imagen("her-lla-010") },
        new() { Id = 7, Nombre = "Esmeriladora angular 4 1/2 pulgadas", Descripcion = "Esmeriladora de 850W con disco de corte, protector de seguridad y empuñadura lateral de tres posiciones.", PrecioUsd = 52.30m, CategoriaId = 1, Stock = 14, Sku = "HER-ESM-115", ImagenUrl = Imagen("her-esm-115") },
        new() { Id = 8, Nombre = "Nivel de burbuja 24 pulgadas", Descripcion = "Nivel de aluminio de 24 pulgadas con tres burbujas de precisión y bordes mecanizados antiimpacto.", PrecioUsd = 13.20m, CategoriaId = 1, Stock = 23, Sku = "HER-NIV-024", ImagenUrl = Imagen("her-niv-024") },

        // Plomería
        new() { Id = 9, Nombre = "Tubo PVC 1/2 pulgada x 3 metros", Descripcion = "Tubo de PVC para agua fría de 1/2 pulgada y 3 metros de largo, presión de trabajo de 200 PSI.", PrecioUsd = 3.15m, CategoriaId = 2, Stock = 120, Sku = "PLO-TUB-012", ImagenUrl = Imagen("plo-tub-012") },
        new() { Id = 10, Nombre = "Llave de paso 1/2 pulgada", Descripcion = "Llave de paso de bronce para tubería de 1/2 pulgada, accionamiento de mariposa y empaque resistente.", PrecioUsd = 7.90m, CategoriaId = 2, Stock = 48, Sku = "PLO-LLA-012", ImagenUrl = Imagen("plo-lla-012") },
        new() { Id = 11, Nombre = "Mezcladora monomando para lavabo", Descripcion = "Grifería monomando en acero inoxidable con acabado cromado, cartucho cerámico y aireador antical.", PrecioUsd = 34.60m, CategoriaId = 2, Stock = 16, Sku = "PLO-MEZ-001", ImagenUrl = Imagen("plo-mez-001") },
        new() { Id = 12, Nombre = "Inodoro de dos piezas blanco", Descripcion = "Inodoro de dos piezas en porcelana vitrificada, descarga dual de 3 y 6 litros, incluye asiento.", PrecioUsd = 112.00m, CategoriaId = 2, Stock = 7, Sku = "PLO-INO-002", ImagenUrl = Imagen("plo-ino-002") },
        new() { Id = 13, Nombre = "Pegamento PVC 125 gramos", Descripcion = "Adhesivo solvente para uniones de PVC, secado rápido y resistente a la presión de agua potable.", PrecioUsd = 4.25m, CategoriaId = 2, Stock = 75, Sku = "PLO-PEG-125", ImagenUrl = Imagen("plo-peg-125") },
        new() { Id = 14, Nombre = "Flexible de ducha 1.5 metros", Descripcion = "Manguera flexible de acero inoxidable trenzado con rosca hembra-hembra de 1/2 pulgada.", PrecioUsd = 5.60m, CategoriaId = 2, Stock = 52, Sku = "PLO-FLE-150", ImagenUrl = Imagen("plo-fle-150") },

        // Electricidad
        new() { Id = 15, Nombre = "Cable eléctrico THW 12 AWG x 100 metros", Descripcion = "Rollo de cable de cobre THW calibre 12 AWG para instalaciones residenciales de 600V.", PrecioUsd = 42.80m, CategoriaId = 3, Stock = 22, Sku = "ELE-CAB-212", ImagenUrl = Imagen("ele-cab-212") },
        new() { Id = 16, Nombre = "Breaker monofásico 20A", Descripcion = "Interruptor termomagnético de 20 amperios, riel DIN, capacidad de corte de 10 kA.", PrecioUsd = 8.45m, CategoriaId = 3, Stock = 64, Sku = "ELE-BRE-120", ImagenUrl = Imagen("ele-bre-120") },
        new() { Id = 17, Nombre = "Tomacorriente doble con placa", Descripcion = "Tomacorriente doble polarizado de 15A con placa de acabado blanco y tornillos de montaje.", PrecioUsd = 3.90m, CategoriaId = 3, Stock = 90, Sku = "ELE-TOM-015", ImagenUrl = Imagen("ele-tom-015") },
        new() { Id = 18, Nombre = "Bombillo LED 9W luz blanca", Descripcion = "Bombillo LED de 9W con rosca E27, 900 lúmenes y 25 000 horas de vida útil.", PrecioUsd = 2.75m, CategoriaId = 3, Stock = 150, Sku = "ELE-BOM-009", ImagenUrl = Imagen("ele-bom-009") },
        new() { Id = 19, Nombre = "Tablero eléctrico 12 circuitos", Descripcion = "Tablero metálico para 12 circuitos con puerta abatible, barra de tierra y riel DIN.", PrecioUsd = 29.90m, CategoriaId = 3, Stock = 11, Sku = "ELE-TAB-012", ImagenUrl = Imagen("ele-tab-012") },
        new() { Id = 20, Nombre = "Extensión eléctrica 10 metros", Descripcion = "Extensión de 10 metros con triple tomacorriente, interruptor y cable 14 AWG.", PrecioUsd = 15.40m, CategoriaId = 3, Stock = 31, Sku = "ELE-EXT-010", ImagenUrl = Imagen("ele-ext-010") },

        // Pintura
        new() { Id = 21, Nombre = "Pintura acrílica blanca 1 galón", Descripcion = "Pintura acrílica mate para interiores y exteriores, alto poder cubriente y bajo olor.", PrecioUsd = 18.70m, CategoriaId = 4, Stock = 40, Sku = "PIN-ACR-BLA", ImagenUrl = Imagen("pin-acr-bla") },
        new() { Id = 22, Nombre = "Esmalte sintético azul 1/4 galón", Descripcion = "Esmalte de secado rápido para metal y madera, acabado brillante resistente a la intemperie.", PrecioUsd = 9.30m, CategoriaId = 4, Stock = 26, Sku = "PIN-ESM-AZU", ImagenUrl = Imagen("pin-esm-azu") },
        new() { Id = 23, Nombre = "Brocha de cerda natural 3 pulgadas", Descripcion = "Brocha de cerda natural con mango de madera barnizada, ideal para esmaltes y barnices.", PrecioUsd = 4.80m, CategoriaId = 4, Stock = 58, Sku = "PIN-BRO-003", ImagenUrl = Imagen("pin-bro-003") },
        new() { Id = 24, Nombre = "Rodillo antigoteo 9 pulgadas", Descripcion = "Rodillo de felpa antigoteo con mango ergonómico y jaula reforzada para pintura acrílica.", PrecioUsd = 6.20m, CategoriaId = 4, Stock = 44, Sku = "PIN-ROD-009", ImagenUrl = Imagen("pin-rod-009") },
        new() { Id = 25, Nombre = "Cinta de enmascarar 2 pulgadas", Descripcion = "Cinta de papel para delimitar bordes de pintura, adhesivo suave que no daña la superficie.", PrecioUsd = 2.10m, CategoriaId = 4, Stock = 88, Sku = "PIN-CIN-002", ImagenUrl = Imagen("pin-cin-002") },
        new() { Id = 26, Nombre = "Thinner industrial 1 litro", Descripcion = "Diluyente para esmaltes y lacas, alto poder de disolución y evaporación controlada.", PrecioUsd = 5.95m, CategoriaId = 4, Stock = 37, Sku = "PIN-THI-001", ImagenUrl = Imagen("pin-thi-001") },

        // Tornillería
        new() { Id = 27, Nombre = "Tornillo autoperforante 8 x 1 pulgada (100 unidades)", Descripcion = "Caja de 100 tornillos autoperforantes con punta broca y cabeza hexagonal para metal.", PrecioUsd = 7.60m, CategoriaId = 5, Stock = 55, Sku = "TOR-AUT-081", ImagenUrl = Imagen("tor-aut-081") },
        new() { Id = 28, Nombre = "Taco de expansión 3/8 (50 unidades)", Descripcion = "Tacos plásticos de expansión para concreto y ladrillo, paquete de 50 unidades.", PrecioUsd = 4.35m, CategoriaId = 5, Stock = 70, Sku = "TOR-TAC-038", ImagenUrl = Imagen("tor-tac-038") },
        new() { Id = 29, Nombre = "Perno galvanizado 1/2 x 4 pulgadas", Descripcion = "Perno hexagonal galvanizado en caliente con tuerca y arandela, resistente a la corrosión.", PrecioUsd = 1.85m, CategoriaId = 5, Stock = 140, Sku = "TOR-PER-124", ImagenUrl = Imagen("tor-per-124") },
        new() { Id = 30, Nombre = "Arandela plana 5/16 (100 unidades)", Descripcion = "Arandelas planas de acero zincado para distribución de carga en uniones atornilladas.", PrecioUsd = 3.20m, CategoriaId = 5, Stock = 96, Sku = "TOR-ARA-516", ImagenUrl = Imagen("tor-ara-516") },
        new() { Id = 31, Nombre = "Alambre galvanizado N° 12 (1 kg)", Descripcion = "Alambre de acero galvanizado calibre 12 para amarre de estructuras y cercas.", PrecioUsd = 3.75m, CategoriaId = 5, Stock = 80, Sku = "TOR-ALA-012", ImagenUrl = Imagen("tor-ala-012") },
        new() { Id = 32, Nombre = "Remache pop 3/16 (100 unidades)", Descripcion = "Remaches pop de aluminio con mandril de acero, caja de 100 unidades para lámina metálica.", PrecioUsd = 6.10m, CategoriaId = 5, Stock = 62, Sku = "TOR-REM-316", ImagenUrl = Imagen("tor-rem-316") },

        // Jardín
        new() { Id = 33, Nombre = "Manguera de jardín 15 metros", Descripcion = "Manguera reforzada de 1/2 pulgada y 15 metros con conectores de ajuste rápido.", PrecioUsd = 19.50m, CategoriaId = 6, Stock = 25, Sku = "JAR-MAN-150", ImagenUrl = Imagen("jar-man-150") },
        new() { Id = 34, Nombre = "Pala cuadrada con mango de madera", Descripcion = "Pala de acero templado con mango de madera de 90 cm, ideal para excavación en tierra dura.", PrecioUsd = 13.80m, CategoriaId = 6, Stock = 20, Sku = "JAR-PAL-001", ImagenUrl = Imagen("jar-pal-001") },
        new() { Id = 35, Nombre = "Tijera de podar profesional", Descripcion = "Tijera de podar con hoja de acero al carbono y mango ergonómico con seguro de bloqueo.", PrecioUsd = 11.20m, CategoriaId = 6, Stock = 33, Sku = "JAR-TIJ-002", ImagenUrl = Imagen("jar-tij-002") },
        new() { Id = 36, Nombre = "Fertilizante granular 1 kg", Descripcion = "Fertilizante NPK 15-15-15 de liberación gradual para plantas ornamentales y huertos.", PrecioUsd = 6.90m, CategoriaId = 6, Stock = 48, Sku = "JAR-FER-001", ImagenUrl = Imagen("jar-fer-001") },
        new() { Id = 37, Nombre = "Aspersor de jardín ajustable", Descripcion = "Aspersor oscilante de plástico reforzado con cobertura ajustable de hasta 15 metros.", PrecioUsd = 8.60m, CategoriaId = 6, Stock = 29, Sku = "JAR-ASP-001", ImagenUrl = Imagen("jar-asp-001") },

        // Seguridad industrial
        new() { Id = 38, Nombre = "Casco de seguridad con barbiquejo", Descripcion = "Casco de polietileno de alta densidad con suspensión ajustable y barbiquejo de nylon.", PrecioUsd = 10.40m, CategoriaId = 7, Stock = 46, Sku = "SEG-CAS-001", ImagenUrl = Imagen("seg-cas-001") },
        new() { Id = 39, Nombre = "Guantes de látex reforzados (par)", Descripcion = "Guantes de látex con palma reforzada y puño elástico para manipulación de materiales.", PrecioUsd = 2.90m, CategoriaId = 7, Stock = 110, Sku = "SEG-GUA-001", ImagenUrl = Imagen("seg-gua-001") },
        new() { Id = 40, Nombre = "Lentes de protección transparentes", Descripcion = "Lentes de seguridad con lente de policarbonato antiempañante y patillas ajustables.", PrecioUsd = 4.15m, CategoriaId = 7, Stock = 82, Sku = "SEG-LEN-001", ImagenUrl = Imagen("seg-len-001") },
    ];

    private static string Imagen(string semilla) => $"https://picsum.photos/seed/{semilla}/600/400";
}
