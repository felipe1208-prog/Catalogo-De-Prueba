using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ferreteria.Api.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 600, nullable: false),
                    PrecioUsd = table.Column<decimal>(type: "TEXT", nullable: false),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ImagenUrl = table.Column<string>(type: "TEXT", maxLength: 400, nullable: true),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    Sku = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Nombre", "Slug" },
                values: new object[,]
                {
                    { 1, "Herramientas", "herramientas" },
                    { 2, "Plomería", "plomeria" },
                    { 3, "Electricidad", "electricidad" },
                    { 4, "Pintura", "pintura" },
                    { 5, "Tornillería", "tornilleria" },
                    { 6, "Jardín", "jardin" },
                    { 7, "Seguridad industrial", "seguridad-industrial" }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "CategoriaId", "Descripcion", "ImagenUrl", "Nombre", "PrecioUsd", "Sku", "Stock" },
                values: new object[,]
                {
                    { 1, 1, "Taladro percutor de 750W con mandril de 13 mm, velocidad variable y empuñadura ergonómica ideal para concreto y madera.", "https://picsum.photos/seed/her-tal-001/600/400", "Taladro percutor 750W", 68.90m, "HER-TAL-001", 18 },
                    { 2, 1, "Juego de destornilladores planos y de estrella con puntas imantadas y mangos de goma antideslizante.", "https://picsum.photos/seed/her-des-012/600/400", "Juego de destornilladores 12 piezas", 14.50m, "HER-DES-012", 42 },
                    { 3, 1, "Sierra circular de 1400W con disco de carburo, guía paralela y protección ajustable para cortes rectos en madera.", "https://picsum.photos/seed/her-sie-185/600/400", "Sierra circular 7 1/4 pulgadas", 89.00m, "HER-SIE-185", 9 },
                    { 4, 1, "Martillo de acero forjado con mango de fibra de vidrio y agarre de goma, balanceado para trabajo prolongado.", "https://picsum.photos/seed/her-mar-016/600/400", "Martillo de uña 16 oz", 11.75m, "HER-MAR-016", 35 },
                    { 5, 1, "Cinta métrica retráctil de 8 m con carcasa reforzada, freno automático y gancho magnético.", "https://picsum.photos/seed/her-cin-008/600/400", "Cinta métrica 8 metros", 6.40m, "HER-CIN-008", 60 },
                    { 6, 1, "Llave ajustable de 10 pulgadas en acero cromado, mandíbula móvil calibrada para tuercas y tuberías.", "https://picsum.photos/seed/her-lla-010/600/400", "Llave de tubo ajustable 10 pulgadas", 9.80m, "HER-LLA-010", 27 },
                    { 7, 1, "Esmeriladora de 850W con disco de corte, protector de seguridad y empuñadura lateral de tres posiciones.", "https://picsum.photos/seed/her-esm-115/600/400", "Esmeriladora angular 4 1/2 pulgadas", 52.30m, "HER-ESM-115", 14 },
                    { 8, 1, "Nivel de aluminio de 24 pulgadas con tres burbujas de precisión y bordes mecanizados antiimpacto.", "https://picsum.photos/seed/her-niv-024/600/400", "Nivel de burbuja 24 pulgadas", 13.20m, "HER-NIV-024", 23 },
                    { 9, 2, "Tubo de PVC para agua fría de 1/2 pulgada y 3 metros de largo, presión de trabajo de 200 PSI.", "https://picsum.photos/seed/plo-tub-012/600/400", "Tubo PVC 1/2 pulgada x 3 metros", 3.15m, "PLO-TUB-012", 120 },
                    { 10, 2, "Llave de paso de bronce para tubería de 1/2 pulgada, accionamiento de mariposa y empaque resistente.", "https://picsum.photos/seed/plo-lla-012/600/400", "Llave de paso 1/2 pulgada", 7.90m, "PLO-LLA-012", 48 },
                    { 11, 2, "Grifería monomando en acero inoxidable con acabado cromado, cartucho cerámico y aireador antical.", "https://picsum.photos/seed/plo-mez-001/600/400", "Mezcladora monomando para lavabo", 34.60m, "PLO-MEZ-001", 16 },
                    { 12, 2, "Inodoro de dos piezas en porcelana vitrificada, descarga dual de 3 y 6 litros, incluye asiento.", "https://picsum.photos/seed/plo-ino-002/600/400", "Inodoro de dos piezas blanco", 112.00m, "PLO-INO-002", 7 },
                    { 13, 2, "Adhesivo solvente para uniones de PVC, secado rápido y resistente a la presión de agua potable.", "https://picsum.photos/seed/plo-peg-125/600/400", "Pegamento PVC 125 gramos", 4.25m, "PLO-PEG-125", 75 },
                    { 14, 2, "Manguera flexible de acero inoxidable trenzado con rosca hembra-hembra de 1/2 pulgada.", "https://picsum.photos/seed/plo-fle-150/600/400", "Flexible de ducha 1.5 metros", 5.60m, "PLO-FLE-150", 52 },
                    { 15, 3, "Rollo de cable de cobre THW calibre 12 AWG para instalaciones residenciales de 600V.", "https://picsum.photos/seed/ele-cab-212/600/400", "Cable eléctrico THW 12 AWG x 100 metros", 42.80m, "ELE-CAB-212", 22 },
                    { 16, 3, "Interruptor termomagnético de 20 amperios, riel DIN, capacidad de corte de 10 kA.", "https://picsum.photos/seed/ele-bre-120/600/400", "Breaker monofásico 20A", 8.45m, "ELE-BRE-120", 64 },
                    { 17, 3, "Tomacorriente doble polarizado de 15A con placa de acabado blanco y tornillos de montaje.", "https://picsum.photos/seed/ele-tom-015/600/400", "Tomacorriente doble con placa", 3.90m, "ELE-TOM-015", 90 },
                    { 18, 3, "Bombillo LED de 9W con rosca E27, 900 lúmenes y 25 000 horas de vida útil.", "https://picsum.photos/seed/ele-bom-009/600/400", "Bombillo LED 9W luz blanca", 2.75m, "ELE-BOM-009", 150 },
                    { 19, 3, "Tablero metálico para 12 circuitos con puerta abatible, barra de tierra y riel DIN.", "https://picsum.photos/seed/ele-tab-012/600/400", "Tablero eléctrico 12 circuitos", 29.90m, "ELE-TAB-012", 11 },
                    { 20, 3, "Extensión de 10 metros con triple tomacorriente, interruptor y cable 14 AWG.", "https://picsum.photos/seed/ele-ext-010/600/400", "Extensión eléctrica 10 metros", 15.40m, "ELE-EXT-010", 31 },
                    { 21, 4, "Pintura acrílica mate para interiores y exteriores, alto poder cubriente y bajo olor.", "https://picsum.photos/seed/pin-acr-bla/600/400", "Pintura acrílica blanca 1 galón", 18.70m, "PIN-ACR-BLA", 40 },
                    { 22, 4, "Esmalte de secado rápido para metal y madera, acabado brillante resistente a la intemperie.", "https://picsum.photos/seed/pin-esm-azu/600/400", "Esmalte sintético azul 1/4 galón", 9.30m, "PIN-ESM-AZU", 26 },
                    { 23, 4, "Brocha de cerda natural con mango de madera barnizada, ideal para esmaltes y barnices.", "https://picsum.photos/seed/pin-bro-003/600/400", "Brocha de cerda natural 3 pulgadas", 4.80m, "PIN-BRO-003", 58 },
                    { 24, 4, "Rodillo de felpa antigoteo con mango ergonómico y jaula reforzada para pintura acrílica.", "https://picsum.photos/seed/pin-rod-009/600/400", "Rodillo antigoteo 9 pulgadas", 6.20m, "PIN-ROD-009", 44 },
                    { 25, 4, "Cinta de papel para delimitar bordes de pintura, adhesivo suave que no daña la superficie.", "https://picsum.photos/seed/pin-cin-002/600/400", "Cinta de enmascarar 2 pulgadas", 2.10m, "PIN-CIN-002", 88 },
                    { 26, 4, "Diluyente para esmaltes y lacas, alto poder de disolución y evaporación controlada.", "https://picsum.photos/seed/pin-thi-001/600/400", "Thinner industrial 1 litro", 5.95m, "PIN-THI-001", 37 },
                    { 27, 5, "Caja de 100 tornillos autoperforantes con punta broca y cabeza hexagonal para metal.", "https://picsum.photos/seed/tor-aut-081/600/400", "Tornillo autoperforante 8 x 1 pulgada (100 unidades)", 7.60m, "TOR-AUT-081", 55 },
                    { 28, 5, "Tacos plásticos de expansión para concreto y ladrillo, paquete de 50 unidades.", "https://picsum.photos/seed/tor-tac-038/600/400", "Taco de expansión 3/8 (50 unidades)", 4.35m, "TOR-TAC-038", 70 },
                    { 29, 5, "Perno hexagonal galvanizado en caliente con tuerca y arandela, resistente a la corrosión.", "https://picsum.photos/seed/tor-per-124/600/400", "Perno galvanizado 1/2 x 4 pulgadas", 1.85m, "TOR-PER-124", 140 },
                    { 30, 5, "Arandelas planas de acero zincado para distribución de carga en uniones atornilladas.", "https://picsum.photos/seed/tor-ara-516/600/400", "Arandela plana 5/16 (100 unidades)", 3.20m, "TOR-ARA-516", 96 },
                    { 31, 5, "Alambre de acero galvanizado calibre 12 para amarre de estructuras y cercas.", "https://picsum.photos/seed/tor-ala-012/600/400", "Alambre galvanizado N° 12 (1 kg)", 3.75m, "TOR-ALA-012", 80 },
                    { 32, 5, "Remaches pop de aluminio con mandril de acero, caja de 100 unidades para lámina metálica.", "https://picsum.photos/seed/tor-rem-316/600/400", "Remache pop 3/16 (100 unidades)", 6.10m, "TOR-REM-316", 62 },
                    { 33, 6, "Manguera reforzada de 1/2 pulgada y 15 metros con conectores de ajuste rápido.", "https://picsum.photos/seed/jar-man-150/600/400", "Manguera de jardín 15 metros", 19.50m, "JAR-MAN-150", 25 },
                    { 34, 6, "Pala de acero templado con mango de madera de 90 cm, ideal para excavación en tierra dura.", "https://picsum.photos/seed/jar-pal-001/600/400", "Pala cuadrada con mango de madera", 13.80m, "JAR-PAL-001", 20 },
                    { 35, 6, "Tijera de podar con hoja de acero al carbono y mango ergonómico con seguro de bloqueo.", "https://picsum.photos/seed/jar-tij-002/600/400", "Tijera de podar profesional", 11.20m, "JAR-TIJ-002", 33 },
                    { 36, 6, "Fertilizante NPK 15-15-15 de liberación gradual para plantas ornamentales y huertos.", "https://picsum.photos/seed/jar-fer-001/600/400", "Fertilizante granular 1 kg", 6.90m, "JAR-FER-001", 48 },
                    { 37, 6, "Aspersor oscilante de plástico reforzado con cobertura ajustable de hasta 15 metros.", "https://picsum.photos/seed/jar-asp-001/600/400", "Aspersor de jardín ajustable", 8.60m, "JAR-ASP-001", 29 },
                    { 38, 7, "Casco de polietileno de alta densidad con suspensión ajustable y barbiquejo de nylon.", "https://picsum.photos/seed/seg-cas-001/600/400", "Casco de seguridad con barbiquejo", 10.40m, "SEG-CAS-001", 46 },
                    { 39, 7, "Guantes de látex con palma reforzada y puño elástico para manipulación de materiales.", "https://picsum.photos/seed/seg-gua-001/600/400", "Guantes de látex reforzados (par)", 2.90m, "SEG-GUA-001", 110 },
                    { 40, 7, "Lentes de seguridad con lente de policarbonato antiempañante y patillas ajustables.", "https://picsum.photos/seed/seg-len-001/600/400", "Lentes de protección transparentes", 4.15m, "SEG-LEN-001", 82 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Slug",
                table: "Categorias",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaId",
                table: "Productos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Sku",
                table: "Productos",
                column: "Sku",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
