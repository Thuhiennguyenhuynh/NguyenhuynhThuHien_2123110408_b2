//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace NguyenhuynhThuHien.Domain.Migrations
//{
//    /// <inheritdoc />
//    public partial class AddAppointmentSP : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "Chairs",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Status = table.Column<byte>(type: "tinyint", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Chairs", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Dentists",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Specialty = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Status = table.Column<byte>(type: "tinyint", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Dentists", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Patients",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
//                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Patients", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Services",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Duration = table.Column<int>(type: "int", nullable: false),
//                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Services", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "TimeSlots",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    DentistId = table.Column<int>(type: "int", nullable: false),
//                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_TimeSlots", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_TimeSlots_Dentists_DentistId",
//                        column: x => x.DentistId,
//                        principalTable: "Dentists",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "Appointments",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    PatientId = table.Column<int>(type: "int", nullable: false),
//                    DentistId = table.Column<int>(type: "int", nullable: false),
//                    ServiceId = table.Column<int>(type: "int", nullable: false),
//                    ChairId = table.Column<int>(type: "int", nullable: false),
//                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Status = table.Column<byte>(type: "tinyint", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Appointments", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_Appointments_Chairs_ChairId",
//                        column: x => x.ChairId,
//                        principalTable: "Chairs",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_Appointments_Dentists_DentistId",
//                        column: x => x.DentistId,
//                        principalTable: "Dentists",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_Appointments_Patients_PatientId",
//                        column: x => x.PatientId,
//                        principalTable: "Patients",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_Appointments_Services_ServiceId",
//                        column: x => x.ServiceId,
//                        principalTable: "Services",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_Appointments_PatientId",
//                table: "Appointments",
//                column: "PatientId");

//            migrationBuilder.CreateIndex(
//                name: "IX_Appointments_ServiceId",
//                table: "Appointments",
//                column: "ServiceId");

//            migrationBuilder.CreateIndex(
//                name: "UX_Chair_Time",
//                table: "Appointments",
//                columns: new[] { "ChairId", "StartTime" },
//                unique: true);

//            migrationBuilder.CreateIndex(
//                name: "UX_Dentist_Time",
//                table: "Appointments",
//                columns: new[] { "DentistId", "StartTime" },
//                unique: true);

//            migrationBuilder.CreateIndex(
//                name: "IX_Patients_Phone",
//                table: "Patients",
//                column: "Phone",
//                unique: true);

//            migrationBuilder.CreateIndex(
//                name: "IX_TimeSlots_DentistId",
//                table: "TimeSlots",
//                column: "DentistId");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "Appointments");

//            migrationBuilder.DropTable(
//                name: "TimeSlots");

//            migrationBuilder.DropTable(
//                name: "Chairs");

//            migrationBuilder.DropTable(
//                name: "Patients");

//            migrationBuilder.DropTable(
//                name: "Services");

//            migrationBuilder.DropTable(
//                name: "Dentists");
//        }
//    }
//}


using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NguyenhuynhThuHien.Domain.Migrations
{
    public partial class AddAppointmentSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- 1. PHẦN TẠO CÁC BẢNG ---
            migrationBuilder.CreateTable(
                name: "Chairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chairs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dentists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dentists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DentistId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSlots_Dentists_DentistId",
                        column: x => x.DentistId,
                        principalTable: "Dentists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DentistId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ChairId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Chairs_ChairId",
                        column: x => x.ChairId,
                        principalTable: "Chairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Dentists_DentistId",
                        column: x => x.DentistId,
                        principalTable: "Dentists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ServiceId",
                table: "Appointments",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "UX_Chair_Time",
                table: "Appointments",
                columns: new[] { "ChairId", "StartTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Dentist_Time",
                table: "Appointments",
                columns: new[] { "DentistId", "StartTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Phone",
                table: "Patients",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_DentistId",
                table: "TimeSlots",
                column: "DentistId");

            // --- 2. PHẦN TẠO STORED PROCEDURE ---
            var sp = @"
            CREATE PROCEDURE CreateAppointment
                @PatientId INT,
                @DentistId INT,
                @ChairId INT,
                @ServiceId INT,
                @StartTime DATETIME,
                @EndTime DATETIME,
                @Status TINYINT
            AS
            BEGIN
                SET NOCOUNT ON;
                IF EXISTS (
                    SELECT 1 FROM Appointments
                    WHERE (DentistId = @DentistId OR ChairId = @ChairId)
                      AND Status IN (0, 1, 2, 3) 
                      AND (StartTime < @EndTime AND EndTime > @StartTime)
                )
                BEGIN
                    THROW 50001, 'Trùng lịch! Nha sĩ hoặc ghế đã được đặt trong khoảng thời gian này.', 1;
                    RETURN;
                END

                INSERT INTO Appointments (PatientId, DentistId, ChairId, ServiceId, StartTime, EndTime, Status)
                VALUES (@PatientId, @DentistId, @ChairId, @ServiceId, @StartTime, @EndTime, @Status);
            END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa SP trước
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateAppointment;");

            // Xóa bảng sau
            migrationBuilder.DropTable(name: "Appointments");
            migrationBuilder.DropTable(name: "TimeSlots");
            migrationBuilder.DropTable(name: "Chairs");
            migrationBuilder.DropTable(name: "Patients");
            migrationBuilder.DropTable(name: "Services");
            migrationBuilder.DropTable(name: "Dentists");
        }
    }
}